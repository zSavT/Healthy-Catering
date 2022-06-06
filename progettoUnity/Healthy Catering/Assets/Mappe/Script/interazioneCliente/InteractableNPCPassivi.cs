using UnityEngine;

public class InteractableNPCPassivi : MonoBehaviour
{

    private Animator animazione;

    // Start is called before the first frame update
    void Start()
    {
        animazione = GetComponentInParent<Animator>();
    }

    public void animazioneParlata()
    {
        animazione.SetBool("parlando", true);
    }

    public void stopAnimazioneParlata()
    {
        animazione.SetBool("parlando", false);
    }
}
