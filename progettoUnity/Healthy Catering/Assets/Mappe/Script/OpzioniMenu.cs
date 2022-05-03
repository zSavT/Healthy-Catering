using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OpzioniMenu : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private TMP_Dropdown risoluzioniDisponibili;

    [SerializeField] private TMP_Dropdown livelloGrafica;
    [SerializeField] private Toggle schermoIntero;
    [SerializeField] private Slider sliderVolume;
    [SerializeField] private Toggle vSynch;

    [SerializeField] private Resolution[] risoluzioni;
   
    void Start()
    {
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
        risoluzioniDisponibili.value = indiceRisoluzioneCorrente;
        risoluzioniDisponibili.RefreshShownValue();
        livelloGrafica.value = QualitySettings.GetQualityLevel();
        schermoIntero.isOn = Screen.fullScreen;
        float valoreAudioAperturaScena = 0f;
        audioMixer.GetFloat("volume", out valoreAudioAperturaScena);
        sliderVolume.value = valoreAudioAperturaScena;
        int vSyncVal = QualitySettings.vSyncCount;
        if (vSyncVal == 0)
        {
            vSynch.isOn = false;
        } else if (vSyncVal == 1)
        {
            vSynch.isOn = true;
        }
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


    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void setQualita(int indiceQualita)
    {
        QualitySettings.SetQualityLevel(indiceQualita);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
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
        Screen.SetResolution(risoluzione.width, risoluzione.height, Screen.fullScreen);
    }
}
