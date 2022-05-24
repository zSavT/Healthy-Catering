using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    public UnityEvent onInteract;   //variabile per trigger dell'evento
    public int IDCliente;
    private Animator controllerAnimazione;


    void Start()
    {
        controllerAnimazione = GetComponentInChildren<Animator>();
    }

    public void animazioneContenta()
    {
        controllerAnimazione.SetBool("servito", true);
        controllerAnimazione.SetBool("affinitaPatologiePiatto", true);
        controllerAnimazione.SetBool("affinitaDietaPiatto", true);
    }

    public void animazioneScontenta()
    {
        controllerAnimazione.SetBool("servito", true);
        controllerAnimazione.SetBool("affinitaPatologiePiatto", false);
        controllerAnimazione.SetBool("affinitaDietaPiatto", false);
    }

    public void animazioneCamminata()
    {
        controllerAnimazione.SetBool("finito", true);
    }

}


