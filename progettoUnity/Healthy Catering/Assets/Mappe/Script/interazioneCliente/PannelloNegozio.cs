using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelloNegozio : MonoBehaviour
{
    [SerializeField] private GameObject pannelloNegozio;
    private bool pannelloAperto = false;
    private Animator animazione;

    // Start is called before the first frame update
    void Start()
    {
        animazione = GetComponentInParent<Animator>();
        pannelloAperto = false;
        pannelloNegozio.SetActive(false);
    }

    public void apriPannelloNegozio()
    {
        animazioneNPCParlante();
        pannelloAperto = true;
        pannelloNegozio.SetActive(true);
    }

    public void chiudiPannelloNegozio()
    {
        pannelloAperto = false;
        pannelloNegozio.SetActive(false);
        animazioneNPCIdle();
    }

    public bool getPannelloAperto()
    {
        return pannelloAperto;
    }

    public void animazioneNPCInquadrato()
    {
        animazione.SetBool("inquadrato", true);
    }

    private void animazioneNPCIdle()
    {
        animazione.SetBool("parlante", false);
        animazione.SetBool("inquadrato", false);
    }

    private void animazioneNPCParlante()
    {
        animazione.SetBool("parlante", true);
    }


}
