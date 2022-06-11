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

    private List<Ingrediente> databaseIngredienti;
    private List<Piatto> databasePiatti;
    [SerializeField] private PannelloMostraRicette pannelloMostraRicette;

    //readonly == final in java
    private readonly int numeroBottoniNellaPagina = 9;
    private readonly int numeroPannelliXElementiNellaPagina = 3;
    private Button[] ingredientiBottoniFake;
    private int ultimaPaginaVisualizzata = 0;
    private int ultimaPaginaPossibile;

    private Player giocatore;

    // Start is called before the first frame update
    void Start()
    {
        //TODO sotstituire con l'istanza del giocatore attivo
        giocatore = new Player();

        //GESTIONE PANNELLO E RELATIVI
        animazione = GetComponentInParent<Animator>();
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false);
        pannelloXElementi.SetActive(false);

        //INTERAZIONE NEGOZIO
        databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
        databasePiatti = Database.getDatabaseOggetto(new Piatto());
        ultimaPaginaPossibile = (databaseIngredienti.Count / numeroBottoniNellaPagina) + 1;

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
                    pannelloXElementiTemp = aggiungiBottoneFakeIngredientiAlPannelloXElementi(pannelloXElementiTemp, indicePiattoDaAggiungereNelDatabase);
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

        if (indice != databaseIngredienti.Count)
            return indice;

        return -1;
    }

    private GameObject aggiungiBottoneFakeIngredientiAlPannelloXElementi(GameObject pannelloXElementiTemp, int indicePiattoDaAggiungereNelDatabase)
    {
        Button singoloIngredienteTemp = Instantiate(copiaTemplateSingoloIngrediente);

        singoloIngredienteTemp = popolaSingoloIngrediente(singoloIngredienteTemp, databaseIngredienti[indicePiattoDaAggiungereNelDatabase]);

        aggiungiSingoloIngredienteAPanelloXElementi(singoloIngredienteTemp, pannelloXElementiTemp);

        return pannelloXElementiTemp;
    }

    private Button popolaSingoloIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        singoloIngredienteTemp = modificaTesto(singoloIngredienteTemp, ingrediente.nome, ingrediente.costo.ToString());

        singoloIngredienteTemp = aggiungiGestioneBottoniQuantita(singoloIngredienteTemp, ingrediente.costo);

        singoloIngredienteTemp = aggiungiListenerBottoniQuantita(singoloIngredienteTemp);

        singoloIngredienteTemp = aggiungiListenerBottoneMostraIngredienti(singoloIngredienteTemp, ingrediente);

        singoloIngredienteTemp = aggiungiListenerCompraIngrediente(singoloIngredienteTemp, ingrediente);
            
        return singoloIngredienteTemp;
    }

    private Button modificaTesto(Button singoloIngredienteTemp, string nomeIngrediente, string costoIngrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = nomeIngrediente;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[1].text = costoIngrediente;

        return singoloIngredienteTemp;
    }


    private Button aggiungiGestioneBottoniQuantita(Button singoloIngredienteTemp, float costoIngrediente)
    {
        string quantitaSelezionata = singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text;
        
        //bottone diminuisci quantita
        if (quantitaSelezionata.Equals("0"))
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[0].interactable = false;
        else
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[0].interactable = true;

        //bottone aumenta quantita
            //se il resto della divisione fra i soldi del giocatore e il costo
            //della merce che vuole comprare è minore del costo dell'ingrediente
            //se ne aggiunge 1 non può più comprarlo
            //quindi ha raggiunto il massimo
        if (giocatore.soldi % (costoIngrediente * System.Int32.Parse(quantitaSelezionata)) < costoIngrediente)
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].interactable = false;
        else
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].interactable = true;
        

        return singoloIngredienteTemp;
    }

    private Button aggiungiListenerBottoniQuantita(Button singoloIngredienteTemp)
    {
        //bottone diminuisci quantita    
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[0].onClick.AddListener(()=> {
            cambiaQuantitaAcquistare(false, singoloIngredienteTemp);
        });

        //bottone aumenta quantita
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            cambiaQuantitaAcquistare(true, singoloIngredienteTemp);
        });

        return singoloIngredienteTemp;
    }

    private void cambiaQuantitaAcquistare (bool diPiu, Button singoloIngredienteTemp)
    {
        //testo quantita
        int quantitaPrecedente = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
        
        if (diPiu)
        {
            singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaPrecedente + 1).ToString();
        }
        else // controllo per andare sotto lo 0 sul bottone che chiama il metodo (diventa non interagibile se si è a 0)
        {
            singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaPrecedente - 1).ToString();
        }
    }

    private Button aggiungiListenerBottoneMostraIngredienti (Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        //bottone mostra ingredienti
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[2].onClick.AddListener(() =>
        {
            pannelloMostraRicette.apriPannelloMostraRicette(ingrediente, databaseIngredienti, databasePiatti);
        });

        return singoloIngredienteTemp;
    }

    private Button aggiungiListenerCompraIngrediente (Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        //bottone mostra compra
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[3].onClick.AddListener(() =>
        {
            compraIngrediente(ingrediente, System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text));
        });
        
        return singoloIngredienteTemp;
    }

    private void compraIngrediente(Ingrediente ingrediente, int quantitaDaComprare)
    {
        float prezzoDaPagare = ingrediente.costo * quantitaDaComprare;
        giocatore.guadagna(-prezzoDaPagare);

        giocatore.aggiornaInventario(ingrediente, quantitaDaComprare);
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
    public void apriPannelloNegozio(Player giocatorePassato)
    {
        giocatore = giocatorePassato;
        animazioneNPCParlante();
        pannelloAperto = true;
        canvasPannelloNegozio.SetActive(true);
        interazioneNegozio();
        pannelloMostraRicette.chiudiPannelloMostraRicette();
    }

    public void chiudiPannelloNegozio()
    {
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false);
        animazioneNPCIdle(); 
        pannelloMostraRicette.chiudiPannelloMostraRicette();
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
