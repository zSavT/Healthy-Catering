using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class OkBoxVideo : MonoBehaviour
{
    [SerializeField] private GameObject pannello;
    [SerializeField] private TextMeshProUGUI titolo;
    [SerializeField] private TextMeshProUGUI testo;
    [SerializeField] private Image immagineOGIF;

    public static readonly int meccanicheServire = 0;
    public static readonly int finitiIngredienti = 1;
    public static readonly int doveEIlNegozio = 2;
    public static readonly int interazioneNPC = 3;

    public static bool meccanicheServireMostrato = false;
    public static bool finitiIngredientiMostrato = false;
    public static bool doveEIlNegozioMostrato = false;
    public static bool interazioneNPCMostrato = false;

    List<string> titoli = new List<string>
    {
        "spiegazione meccaniche per servire",
        "sono finiti gli ingredienti per fare i piatti",
        "Dov'e' il negozio",
        "interazione con gli npc"
    };
    
    List<string> testi = new List<string>
    {
        "testo spiegazione meccaniche per servire",
        "testo sono finiti gli ingredienti per fare i piatti",
        "testo Dov'e' il negozio",
        "testo interazione con gli npc"
    };

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;

    void Start()
    {
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
        cambiaImmagine(titoli.Count + 1); //setto l'immagine a quella di default
    }

    public void apriOkBoxVideo(int posizione)
    {
        pannello.SetActive(true);
        playerStop.Invoke();
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();

        if (posizione < titoli.Count)
        {
            titolo.text = titoli[posizione];
            testo.text = testi[posizione];
            cambiaImmagine(posizione);
        }
        else
        {
            titolo.text = "errore";
            testo.text = "";
            cambiaImmagine(titoli.Count + 1); //setto l'immagine a quella di default
        }
    }

    public void chiudiOkBoxVideo()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false); 
        playerRiprendiMovimento.Invoke();

        titolo.text = "";
        testo.text = "";
        immagineOGIF = null;
    }

    private void cambiaImmagine(int posizione)
    {
        Sprite nuovaImmagine = Resources.Load<Sprite>("immaginiOGifOkBoxVideo/" + "immagineOGifOkBoxVideo " + titoli[posizione]);
        print(nuovaImmagine);
        if (nuovaImmagine == null)
        {
            nuovaImmagine = Resources.Load<Sprite>("immaginiOGifOkBoxVideo/immagineOGifOkBoxVideoDefault"); ;
        }
        immagineOGIF.sprite = nuovaImmagine;
    }
}
