using UnityEngine;

public class InteractableNPCPassivi : MonoBehaviour
{

    private Animator animazione;
    private Quaternion rotazioneOriginale;
    [SerializeField] private bool girabile = true;

    // Start is called before the first frame update
    void Start()
    {
        if(girabile)
            rotazioneOriginale = GetComponentInParent<Transform>().rotation;
        animazione = GetComponentInParent<Animator>();
    }

    public void animazioneParlata(Transform posizionePlayer)
    {
        if(girabile)
            gameObject.transform.parent.LookAt(posizionePlayer);
        animazione.SetBool("parlando", true);
    }

    public void stopAnimazioneParlata(Transform posizionePlayer)
    {
        if(girabile)
            gameObject.transform.parent.transform.rotation = rotazioneOriginale;
        animazione.SetBool("parlando", false);
    }

}
