using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

/// <summary>
/// Classe per la gestione delle impostazioni del gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello opzioni.
/// </para>
/// </summary>
public class OpzioniMenu : MonoBehaviour
{
    [Header("Impostazioni Grafica e Schermo")]
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
    [SerializeField] private AudioSource suonoClick;
    
    [Header("Impostazioni Sensibilit�")]
    [SerializeField] private TextMeshProUGUI sliderSensibilitaTesto;
    [SerializeField] private Slider sliderSensibilita;
    
    [Header("Impostazioni Fov")]
    [SerializeField] private TextMeshProUGUI sliderFovTesto;
    [SerializeField] private Slider sliderFov;
    private bool caricatiValori = false;

    void Start()
    {
        CambioCursore.cambioCursoreNormale();

        //DALTONISMO
        daltonismo.value = PlayerSettings.caricaImpostazioniDaltonismo();

        //IMPOSTAZIONI CONTROLLI
        sliderFov.value = PlayerSettings.caricaImpostazioniFov();
        sliderSensibilita.value = PlayerSettings.caricaImpostazioniSensibilita();

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
        vSync.isOn = false;
        if (vSyncVal == 1) vSync.isOn = true;
        
        //FRAMERATE LIBERO
        framerateLibero.isOn = PlayerSettings.caricaImpostazioniFramerateLibero();

        //AUDIO
        sliderVolumeMusica.value = PlayerSettings.caricaImpostazioniVolumeMusica();
        sliderVolumeSuoni.value = PlayerSettings.caricaImpostazioniVolumeSuoni();

        caricatiValori = true;
    }

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
            suonoClick.Play();
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
            suonoClick.Play();
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
    /// Salva impostazione daltonismo.
    /// </summary>
    /// <param name="scelta">Indice scelta daltonismo dropdown</param>
    public void setDaltonismo(int scelta)
    {
        if (caricatiValori)
            suonoClick.Play();
        PlayerSettings.salvaImpostazioniDaltonismo(scelta);
    }
}
