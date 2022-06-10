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
    [SerializeField] private GameObject pannelloXElementi;
    [SerializeField] private Button templateSingoloIngrediente;
    private Button copiaTemplateSingoloIngrediente;

    [SerializeField] private Button bottoneAvantiPannelloNegozio;
    [SerializeField] private Button bottoneIndietroPannelloNegozio;

    private List<Ingrediente> tuttiGliIngredienti;

    //readonly == final in java
    private readonly int numeroBottoniNellaPagina = 9;
    private readonly int numeroPannelliXElementiNellaPagina = 3;
    private Button[] ingredientiBottoniFake;
    private int ultimaPaginaVisualizzata = 0;
    private int ultimaPaginaPossibile;

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
        ultimaPaginaPossibile = (tuttiGliIngredienti.Count / numeroBottoniNellaPagina) + 1;

        copiaTemplateSingoloIngrediente = Instantiate(templateSingoloIngrediente);
        Destroy(templateSingoloIngrediente);

        ingredientiBottoniFake = new Button[numeroBottoniNellaPagina];
        int i = 0;
        while (i < numeroBottoniNellaPagina)
        {
            ingredientiBottoniFake[i] = copiaTemplateSingoloIngrediente;
            i++;
        }

        bottoneAvantiPannelloNegozio.onClick.AddListener(()=>{cambiaPannelloCarosello(true);});
        bottoneIndietroPannelloNegozio.onClick.AddListener(()=>{cambiaPannelloCarosello(false);});
    }
    
    //INTERAZIONE NEGOZIO
    private void cambiaPannelloCarosello(bool avanti)
    {
        if (avanti)
            ultimaPaginaVisualizzata++;
        else
            ultimaPaginaVisualizzata--;

        disattivaBottoniAvantiDietroSeServe();
        interazioneNegozio();
    }

    private void disattivaBottoniAvantiDietroSeServe()
    {
        if (ultimaPaginaVisualizzata == ultimaPaginaPossibile)
        {
            bottoneAvantiPannelloNegozio.interactable = false;
        }
        else
        {
            bottoneAvantiPannelloNegozio.interactable = true;
        }

        if (ultimaPaginaVisualizzata == 0)
        {
            bottoneIndietroPannelloNegozio.interactable = false;
        }
        else
        {
            bottoneIndietroPannelloNegozio.interactable = true;
        }
    }

    public void interazioneNegozio()
    {
        eliminaElementiPrecedentiSePresenti();
        fillNuoviIngredientiBottoniFake();
    }

    private void eliminaElementiPrecedentiSePresenti()
    {
        GameObject[] pannelliXElementiPresenti = new GameObject[numeroPannelliXElementiNellaPagina];
        int numeroPannelliXElementiAggiunti = 0;
       
        foreach (GameObject gameobject in pannelloNegozio.GetComponentsInChildren<GameObject>())
        {
            if (gameobject.name.ToLower().Contains("pannelloxelementi"))
            {
                pannelliXElementiPresenti[numeroPannelliXElementiAggiunti] = gameobject;
            }
        }

        foreach (GameObject pannelloXElementiTemp in pannelliXElementiPresenti)
        {
            foreach (Button bottoneIngrediente in pannelloXElementiTemp.GetComponentsInChildren<Button>())
            {
                Destroy(bottoneIngrediente);
            }
            Destroy(pannelloXElementiTemp);
        }
    }
    
    private void fillNuoviIngredientiBottoniFake()
    {
        int numeroBottoniFakeIngredientiInseriti = 0;
        GameObject pannelloXElementiTemp = Instantiate(pannelloXElementi);

        foreach (Button bottoneFakeIngrediente in ingredientiBottoniFake)
        {
            int indicePiattoDaAggiungereNelDatabase = trovaIndicePiattoDaInserire(numeroBottoniFakeIngredientiInseriti);

            if (indicePiattoDaAggiungereNelDatabase != -1)
            {
                if (numeroBottoniFakeIngredientiInseriti == (numeroBottoniNellaPagina/numeroPannelliXElementiNellaPagina))
                {
                    pannelloXElementiTemp = aggiungiBottoneFakeIngredientiAlPannelloXElementi(pannelloXElementiTemp, numeroBottoniFakeIngredientiInseriti);
                    numeroBottoniFakeIngredientiInseriti++;
                }
                else
                {
                    aggiungiPannelloXElementiAllaSchermata(pannelloXElementiTemp);
                    pannelloXElementiTemp = Instantiate(pannelloXElementi);
                }
            }
            else
            {
                return;
            }
        }
    }

    private int trovaIndicePiattoDaInserire(int numeroIngredientiInseritiFinoAdOra)
    {
        int indice = (ultimaPaginaVisualizzata * numeroBottoniNellaPagina) - numeroIngredientiInseritiFinoAdOra + 1;

        if (indice != tuttiGliIngredienti.Count)
            return indice;

        return -1;
    }

    private GameObject aggiungiBottoneFakeIngredientiAlPannelloXElementi(GameObject pannelloXElementiTemp, int numeroIngredientiInseritiFinoAdOra)
    {
        Button singoloIngredienteTemp = Instantiate(copiaTemplateSingoloIngrediente);

        singoloIngredienteTemp = popolaSingoloIngrediente(singoloIngredienteTemp, tuttiGliIngredienti[indicePiattoDaAggiungereNelDatabase]);

        aggiungiSingoloIngredienteAPanelloXElementi(singoloIngredienteTemp, pannelloXElementiTemp);

        return pannelloXElementiTemp;
    }

    private Button popolaSingoloIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ingrediente.nome;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ingrediente.costo.ToString();


        return singoloIngredienteTemp;
    }



    private void aggiungiSingoloIngredienteAPanelloXElementi (Button singoloIngrediente, GameObject pannelloXElementi)
    {
        singoloIngrediente.transform.SetParent(pannelloXElementi.transform, false);
    }

    private void aggiungiPannelloXElementiAllaSchermata(GameObject pannelloXElementiTemp)
    {
        pannelloXElementi.transform.SetParent(pannelloNegozio.transform, false);
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
