using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Cosa ha disegnato l'utente sul chart, letto per reflection: <c>/disegni</c>.
///
/// <para><b>Perche' per reflection.</b> La documentazione di <c>IDrawingObjectsListInfo</c> elenca
/// solo linee e canali orizzontali; il 1 ottobre 2026 servivano i rettangoli dell'utente, per
/// copiarne il colore sulle zone LVN. La regola del repo e' ispezionare la build installata invece
/// di fidarsi dei documenti: qui si scarica tutto quello che l'oggetto espone, tipo per tipo.</para>
/// </summary>
public sealed partial class DataBridge
{
    private object Disegni()
    {
        var info = ChartInfo?.DrawingObjectsListInfo;
        if (info is null)
        {
            return new { schema = Schema, error = "DrawingObjectsListInfo non disponibile" };
        }

        var tipo = info.GetType();
        var proprieta = new Dictionary<string, object?>();
        foreach (var p in tipo.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            object? valore;
            try
            {
                valore = p.GetValue(info);
            }
            catch (Exception errore)
            {
                proprieta[p.Name] = $"errore: {errore.GetType().Name}";
                continue;
            }

            proprieta[p.Name] = valore is IEnumerable lista and not string
                ? lista.Cast<object?>().Take(50).Select(Descrivi).ToList()
                : Descrivi(valore);
        }

        // I campi privati: uno e' la DrawingObjectsCollection del chart, e GetAllObjects() da' tutto,
        // rettangoli compresi.
        var campi = new Dictionary<string, object?>();
        foreach (var f in tipo.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var v = f.GetValue(info);
            if (v is null || !v.GetType().Name.Contains("DrawingObjectsCollection", StringComparison.Ordinal))
            {
                continue;
            }

            foreach (var m in v.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         .Where(m => m.Name == "GetAllObjects"))
            {
                var firma = string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                try
                {
                    // Lo strumento che vuole come argomento e' nell'altro campo privato dello stesso oggetto.
                    var valori = tipo.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(x => x.GetValue(info)).Where(x => x is not null).ToList();
                    var argomenti = m.GetParameters().Select(p =>
                        valori.FirstOrDefault(x => p.ParameterType.IsInstanceOfType(x))
                        ?? (p.HasDefaultValue ? p.DefaultValue
                            : p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null)).ToArray();
                    var r = m.Invoke(v, argomenti);
                    campi[$"GetAllObjects({firma})"] = r is IEnumerable l and not string
                        ? l.Cast<object?>().Take(80).Select(Descrivi).ToList()
                        : Descrivi(r);
                }
                catch (Exception errore)
                {
                    campi[$"GetAllObjects({firma})"] = $"errore: {(errore.InnerException ?? errore).GetType().Name}: {(errore.InnerException ?? errore).Message}";
                }
            }
        }

        return new { schema = Schema, tipo = tipo.FullName, proprieta, campi };
    }

    /// <summary>Un oggetto come dizionario dei suoi valori semplici, un livello solo.</summary>
    private static object? Descrivi(object? o)
    {
        if (o is null)
        {
            return null;
        }

        var t = o.GetType();
        if (t.IsPrimitive || o is string or decimal or DateTime or Enum)
        {
            return o.ToString();
        }

        var campi = new Dictionary<string, object?> { ["_tipo"] = t.FullName };
        foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0).Take(80))
        {
            try
            {
                var v = p.GetValue(o);
                campi[p.Name] = v is null ? null
                    : v.GetType().IsPrimitive || v is string or decimal or DateTime or Enum ? v.ToString()
                    : v is IEnumerable e and not string ? $"[{e.Cast<object?>().Take(6).Count()} elementi] " + string.Join("; ", e.Cast<object?>().Take(6).Select(x => x?.ToString()))
                    : v.ToString();
            }
            catch (Exception errore)
            {
                campi[p.Name] = $"errore: {errore.GetType().Name}";
            }
        }

        return campi;
    }
}
