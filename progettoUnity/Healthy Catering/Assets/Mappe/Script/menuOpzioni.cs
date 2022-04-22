using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MenuOpzioni : MonoBehaviour
{

    public AudioMixer audioMixer;

    public TMP_Dropdown risoluzioniDisponibili;

    public TMP_Dropdown livelloGrafica;

    public Resolution[] risoluzioni;

    void Start()
    {
        risoluzioni = Screen.resolutions;
        risoluzioniDisponibili.ClearOptions();      //svuota le scelte
        List<string> opzioni = new List<string>();
        int indiceRisoluzioneCorrente = 0;
        for (int i = 0; i < risoluzioni.Length; i++)
        {
            string risoluzione = risoluzioni[i].width + " x " + risoluzioni[i].height;
            opzioni.Add(risoluzione);
            if (risoluzioni[i].width == Screen.currentResolution.width &&
                risoluzioni[i].height == Screen.currentResolution.height)
            {
                indiceRisoluzioneCorrente = i;
            }
        }
        risoluzioniDisponibili.AddOptions(opzioni);
        risoluzioniDisponibili.value = indiceRisoluzioneCorrente; ;
        risoluzioniDisponibili.RefreshShownValue();
        livelloGrafica.value = QualitySettings.GetQualityLevel();
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
