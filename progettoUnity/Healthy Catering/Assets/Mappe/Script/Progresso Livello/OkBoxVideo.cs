using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OkBoxVideo : MonoBehaviour
{
    [SerializeField] private GameObject pannello;
    [SerializeField] private Button bottoneConferma;
    [SerializeField] private TextMeshProUGUI titolo;
    [SerializeField] private TextMeshProUGUI testo;
    [SerializeField] private Image immagineOGIF;

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;
    private ControllerInput controllerInput;
    [SerializeField] private MovimentoPlayer movimentoPlayer;
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
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
        animazione = immagineOGIF.GetComponent<Animazione>();
    }

    private void OnEnable()
    {
        controllerInput = new ControllerInput();
        controllerInput.UI.Disable();
        controllerInput.Player.Enable();
        controllerInput.Player.Salto.Disable();
    }

    private void OnDisable()
    {
        controllerInput.UI.Disable();
        controllerInput.Player.Salto.Enable();
    }

    public void apriOkBoxVideo(int posizione)
    {
        playerStop.Invoke();
        indiceCorrente = posizione;
        Interactor.menuApribile = false;
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
        pannello.SetActive(true);
        PlayerSettings.addattamentoSpriteComandi(testo);
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
        EventSystem.current.SetSelectedGameObject(bottoneConferma.gameObject);
    }

    public void chiudiOkBoxVideo()
    {
        StartCoroutine(attendi(2f));
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();

        titolo.text = "";
        testo.text = "";
        Interactor.menuApribile = true;
        if(indiceCorrente == Costanti.parlaZio)
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
            Costanti.nomiAnimazioniOkBoxVideo [posizione], 
            "default"
        );
    }

    /// <summary>
    /// Il metodo permette di attendere prima di proseguire
    /// </summary>
    /// <param name="attesa">Durata attesa</param>
    /// <returns></returns>
    IEnumerator attendi(float attesa)
    {
        Debug.Log(attesa);
        yield return new WaitForSecondsRealtime(attesa);
    }
}
