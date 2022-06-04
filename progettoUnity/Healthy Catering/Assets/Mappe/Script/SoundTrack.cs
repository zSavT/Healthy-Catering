using UnityEngine;
using UnityEngine.Audio;

public class SoundTrack : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        Debug.Log(PlayerSettings.caricaImpostazioniVolume());
        audioMixer.SetFloat("volume", PlayerSettings.caricaImpostazioniVolume());
    }

    private void Awake()
    {
        GameObject[] soundtrack = GameObject.FindGameObjectsWithTag("SoundTrack");
        if (soundtrack.Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
}
