using UnityEngine;
using TMPro;

/// <summary>
/// Classe per la gestione dei cambi colore del testo quando cliccati<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Sull'elemento (testo) che si vuol far cambiare colore.
/// </para>
/// </summary>
public class CambioColoreScritta : MonoBehaviour
{

    /// <summary>
    /// Imposta colore grigio
    /// </summary>
    public void cambioColoreNonCliccatoGrigio()
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(202, 202, 202, 125);
    }

    /// <summary>
    /// Imposta il colore nero per il testo cliccato
    /// </summary>
    public void cambioColoreCliccatoNero()
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;
    }

    /// <summary>
    /// Imposta il colore bianco per il testo cliccato
    /// </summary>
    public void cambioColoreCliccatoBianco()
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,255);
    }
}