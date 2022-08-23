using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe per la gestione del crossHair del gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// CrossHair in game.
/// </para>
/// </summary>
public class CrossHairGioco : MonoBehaviour
{
    [Header("CrossHair")]
    //riferimento allo sprit del crossHair
    private RawImage crossHair;
    //colore base del crossHair
    [SerializeField] private Color32 coloreNormale;
    //colore del crossHair quando viene in contatto con un entità interagibile
    [SerializeField] private Color32 coloreInterazione;                               

    void Start()
    {
        crossHair = GetComponent<RawImage>();
        crossHair.color = coloreNormale;
    }

    /// <summary>
    /// Imposta il colore del crossHair con quello settato in <strong>coloreInterazione</strong>.
    /// </summary>
    public void cambioColoreInterazione()
    {
        crossHair.color = coloreInterazione;    //cambia colore crosshair
    }

    /// <summary>
    /// Imposta il colore del crossHair con quello settato in <strong>coloreNormale</strong>.
    /// </summary>
    public void cambioColoreNormale()
    {
        crossHair.color = coloreNormale;
    }

    /// <summary>
    /// Attiva o disattiva il crossHair del gioco</strong>.
    /// </summary>
    public void attivaDisattivaPuntatore()
    {
        crossHair.enabled = !crossHair.enabled;
    }


    /// <summary>
    /// Attiva il puntatore
    /// </summary>
    public void attivaPuntatore()
    {
        crossHair.enabled = true;
    }

    /// <summary>
    /// Disattiva il puntatore
    /// </summary>
    public void disattivaPuntatore()
    {
        crossHair.enabled = false;
    }
}
