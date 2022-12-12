using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Classe per la gestione del pannello dei messaggi per il tutorial sa il giocatore ha mai giocato il FPS<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore/pannello del box per il tutorial PlayerSaGiocareFPS.
/// </para>
/// </summary>
public class PlayerSaGiocareFPS : MonoBehaviour
{

    [SerializeField] GameObject pannello;
    [SerializeField] GameObject bottoneNo;
    [SerializeField] GameObject bottoneSi;
    private GameObject ultimoElementoSelezionato;
    // 0 = nessuno selezionato
    // 1 = si
    // -1 = no
    public static int siOno = 0;
    public static bool pannelloSaGiocareAperto = false;

    [SerializeField] private UnityEvent playerRiprendiMovimento;

    void Start()
    {
        ultimoElementoSelezionato = bottoneSi;
        pannello.SetActive(false);
        pannelloSaGiocareAperto = false;
        siOno = 0;
    }

    private void Update()
    {
        if (Utility.gamePadConnesso())
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (EventSystem.current.currentSelectedGameObject != bottoneNo && EventSystem.current.currentSelectedGameObject != bottoneSi)
                    EventSystem.current.SetSelectedGameObject(ultimoElementoSelezionato);
            } else
                ultimoElementoSelezionato = EventSystem.current.currentSelectedGameObject;
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(bottoneNo);
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
    /// Il metodo imposta come elemento selzionato dell'EventSystem l'oggetto passato in input
    /// </summary>
    /// <param name="elementoDaSelezionare">GameObject da impostare come elemento selezionato</param>
    private void aggioraEventSystemPerControllerConnesso(GameObject elementoDaSelezionare)
    {
        if (Utility.gamePadConnesso())
            EventSystem.current.SetSelectedGameObject(elementoDaSelezionare);
    }

    /// <summary>
    /// Il metodo permette di aprire il pannello Player Sa Giocare FPS
    /// </summary>
    public void apriPannelloPlayerSaGiocareFPS()
    {
        pauseGame();
        pannello.SetActive(true);
        //playerStop.Invoke () chiamato in progresso tutorial
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
        pannelloSaGiocareAperto = true;
    }

    /// <summary>
    /// Il metodo inizializza i valori per il caso in cui il giocatore sa giocare già agli FPS
    /// </summary>
    public void chiudiPannelloSi()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();
        siOno = 1;
        pannelloSaGiocareAperto = false;
        resumeGame();
    }

    /// <summary>
    /// Il metodo inizializza i valori per il caso in cui il giocatore non sa giocare già agli FPS
    /// </summary>
    public void chiudiPannelloNo()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();
        siOno = -1;
        pannelloSaGiocareAperto = false;
        resumeGame();
    }

    /// <summary>
    /// Il metodo controlla se la variabile del giocatore è settata o meno (0 vuol dire che non è stata ancora inserita la scelta)
    /// </summary>
    /// <returns>bool True: Il giocatore ha effettuato la scelta, False: Il giocatore non ha ancora inserito la scelta</returns>
    public static bool siOnoSettato ()
    {
        return siOno != 0;
    }

    /// <summary>
    /// Il metodo restituisce il metodo
    /// </summary>
    /// <returns>int valore della scelta, <strong>0: Non settato, 1: Il giocatore sa giocare, -1: Non sa giocare</strong>
    /// </returns>
    public static int getSiOno()
    {
        return siOno;
    }

    /// <summary>
    /// Metodo per ripristinare lo scorrere del tempo in gioco
    /// </summary>
    void resumeGame()
    {
        Time.timeScale = 1f; //sblocca il tempo
    }

    /// <summary>
    /// Metodo per bloccare lo scorrere del tempo in gioco.
    /// </summary>
    void pauseGame()
    {
        Time.timeScale = 0f; //blocca il tempo
    }

    /// <summary>
    /// Il metodo distrugge il gameObject della classe
    /// </summary>
    public void distruggiOggetto()
    {
        iTween.Destroy(this.gameObject, 0.2f);
    }
}
