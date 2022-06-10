using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System;

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
    [SerializeField] private TextMeshProUGUI sliderFovTesto;
    [SerializeField] private TextMeshProUGUI sliderSensibilitaTesto;

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
        risoluzioniDisponibili.value = PlayerSettings.caricaImpostazioniRisoluzione ();
        risoluzioniDisponibili.RefreshShownValue();
        risoluzioniDisponibili.value = PlayerSettings.caricaImpostazioniRisoluzione();

        //GRAFICA
        livelloGrafica.value = QualitySettings.GetQualityLevel();

        //SCHERMO INTERO
        schermoIntero.isOn = PlayerSettings.caricaImpostazioniFullScreen();

        //VSYNCH
        int vSyncVal = QualitySettings.vSyncCount;
        if (vSyncVal == 0)
        {
            vSynch.isOn = false;
        } else if (vSyncVal == 1)
        {
            vSynch.isOn = true;
        }

        framerateLibero.isOn = PlayerSettings.caricaImpostazioniFramerateLibero();

        //AUDIO
        sliderVolume.value = PlayerSettings.caricaImpostazioniVolume();

    }

    public void setFramerateLibero(bool isActive)
    {
        if (isActive)
            Application.targetFrameRate = -1;
        else
            setRefreshRateToScelta(Screen.currentResolution.refreshRate);
        PlayerSettings.salvaImpostazioniFramerateLibero(isActive);
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
        PlayerSettings.salvaImpostazioniVolume(volume);
    }

    public void setQualita(int indiceQualita)
    {
        QualitySettings.SetQualityLevel(indiceQualita);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerSettings.salvaImpostazioniFullScreen(isFullscreen);
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
        PlayerSettings.salvaImpostazioniRisoluzione(risoluzioneSelezionata);
    }

    public void aggiornaValoreScrittaFov()
    {
        sliderFovTesto.text = sliderFov.value.ToString();
        PlayerSettings.salvaImpostazioniFov(sliderFov.value);
    }
    public void aggiornaValoreScrittaSensibilita()
    {
        sliderSensibilitaTesto.text = sliderSensibilita.value.ToString();
        PlayerSettings.salvaImpostazioniSensibilita(sliderSensibilita.value);
    }

    public void setDaltonismo(int scelta)
    {
        PlayerSettings.salvaImpostazioniDaltonismo(scelta);
    }
}
