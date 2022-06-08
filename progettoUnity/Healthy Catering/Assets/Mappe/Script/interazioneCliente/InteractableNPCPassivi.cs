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
        gameObject.transform.parent.LookAt(posizionePlayer);
        animazione.SetBool("parlando", true);
    }

    public void stopAnimazioneParlata()
    {
        gameObject.transform.parent.localRotation = rotazioneOriginale.localRotation;
        animazione.SetBool("parlando", false);
    }

}
