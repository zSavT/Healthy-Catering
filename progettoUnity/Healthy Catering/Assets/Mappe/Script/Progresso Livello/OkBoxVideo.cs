using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Classe per la gestione del pannello dei messaggi per il tutorial<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore/pannello del box per il tutorial.
/// </para>
/// </summary>
public class OkBoxVideo : MonoBehaviour
{
    [Header("Elementi pannello Ok Box Video")]
    private GameObject pannello;
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
    public static bool okBoxVideoAperto = false;
    public static int indiceCorrente = 0;

    void Start()
    {
        pannello = this.gameObject.GetComponentsInChildren<Transform>()[1].gameObject;
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
        okBoxVideoAperto = false;
        CheckTutorial.tastiMovimentoPremuti = false;
        indiceCorrente = 0;
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
        animazione = immagineOGIF.GetComponent<Animazione>();
    }

    private void FixedUpdate()
    {
        if(okBoxVideoAperto)
            if (EventSystem.current.currentSelectedGameObject == null && Utility.gamePadConnesso())
                    EventSystem.current.SetSelectedGameObject(bottoneConferma.gameObject);
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

    /// <summary>
    /// Il metodo apre il pannello Ok Box Video con inizializzando tutti i valori in base all'indice del box da dover aprire
    /// </summary>
    /// <param name="posizione">int indice tipologia di testo da visualizzare</param>
    public void apriOkBoxVideo(int posizione)
    {
        okBoxVideoAperto = true;
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
            testo.text = string.Empty;
        }
        cambiaImmagine(posizione);
        EventSystem.current.SetSelectedGameObject(bottoneConferma.gameObject);

    }

    /// <summary>
    /// Il metodo chiude il pannello Ok Box Video
    /// </summary>
    public void chiudiOkBoxVideo()
    {
        okBoxVideoAperto = false;
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

    /// <summary>
    /// Il metodo aggiorna la gif presente nel pannello in base all'indice passato
    /// </summary>
    /// <param name="posizione">int indice posizione valori da mostrare</param>
    private void cambiaImmagine(int posizione)
    {
        animazione.caricaAnimazione(
            "immaginiOGifOkBoxVideo", 
            Costanti.nomiAnimazioniOkBoxVideo [posizione], 
            "default"
        );
    }


}
