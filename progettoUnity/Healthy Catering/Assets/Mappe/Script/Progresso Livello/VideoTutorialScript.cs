using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class VideoTutorialScript : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] VideoPlayer videoPlayer;
    [Header("Elementi Caricamento Livello")]
    [SerializeField] private Slider sliderCaricamento;        //slider del caricamento della partita
    [SerializeField] private UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione

    // Examples of VideoPlayer function
    void Start()
    {   
        /*
        // Will attach a VideoPlayer to the main camera.
        GameObject camera = GameObject.Find("Main Camera");
        */

        /*
        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        videoPlayer = canvas.GetComponent<VideoPlayer>();
        */

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        videoPlayer.playOnAwake = false;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

        // This will cause our Scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 1;

        /*
        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        videoPlayer.url = "/Users/graham/movie.mov";
        */

        // Skip the first 100 frames.
        videoPlayer.frame = 0;

        // Restart from beginning when done.
        videoPlayer.isLooping = false;

        /*
        // Each time we reach the end, we slow down the playback by a factor of 10.
        videoPlayer.loopPointReached += EndReached;
        */

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        videoPlayer.Play();
        StartCoroutine(autoSkip(((float)videoPlayer.length) + 2f));          //Aggiungo un secondo di delay per dare la possibilità ai pc poco performanti di non caricarli troppo

    }


    public void caricaLivelloTutorial()
    {
        StartCoroutine(caricamentoAsincrono(2));
    }

    IEnumerator autoSkip(float tempo)
    {
        yield return new WaitForSecondsRealtime(tempo);
        caricaLivelloTutorial();
    }



    /// <summary>
    /// Carica il livello con la barra di caricamento
    /// </summary>
    /// <param name="sceneIndex">Indice della scena da caricare</param>
    IEnumerator caricamentoAsincrono(int sceneIndex)
    {
        allAvvio.Invoke();
        yield return new WaitForSecondsRealtime(2f);
        AsyncOperation caricamento = SceneManager.LoadSceneAsync(sceneIndex);

        while (!caricamento.isDone)
        {
            float progresso = Mathf.Clamp01(caricamento.progress / .9f);
            sliderCaricamento.value = progresso;
            yield return null;
        }
    }

}
