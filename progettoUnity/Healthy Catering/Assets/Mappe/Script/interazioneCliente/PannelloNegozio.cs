using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PannelloNegozio : MonoBehaviour
{
    [SerializeField] private GameObject canvasPannelloNegozio;
    private bool pannelloAperto = false;
    private Animator animazione;

    //INTERAZIONE NEGOZIO
    [SerializeField] private GameObject pannelloXElementi;
    [SerializeField] private Button templateSingoloIngrediente;
    private Button copiaTemplateSingoloIngrediente;

    [SerializeField] private Button bottoneAvantiPannelloNegozio;
    [SerializeField] private Button bottoneIndietroPannelloNegozio;

    private List<Ingrediente> tuttiGliIngredienti;

    //readonly == final in java
    private readonly int numeroBottoniNellaPagina = 9;
    private Button[] ingredientiBottoniFake;
    private int ultimaPaginaVisualizzata = 0;

    // Start is called before the first frame update
    void Start()
    {
        //GESTIONE PANNELLO E RELATIVI
        animazione = GetComponentInParent<Animator>();
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false);
        pannelloXElementi.SetActive(false);
        
        //INTERAZIONE NEGOZIO
        tuttiGliIngredienti = Database.getDatabaseOggetto(new Ingrediente());

        copiaTemplateSingoloIngrediente = Instantiate(templateSingoloIngrediente);
        Destroy(templateSingoloIngrediente);

        ingredientiBottoniFake = new Button[numeroBottoniNellaPagina];
        int i = 0;
        while (i < numeroBottoniNellaPagina)
        {
            ingredientiBottoniFake[i] = copiaTemplateSingoloIngrediente;
            i++;
        }

        bottoneAvantiPannelloNegozio.onClick.AddListener(cambiaPannelloCarosello (true));
        bottoneIndietroPannelloNegozio.onClick.AddListener(cambiaPannelloCarosello(false));
    }
    
    //INTERAZIONE NEGOZIO
    private void cambiaPannelloCarosello(bool avanti)
    {
        if (avanti)
            ultimaPaginaVisualizzata++;
        else
            ultimaPaginaVisualizzata--;

        disattivaBottoniAvantiDietroSeServe();
        caricaElementiNelCanvas();
    }


    public void interazioneNegozio()
    {
        caricaElementiNelCanvas();


    }

    private void caricaElementiNelCanvas()
    {
        fillIngredientiBottoniFake();
        mettiIngredientiBottoniFakeNellaSchermata();
    }

    //GESTIONE PANNELLO E RELATIVI
    public void apriPannelloNegozio()
    {
        animazioneNPCParlante();
        pannelloAperto = true;
        canvasPannelloNegozio.SetActive(true);
        interazioneNegozio();
    }

    public void chiudiPannelloNegozio()
    {
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false);
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
