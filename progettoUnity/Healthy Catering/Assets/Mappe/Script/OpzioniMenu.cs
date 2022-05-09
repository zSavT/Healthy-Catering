using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using Wilberforce;


public class OpzioniMenu : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private TMP_Dropdown risoluzioniDisponibili;

    [SerializeField] private TMP_Dropdown livelloGrafica;
    [SerializeField] private Toggle schermoIntero;
    [SerializeField] private Slider sliderVolume;
    [SerializeField] private Toggle vSynch;
    [SerializeField] private Toggle framerateLibero;

    [SerializeField] private Resolution[] risoluzioni;
    [SerializeField] private TMP_Dropdown daltonismo;

    [SerializeField] private Slider sliderFov;
    [SerializeField] private Slider sliderSensibilita;

    void Start()
    {
        //DALTONISMO
        daltonismo.value = caricaImpostazioniDaltonismo();


        //IMPOSTAZIONI CONTROLLI

        //sliderFov.value = caricaImpostazioniFov();
        //sliderSensibilita.value = caricaImpostazioniSensibilita();

        //RISOLUZIONE
        risoluzioni = Screen.resolutions;
        risoluzioniDisponibili.ClearOptions();      //svuota le scelte
        List<string> opzioni = new List<string>();
        int indiceRisoluzioneCorrente = 0;
        for (int i = 0; i < risoluzioni.Length; i++)
        {
            string risoluzione = risoluzioni[i].width + " x " + risoluzioni[i].height + " (" + risoluzioni[i].refreshRate + ")";
            opzioni.Add(risoluzione);
            if (risoluzioni[i].width == Screen.currentResolution.width &&
                risoluzioni[i].height == Screen.currentResolution.height)
            {
                indiceRisoluzioneCorrente = i;
            }
        }
        risoluzioniDisponibili.AddOptions(opzioni);
        risoluzioniDisponibili.value = caricaImpostazioniRisoluzione ();
        risoluzioniDisponibili.RefreshShownValue();
        risoluzioniDisponibili.value = caricaImpostazioniRisoluzione();

        //GRAFICA
        livelloGrafica.value = QualitySettings.GetQualityLevel();

        //SCHERMO INTERO
        schermoIntero.isOn = caricaImpostazioniFullScreen();

        //VSYNCH
        int vSyncVal = QualitySettings.vSyncCount;
        if (vSyncVal == 0)
        {
            vSynch.isOn = false;
        } else if (vSyncVal == 1)
        {
            vSynch.isOn = true;
        }

        framerateLibero.isOn = caricaImpostazioniFramerateLibero();

        //AUDIO
        sliderVolume.value = caricaImpostazioniVolume();

    }

    private void salvaImpostazioniFullScreen(bool fullScreen)
    {
        if (fullScreen)
        {
            PlayerPrefs.SetInt("fullScreen", 0);        //attivo
        } else
        {
            PlayerPrefs.SetInt("fullScreen", 1);            //disattivo
        }
    }

    private bool caricaImpostazioniFullScreen()
    {
        if (PlayerPrefs.GetInt("fullScreen") == 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void salvaImpostazioniFramerateLibero(bool framerateLibero)
    {
        if (framerateLibero)
        {
            PlayerPrefs.SetInt("framerateLibero", 0);        //attivo
        }
        else
        {
            PlayerPrefs.SetInt("framerateLibero", 1);            //disattivo
        }

    }
    
    private bool caricaImpostazioniFramerateLibero()
    {
        if (PlayerPrefs.GetInt("framerateLibero") == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setFramerateLibero(bool isActive)
    {
        if (isActive)
            Application.targetFrameRate = -1;
        else
            setRefreshRateToScelta(Screen.currentResolution.refreshRate);
        salvaImpostazioniFramerateLibero(isActive);
    }

    private void salvaImpostazioniRisoluzione(int indiceRisoluzione)
    {
        PlayerPrefs.SetInt("risoluzione", indiceRisoluzione);
    }

    private int caricaImpostazioniRisoluzione()
    {
        return PlayerPrefs.GetInt("risoluzione");
    }

    public void salvaImpostazioniDaltonismo(int indiceDaltonismo)
    {
        PlayerPrefs.SetInt("daltonismo", indiceDaltonismo);
    }

    private int caricaImpostazioniDaltonismo()
    {
        return PlayerPrefs.GetInt("daltonismo");
    }

    private void salvaImpostazioniVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    private float caricaImpostazioniVolume()
    {
        return PlayerPrefs.GetFloat("volume");
    }

    public void setVSync(bool isActive)
    {
        if (isActive == true)
        {
            QualitySettings.vSyncCount = 1;
        } else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    void setRefreshRateToScelta(int value)
    {
        Application.targetFrameRate = value;
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        salvaImpostazioniVolume(volume);
    }

    public void setQualita(int indiceQualita)
    {
        QualitySettings.SetQualityLevel(indiceQualita);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        salvaImpostazioniFullScreen(isFullscreen);
    }

    public void menuPrincipale()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f; //sblocca il tempo
        }
        SceneManager.LoadScene(0);
    }

    public void setRisoluzione(int risoluzioneSelezionata)
    {
        Resolution risoluzione = risoluzioni[risoluzioneSelezionata];
        Screen.SetResolution(risoluzione.width, risoluzione.height, schermoIntero.isOn, risoluzione.refreshRate);
        setRefreshRateToScelta (risoluzione.refreshRate);
        salvaImpostazioniRisoluzione(risoluzioneSelezionata);
    }

    public void salvaImpostazioniFov(float fov)
    {
        PlayerPrefs.SetFloat("fov", fov);
    }

    public void salvaImpostazioniSensibilita(float sensibilita)
    {
        PlayerPrefs.SetFloat("sensibilita", sensibilita);
    }

    private float caricaImpostazioniFov()
    {
        return PlayerPrefs.GetFloat("fov");
    }

    private float caricaImpostazioniSensibilita()
    {
        return PlayerPrefs.GetFloat("sensibilita");
    }
}
