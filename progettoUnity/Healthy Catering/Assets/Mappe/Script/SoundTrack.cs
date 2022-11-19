using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Classe per gestire la musica nel gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Qualsiasi oggetto presente nella prima scena (prefiribilmente uno vuoto).
/// </para>
/// </summary>
public class SoundTrack : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        audioMixer.SetFloat("volume", PlayerSettings.caricaImpostazioniVolumeMusica());
    }

    private void Awake()
    {
        GameObject[] soundtrack = GameObject.FindGameObjectsWithTag("SoundTrack");
        if (soundtrack.Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
}
