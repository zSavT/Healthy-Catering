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
    [SerializeField] private PannelloMostraRicette pannelloMostraRicette;

    //readonly == final in java
    private readonly int numeroBottoniNellaPagina = 9;
    private readonly int numeroPannelliXElementiNellaPagina = 3;
    private int numeroIngredientiPerPannelloXElementi;
    private Button[] ingredientiBottoniFake;
    private int ultimaPaginaVisualizzata = 0;
    private int ultimaPaginaPossibile;

    private Player giocatore;

    [SerializeField] private GameObject pannelloSeiSicuro;
    private Ingrediente ingredienteAttualmenteSelezionato;
    private int quantitaAttualmenteSelezionata;

    void Start()
    {
        //GESTIONE PANNELLO E RELATIVI
        animazione = GetComponentInParent<Animator>();
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false);
        pannelloXElementi.SetActive(false);

        //INTERAZIONE NEGOZIO
        databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
        ultimaPaginaPossibile = (databaseIngredienti.Count / numeroBottoniNellaPagina);

        copiaTemplateSingoloIngrediente = Instantiate(templateSingoloIngrediente);
        numeroIngredientiPerPannelloXElementi = numeroBottoniNellaPagina / numeroPannelliXElementiNellaPagina;

        bottoneAvantiPannelloNegozio.onClick.AddListener(() => { cambiaPannelloCarosello(true); });
        bottoneIndietroPannelloNegozio.onClick.AddListener(() => { cambiaPannelloCarosello(false); });
        disattivaBottoniAvantiDietroSeServe();

        chiudiPannelloSeiSicuro();
        ingredienteAttualmenteSelezionato = null;
        quantitaAttualmenteSelezionata = 0;
    }

    //INTERAZIONE NEGOZIO
    private void cambiaPannelloCarosello(bool avanti)
    {
        if (avanti)
            ultimaPaginaVisualizzata++;
        else
            ultimaPaginaVisualizzata--;

        disattivaBottoniAvantiDietroSeServe();
        aggiornaBottoniPaginaCarosello();
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

    public void aggiornaBottoniPaginaCarosello()
    {
        if (ingredientiBottoniFake == null)
            ingredientiBottoniFake = creaIstanzeBottoniFakeNeiPannelli();

        aggiornaValoriBottoniFake();
    }

    private Button [] creaIstanzeBottoniFakeNeiPannelli()
    {
        Button[] output = new Button[numeroBottoniNellaPagina];

        int i = 0;
        while (i < numeroPannelliXElementiNellaPagina)
        {
            Button [] temp = inizializzaPannelloXElementiVuoto(i);
            int j = 0;
            while(j < numeroIngredientiPerPannelloXElementi)
            {
                output[(i * numeroIngredientiPerPannelloXElementi) + j] = temp [j];//LE MATRICIIIIIIIIIIIII
                j++;
            }
            i++;
        }

        return output;
    }

    private Button[] inizializzaPannelloXElementiVuoto(int volte)
    {
        Button[] output = new Button[numeroIngredientiPerPannelloXElementi];

        GameObject pannelloXElementiTemp = Instantiate(pannelloXElementi);
        //elimino il bottone template che era presente prima
        Destroy(pannelloXElementiTemp.transform.GetChild(0).gameObject);

        int i = 0;
        while (i < numeroIngredientiPerPannelloXElementi)
        {
            output [i] = Instantiate(copiaTemplateSingoloIngrediente);
            aggiungiBottoneFakeIngredientiAlPannelloXElementi(pannelloXElementiTemp, output[i]);
            i++;
        }
        aggiungiPannelloXElementiAllaSchermata(pannelloXElementiTemp);

        return output;
    }

    private GameObject aggiungiBottoneFakeIngredientiAlPannelloXElementi(GameObject pannelloXElementiTemp, Button singoloIngredienteTemp)
    {
        aggiungiSingoloIngredienteAPanelloXElementi(singoloIngredienteTemp, pannelloXElementiTemp);

        return pannelloXElementiTemp;
    }

    private void aggiungiSingoloIngredienteAPanelloXElementi(Button singoloIngrediente, GameObject pannelloXElementi)
    {
        singoloIngrediente.gameObject.transform.SetParent(pannelloXElementi.transform, false);
    }

    private void aggiungiPannelloXElementiAllaSchermata(GameObject pannelloXElementiTemp)
    {
        pannelloXElementiTemp.gameObject.transform.SetParent(pannelloNegozio.transform, false);
        pannelloXElementiTemp.SetActive(true);
    }

    private void aggiornaValoriBottoniFake()
    {
        //serve perche' se qualche ingrediente e' stato disattivato prima,
        //perche' era gia stato mostrato l'ultimo elemento del database degli ingredienti
        //ora va riattivato
        attivaTuttiIBottoniNelPannelloNegozio();         

        int indicePiattoDaAggiungereNelDatabase;
        int i = 0;
        while (i < numeroBottoniNellaPagina)
        {
            indicePiattoDaAggiungereNelDatabase = trovaIndicePiattoDaInserire(i);

            //-1 quando è stato aggiunto anche l'ultimo piatto del database
            if (indicePiattoDaAggiungereNelDatabase != -1)
            {
                ingredientiBottoniFake[i] = popolaSingoloIngrediente(
                    ingredientiBottoniFake[i], 
                    databaseIngredienti[indicePiattoDaAggiungereNelDatabase]
                );
            }
            else
            {
                disattivaIBottoniSuccessivi(i);//i e' l'indice del bottone dal quale bisogna disattivare
                return;
            }
            i++;
        }
    }

    private void attivaTuttiIBottoniNelPannelloNegozio()
    {
        foreach (Button temp in ingredientiBottoniFake)
        {
            if (!temp.IsActive())
            {
                temp.gameObject.SetActive(true);
            }
        }
    }

    private int trovaIndicePiattoDaInserire(int numeroIngredientiInseritiFinoAdOra)
    {
        int indice = (ultimaPaginaVisualizzata * numeroBottoniNellaPagina) + numeroIngredientiInseritiFinoAdOra;

        if (indice != databaseIngredienti.Count)
            return indice;

        return -1;
    }

    private Button popolaSingoloIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        singoloIngredienteTemp = modificaTesto(singoloIngredienteTemp, ingrediente.nome, ingrediente.costo.ToString());

        singoloIngredienteTemp = aggiungiGestioneBottoniQuantita(singoloIngredienteTemp, ingrediente.costo);

        singoloIngredienteTemp = aggiungiListenerBottoniQuantita(singoloIngredienteTemp);

        singoloIngredienteTemp = aggiungiListenerCompraIngrediente(singoloIngredienteTemp, ingrediente);

        singoloIngredienteTemp = modificaImmagineIngrediente(singoloIngredienteTemp, ingrediente);

        return singoloIngredienteTemp;
    }

    private Button modificaTesto(Button singoloIngredienteTemp, string nomeIngrediente, string costoIngrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = nomeIngrediente;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Costo: " + costoIngrediente;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = 0.ToString();
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
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => {
            cambiaQuantitaAcquistare(false, singoloIngredienteTemp);
        });

        //bottone aumenta quantita
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            cambiaQuantitaAcquistare(true, singoloIngredienteTemp);
        });

        return singoloIngredienteTemp;
    }

    private void cambiaQuantitaAcquistare(bool diPiu, Button singoloIngredienteTemp)
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

    private Button aggiungiListenerCompraIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        //bottone mostra compra
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[2].onClick.AddListener(() =>
        {
            ingredienteAttualmenteSelezionato = ingrediente;
            quantitaAttualmenteSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            apriPannelloSeiSicuro();
        });

        return singoloIngredienteTemp;
    }

    public void apriPannelloSeiSicuro()
    {
        pannelloSeiSicuro.SetActive(true);
    }

    private Button modificaImmagineIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<Image>()[6].name = "immagine ingrediente " + ingrediente.nome;

        Sprite nuovaImmagine = Resources.Load<Sprite>(ingrediente.nome);
        if (nuovaImmagine == null)
        {
            nuovaImmagine = Resources.Load<Sprite>("ImmagineIngredienteDefault");
        }
        singoloIngredienteTemp.GetComponentsInChildren<Image>()[6].sprite = nuovaImmagine;

        return singoloIngredienteTemp;
    }

    private void disattivaIBottoniSuccessivi(int indicePrimoIngredienteDaDisattivare)
    {
        while (indicePrimoIngredienteDaDisattivare < numeroBottoniNellaPagina)
        {
            ingredientiBottoniFake[indicePrimoIngredienteDaDisattivare].gameObject.SetActive(false);
            indicePrimoIngredienteDaDisattivare++;
        }
    }

    //METODI DEI BOTTONI DEL PANNELLO SEI SICURO
    public void compraIngrediente()
    {
        if ((ingredienteAttualmenteSelezionato != null) && (quantitaAttualmenteSelezionata != 0))
        {
            float prezzoDaPagare = ingredienteAttualmenteSelezionato.costo * quantitaAttualmenteSelezionata;
            giocatore.guadagna(-prezzoDaPagare);

            giocatore.aggiornaInventario(ingredienteAttualmenteSelezionato, quantitaAttualmenteSelezionata);
        }
        chiudiPannelloSeiSicuro();
    }

    public void annullaCompraIngrediente()
    {
        ingredienteAttualmenteSelezionato = null;
        quantitaAttualmenteSelezionata = 0;
        chiudiPannelloSeiSicuro();
    }

    private void chiudiPannelloSeiSicuro()
    {
        pannelloSeiSicuro.SetActive(false);
    }

    //GESTIONE PANNELLO E RELATIVI
    public void apriPannelloNegozio(Player giocatorePassato)
    {
        giocatore = giocatorePassato;
        animazioneNPCParlante();
        pannelloAperto = true;
        canvasPannelloNegozio.SetActive(true);
        aggiornaBottoniPaginaCarosello();
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        chiudiPannelloSeiSicuro();
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
