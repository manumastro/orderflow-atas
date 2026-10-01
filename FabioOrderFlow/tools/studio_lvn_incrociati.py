#!/usr/bin/env python3
"""Gli LVN incrociati: lo studio dietro a GliLvnIncrociati.cs, rifatto dal bridge.

Per ogni seduta di cassa dal 15/09/2026 calcola, all'apertura (13:30Z), le zone sottili di tre
profili - il composito di 90 giorni dal chart NQ Daily continuo, aggiustato al rollover; la
settimana dal lunedi'; il giorno dalle 22:00Z - e i tratti dove almeno due coincidono. Poi guarda
il primo ingresso del prezzo in ciascun tratto durante la cassa: "respinta" se torna di 20 punti
dalla parte da cui e' arrivato, "attraversata" se esce dall'altra parte.

E' una DESCRIZIONE, non una validazione: le soglie (20 punti, le grane, le profondita') non sono
state ottimizzate ne' provate, e i tratti attaccati della stessa zona non sono eventi indipendenti.
Richiede il bridge acceso con i chart NQZ6 M1 e NQ Daily (continuo).

    python FabioOrderFlow/tools/studio_lvn_incrociati.py
"""

import json,urllib.request,os,math,datetime as dt
P=json.load(open(os.path.expanduser('~/.fabio-data-bridge.json')))['port']
g=lambda p: json.load(urllib.request.urlopen(f"http://127.0.0.1:{P}{p}",timeout=180))
ids={(c['instrument'],c['timeFrame']):c['id'] for c in g('/charts')['charts']}
if ('NQ','Daily') not in ids:
    raise SystemExit("serve il chart NQ Daily continuo col bridge: il composito a 90 giorni viene da li'")
G=5.0
ROLL='2026-09-14'; OFF=299.25    # spread NQZ6-NQU6 all'11/09, ultimo giorno prima del cambio
def mday(t):
    tt=dt.datetime.fromisoformat(t.replace('Z','+00:00')); return (tt+dt.timedelta(hours=2)).date().isoformat()
daily=g(f"/candles?chart={ids[('NQ','Daily')]}&from=2026-01-01T00:00:00Z&to=2026-10-02T00:00:00Z&levels=true")['candles']
m1=g(f"/candles?chart={ids[('NQZ6','M1')]}&from=2026-09-13T22:00:00Z&to=2026-10-02T00:00:00Z&levels=true")['candles']
def add(h,levels,off=0.0):
    for l in levels or []:
        k=math.floor((float(l['price'])+off)/G)*G; h[k]=h.get(k,0)+l['volume']
def composito(fino):   # giorni < fino
    h={}
    for x in daily:
        d=mday(x['time'])
        if d>=fino: continue
        add(h,x['levels'], OFF if d<ROLL else 0.0)
    return h
def da_m1(a,b):
    h={}
    for x in m1:
        if a<=x['time']<b: add(h,x['levels'])
    return h

def regrid(h,G):
    o={}
    for k,v in h.items(): kk=math.floor(k/G)*G; o[kk]=o.get(kk,0)+v
    return o
def sottili(h,G,liscia,span,prof,minlen):
    h=regrid(h,G); ks=[]; k=min(h)
    while k<=max(h): ks.append(k); k+=G
    v=[h.get(k,0) for k in ks]; n=len(v)
    s=[sum(v[max(0,i-liscia):i+liscia+1])/len(v[max(0,i-liscia):i+liscia+1]) for i in range(n)]
    w=int(span/G); out=[]; a=None
    for i in range(n+1):
        t = i<n and s[i] < prof*max(s[max(0,i-w):i+w+1])
        if t and a is None: a=i
        if not t and a is not None:
            if (i-a)*G>=minlen: out.append((ks[a],ks[i-1]+G))
            a=None
    return out
SCALE={'90G':(10,3,300,0.6,30),'SETT':(5,2,100,0.5,15),'OGGI':(5,1,60,0.5,15)}
def tutte(oggi,settda,giornoda,fino,settfino=None):
    hs={'90G':composito(oggi),'SETT':da_m1(settda,settfino or fino),'OGGI':da_m1(giornoda,fino)}
    return {n:sottili(hs[n],*SCALE[n]) for n in hs}

def incroci(z):
    b=sorted({x for zs in z.values() for a,c in zs for x in (a,c)}); out=[]
    for i in range(len(b)-1):
        a,c=b[i],b[i+1]; chi=[n for n,zs in z.items() if any(p<=a and q>=c for p,q in zs)]
        if len(chi)<2: continue
        if out and out[-1][1]==a and out[-1][2]==chi: out[-1]=(out[-1][0],c,chi)
        else: out.append((a,c,chi))
    return out
giorni=[d for d in sorted({mday(x['time']) for x in m1}) if d>='2026-09-15' and dt.date.fromisoformat(d).weekday()<5]
ris=[]
for d in giorni:
    D=dt.date.fromisoformat(d); lun=D-dt.timedelta(days=D.weekday())
    # Il lunedi' la settimana e' lo stesso profilo del giorno: si usa la settimana prima (audit 01/10/2026).
    lun_fine = None
    if D.weekday()==0:
        lun_fine = f"{(lun-dt.timedelta(days=1)).isoformat()}T22:00:00Z"; lun = lun-dt.timedelta(days=7)
    apre=f"{d}T13:30:00Z"; chiude=f"{d}T20:00:00Z"
    z=tutte(d, f"{(lun-dt.timedelta(days=1)).isoformat()}T22:00:00Z", f"{(D-dt.timedelta(days=1)).isoformat()}T22:00:00Z", apre, lun_fine)
    inc=incroci(z)
    cs=[x for x in m1 if apre<=x['time']<chiude]
    if not cs: continue
    p0=cs[0]['open']
    # In piu' le zone del solo composito, intere: l'audit dei 90 giorni da soli.
    for a,c,chi in inc+[(a,c,['90G-solo']) for a,c in z['90G']]:
        if c<p0-400 or a>p0+400: continue
        # primo ingresso nella zona
        dentro=None
        for i,x in enumerate(cs):
            if x['low']<=c and x['high']>=a: dentro=i; break
        if dentro is None: continue
        da_sopra = cs[max(0,dentro-1)]['close']>c if dentro>0 else p0>c
        esito='nessuno'; dur=None
        for j in range(dentro,len(cs)):
            x=cs[j]
            if da_sopra:
                if x['low']<a: esito='attraversata'; dur=j-dentro; break
                if x['high']>=c+20 and j>dentro: esito='respinta'; dur=j-dentro; break
            else:
                if x['high']>c: esito='attraversata'; dur=j-dentro; break
                if x['low']<=a-20 and j>dentro: esito='respinta'; dur=j-dentro; break
        ris.append((d,a,c,'+'.join(chi),'dall\'alto' if da_sopra else 'dal basso',cs[dentro]['time'][11:16],esito,dur))
for r in ris: print('%s  %.0f-%.0f  x%-14s %-9s %s  %-12s %s min'%(r[0],r[1],r[2],r[3],r[4],r[5],r[6],r[7]))
import collections
print(collections.Counter(('90G da solo' if r[3]=='90G-solo' else len(r[3].split('+')),r[6]) for r in ris))
