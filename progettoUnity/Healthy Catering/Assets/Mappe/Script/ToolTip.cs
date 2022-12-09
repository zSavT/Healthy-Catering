using UnityEngine;

/// <summary>
/// Classe per la gestione dell'attivazione del ToolTip.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Oggetto che triggera l'attivazione del ToolTip
/// </para>
/// </summary>
public class ToolTip : MonoBehaviour
{
    private string messaggio;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.impostaAttivaToolTip(messaggio);
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.nascondiResettaToolTip();
    }

    /// <summary>
    /// Il metodo imposta il messaggio da visualizzare nel ToolTip
    /// </summary>
    /// <param name="messaggio">string messaggio da inserire nel ToolTip</param>
    public void setMessaggio(string messaggio)
    {
        this.messaggio = messaggio;
    }

}
