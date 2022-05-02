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

    [SerializeField] private Resolution[] risoluzioni;
    private bool aspectRatioStandard;                       //se true l'aspect ratio è 16:9, se false è 4:3
    private int indiceRisoluzioneCorrente;

    void Start()
    {
        risoluzioniDisponibili.AddOptions(setRisoluzioni());
        risoluzioniDisponibili.value = indiceRisoluzioneCorrente;
        risoluzioniDisponibili.RefreshShownValue();
        livelloGrafica.value = QualitySettings.GetQualityLevel();
        schermoIntero.isOn = Screen.fullScreen;
        float valoreAudioAperturaScena = 0f;
        audioMixer.GetFloat("volume", out valoreAudioAperturaScena);
        sliderVolume.value = valoreAudioAperturaScena;
    }

    private List<string> setRisoluzioni()
    {
        risoluzioni = Screen.resolutions;
       // print("Lista risoluzioni\n" + risoluzioni);
        risoluzioniDisponibili.ClearOptions();      //svuota le scelte
        List<string> opzioni = new List<string>();
        int indiceRisoluzioneCorrente = 0;
        string risoluzione = null;
        float valoreQuattroTerzi = (float)4 / 3;
        print(valoreQuattroTerzi);
        for (int i = 0; i < risoluzioni.Length; i++)
        {
            risoluzione = risoluzioni[i].width + " x " + risoluzioni[i].height + " (" + risoluzioni[i].refreshRate + ")";
            print(aspectRatioStandard);
            opzioni.Add(risoluzione);
            if (risoluzioni[i].width == Screen.currentResolution.width && risoluzioni[i].height == Screen.currentResolution.height)
            {
                indiceRisoluzioneCorrente = i;
            }
        }
        return opzioni;
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
