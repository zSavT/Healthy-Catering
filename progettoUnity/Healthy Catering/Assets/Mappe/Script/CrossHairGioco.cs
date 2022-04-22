using UnityEngine;
using UnityEngine.UI;

public class CrossHairGioco : MonoBehaviour
{
    [Header("CrossHair")]
    public RawImage crossHair;                                      //riferimento allo sprit del crossHair
    public Color32 coloreNormale;                                   //colore base del crossHair
    public Color32 coloreInterazione;                               //colore del crossHair quando viene in contatto con un entità interagibile

    void Start()
    {
        crossHair.color = coloreNormale;
    }

    public void cambioColoreInterazione()
    {
        crossHair.color = coloreInterazione;    //cambia colore crosshair
    }

    public void cambioColoreNormale()
    {
        crossHair.color = coloreNormale;
    }
    public void attivaDisattivaPuntatore()
    {
        crossHair.enabled = !crossHair.enabled;
    }
}
