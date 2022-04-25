using UnityEngine;
using UnityEngine.UI;

public class CrossHairGioco : MonoBehaviour
{
    [Header("CrossHair")]
    [SerializeField] private RawImage crossHair;                                      //riferimento allo sprit del crossHair
    [SerializeField] private Color32 coloreNormale;                                   //colore base del crossHair
    [SerializeField] private Color32 coloreInterazione;                               //colore del crossHair quando viene in contatto con un entità interagibile

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
