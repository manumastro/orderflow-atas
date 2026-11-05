// Il glossario della pagina (B.5): ogni termine in due righe al massimo, in italiano, che cominciano
// con il termine: cosa significa per il prezzo, e quanto e' provato. L'utente sta imparando le opzioni.
// Lo usa <Termine k="flip">flip</Termine>.

export const GLOSSARIO = {
  freno:
    'Freno: dove i market maker hanno gamma positiva e per coprirsi comprano quando il prezzo scende e vendono quando sale; tende a smorzare i movimenti. Non provato su NQ.',
  acceleratore:
    'Acceleratore: dove i market maker hanno gamma negativa e per coprirsi vendono quando il prezzo scende e comprano quando sale; tende ad allungare i movimenti. Non provato su NQ.',
  flip:
    'Flip: il prezzo dove i market maker passano da freno ad acceleratore. Dice quanto si allunga un movimento, non la direzione; non provato.',
  'zero gamma':
    'Zero gamma: il prezzo dove la gamma totale dei market maker passa per lo zero; è il flip calcolato sulla catena. Una stima, non provata come livello.',
  GEX: 'GEX: l’esposizione gamma dei market maker, cioè quanto devono comprare o vendere per coprirsi a ogni movimento del prezzo. Positiva frena, negativa accelera; il segno è una stima.',
  DEX: 'DEX: il delta scambiato da un gruppo (market maker, retail, ...) da inizio giornata. È un flusso sommato, non una posizione; non dice da solo dove va il prezzo.',
  VEX: 'VEX: la vanna, cioè quanto cambia il delta dei market maker se cambia la volatilità implicita. Misura sperimentale di OptionsDepth, non provata.',
  charm:
    'Charm: quanto cambia il delta dei market maker con il solo passare del tempo, a prezzo fermo. Conta di più vicino alla scadenza; non provato come segnale.',
  '0DTE': '0DTE: le opzioni che scadono oggi. Pesano molto sul prezzo della giornata e spariscono alle 16:00 di New York.',
  muro: 'Muro: lo strike dove l’esposizione dei market maker è più forte, in freno o in acceleratore. Un posto dove il prezzo può rallentare o accelerare; non provato come supporto o resistenza.',
  'call wall':
    'Call wall: lo strike con la gamma delle call più forte, spesso letto come tetto. Su NQ è calcolato nella convenzione A (segno supposto); non provato.',
  'put wall':
    'Put wall: lo strike con la gamma delle put più forte, spesso letto come pavimento. Su NQ è calcolato nella convenzione A (segno supposto); non provato.',
  base: 'Base: la differenza fra il future ES e l’indice SPX (la mediana della cassa). Serve solo a portare i prezzi di SPX in prezzi di ES, come sul chart di ATAS.',
  opex: 'Opex: la scadenza mensile delle opzioni, il terzo venerdì del mese. Molte posizioni si chiudono insieme, e la gamma dopo cambia.',
  skew: 'Skew: quanto le put costano più delle call alla stessa distanza dal prezzo. Alto vuol dire che ci si assicura dalla discesa; è un prezzo, non una previsione.',
  VAL: 'VAL: il bordo basso dell’area di valore del profilo (dove si è scambiato il 70% del volume). Viene dal chart di ATAS, non dalle opzioni.',
  VAH: 'VAH: il bordo alto dell’area di valore del profilo (dove si è scambiato il 70% del volume). Viene dal chart di ATAS, non dalle opzioni.',
  POC: 'POC: il prezzo dove si è scambiato più volume nella seduta. Viene dal chart di ATAS, non dalle opzioni.',
  'ping-pong':
    'Ping-pong: i muri alto e basso degli scalini giornalieri, fra cui il prezzo ha rimbalzato. Una lettura del profilo, non provata come regola.',
  'movimento atteso':
    'Movimento atteso: di quanto il prezzo si muove in un giorno secondo la volatilità implicita (circa una deviazione standard). È una scala, non una direzione.',
  'implicita − realizzata':
    'Implicita − realizzata: la volatilità che il mercato paga meno quella che il prezzo ha fatto davvero. Positiva vuol dire assicurazione cara; non è una previsione.',
  'stile VIX':
    'Stile VIX: una misura della volatilità a 30 giorni costruita come il VIX, su tutta la catena. Si confronta solo con misure dello stesso tipo, non con quella al prezzo (ATM).',
  ATM: 'ATM: l’opzione con lo strike al prezzo di adesso. La sua volatilità è un’altra misura rispetto allo stile VIX, e non si confrontano.',
  'convenzione A':
    'Convenzione A: l’ipotesi che i market maker siano lunghi le call e corti le put. Serve a dare un segno alla GEX di NQ, ma è supposta, non misurata.',
  OptionsDepth:
    'OptionsDepth: il fornitore di X-Ray, che stima su SPX le posizioni per tipo di partecipante dai dati del Cboe. È una stima del fornitore, non una misura nostra; non provata su NQ.',
} as const;

export type ChiaveGlossario = keyof typeof GLOSSARIO;
