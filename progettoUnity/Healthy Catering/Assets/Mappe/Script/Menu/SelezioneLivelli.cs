using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using Wilberforce;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Classe per il caricamento dei livelli<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu della scelta dei livelli.
/// </para>
/// </summary>
public class SelezioneLivelli : MonoBehaviour
{
    /*
     * Menu Principale = 0
     * Menu Opzioni = 1
     * Citta Livello 0 = 2                  
     * Menu Selezione livello = 3
     * Menu Selezione Profilo Utente = 4
     * Menu Creazione Profilo Utente = 5
     * Classifica = 6
     * Video Tutorial = 7
     */

    private Camera cameraGioco;

    [Header("Elementi Uscita")]
    [SerializeField] private GameObject elementiDomandaUscita;

    [Header("Controller comandi")]
    private GameObject ultimoElementoSelezionato;
    private ControllerInput controllerInput;

    [Header("Bottoni Livelli")]
    [SerializeField] private Button bottoneLivello0;
    [SerializeField] private Button bottoneLivello1;
    [SerializeField] private Button bottoneLivello2;
    
    [Header("Elementi Caricamento Livello")]
    [SerializeField] private Slider sliderCaricamento;        //slider del caricamento della partita
    [SerializeField] private UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione

    void Start()
    {
        inizializzaElementiIniziali();
    }

    private void Update()
    {
        ControlloElementoDaSelezionare();
    }

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDisable()
    {
        controllerInput.Disable();
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
    /// Il metodo inizializza gli elementi iniziali del menu della scelta dei livelli
    /// </summary>
    private void inizializzaElementiIniziali()
    {
        cameraGioco = FindObjectOfType<Camera>();
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        if (PlayerSettings.caricaProgressoLivello1() == 1)
        {
            bottoneLivello1.interactable = true;
        }
        if (PlayerSettings.caricaProgressoLivello2() == 1)
        {
            bottoneLivello2.interactable = true;
        }
        elementiDomandaUscita.SetActive(false);                 //disattiva gli elementi della domanda all'uscita per non visualizzarli fin da subito
    }

    /// <summary>
    /// Il metodo se un GamePad è connesso, controlla se l'eventsystem.currentSelectedGameObject risulta nullo ed imposta quello corretto
    /// </summary>
    private void ControlloElementoDaSelezionare()
    {
        if(Utility.gamePadConnesso())
            if(Utility.qualsiasiTastoPremuto(controllerInput))
                if (EventSystem.current.currentSelectedGameObject == null)
                    if (elementiDomandaUscita.activeSelf)
                        EventSystem.current.SetSelectedGameObject(elementiDomandaUscita.GetComponentsInChildren<Button>()[1].gameObject);
                    else
                        EventSystem.current.SetSelectedGameObject(bottoneLivello0.gameObject);
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
    /// Avvia il caricamento del livello
    /// </summary>
    /// <param name="sceneIndex">Indice scena da caricare</param>
    public void playGame(int sceneIndex)
    {
        if (sceneIndex == 7)
        {
            PlayerSettings.livelloSelezionato = 0;
            caricareVideoTutorial();
        } else if (sceneIndex == 8) 
        {
            PlayerSettings.livelloSelezionato = 1;
            avvioLivelloSelezionato(sceneIndex);
        } else if (sceneIndex == 9) 
        {
            PlayerSettings.livelloSelezionato = 2;
            avvioLivelloSelezionato(sceneIndex);
        }
    }

    /// <summary>
    /// Metodo che carica il caricamento del livello selezionato
    /// </summary>
    /// <param name="sceneIndex">Indice scena da caricare</param>
    private void avvioLivelloSelezionato(int sceneIndex)
    {
        allAvvio.Invoke();
        StartCoroutine(caricamentoAsincrono(sceneIndex));
    }

    /// <summary>
    /// Carica il livello con la barra di caricamento
    /// </summary>
    /// <param name="sceneIndex">Indice della scena da caricare</param>
    IEnumerator caricamentoAsincrono(int sceneIndex)
    {
        AsyncOperation caricamento = SceneManager.LoadSceneAsync(sceneIndex);

        while (!caricamento.isDone)
        {
            float progresso = Mathf.Clamp01(caricamento.progress / .9f);
            sliderCaricamento.value = progresso;
            yield return null;
        }
    }

    /// <summary>
    /// Carica il menu principale.
    /// </summary>
    public static void caricaMenuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Carica il menu delle opzioni.
    /// </summary>
    public static void caricaMenuOpzioni()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Carica il menu della selezione livelli.
    /// </summary>
    public static void caricaMenuSelezioneLivello()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// Carica il menu della selezione del profilo utente
    /// </summary>
    public static void caricaMenuSelezioneProfiloUtente()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// Carica il menu della creazione del profilo utente.
    /// </summary>
    public static void caricaMenuCreazioneProfiloUtente()
    {
        SceneManager.LoadScene(5);
    }

    /// <summary>
    /// Carica il livello della citta.
    /// </summary>
    public static void caricaLivelloCitta()
    {
        SceneManager.LoadScene(2);
    }


    /// <summary>
    /// Carica la scena della classifica.
    /// </summary>
    public static void caricaClassifica()
    {
        SceneManager.LoadScene(6);
    }

    /// <summary>
    /// Carica la scena del videoTutorial.
    /// </summary>
    public static void caricareVideoTutorial()
    {
        SceneManager.LoadScene(7);
    }

}

