using UnityEngine;

public class InteractableNPCPassivi : MonoBehaviour
{

    private Animator animazione;
    private Transform rotazioneOriginale;

    // Start is called before the first frame update
    void Start()
    {
        rotazioneOriginale = GetComponentInParent<Transform>();
        animazione = GetComponentInParent<Animator>();
    }

    public void animazioneParlata(Transform posizionePlayer)
    {
        rotazioneNPC(posizionePlayer);
        animazione.SetBool("parlando", true);
    }

    public void stopAnimazioneParlata()
    {
        gameObject.transform.parent.localRotation = rotazioneOriginale.localRotation;
        animazione.SetBool("parlando", false);
    }

    private void rotazioneNPC(Transform posizionePlayer)
    {
        //Il movimento è istantaneo per renderlo più smooth si può usare eventualmente la stessa logica CountText() in HUB
        //in alternativa si puoò usare il GameObject.FindGameObjectWithTag("Player").transform;
        /*
        Transform npcPos = gameObject.transform.parent;         //parent perchè la rotazione è dovuta dal contenitore in game e non dal modello
        Vector3 delta = new Vector3(posizionePlayer.rotation.x - npcPos.position.x, 0.0f, posizionePlayer.rotation.z - npcPos.position.z);
        Quaternion rotation = Quaternion.LookRotation(delta);
        gameObject.transform.parent.localRotation = rotation;
        */
        gameObject.transform.parent.LookAt(posizionePlayer);
    }
}
