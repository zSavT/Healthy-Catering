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
    [SerializeField] private GameObject pannelloNegozio;
    [SerializeField] private Button bottoneAvantiPannelloNegozio;
    [SerializeField] private Button bottoneIndietroPannelloNegozio;

    private List<Ingrediente> tuttiGliIngredienti;

    //readonly == final in java
    private readonly int numeroBottoniNellaPagina = 9;
    private Button[] ingredientiBottoniFake;

    // Start is called before the first frame update
    void Start()
    {
        animazione = GetComponentInParent<Animator>();
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false); 
        tuttiGliIngredienti = Database.getDatabaseOggetto(new Ingrediente());
    }

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

    //INTERAZIONE NEGOZIO
    public void interazioneNegozio()
    {
        ingredientiBottoniFake = pannelloNegozio.GetComponentsInChildren<Button>();
        caricaPaginaIngredientiInPannelloNegozio(0);
    }

    private void caricaPaginaIngredientiInPannelloNegozio(int pagina)
    {
        int indiceIniziale = calcolaIndiceInizialeLista(pagina);

        while (indiceIniziale < (indiceIniziale + numeroBottoniNellaPagina))
        {
            if (indiceIniziale < tuttiGliIngredienti.Count)
            {
                modificaBottoneIngredienteNegozio(ingredientiBottoniFake[indiceIniziale]);
            }
            indiceIniziale++;
        }
    }

    private int calcolaIndiceInizialeLista(int pagina)
    {
        return pagina * numeroBottoniNellaPagina;
    }

    private void modificaBottoneIngredienteNegozio(Button bottoneIngrediente)
    {
        print(bottoneIngrediente.transform.Find("nome"));
    }
}
