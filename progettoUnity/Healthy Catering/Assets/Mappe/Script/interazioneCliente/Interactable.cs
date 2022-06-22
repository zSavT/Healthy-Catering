using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;   //variabile per trigger dell'evento
    public int IDCliente;
    private Animator controllerAnimazione;
    [SerializeField] private ParticleSystem effettoPositivo;
    [SerializeField] private ParticleSystem effettoNegativo;
    [SerializeField] private GameObject modelloCliente;

    void Start()
    {
        controllerAnimazione = GetComponentInChildren<Animator>();
        effettoPositivo.Stop();
        effettoNegativo.Stop();
        //modelloCliente.SetActive(false);
    }

    public void animazioneContenta()
    {
        controllerAnimazione.SetBool("servito", true);
        controllerAnimazione.SetBool("affinitaPatologiePiatto", true);
        controllerAnimazione.SetBool("affinitaDietaPiatto", true);
        effettoPositivo.Play();
    }

    public void animazioneScontenta()
    {
        controllerAnimazione.SetBool("servito", true);
        controllerAnimazione.SetBool("affinitaPatologiePiatto", false);
        controllerAnimazione.SetBool("affinitaDietaPiatto", false);
        effettoNegativo.Play();
    }

    public void animazioneCamminata()
    {
        controllerAnimazione.SetBool("finito", true);
    }
}


