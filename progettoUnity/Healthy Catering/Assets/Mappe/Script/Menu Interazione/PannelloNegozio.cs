using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PannelloNegozio : MonoBehaviour
{
    private Animator animazione;

    [Header("Interazione Negozio")]
    [SerializeField] private GameObject canvasPannelloNegozio;
    [SerializeField] private GameObject pannelloNegozio;
    [SerializeField] private GameObject pannelloXElementi;
    [SerializeField] private Button templateSingoloIngrediente;
    [SerializeField] private Button bottoneCarrello;
    private Button copiaTemplateSingoloIngrediente;
    private bool pannelloAperto = false;

    [SerializeField] private Button bottoneAvantiPannelloNegozio;
    [SerializeField] private Button bottoneIndietroPannelloNegozio;
    
    [SerializeField] TextMeshProUGUI testoEsc;

    [SerializeField] private Gui guiInGame;

    private int numeroIngredientiPerPannelloXElementi;
    private Button[] ingredientiBottoniFake;
    private int ultimaPaginaVisualizzata = 0;
    private int ultimaPaginaPossibile;
    private bool inNegozio = false;

    [Header("Player")]
    private Player giocatore;
    [SerializeField] private TextMeshProUGUI soldiGiocatore;

    [Header("Conferma")]
    [SerializeField] private GameObject pannelloSeiSicuro;
    [SerializeField] private TextMeshProUGUI testoPannelloSeiSicuro;
    private bool pannelloSeiSicuroAperto = false;

    [Header ("Gestione carrello")]
    private Ingrediente ingredienteAttualmenteSelezionato;
    private int quantitaAttualmenteSelezionata;
    private List<Ingrediente> carrello = new List<Ingrediente>();
    private float prezzoDaPagare;
    [SerializeField] TextMeshProUGUI testoTotaleCarello;

    [Header ("tutorial")]
    public static bool compratoIngredientePerTutorial = false;


    void Start()
    {
        animazione = GetComponentInParent<Animator>();
        bottoneCarrello.interactable = false;
        //GESTIONE PANNELLO E FIGLI
        canvasPannelloNegozio.SetActive(false);
        pannelloXElementi.SetActive(false);
        pannelloAperto = false;

        //INTERAZIONE NEGOZIO
        ultimaPaginaPossibile = (Costanti.databaseIngredienti.Count / Costanti.numeroBottoniNellaPaginaNegozio);
        numeroIngredientiPerPannelloXElementi = Costanti.numeroBottoniNellaPaginaNegozio / Costanti.numeroPannelliXElementiNellaPaginaNegozio;

        copiaTemplateSingoloIngrediente = Instantiate(templateSingoloIngrediente);

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
        Button[] output = new Button[Costanti.numeroBottoniNellaPaginaNegozio];

        int i = 0;
        while (i < Costanti.numeroPannelliXElementiNellaPaginaNegozio)
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

    private void aggiungiBottoneFakeIngredientiAlPannelloXElementi(GameObject pannelloXElementiTemp, Button singoloIngredienteTemp)
    {
        aggiungiSingoloIngredienteAPanelloXElementi(singoloIngredienteTemp, pannelloXElementiTemp);
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
        while (i < Costanti.numeroBottoniNellaPaginaNegozio)
        {
            indicePiattoDaAggiungereNelDatabase = trovaIndicePiattoDaInserire(i);

            //-1 quando e' stato aggiunto anche l'ultimo piatto del database
            if (indicePiattoDaAggiungereNelDatabase != -1)
            {
                ingredientiBottoniFake[i] = popolaSingoloIngrediente(
                    ingredientiBottoniFake[i], 
                    Costanti.databaseIngredienti[indicePiattoDaAggiungereNelDatabase]
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
        int indice = (ultimaPaginaVisualizzata * Costanti.numeroBottoniNellaPaginaNegozio) + numeroIngredientiInseritiFinoAdOra;

        if (indice != Costanti.databaseIngredienti.Count)
            return indice;

        return -1;
    }

    private Button popolaSingoloIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        rimuoviTuttiIVecchiListener(singoloIngredienteTemp);

        modificaTesto(singoloIngredienteTemp, ingrediente.nome, ingrediente.costo.ToString());

        aggiungiGestioneBottoniQuantita(singoloIngredienteTemp, ingrediente.costo);//#

        aggiungiListenerCompraIngrediente(singoloIngredienteTemp, ingrediente);//#

        attivaDisattivaBottoneCompra(singoloIngredienteTemp, 0);

        modificaImmagineIngrediente(singoloIngredienteTemp, ingrediente);

        return singoloIngredienteTemp;
    }

    private void rimuoviTuttiIVecchiListener(Button singoloIngredienteTemp)
    {
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAumentaQuantita].onClick.RemoveAllListeners();
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneDiminuisciQuantita].onClick.RemoveAllListeners();
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAggiungiIngredienteAlCarrello].onClick.RemoveAllListeners();
    }

    private void modificaTesto(Button singoloIngredienteTemp, string nomeIngrediente, string costoIngrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Costanti.coloreIngredienti + nomeIngrediente + Costanti.fineColore;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Costo: " + costoIngrediente;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = "0";
    }

    private void aggiungiGestioneBottoniQuantita(Button singoloIngredienteTemp, float costoIngrediente)
    {
        aggiungiGestioneBottoneQuantitaDiminuisci(singoloIngredienteTemp);

        aggiungiGestioneBottoneQuantitaAumenta(singoloIngredienteTemp, costoIngrediente);
    }

    private void aggiungiGestioneBottoneQuantitaDiminuisci(Button singoloIngredienteTemp)
    {
        //bottone diminuisci quantita 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneDiminuisciQuantita].onClick.AddListener(() => {
            int quantitaSelezionata = System.Int32.Parse(
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text
            );

            attivaDisattivaBottoniPiuMenoSeServe(singoloIngredienteTemp, quantitaSelezionata, 0);

            if (quantitaSelezionata > 0)
            {
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaSelezionata - 1).ToString();
            }

            quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            attivaDisattivaBottoneCompra(singoloIngredienteTemp, quantitaSelezionata);
        });
    }

    private void aggiungiGestioneBottoneQuantitaAumenta(Button singoloIngredienteTemp, float costoIngrediente)
    {
        //bottone aumenta quantita 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAumentaQuantita].onClick.AddListener(() => {
            int quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);

            attivaDisattivaBottoniPiuMenoSeServe(singoloIngredienteTemp, quantitaSelezionata, costoIngrediente);

            if (giocatore.soldi - prezzoDaPagare - (costoIngrediente * (quantitaSelezionata + 1)) >= 0)
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaSelezionata + 1).ToString();

            quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            attivaDisattivaBottoneCompra(singoloIngredienteTemp, quantitaSelezionata);
        });
    }

    private void attivaDisattivaBottoniPiuMenoSeServe(Button singoloIngredienteTemp, int quantitaSelezionata, float costoIngrediente)
    {
        if (quantitaSelezionata == 0)
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[
                Costanti.posizioneBottoneDiminuisciQuantita
            ].interactable = false;
        else
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[
                Costanti.posizioneBottoneDiminuisciQuantita
        ].interactable = true;

        //se il resto della divisione fra i soldi del giocatore e il costo
        //della merce che vuole comprare e' minore del costo dell'ingrediente
        //se ne aggiunge 1 non puo' piu' comprarlo
        //quindi ha raggiunto il massimo
        if (giocatore.soldi - prezzoDaPagare - (costoIngrediente * (quantitaSelezionata)) < 0)
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAumentaQuantita].interactable = false;
        else
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAumentaQuantita].interactable = true;
    }

    private void aggiungiListenerCompraIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        //bottone mostra compra 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAggiungiIngredienteAlCarrello].onClick.AddListener(() =>
        {
            ingredienteAttualmenteSelezionato = ingrediente;
            quantitaAttualmenteSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            apriPannelloSeiSicuro();
        });
    }

    private void attivaDisattivaBottoneCompra(Button singoloIngredienteTemp, int quantita)
    {
        if (quantita > 0)
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAggiungiIngredienteAlCarrello].interactable = true;
        else
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAggiungiIngredienteAlCarrello].interactable = false;
    }

    private void modificaImmagineIngrediente(Button singoloIngredienteTemp, Ingrediente ingrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<Image>()[6].name = "immagine ingrediente " + ingrediente.nome;

        Sprite nuovaImmagine = Resources.Load<Sprite>("immaginiIngredienti/" + ingrediente.nome);
        if (nuovaImmagine == null)
        {
            nuovaImmagine = Resources.Load<Sprite>("immaginiIngredienti/ImmagineIngredienteDefault");
        }
        singoloIngredienteTemp.GetComponentsInChildren<Image>()[6].sprite = nuovaImmagine;
    }

    public void apriPannelloSeiSicuro()
    {
        if (inNegozio)
        {
            testoPannelloSeiSicuro.text = "Sei sicuro di voler aggiungere al carrello " + Costanti.coloreIngredienti + ingredienteAttualmenteSelezionato.nome + Costanti.fineColore + " x" + quantitaAttualmenteSelezionata.ToString();
        }
        else
        {
            testoPannelloSeiSicuro.text = creaStringaPannelloSeiSicuroCarrello();
        }
        pannelloSeiSicuro.SetActive(true);
        testoEsc.gameObject.SetActive(false);
        PlayerSettings.addattamentoSpriteComandi(testoEsc);
        pannelloSeiSicuroAperto = true;
    }

    private string creaStringaPannelloSeiSicuroCarrello()
    {
        string output = "Sei sicuro di voler comprare i seguenti ingredienti:\n";

        List<OggettoQuantita<int>> carrelloOggettoQuantita = trasformaCarrelloInOggettoQuantita();

        foreach (OggettoQuantita<int> temp in carrelloOggettoQuantita)
        {
            output += Costanti.coloreIngredienti +  Ingrediente.idToIngrediente(temp.oggetto).nome + Costanti.fineColore + " x" + temp.quantita.ToString() + "\n";
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

    private void disattivaIBottoniSuccessivi(int indicePrimoIngredienteDaDisattivare)
    {
        while (indicePrimoIngredienteDaDisattivare < Costanti.numeroBottoniNellaPaginaNegozio)
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

                testoTotaleCarello.text = Costanti.coloreVerde + "Totale Carrello: " + Costanti.fineColore + prezzoDaPagare.ToString("0.00");
            }
            chiudiPannelloSeiSicuro();
            if (carrello.Count > 0)
                bottoneCarrello.interactable = true;
            else
                bottoneCarrello.interactable = false;
        }
    }

    public void compraIngredientiNelCarrello()
    {
        if (!inNegozio)
        {
            giocatore.paga(prezzoDaPagare);
            guiInGame.aggiornaValoreSoldi(giocatore.soldi);

            foreach (Ingrediente temp in carrello)
            {
                giocatore.aggiornaInventario(new OggettoQuantita<int>(temp.idIngrediente, 1), true);//visto che aggiungo un elemento alla volta la quantita da aggiungere ora e' 1
            }

            resetQuantitaTuttiBottoni();
            quantitaAttualmenteSelezionata = 0;
            compratoIngredientePerTutorial = true;
            soldiGiocatore.text = Costanti.coloreVerde + "Denaro: " + Costanti.fineColore + giocatore.soldi.ToString("0.00");

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
            ingrediente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = "0";
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
                attivaDisattivaBottoneCompra(ingrediente, 0);
        bottoneCarrello.interactable = false;
        pannelloSeiSicuroAperto = false;

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
        soldiGiocatore.text = Costanti.coloreVerde + "Denaro: " + Costanti.fineColore + giocatore.soldi.ToString("0.00");
        resetSituazioneCarello();
    }

    private void resetSituazioneCarello()
    {
        //reset delle cose nel carrello
        prezzoDaPagare = 0;
        carrello = new List<Ingrediente>();
        testoTotaleCarello.text = Costanti.coloreVerde + "Totale Carrello: " + Costanti.fineColore + 0.ToString("0.00");
    }

    public void chiudiPannelloNegozio()
    {
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false);
        guiInGame.aggiornaValoreSoldi(giocatore.soldi);
        animazioneNPCIdle();
    }

    public bool getPannelloAperto()
    {
        return pannelloAperto;
    }

    public bool getPannelloConfermaAperto()
    {
        return pannelloSeiSicuroAperto;
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
