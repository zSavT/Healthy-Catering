using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Wilberforce;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Classe per la gestione delle impostazioni presenti nel menu del profilo utente per la modifica e selezione.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu per la scelta delle impostazioni del player.
/// </para>
/// </summary>
public class SceltaImpostazioniPlayer : MonoBehaviour
{
    [Header("Pannelli Elementi")]
    [SerializeField] private GameObject elementiGenereNeutro;
    [SerializeField] private GameObject elementiSalvataggio;
    [SerializeField] private GameObject elementiConferma;
    [Header("Altro")]
    [SerializeField] private TMP_InputField inputFieldNomeGiocatore;
    [SerializeField] private GameObject nomeGiaPreso;
    [SerializeField] private GameObject tastoIndietro;
    [SerializeField] private Button bottoneSalva;
    [SerializeField] private AudioSource suonoClick;
    private ControllerInput controllerInput;
    private Camera cameraGioco;
    private GameObject ultimoElementoSelezionato;
    private List<Player> player = new List<Player>();
    private List<string> nomiPlayerPresenti = new List<string>();
    private string nomeGiocatoreScritto;
    private int sceltaGenere;
    private int sceltaColorePelle;
    private int sceltaModelloPlayer;
    bool genereNeutroScelto = false;

    // Start is called before the first frame update
    void Start()
    {
        inizializzaElementiIniziali();
    }

    private void Update()
    {
        controlloElementoDaSelezionare();
    }

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        controllerInput.Disable();
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    /// <summary>
    /// Il metodo controlla e gestiscisce le periferiche di Input 
    /// </summary>
    /// <param name="device"></param>
    /// <param name="change"></param>
    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                // New Device.
                break;
            case InputDeviceChange.Disconnected:
                ultimoElementoSelezionato = EventSystem.current.currentSelectedGameObject;
                break;
            case InputDeviceChange.Reconnected:
                aggioraEventSystemPerControllerConnesso(ultimoElementoSelezionato);
                break;
            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }
    }

    /// <summary>
    /// Il metodo inizializza tutti gli elementi iniziali per il menu
    /// </summary>
    private void inizializzaElementiIniziali()
    {
        cameraGioco = FindObjectOfType<Camera>();
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        PuntatoreMouse.abilitaCursore();
        nomeGiocatoreScritto = string.Empty;
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        player = new List<Player>();
        genereNeutroScelto = false;
        disattivaElementi();
        controlloEsistenzaProfiliPlayer();
        controlloNomeEsistente();
    }

    /// <summary>
    /// Il metodo se un GamePad è connesso, controlla se l'eventsystem.currentSelectedGameObject risulta nullo ed imposta quello corretto
    /// </summary>
    private void controlloElementoDaSelezionare()
    {
        if (Utility.gamePadConnesso())
            if (EventSystem.current.currentSelectedGameObject == null)
                if (Utility.qualsiasiTastoPremuto(controllerInput))
                    if (elementiConferma.activeSelf && !elementiSalvataggio.activeSelf)
                        EventSystem.current.SetSelectedGameObject(elementiConferma.GetComponentsInChildren<Button>()[1].gameObject);
                    else if (!elementiConferma.activeSelf && elementiSalvataggio.activeSelf)
                        EventSystem.current.SetSelectedGameObject(elementiSalvataggio.GetComponentsInChildren<Button>()[1].gameObject);
                    else if (!elementiConferma.activeSelf && !elementiSalvataggio.activeSelf)
                        EventSystem.current.SetSelectedGameObject(FindObjectOfType<TMP_Dropdown>().gameObject);
    }

    /// <summary>
    /// Il metodo imposta come elemento selzionato dell'EventSystem l'oggetto passato in input
    /// </summary>
    /// <param name="elementoDaSelezionare">GameObject da impostare come elemento selezionato</param>
    private void aggioraEventSystemPerControllerConnesso(GameObject elementoDaSelezionare)
    {
        if (Utility.gamePadConnesso())
            EventSystem.current.SetSelectedGameObject(elementoDaSelezionare);
    }

    /// <summary>
    /// Disattiva tutti gli elementi presenti.
    /// </summary>
    private void disattivaElementi()
    {
        nomeGiaPreso.SetActive(false);
        elementiGenereNeutro.SetActive(false);
        elementiSalvataggio.SetActive(false);
        elementiConferma.SetActive(false);
    }

    /// <summary>
    /// Controlla se il nome inserito dall'utente, corrisponde con uno gi� presente nella lista.
    /// </summary>
    public void controlloNomeEsistente()
    {
        if (nomeGiocatoreScritto != string.Empty)
        {
            bottoneSalva.interactable = true;
            if (PlayerSettings.caricaPrimoAvvio() == 1)
            {
                foreach (string temp in nomiPlayerPresenti)
                {
                    if (temp.ToUpper() == nomeGiocatoreScritto.ToUpper())
                    {
                        nomeGiaPreso.SetActive(true);
                        bottoneSalva.interactable = false;
                        break;
                    }
                    else
                    {
                        nomeGiaPreso.SetActive(false);
                        bottoneSalva.interactable = true;
                    }
                }
            }
        }  
        else
        {
            bottoneSalva.interactable = false;
        }
    }

    /// <summary>
    /// Inizializza la lista dei nomi dei player presenti.
    /// </summary>
    private void aggiuntaNomiPresentiInLista()
    {
        for (int i = 0; i < player.Count; i++)
        {
            nomiPlayerPresenti.Add(player[i].nome);
        }
    }

    /// <summary>
    /// Legge i player presenti, se esistono attiva il tasto per tornare indietro, altrimenti lo disattiva.
    /// </summary>
    public void controlloEsistenzaProfiliPlayer()
    {
        letturaNomiUtenti();
        if (presentePlayer())
        {
            aggiuntaNomiPresentiInLista();
            attivaTastoIndietro();
            bottoneSalva.interactable = true;
        } else
        {
            disattivaTastoIndietro();
            bottoneSalva.interactable = false;
        }
    }

    /// <summary>
    /// Controlla se sono presenti player nel database.
    /// </summary>
    /// <returns><br><strong>True</strong>: Presente almeno un player nel database.<br><strong>False</strong>: Non presente almeno un player nel database.</br></br></returns>
    private bool presentePlayer()
    {
        return player.Count > 0;
    }

    /// <summary>
    /// Legge da file tutti i player presenti e li salva nella lista.
    /// </summary>
    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto(new Player());
        
    }

    /// <summary>
    /// Attiva il tasto indietro.
    /// </summary>
    private void attivaTastoIndietro()
    {
        tastoIndietro.SetActive(true);
    }

    /// <summary>
    /// Disattiva il tasto indietro.
    /// </summary>
    private void disattivaTastoIndietro()
    {
        tastoIndietro.SetActive(false);
    }

    /// <summary>
    /// Salva tutte le impostazioni fatte dal giocatore, salva il giocatore su file<br></br>
    /// Se la scena � stata caricata dal livello tutorial, dopo il salvataggio ritorna al livello tutorial.<br></br>
    /// In caso contrario carica la scena del menu principale. Utilizza 
    /// <see cref="PlayerSettings"/>.
    /// </summary>
    public void salvaImpostazioni()
    {
        PlayerSettings.salvaPrimoAvvio();
        PlayerSettings.salvaNomePlayerGiocante(nomeGiocatoreScritto);
        PlayerSettings.salvaGenereGiocatore(nomeGiocatoreScritto, sceltaGenere);
        PlayerSettings.salvaColorePelle(nomeGiocatoreScritto, sceltaColorePelle);
        if (genereNeutroScelto)
        {
            PlayerSettings.salvaGenereModello3D(nomeGiocatoreScritto, sceltaModelloPlayer);
        }
        Database.salvaNuovoOggettoSuFile(new Player(nomeGiocatoreScritto));
        if(!PlayerSettings.profiloUtenteCreato)
        {
            PlayerSettings.profiloUtenteCreato = true;
            SelezioneLivelli.caricaLivelloCitta();
        } else
        {
            SelezioneLivelli.caricaMenuPrincipale();
        }
    }

    /// <summary>
    /// Salva localmente il nome scelto dal giocatore.
    /// </summary>
    /// <param name="testo">Nome scritto dal giocatore.</param>
    public void leggiInputNomeScritto(string testo)
    {
        nomeGiocatoreScritto = testo;
    }

    /// <summary>
    /// Salva localmente il valore della pelle scelto dal giocatore.
    /// </summary>
    /// <param name="indice">Indice del dropdown scelta colore pelle modello.<br><strong>0: Caucasico<br>1: Asiatico</br><br>2: Afro</br></strong></br></param>
    public void setPellePlayer(int indice)
    {
        suonoClick.Play();
        sceltaColorePelle = indice;
    }

    /// <summary>
    /// Metodo che salva localmente il valore della scelta del genere del modello del giocatore.
    /// </summary>
    /// <param name="indice">Indice scela del modello player<br><strong>0: Maschio<br>1: Femmina</br></strong></br></param>
    public void setSceltaModelloGiocatore(int indice)
    {
        suonoClick.Play();
        sceltaModelloPlayer = indice;
    }

    /// <summary>
    /// Metodo che salva localmente la scelta del genere scelto dal giocatore e controlla se il genere scelto sia quello neutro.<br></br>
    /// In caso affermativo attiva il dropdown per la scelta del genere del modello, altrimenti lo disattiva.
    /// </summary>
    /// <param name="indiceScelta">Indice dropdown del genere<br><strong>0: Maschio<br>1: Femmina</br><br>2: Neutro</br></strong></br></param>
    public void dropdownGenere(int indiceScelta)
    {
        suonoClick.Play();
        sceltaGenere = indiceScelta;
        if (indiceScelta == 2)
        {
            genereNeutroScelto = true;
            elementiGenereNeutro.SetActive(true);
        }
        else
        {
            genereNeutroScelto = false;
            elementiGenereNeutro.SetActive(false);
        }
    }

    /// <summary>
    /// Carica la scena del menu pricipale
    /// </summary>
    public void menuPrincipale()
    {
        SelezioneLivelli.caricaMenuPrincipale();
    }

}
