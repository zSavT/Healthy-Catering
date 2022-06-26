using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    void Start()
    {
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
    }

    public void apriOkBoxVideo(int posizione)
    {
        if (posizione < titoli.Count)
        {
            titolo.text = titoli[posizione];
            testo.text = testi[posizione];
        }
        else
        {
            titolo.text = "errore";
            testo.text = "";
        }
        cambiaImmagine(posizione);
    }

    public void chiudiOkBoxVideo()
    {
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
        immagineOGIF = null;
    }

    private void cambiaImmagine(int posizione)
    {
        Sprite nuovaImmagine = Resources.Load<Sprite>("immaginiAiuto/" + "immagineOGifOkBoxVideo" + titoli[posizione]);
        if (nuovaImmagine == null)
        {
            nuovaImmagine = Resources.Load<Sprite>("immaginiAiuto/immagineAiutoDefault"); ;
        }
        immagineOGIF.sprite = nuovaImmagine;
    }

    
}
