using UnityEngine;

/// <summary>
/// Classe per la gestione dei NPC passivi<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Modello Contenitore NPC passivo.
/// </para>
/// </summary>
public class InteractableNPCPassivi : MonoBehaviour
{

    private Animator animazione;
    private Quaternion rotazioneOriginale;
    [SerializeField] private bool girabile = true;
    [SerializeField] private bool animazioneParlanteAttiva = true;

    // Start is called before the first frame update
    void Start()
    {
        if (girabile)
        {
            rotazioneOriginale = GetComponentInParent<Transform>().rotation;
        }

        animazione = GetComponentInParent<Animator>();
    }

    /// <summary>
    /// Imposta lo sguardo del npc verso il giocatore ed avvio l'animazione della parlata.
    /// </summary>
    /// <param name="posizionePlayer">Posizione del giocatore per orientamento squardo.</param>
    public void animazioneParlata(Transform posizionePlayer)
    {
        if (girabile)
        {
            gameObject.transform.parent.LookAt(posizionePlayer);
        }
        if(animazioneParlanteAttiva)
            animazione.SetBool("parlando", true);
    }

    /// <summary>
    /// Disattiva l'animazione del npc della parlta ed ritorna a quella originaria.
    /// </summary>
    /// <param name="posizionePlayer">Posizione del giocatore per orientamento squadro.</param>
    public void stopAnimazioneParlata(Transform posizionePlayer)
    {
        if (girabile)
        {
            gameObject.transform.parent.transform.rotation = rotazioneOriginale;
        }
        if(!animazioneParlanteAttiva)
            animazione.SetBool("parlando", false);
    }

}
