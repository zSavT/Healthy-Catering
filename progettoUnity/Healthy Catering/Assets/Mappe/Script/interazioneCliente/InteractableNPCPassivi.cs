using UnityEngine;

public class InteractableNPCPassivi : MonoBehaviour
{

    private Animator animazione;
    private Quaternion rotazioneOriginale;

    // Start is called before the first frame update
    void Start()
    {
        rotazioneOriginale = GetComponentInParent<Transform>().rotation;
        animazione = GetComponentInParent<Animator>();
    }

    public void animazioneParlata(Transform posizionePlayer)
    {
        gameObject.transform.parent.LookAt(posizionePlayer);
        animazione.SetBool("parlando", true);
    }

    public void stopAnimazioneParlata(Transform posizionePlayer)
    {
        gameObject.transform.parent.transform.rotation = rotazioneOriginale;
        animazione.SetBool("parlando", false);
    }

}
