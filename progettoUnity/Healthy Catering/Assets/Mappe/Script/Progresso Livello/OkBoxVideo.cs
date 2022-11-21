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

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;

    private Animazione animazione;

    public static bool WASDmostrato = false;
    public static bool saltoMostrato = false;
    public static bool sprintMostrato = false;
    public static bool parlaZioMostrato = false;
    public static bool vaiAlRistoranteMostrato = false;
    public static bool meccanicheServireCompatibileMostrato = false;
    public static bool meccanicheServireNonCompatibileMostrato = false;
    public static bool finitiIngredientiMostrato = false;
    public static bool doveEIlNegozioMostrato = false;
    public static bool interazioneNPCMostrato = false;
    public static bool apriRicettarioMostrato = false;
    public static bool apriMenuAiutoMostrato = false;
    public static int indiceCorrente = 0;

    void Start()
    {
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
        animazione = immagineOGIF.GetComponent<Animazione>();

        WASDmostrato = false;
        saltoMostrato = false;
        sprintMostrato = false;
        parlaZioMostrato = false;
        vaiAlRistoranteMostrato = false;
        meccanicheServireCompatibileMostrato = false;
        meccanicheServireNonCompatibileMostrato = false;
        finitiIngredientiMostrato = false;
        doveEIlNegozioMostrato = false;
        interazioneNPCMostrato = false;
        apriRicettarioMostrato = false;
        apriMenuAiutoMostrato = false;

        indiceCorrente = 0;
    }

    public void apriOkBoxVideo(int posizione)
    {
        playerStop.Invoke();
        indiceCorrente = posizione;
        Interactor.menuApribile = false;
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
        pannello.SetActive(true);

        if (posizione < Costanti.titoliOkBoxVideo.Count)
        {
            titolo.text = Costanti.titoliOkBoxVideo[posizione];
            testo.text = Costanti.testiOkBoxVideo[posizione];
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
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();

        titolo.text = "";
        testo.text = "";
        Interactor.menuApribile = true;
        if (indiceCorrente == Costanti.parlaZio)
        {
            parlaZioMostrato = true;
        }
        if (indiceCorrente == Costanti.vaiAlRistorante)
        {
            vaiAlRistoranteMostrato = true;
        }
        if (indiceCorrente == Costanti.doveEIlNegozio)
        {
            doveEIlNegozioMostrato = true;
        }
    }

    private void cambiaImmagine(int posizione)
    {
        animazione.caricaAnimazione(
            "immaginiOGifOkBoxVideo",
            Costanti.nomiAnimazioniOkBoxVideo[posizione],
            "default"
        );
    }
}