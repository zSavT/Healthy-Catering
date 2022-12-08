using TMPro;
using UnityEngine;

/// <summary>
/// Classe per la gestione del ToolTip.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello contenente l'area del ToolTip
/// </para>
/// </summary>
public class ToolTipManager : MonoBehaviour
{
    private TextMeshProUGUI testo;
    public static ToolTipManager instance;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        testo = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Il metodo attiva il ToolTip ed imposta il messaggio passatto in input al TextMeshProUGui presente nel pannello
    /// </summary>
    /// <param name="messaggio">string del messaggio da visualizzare nel ToolTip</param>
    public void impostaAttivaToolTip(string messaggio)
    {
        this.gameObject.SetActive(true);
        testo.text = messaggio;
    }

    /// <summary>
    /// Il metodo disattiva il pannello del ToolTip e inserisce la stringa vuota al TextMeshProUGui
    /// </summary>
    public void nascondiResettaToolTip()
    { 
        this.gameObject.SetActive(false);
        testo.text = string.Empty;
    }
}
