using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


/// <summary>
/// Classe per la gestione delle impostazioni del gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello opzioni.
/// </para>
/// </summary>
public class OpzioniMenu : MonoBehaviour
{
    [Header("Impostazioni Grafica")]
    [SerializeField] private GameObject tastoGrafica;
    [SerializeField] private GameObject impostazioniGrafica;
    [SerializeField] private TMP_Dropdown risoluzioniDisponibili;
    [SerializeField] private TMP_Dropdown livelloGrafica;
    [SerializeField] private Toggle schermoIntero;
    [SerializeField] private Toggle vSync;
    [SerializeField] private Toggle framerateLibero;
    [SerializeField] private Resolution[] risoluzioni;
    [SerializeField] private TMP_Dropdown daltonismo;
    
    [Header("Impostazioni Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderVolumeMusica;
    [SerializeField] private AudioMixer audioMixerSuoni;
    [SerializeField] private Slider sliderVolumeSuoni;

    [Header("Impostazioni Controlli")]
    [SerializeField] private GameObject tastoControlli;
    [SerializeField] private GameObject impostazioniControlli;
    private bool comandiAttivi = true;

    [Header("Impostazioni Sensibilit�")]
    [SerializeField] private TextMeshProUGUI sliderSensibilitaTesto;
    [SerializeField] private Slider sliderSensibilita;
    [SerializeField] private TextMeshProUGUI sliderSensibilitaStickTesto;
    [SerializeField] private Slider sliderSensibilitaStick;

    [Header("Impostazioni Fov")]
    [SerializeField] private TextMeshProUGUI sliderFovTesto;
    [SerializeField] private Slider sliderFov;
    private bool caricatiValori = false;


    [Header("Suoni Scena")]
    [SerializeField] private AudioSource[] vetAudio;

    [Header("Controller Input")]
    [SerializeField] private Image[] immaginiControlli;
    private GameObject ultimoElementoSelezionato;
    private EventSystem eventSystem;
    private ControllerInput controllerInput;

    void Start()
    {
        CambioCursore.cambioCursoreNormale();
        eventSystem = EventSystem.current;
        //DALTONISMO
        daltonismo.value = PlayerSettings.caricaImpostazioniDaltonismo();

        //IMPOSTAZIONI CONTROLLI
        if(PlayerSettings.caricaPrimoAvvioSettaggiSensibilita() != 0)
        {
            sliderFov.value = PlayerSettings.caricaImpostazioniFov();
            sliderSensibilita.value = PlayerSettings.caricaImpostazioniSensibilita();
            sliderSensibilitaStick.value = PlayerSettings.caricaImpostazioniSensibilitaStick();
        }

        //RISOLUZIONE
        risoluzioni = Screen.resolutions;
        risoluzioniDisponibili.ClearOptions();      //svuota le scelte

        risoluzioniDisponibili.AddOptions(getOpzioniRisoluzioni());
        int indiceRisoluzioneCorrente = getIndiceRisoluzioneCorrente();

        if(PlayerSettings.caricaImpostazioniPrimoAvvioRisoluzione()==0)
        {
            risoluzioniDisponibili.value = indiceRisoluzioneCorrente;
            risoluzioniDisponibili.RefreshShownValue();
            PlayerSettings.salvaImpostazioniPrimoAvvioRisoluzione(1);
            risoluzioniDisponibili.value = indiceRisoluzioneCorrente;
        } else
        {
            //vengono chiamati 2 volte i metodi perch� al primo avvio vanno resettati i valori
            //per poi poterli impostare nel modo corretto
            risoluzioniDisponibili.value = PlayerSettings.caricaImpostazioniRisoluzione();
            risoluzioniDisponibili.RefreshShownValue();
            risoluzioniDisponibili.value = PlayerSettings.caricaImpostazioniRisoluzione();
        }

        //GRAFICA
        livelloGrafica.value = QualitySettings.GetQualityLevel();

        //SCHERMO INTERO
        schermoIntero.isOn = PlayerSettings.caricaImpostazioniFullScreen();

        //VSYNCH
        int vSyncVal = QualitySettings.vSyncCount;
        if (vSyncVal == 0)
            vSync.isOn = false;
        else if (vSyncVal == 1)
            vSync.isOn = true;

        //FRAMERATE LIBERO
        framerateLibero.isOn = PlayerSettings.caricaImpostazioniFramerateLibero();

        //AUDIO
        sliderVolumeMusica.value = PlayerSettings.caricaImpostazioniVolumeMusica();
        sliderVolumeSuoni.value = PlayerSettings.caricaImpostazioniVolumeSuoni();

        caricatiValori = true;
        addattamentoImmaginiControlli();

        comandiAttivi = true;
    }

    private void OnEnable()
    {
        controllerInput = new ControllerInput();
        controllerInput.UI.Enable();
        controllerInput.Player.Disable();
    }
    private void OnDisable()
    {
        controllerInput.UI.Disable();
        controllerInput.Player.Enable();
    }


    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
        controllerInput.Disable();
    }

    private void Update()
    {
        if(Utility.gamePadConnesso())
        {
            if (comandiAttivi)
            {
                if (controllerInput.UI.Avanti.IsPressed())
                {
                    clickSuTastoControlli();
                }
                else if (controllerInput.UI.Indietro.IsPressed())
                {
                    clickSuTastoGrafico();
                }
                if(EventSystem.current.currentSelectedGameObject == null)
                {
                    if (Utility.qualsiasiTastoPremuto(controllerInput))
                        if (impostazioniGrafica.activeSelf && !impostazioniControlli.activeSelf)
                            EventSystem.current.SetSelectedGameObject(risoluzioniDisponibili.gameObject);
                        else if (!impostazioniGrafica.activeSelf && impostazioniControlli.activeSelf)
                            EventSystem.current.SetSelectedGameObject(sliderSensibilita.gameObject);
                }
            }
        }

    }

    /// <summary>
    /// Il metodo permette di aggiornare l'immagine presente nel pannello per i comandi in base all'input utilizzato
    /// </summary>
    private void addattamentoImmaginiControlli()
    {
        immaginiControlli[0].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("L1");
        immaginiControlli[1].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("R1");
    }

    /// <summary>
    /// Il metodo imposta il valore alla variabile booleana comandiAttivi con quella passata in input
    /// </summary>
    /// <param name="comandiAttivi">bool comandiAttivi</param>
    public void setComandiAttivi(bool comandiAttivi)
    {
        this.comandiAttivi = comandiAttivi;
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
                addattamentoImmaginiControlli();
                break;
            case InputDeviceChange.Disconnected:
                ultimoElementoSelezionato = eventSystem.currentSelectedGameObject;
                addattamentoImmaginiControlli();
                break;
            case InputDeviceChange.Reconnected:
                aggioraEventSystemPerControllerConnesso(ultimoElementoSelezionato);
                addattamentoImmaginiControlli();
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
    /// Il metodo riproduce il suono dopo il click nel menu aiuto
    /// </summary>
    private void playSuoniClick()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (MenuInGame.menuAperto)
            {
                vetAudio[0].Play();
                vetAudio[1].PlayDelayed(0.1f);
            }
        }
        else
        {
                vetAudio[0].Play();
                vetAudio[1].PlayDelayed(0.1f);
        }
    }

    /// <summary>
    /// Il metodo attiva tutte le impostazioni per il click sul tasto controlli, ovvero attivare le impostazioni per il controller, attivare gli elementi dei controller, disattivare gli elementi della grafica e avviare il suono
    /// </summary>
    public void clickSuTastoControlli()
    {
        setControllerImpostazioniController();
        disattivaElementiGrafica();
        attivaElementiControlli(SceneManager.GetActiveScene().buildIndex != 1);
        playSuoniClick();
    }

    /// <summary>
    /// Il metodo attiva tutte le impostazioni per il click sul tasto grafico, ovvero attivare le impostazioni per il controller, disattivare gli elementi dei controller, attivare gli elementi della grafica e avviare il suono
    /// </summary>
    public void clickSuTastoGrafico()
    {
        setControllerImpostazioniGrafica();
        disattivaElementiControlli();
        attivaElementiGrafica(SceneManager.GetActiveScene().buildIndex != 1);
        playSuoniClick();
    }

    /// <summary>
    /// Il metodo disattiva tutti gli elementi relativi alla grafici del menu
    /// </summary>
    private void disattivaElementiGrafica()
    {
        impostazioniGrafica.SetActive(false);
        tastoGrafica.GetComponentInChildren<CambioColoreScritta>().cambioColoreNonCliccatoGrigio();
    }

    /// <summary>
    /// Il metodo attiva tutti gli elementi relativi alla grafica del menu
    /// </summary>
    /// <param name="inGame">Booleano True, menu attivo in game, False: No</param>
    private void attivaElementiGrafica(bool inGame)
    {
        impostazioniGrafica.SetActive(true);
        if(!inGame)
            tastoGrafica.GetComponentInChildren<CambioColoreScritta>().cambioColoreCliccatoNero();
        else
            tastoGrafica.GetComponentInChildren<CambioColoreScritta>().cambioColoreCliccatoBianco();
        playSuoniClick();
    }

    /// <summary>
    /// Il metodo disattiva tutti gli elementi relativi ai controlli del menu
    /// </summary>
    private void disattivaElementiControlli()
    {
        impostazioniControlli.SetActive(false);
        tastoControlli.GetComponentInChildren<CambioColoreScritta>().cambioColoreNonCliccatoGrigio();
    }

    /// <summary>
    /// Il metodo attiva tutti gli elementi relativi ai controlli del menu
    /// </summary>
    /// <param name="inGame">Booleano True, menu attivo in game, False: No</param>
    private void attivaElementiControlli(bool inGame)
    {
        impostazioniControlli.SetActive(true);
        if(!inGame)
            tastoControlli.GetComponentInChildren<CambioColoreScritta>().cambioColoreCliccatoNero();
        else
            tastoControlli.GetComponentInChildren<CambioColoreScritta>().cambioColoreCliccatoBianco();
    }

    /// <summary>
    /// Il metodo imposta come elemento selzionato dell'EventSystem l'oggetto passato in input
    /// </summary>
    /// <param name="elementoDaSelezionare">GameObject da impostare come elemento selezionato</param>
    private void aggioraEventSystemPerControllerConnesso(GameObject elementoDaSelezionare)
    {
        if (Utility.gamePadConnesso())
            eventSystem.SetSelectedGameObject(elementoDaSelezionare);
    }

    /// <summary>
    /// Imposta come elemento selezionato dall'event system il primo della sezione controlli
    /// </summary>
    public void setControllerImpostazioniController()
    {
        aggioraEventSystemPerControllerConnesso(sliderFov.gameObject);
    }

    /// <summary>
    /// Imposta come elemento selezionato dall'event system il primo della sezione grafica
    /// </summary>
    public void setControllerImpostazioniGrafica()
    {
        aggioraEventSystemPerControllerConnesso(risoluzioniDisponibili.gameObject);
    }

    /// <summary>
    /// Il metodo restiusce una lista delle risoluzioni supportate
    /// </summary>
    /// <returns>List<String> risoluzioni disponibili</returns>
    private List<string> getOpzioniRisoluzioni()
    {
        List<string> opzioni = new List<string>();
        
        for (int i = 0; i < risoluzioni.Length; i++)
        {
            string risoluzione = risoluzioni[i].width + " x " + risoluzioni[i].height + " (" + risoluzioni[i].refreshRate + ")";
            opzioni.Add(risoluzione);
        }

        return opzioni;
    }

    /// <summary>
    /// Il metodo restituisce l'indice della risoluzione corrente
    /// </summary>
    /// <returns>Intero indice risoluzione corrente</returns>
    private int getIndiceRisoluzioneCorrente()
    {
        int indiceRisoluzioneCorrente = 0;

        for (int i = 0; i < risoluzioni.Length; i++)
        {
            if (risoluzioni[i].width == Screen.currentResolution.width &&
                risoluzioni[i].height == Screen.currentResolution.height)
            {
                indiceRisoluzioneCorrente = i;
            }
        }

        return indiceRisoluzioneCorrente;
    }

    /// <summary>
    /// Salva impostazioni framerate libero e le attiva.
    /// </summary>
    /// <param name="isActive">Valore booleano toggle</param>
    public void setFramerateLibero(bool isActive)
    {
        if (isActive)
            Application.targetFrameRate = -1;
        else
            setRefreshRateToScelta(Screen.currentResolution.refreshRate);
        PlayerSettings.salvaImpostazioniFramerateLibero(isActive);
    }

    /// <summary>
    /// Salva impostazioni vSync e le attiva
    /// </summary>
    /// <param name="isActive">Valore booleano toogle</param>
    public void setVSync(bool isActive)
    {
        if (isActive == true)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    /// <summary>
    /// Imposta valore refresh rate.
    /// </summary>
    /// <param name="value">Indice scelta refreshRate</param>
    void setRefreshRateToScelta(int value)
    {
        Application.targetFrameRate = value;
    }

    /// <summary>
    /// Imposta valore volume musica.
    /// </summary>
    /// <param name="volume">Valore volume</param>
    public void setVolumeMusica(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerSettings.salvaImpostazioniVolumeMusica(volume);
    }

    /// <summary>
    /// Imposta valore volume suoni.
    /// </summary>
    /// <param name="volume">Valore volume</param>
    public void setVolumeSuoni(float volume)
    {
        audioMixerSuoni.SetFloat("volume", volume);
        PlayerSettings.salvaImpostazioniVolumeSuoni(volume);
    }

    /// <summary>
    /// Imposta la qualit� selezionata
    /// </summary>
    /// <param name="indiceQualita">Indice qualit� scelta dropdown</param>
    public void setQualita(int indiceQualita)
    {
        if (caricatiValori)
            vetAudio[0].Play();
        QualitySettings.SetQualityLevel(indiceQualita);
    }

    /// <summary>
    /// Imposta valore fullscreen dal toggle.
    /// </summary>
    /// <param name="isFullscreen">Valore booleano Toggle</param>
    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerSettings.salvaImpostazioniFullScreen(isFullscreen);
    }

    /// <summary>
    /// Sblocca il tempo se si carica il menu principale
    /// </summary>
    public void menuPrincipale()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f; //sblocca il tempo
        }
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Salva e setta impostazioni risoluzione
    /// </summary>
    /// <param name="risoluzioneSelezionata">Indice scelta risoluzione dropdown</param>
    public void setRisoluzione(int risoluzioneSelezionata)
    {
        if (caricatiValori)
            vetAudio[0].Play();
        Resolution risoluzione = risoluzioni[risoluzioneSelezionata];
        Screen.SetResolution(risoluzione.width, risoluzione.height, schermoIntero.isOn, risoluzione.refreshRate);
        setRefreshRateToScelta(risoluzione.refreshRate);
        PlayerSettings.salvaImpostazioniRisoluzione(risoluzioneSelezionata);
    }

    /// <summary>
    /// Aggiorna valore testo fov.
    /// </summary>
    public void aggiornaValoreScrittaFov()
    {
        sliderFovTesto.text = sliderFov.value.ToString();
        PlayerSettings.salvaImpostazioniFov(sliderFov.value);
    }

    /// <summary>
    /// Aggiorna valore testo sensibilit�.
    /// </summary>
    public void aggiornaValoreScrittaSensibilita()
    {
        sliderSensibilitaTesto.text = sliderSensibilita.value.ToString();
        PlayerSettings.salvaImpostazioniSensibilita(sliderSensibilita.value);
    }

    /// <summary>
    /// Aggiorna valore testo sensibilit�.
    /// </summary>
    public void aggiornaValoreScrittaSensibilitaStick()
    {
        sliderSensibilitaStickTesto.text = sliderSensibilitaStick.value.ToString();
        PlayerSettings.salvaImpostazioniSensibilitaStick(sliderSensibilitaStick.value);
    }

    /// <summary>
    /// Salva impostazione daltonismo.
    /// </summary>
    /// <param name="scelta">Indice scelta daltonismo dropdown</param>
    public void setDaltonismo(int scelta)
    {
        if (caricatiValori)
            vetAudio[0].Play();
        PlayerSettings.salvaImpostazioniDaltonismo(scelta);
    }
}
