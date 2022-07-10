using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/*
    bottoni nell'array bottoneFakeIngrediente.GetComponentsInChildren <Button> ():
    0 = bottone generale, l'ingrediente fake
    1 = aumenta quantita
    2 = diminuisci quantita
    3 = compra ingrediente
*/

public class PannelloNegozio : MonoBehaviour
{
    [SerializeField] private GameObject canvasPannelloNegozio;
    private bool pannelloAperto = false;
    private Animator animazione;


    [Header("Interazione Negozio")]
    [SerializeField] private GameObject pannelloNegozio;
    [SerializeField] private GameObject pannelloXElementi;
    [SerializeField] private Button templateSingoloIngrediente;
    private Button copiaTemplateSingoloIngrediente;

    [SerializeField] private Button bottoneAvantiPannelloNegozio;
    [SerializeField] private Button bottoneIndietroPannelloNegozio;

    private List<Ingrediente> databaseIngredienti;
    [SerializeField] private Gui guiInGame;

    //readonly == final in java
    private readonly int numeroBottoniNellaPagina = 9;
    private readonly int numeroPannelliXElementiNellaPagina = 3;
    private int numeroIngredientiPerPannelloXElementi;
    private Button[] ingredientiBottoniFake;
    private int ultimaPaginaVisualizzata = 0;
    private int ultimaPaginaPossibile;

    private Player giocatore;
    [SerializeField] private TextMeshProUGUI soldiGiocatore;

    [SerializeField] private GameObject pannelloSeiSicuro;
    [SerializeField] private TextMeshProUGUI testoPannelloSeiSicuro;
    private bool pannelloConfermaAperto = false;
    private Ingrediente ingredienteAttualmenteSelezionato;
    private int quantitaAttualmenteSelezionata;
    public static bool compratoIngredientePerTutorial = false;
    [SerializeField] TextMeshProUGUI testoEsc;

    private List<Ingrediente> carrello = new List<Ingrediente>();
    private float prezzoDaPagare;
    [SerializeField] TextMeshProUGUI testoTotaleCarello;
    private bool inNegozio = false;

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

    public void aggiornaBottoniPaginaCarosello()//#
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
        singoloIngredienteTemp = rimuoviTuttiIVecchiListener(singoloIngredienteTemp);

        singoloIngredienteTemp = modificaTesto(singoloIngredienteTemp, ingrediente.nome, ingrediente.costo.ToString());

        singoloIngredienteTemp = aggiungiGestioneBottoniQuantita(singoloIngredienteTemp, ingrediente.costo);//#

        singoloIngredienteTemp = aggiungiListenerCompraIngrediente(singoloIngredienteTemp, ingrediente);//#

        attivaDisattivaBottoneCompra(singoloIngredienteTemp, 0);

        singoloIngredienteTemp = modificaImmagineIngrediente(singoloIngredienteTemp, ingrediente);

        return singoloIngredienteTemp;
    }

    private Button rimuoviTuttiIVecchiListener(Button singoloIngredienteTemp)
    {
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[2].onClick.RemoveAllListeners();
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[3].onClick.RemoveAllListeners();

        return singoloIngredienteTemp;
    }

    private Button modificaTesto(Button singoloIngredienteTemp, string nomeIngrediente, string costoIngrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Utility.coloreIngredienti + nomeIngrediente + Utility.fineColore;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Costo: " + costoIngrediente;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = 0.ToString();
        return singoloIngredienteTemp;
    }

    private Button aggiungiGestioneBottoniQuantita(Button singoloIngredienteTemp, float costoIngrediente)
    {
        //bottone diminuisci quantita 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[2].onClick.AddListener(() => {
            int quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            
            if (quantitaSelezionata == 0)
                singoloIngredienteTemp.GetComponentsInChildren<Button>()[2].interactable = false;
            else
                singoloIngredienteTemp.GetComponentsInChildren<Button>()[2].interactable = true;
            
            if (quantitaSelezionata > 0)
            {
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaSelezionata - 1).ToString();
            }

            quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            attivaDisattivaBottoneCompra(singoloIngredienteTemp, quantitaSelezionata);
        });

        //bottone aumenta quantita 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            int quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);

            //se il resto della divisione fra i soldi del giocatore e il costo
            //della merce che vuole comprare è minore del costo dell'ingrediente
            //se ne aggiunge 1 non può più comprarlo
            //quindi ha raggiunto il massimo
            if (giocatore.soldi - prezzoDaPagare - (costoIngrediente * (quantitaSelezionata)) < 0)
                singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].interactable = false;
            else
                singoloIngredienteTemp.GetComponentsInChildren<Button>()[1].interactable = true;


            if (giocatore.soldi - prezzoDaPagare - (costoIngrediente * (quantitaSelezionata + 1)) >= 0)
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaSelezionata + 1).ToString();

            quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            attivaDisattivaBottoneCompra(singoloIngredienteTemp, quantitaSelezionata);
        });

        return singoloIngredienteTemp;
    }

    private Button aggiungiListenerCompraIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        //bottone mostra compra 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[3].onClick.AddListener(() =>
        {
            ingredienteAttualmenteSelezionato = ingrediente;
            quantitaAttualmenteSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            apriPannelloSeiSicuro();
        });

        return singoloIngredienteTemp;
    }

    private void attivaDisattivaBottoneCompra(Button singoloIngredienteTemp, int quantita)
    {
        if (quantita > 0)
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[3].interactable = true;
        else
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[3].interactable = false;
    }

    public void apriPannelloSeiSicuro()
    {
        print("in negozio: " + inNegozio.ToString());
        if (inNegozio)
        {
            testoPannelloSeiSicuro.text = "Sei sicuro di voler aggiungere al carrello x" + quantitaAttualmenteSelezionata.ToString() + "\n" + Utility.coloreIngredienti + ingredienteAttualmenteSelezionato.nome + Utility.fineColore;
        }
        else
        {
            testoPannelloSeiSicuro.text = creaStringaPannelloSeiSicuroCarrello();
        }
        pannelloSeiSicuro.SetActive(true);
        testoEsc.gameObject.SetActive(false);
        pannelloConfermaAperto = true;
    }

    private string creaStringaPannelloSeiSicuroCarrello()
    {
        string output = "Sei sicuro di voler comprare la seguente lista di ingredienti:\n";

        List<OggettoQuantita<int>> carrelloOggettoQuantita = trasformaCarrelloInOggettoQuantita();

        foreach (OggettoQuantita<int> temp in carrelloOggettoQuantita)
        {
            output += Ingrediente.idToIngrediente(temp.oggetto).nome + " x" + temp.quantita.ToString() + "\n";
        }

        output += "?";
        return output;
    }

    private List<OggettoQuantita<int>> trasformaCarrelloInOggettoQuantita()
    {
        List<OggettoQuantita<int>> output = new List<OggettoQuantita<int>>();
        List<int> oggetti = new List<int>();
        List<int> quantita = new List<int>();

        foreach (Ingrediente temp in carrello)
        {
            int posizioneTemp = oggetti.IndexOf(temp.idIngrediente);//returna -1 se non trova l'oggetto
            if (posizioneTemp != -1)
            {
                quantita[posizioneTemp] = quantita[posizioneTemp] + 1;
            }
            else
            {
                oggetti.Add(temp.idIngrediente);
                quantita.Add(1);
            }
        }

        int i = 0;
        while (i < oggetti.Count)
        {
            output.Add(new OggettoQuantita<int>(oggetti[i], quantita[i]));
            i++; 
        }

        return output;
    }

    private Button modificaImmagineIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<Image>()[6].name = "immagine ingrediente " + ingrediente.nome;

        Sprite nuovaImmagine = Resources.Load<Sprite>("immaginiIngredienti/" + ingrediente.nome);
        if (nuovaImmagine == null)
        {
            nuovaImmagine = Resources.Load<Sprite>("immaginiIngredienti/ImmagineIngredienteDefault");
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
    public void aggiungiIngredienteACarrello()
    {
        if (inNegozio)
        {

            if ((ingredienteAttualmenteSelezionato != null) && (quantitaAttualmenteSelezionata > 0))
            {
                prezzoDaPagare += (ingredienteAttualmenteSelezionato.costo * quantitaAttualmenteSelezionata);

                int i = 0;
                while (i < quantitaAttualmenteSelezionata)
                {
                    carrello.Add(ingredienteAttualmenteSelezionato);
                    i++;
                }
            
                resetQuantitaTuttiBottoni();
                quantitaAttualmenteSelezionata = 0;


                testoTotaleCarello.text = "Costo totale del carrello:\n" + prezzoDaPagare.ToString();
            }

            chiudiPannelloSeiSicuro();
        }
    }

    public void compraIngredientiNelCarrello()
    {
        if (!inNegozio)
        {
            giocatore.guadagna(-prezzoDaPagare);
            guiInGame.aggiornaValoreSoldi(giocatore.soldi);

            foreach (Ingrediente temp in carrello)
            {
                giocatore.aggiornaInventario(new OggettoQuantita<int>(temp.idIngrediente, 1), true);//visto che aggiungo un elemento alla volta la quantita da aggiungere ora è 1
            }

            resetQuantitaTuttiBottoni();
            quantitaAttualmenteSelezionata = 0;
            compratoIngredientePerTutorial = true;
            soldiGiocatore.text = Utility.coloreVerde + "Denaro: " + Utility.fineColore + giocatore.soldi.ToString("0.00");

            resetSituazioneCarello();

            inNegozio = true;

            chiudiPannelloSeiSicuro();
        }
    }

    public void setInNegozioToInCarrello()
    {
        inNegozio = false;
    }

    public void resetQuantitaTuttiBottoni()
    {
        foreach (Button ingrediente in ingredientiBottoniFake)
        {
            ingrediente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = 0.ToString();
        }
    }

    public void annullaCompraIngrediente()
    {
        ingredienteAttualmenteSelezionato = null;
        quantitaAttualmenteSelezionata = 0;
        chiudiPannelloSeiSicuro();
    }

    private void chiudiPannelloSeiSicuro()
    {
        testoPannelloSeiSicuro.text = "You werent supposed to be able to get here you know";
        pannelloSeiSicuro.SetActive(false);

        if (ingredientiBottoniFake != null)
            foreach (Button ingrediente in ingredientiBottoniFake)
            {
                attivaDisattivaBottoneCompra(ingrediente, 0);
            }
        pannelloConfermaAperto = false;

        if (!inNegozio)
        {
            inNegozio = true;
        }
    }

    //GESTIONE PANNELLO E RELATIVI
    public void apriPannelloNegozio(Player giocatorePassato)
    {
        giocatore = giocatorePassato;
        animazioneNPCParlante();
        pannelloAperto = true;
        canvasPannelloNegozio.SetActive(true);
        aggiornaBottoniPaginaCarosello();
        chiudiPannelloSeiSicuro();
        soldiGiocatore.text = Utility.coloreVerde + "Denaro: " + Utility.fineColore + giocatore.soldi.ToString("0.00");

        resetSituazioneCarello();
    }

    private void resetSituazioneCarello()
    {
        //reset delle cose nel carrello
        prezzoDaPagare = 0;
        carrello = new List<Ingrediente>();
        testoTotaleCarello.text = "Totale del carrello:\n" + 0.ToString();
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

    public bool getPannelloConfermaAperto()
    {
        return pannelloConfermaAperto;
    }

    public void animazioneNPCInquadrato()
    {
        animazione.SetBool("inquadrato", true);
    }

    public void animazioneNPCIdle()
    {
        animazione.SetBool("parlante", false);
        animazione.SetBool("inquadrato", false);
    }

    private void animazioneNPCParlante()
    {
        animazione.SetBool("parlante", true);
    }
}
