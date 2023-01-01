using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Wilberforce;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Classe per la gestione delle impostazioni presenti nel menu iniziale del Gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu principale del gioco.
/// </para>
/// </summary>
public class Menu : MonoBehaviour
{
    [Header("Pannelli")]
    [SerializeField] private GameObject elementiCrediti;
    [SerializeField] private GameObject elementiMenuPrincipale;
    [SerializeField] private GameObject elementiProfiloNonEsistente;
    [SerializeField] private GameObject elementiUscita;

    [Header("Testo ed Altro")]
    [SerializeField] private TextMeshProUGUI testoVersioneGioco;
    [SerializeField] private TextMeshProUGUI companyName;
    [SerializeField] private Image immagineController;
    //serve per eliminare altri elementi in visualilzzazione
    [SerializeField] private UnityEvent clickCrediti;

    private GameObject ultimoElementoSelezionato;
    private ControllerInput controllerInput;
    private List<Player> player = new List<Player>();
    private Camera cameraGioco;


    void Start()
    {
        inizializzazioneElementiIniziali();
    }

    void Update()
    {
        attivaDisattivaIconaController();
        attivaDisattivaLivelli();
        controlloEventSystemCorretto();
    }

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    /// <summary>
    /// Il metodo controlla che l'EventSystem selezione l'elemento corretto ed in caso contrario, imposta quello corretto
    /// </summary>
    private void controlloEventSystemCorretto()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (Utility.qualsiasiTastoPremuto(controllerInput))
                EventSystem.current.SetSelectedGameObject(ultimoElementoSelezionato);
        }
        else
            ultimoElementoSelezionato = EventSystem.current.currentSelectedGameObject;
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
                aggioraEventSystemPerControllerConnesso();
                break;
            case InputDeviceChange.Disconnected:
                //
                break;
            case InputDeviceChange.Reconnected:
                aggioraEventSystemPerControllerConnesso();
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
    /// Il metodo inizializza tutti gli elementi iniziali per il menu principale
    /// </summary>
    private void inizializzazioneElementiIniziali()
    {
        if (Costanti.spriteTastiera == null)
        {
            Costanti.spriteTastiera = Resources.Load<TMP_SpriteAsset>("tastiTastiera");
            Costanti.spriteXbox = Resources.Load<TMP_SpriteAsset>("tastiXbox");
            Costanti.spritePlaystation = Resources.Load<TMP_SpriteAsset>("tastiPlaystation");
        }
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        cameraGioco = FindObjectOfType<Camera>();
        attivaDisattivaIconaController();
        gameVersion();
        //disattivo a priori, per non visualizzarli in caso di errori di lettura dei nomi utenti ed evitare lo schermo occupato tutto da scritte
        elementiProfiloNonEsistente.SetActive(false);
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        letturaNomiUtenti();
        if (!presentePlayer())
        {
            PlayerSettings.resetTuttiValori();
            elementiProfiloNonEsistente.SetActive(true);
            elementiMenuPrincipale.SetActive(false);
            EventSystem.current.SetSelectedGameObject(elementiProfiloNonEsistente.GetComponentsInChildren<Button>()[1].gameObject);
        }
        else
        {
            elementiProfiloNonEsistente.SetActive(false);
        }
        elementiCrediti.SetActive(false);
        CambioCursore.cambioCursoreNormale();
    }

    /// <summary>
    /// Il metodo permette di caricare la creazione elementi
    /// </summary>
    public void caricaElementiPersonalizzati()
    {
        SelezioneLivelli.caricareCreazioneElementi();
    }

    /// <summary>
    /// Il metodo aumenta la trasparenza dell'icona del controller se inserito
    /// </summary>
    private void attivaDisattivaIconaController()
    {
        if (Utility.gamePadConnesso())
        {
            immagineController.color = new Color32(255, 255, 255, 255);
            immagineController.gameObject.GetComponent<ToolTip>().setMessaggio("Controller " + PlayerSettings.tipologiaControllerInserito() + " connesso.");
        } else
        {
            immagineController.color = new Color32(255, 255, 255, 127);
            immagineController.gameObject.GetComponent<ToolTip>().setMessaggio("Controller non connesso."); ;
        }
    }

    /// <summary>
    /// Il metodo aggiorna l'elemento selezionato
    /// </summary>
    private void aggioraEventSystemPerControllerConnesso()
    {
        if (Utility.gamePadConnesso())
            if (!elementiUscita.activeSelf && !elementiCrediti.activeSelf && elementiMenuPrincipale.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(elementiMenuPrincipale.GetComponentsInChildren<Transform>()[1].gameObject);
            }
                
            else if (!elementiUscita.activeSelf && elementiCrediti.activeSelf && !elementiMenuPrincipale.activeSelf)
                EventSystem.current.SetSelectedGameObject(elementiCrediti.GetComponentsInChildren<Transform>()[10].gameObject);
            else if (elementiUscita.activeSelf && !elementiCrediti.activeSelf && !elementiMenuPrincipale.activeSelf)
                EventSystem.current.SetSelectedGameObject(elementiUscita.GetComponentsInChildren<Transform>()[1].gameObject);
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
    /// Metodo per inzializzare la lista dei player presenti nel database.
    /// </summary>
    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto(new Player());
    }

    /// <summary>
    /// Metodo per controllare se sono presenti o meno dei player nel database.
    /// </summary>
    /// <returns>True: è presente almeno un player, false: non esiste alcun player</returns>
    private bool presentePlayer()
    {
        return player.Count > 0;
    }

    /// <summary>
    /// Metodo per caricare la scena della modifica e selezione del profilo utente.
    /// </summary>
    public void caricaSelezioneModificaProfilo()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// Metodo per caricare la scena della creazione del profilo utente.
    /// </summary>
    public void caricaCreazioneProfilo()
    {
        SceneManager.LoadScene(5);
    }

    /// <summary>
    /// Metodo per attivare i livelli tramite cheatcode
    /// </summary>
    private void attivaDisattivaLivelli()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(PlayerSettings.caricaProgressoLivello1() == 0)
            {
                PlayerSettings.salvaProgressoLivello1(true);
                print("livello 1 attivato");
            } else
            {
                print("livello 1 disattivato");
                PlayerSettings.salvaProgressoLivello1(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (PlayerSettings.caricaProgressoLivello2() == 0)
            {
                PlayerSettings.salvaProgressoLivello2(true);
                print("livello 2 attivato");
            }
            else
            {
                print("livello 2 disattivato");
                PlayerSettings.salvaProgressoLivello2(false);
            }
        }
    }

    /// <summary>
    /// Metodo per caricare la scena del menu iniziale.
    /// </summary>
    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Attiva l'evento per visualizzare i credit del gioco.
    /// </summary>
    public void crediti()
    {
        clickCrediti.Invoke();
    }

    /// <summary>
    /// Metodo per caricare la scena della selezione livelli.
    /// </summary>
    public void menuSelezioneLivelli()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// Metodo per aprire la scena del menu opzioni.
    /// </summary>
    public void menuOpzioni()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Metodo per chiudere il gioco.
    /// </summary>
    public void chiudi()
    {
        Application.Quit();
    }

    /// <summary>
    /// Metodo per aggiornare il testo della versione del gioco e il nome della società.
    /// </summary>
    private void gameVersion()
    {
        testoVersioneGioco.text = testoVersioneGioco.text + Application.version;
        companyName.text = companyName.text + " " + Application.companyName;
    }

    /// <summary>
    /// Metodo che carica la scena della classifica
    /// </summary>
    public void classifica()
    {
        SelezioneLivelli.caricaClassifica();
    }

    /// <summary>
    /// Il metodo imposta il gameObject passato come quello selezionato dal EventSystem
    /// </summary>
    /// <param name="bottoneIniziale">GameObject da impostare come elemento principale</param>
    public void impostaEventSystemSelezionato(GameObject bottoneIniziale)
    {
        EventSystem.current.SetSelectedGameObject(bottoneIniziale);
    }
}
