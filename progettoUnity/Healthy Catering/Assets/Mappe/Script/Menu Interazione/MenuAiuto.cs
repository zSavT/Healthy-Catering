using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Classe per gestire il Menu Aiuto<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject contenente i pannelli del Menu Aiuto
/// </para>
/// </summary>
public class MenuAiuto : MonoBehaviour
{
    private bool pannelloMenuAiutoAperto;

    [Header("Oggetti Pannello")]
    [SerializeField] private GameObject pannelloMenuAiuto;
    [SerializeField] private TextMeshProUGUI titoloTestoAiuto;
    [SerializeField] private TextMeshProUGUI testoAiuto;
    [SerializeField] private Button tastoAvanti;
    [SerializeField] private Button tastoIndietro;
    [SerializeField] private TextMeshProUGUI testoNumeroPannelloAttuale;
    [SerializeField] private TextMeshProUGUI testoUscita;
    [Header("Immagini Comandi")]
    [SerializeField] private Image immagineSchermata;
    [SerializeField] private Image comandiAvantiImmagine;
    [SerializeField] private Image comandiIndietroImmagine;


    private int ultimaPosizione;

    private Animazione animazione;

    public static bool apertoMenuAiuto = false;//TUTORIAL

    private ControllerInput controllerInput;

    void Start()
    {
        pannelloMenuAiuto.SetActive(false);
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        pannelloMenuAiutoAperto = false;
        tastoAvanti.onClick.AddListener(mostraProssimoMessaggioDiAiuto);
        tastoIndietro.onClick.AddListener(mostraPrecedenteMessaggioDiAiuto);

        ultimaPosizione = 0;//cosi la prima volta apre il primo messaggio
        testoNumeroPannelloAttuale.text = "1";
        gestisciBottoniAvantiDietro(ultimaPosizione);

        animazione = immagineSchermata.GetComponent<Animazione>();
    }

    private void Update()
    {
        if (pannelloMenuAiutoAperto)
        {
            addattamentoGraficaComandi();
            if (controllerInput.UI.Avanti.WasPressedThisFrame())
                mostraProssimoMessaggioDiAiuto();
            else if (controllerInput.UI.Indietro.WasPressedThisFrame())
                mostraPrecedenteMessaggioDiAiuto();
        }
    }

    /// <summary>
    /// Disattiva il controller alla eliminazione dell'oggetto
    /// </summary>
    private void OnDestroy()
    {
        controllerInput.Disable();
    }

    /// <summary>
    /// Il metodo permette di attivare il Menu Aiuto inizializzando i valori correttamente
    /// </summary>
    public void apriPannelloMenuAiuto()
    {

        pannelloMenuAiuto.SetActive(true);
        pannelloMenuAiutoAperto = true;

        setMediaPannelloAiuto(ultimaPosizione);

        apertoMenuAiuto = true; //TUTORIAL

        addattamentoGraficaComandi();
    }

    /// <summary>
    /// Il metodo aggiorna la grafica dei tasti in game
    /// </summary>
    private void addattamentoGraficaComandi()
    {
        //Aggiornamento immagini e sprite per il controller
        PlayerSettings.addattamentoSpriteComandi(testoUscita);
        PlayerSettings.addattamentoSpriteComandi(testoAiuto);
        comandiIndietroImmagine.GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("L1");
        comandiAvantiImmagine.GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("R1");
    }

    /// <summary>
    /// Il metodo aggiorna la scherma da visualizzare il menu aiuto in base all'indice della pagina passato
    /// </summary>
    /// <param name="pagina">int indice pagina da visualizzare</param>
    private void setMediaPannelloAiuto(int pagina)
    {
        titoloTestoAiuto.text = Costanti.titoliMenuAiuto[pagina];
        testoAiuto.text = Costanti.testiMenuAiuto[pagina];
        cambiaImmagineSchermataAiuto(pagina);

        aggiornaTestoNumeroPannelloAttuale(pagina);

        gestisciBottoniAvantiDietro(pagina);

        ultimaPosizione = pagina;//aggiorno l'ultima posizione
    }

    /// <summary>
    /// Il metodo aggiorna la gif da visualizzare in relazione all'indice passato in input
    /// </summary>
    /// <param name="ultimaPosizione">int indice pagina da visualizzare</param>
    private void cambiaImmagineSchermataAiuto(int ultimaPosizione)
    {
        animazione.caricaAnimazione(
            "immaginiAiuto",
            Costanti.nomiAnimazioniMenuAiuto[ultimaPosizione],
            "default"
        );
    }

    /// <summary>
    /// Il metodo aggiorna il TextMeshProUGUI relativo alla pagina attualmente visualizzata
    /// </summary>
    /// <param name="ultimaPosizione">int indice pagina visualizzata</param>
    private void aggiornaTestoNumeroPannelloAttuale(int ultimaPosizione)
    {
        testoNumeroPannelloAttuale.text = (ultimaPosizione + 1).ToString();
    }

    /// <summary>
    /// Il metodo attiva o disattiva i tasti avanti ed indietro in base all'indice della pagina passato in input (Indice ultima pagina = Disattiva bottone avanti, Indice prima pagina, disattiva il bottone indietro)
    /// </summary>
    /// <param name="posizione">int pagina attualmente visualizzata</param>
    private void gestisciBottoniAvantiDietro(int posizione)
    {
        //tasto avanti
        if (posizione == Costanti.testiMenuAiuto.Count - 1)
        {
            tastoAvanti.interactable = false;
        }
        else
        {
            tastoAvanti.interactable = true;
        }

        //tasto indietro
        if (posizione == 0)
        {
            tastoIndietro.interactable = false;
        }
        else
        {
            tastoIndietro.interactable = true;
        }
    }

    /// <summary>
    /// Il metodo disattiva il pannello del menu aiuto
    /// </summary>
    public void chiudiPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(false);
        pannelloMenuAiutoAperto = false;
    }

    /// <summary>
    /// Il metodo restituisce la variabile booleana pannelloMenuAiutoAperto
    /// </summary>
    /// <returns>bool True: Pannello Aperto, False: Pannello chiuso</returns>
    public bool getPannelloMenuAiutoAperto()
    {
        return pannelloMenuAiutoAperto;
    }

    /// <summary>
    /// Il metodo permette di visualizzare la pagina successiva nel menu aiuto, aggiornando testo ed indice. Se il pannello è già sull'ultima pagina, non cambia visualizzazione
    /// </summary>
    void mostraProssimoMessaggioDiAiuto()
    {
        int prossimaPosizione = ultimaPosizione;
        if (ultimaPosizione != Costanti.testiMenuAiuto.Count - 1)
        {
            prossimaPosizione = ultimaPosizione + 1;
        }
        setMediaPannelloAiuto(prossimaPosizione);
    }

    /// <summary>
    /// Il metodo permette di tornare alla pagina precedente nel menu aiuto, aggiornando testo ed indice. Se il pannello è già sulla prima pagina, non cambia visualizzazione
    /// </summary>
    private void mostraPrecedenteMessaggioDiAiuto()
    {
        int precedentePosizione = ultimaPosizione;
        if (ultimaPosizione != 0)
        {
            precedentePosizione = ultimaPosizione - 1;
        }

        setMediaPannelloAiuto(precedentePosizione);
    }
}
