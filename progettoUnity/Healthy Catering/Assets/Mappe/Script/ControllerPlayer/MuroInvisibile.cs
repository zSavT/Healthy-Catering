using UnityEngine;

/// <summary>
/// Classe per la gestione dei muri invisibili del gioco e il contatto con l'acqua.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore del Player.
/// </para>
/// </summary>
public class MuroInvisibile : MonoBehaviour
{
    [Header("Muri Invisibili Acqua")]
    //Layer per l'acqua
    [SerializeField] private LayerMask oceano;
    [SerializeField] private Transform posizioneReset;
    [SerializeField] private float tolleranzaAltezzaContatto;
    private GameObject player;
    private Transform triggerTocco;         //deve avere il tag "CheckPavimento" inserito
    private bool toccato;

    void Start()
    {
        player = this.gameObject;
        triggerTocco = GameObject.FindGameObjectWithTag("CheckPavimento").transform;
    }

    void Update()
    {
        controlloCollisioneAcqua();
    }

    /// <summary>
    /// Controllo collisione con l'acqua
    /// </summary>
    private void controlloCollisioneAcqua()
    {
        toccato = Physics.CheckSphere(triggerTocco.position, tolleranzaAltezzaContatto, oceano);
        if (toccato)
        {
            player.transform.position = posizioneReset.transform.position;
        }
    }
}
