using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
using UnityEngine.UI;
using TMPro;

public class VideoTutorialScript : MonoBehaviour
{
    private long playerCurrentFrame;
    private long playerFrameCount;

    [SerializeField] private TextMeshProUGUI testoBottoneSkip;
    [SerializeField] GameObject camera;
    [SerializeField] VideoPlayer videoPlayer;

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
    }

    private void Update()
    {
        playerCurrentFrame = videoPlayer.GetComponent<VideoPlayer>().frame;
        playerFrameCount = Convert.ToInt64(videoPlayer.GetComponent<VideoPlayer>().frameCount);




        if (playerCurrentFrame >= playerFrameCount)
        {
            print("ciao");
            videoPlayer.Pause();
            testoBottoneSkip.text = "Continua";
        }
         
    }

    public void caricaLivelloTutorial()
    {
        SelezioneLivelli.caricaLivelloCitta();
    }
}
