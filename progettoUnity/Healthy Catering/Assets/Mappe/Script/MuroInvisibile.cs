using UnityEngine;

/// <summary>
/// Classe per la gestione dei muri invisibili del gioco e il contatto con l'acqua.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore del Player.
/// </para>
/// </summary>
public class MuroInvisibile : MonoBehaviour
{

    [Header("Muri Invisibili")]
    //Layer per l'acqua
    [SerializeField] private LayerMask oceano;
    //Layer per i muri invisibili //da eliminare se non ci serve.
    [SerializeField] private LayerMask muroInvisiible;
    [SerializeField] private Transform posizioneReset;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform triggerTocco;
    [SerializeField] private float tolleranzaAltezzaContatto;
    private bool toccato;

    // Update is called once per frame
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
