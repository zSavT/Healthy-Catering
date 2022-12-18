using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Classe per gestire il menu Negozio<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject del modello del negoziante (Non il contenitore principale)
/// </para>
/// </summary>
public class PannelloNegozio : MonoBehaviour
{
    private Animator animazione;

    [Header("Interazione Negozio")]
    [SerializeField] private GameObject canvasPannelloNegozio;
    [SerializeField] private GameObject pannelloNegozio;
    [SerializeField] private GameObject pannelloXElementi;
    [SerializeField] private Button templateSingoloIngrediente;
    [SerializeField] private Button bottoneCarrello;
    [SerializeField] private Image[] immaginiAvantiIndietroTasti;
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

    [Header ("Tutorial")]
    public static bool compratoIngredientePerTutorial = false;

    private ControllerInput controllerInput;


    void Start()
    {
        controllerInput = new ControllerInput();
        controllerInput.Enable();
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

        //chiudiPannelloSeiSicuro();
        ingredienteAttualmenteSelezionato = null;
        quantitaAttualmenteSelezionata = 0;
    }

    private void Update()
    {
        if(controllerInput.UI.Avanti.WasPressedThisFrame()  && pannelloAperto && bottoneAvantiPannelloNegozio.interactable && !pannelloSeiSicuroAperto)
            cambiaPannelloCarosello(true);
        else if (controllerInput.UI.Indietro.WasPressedThisFrame() && pannelloAperto && bottoneIndietroPannelloNegozio.interactable && !pannelloSeiSicuroAperto)
            cambiaPannelloCarosello(false);
        if(pannelloAperto)
        {
            aggiornamentoGraficaTasti();
            aggiornaObjectSelected();
        }
    }

    /// <summary>
    /// Il metodo permette di aggiornare gli sprite e le immagini dei comandi
    /// </summary>
    private void aggiornamentoGraficaTasti()
    {
        PlayerSettings.addattamentoSpriteComandi(testoEsc);
        immaginiAvantiIndietroTasti[0].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("L1");
        immaginiAvantiIndietroTasti[1].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("R1");
    }

    //INTERAZIONE NEGOZIO

    /// <summary>
    /// Il metodo permette di cambiare la visualizzazione dei pannelli con gli ingredienti (pagine) controllando se disattivare o meno i bottoni avanti e indietro
    /// </summary>
    /// <param name="avanti">bool avanti, True: Visualizzare pagina successiva, False: Visualizzare pagina precedente</param>
    private void cambiaPannelloCarosello(bool avanti)
    {
        if (avanti)
            ultimaPaginaVisualizzata++;
        else
            ultimaPaginaVisualizzata--;

        disattivaBottoniAvantiDietroSeServe();
        aggiornaBottoniPaginaCarosello();
    }


    /// <summary>
    /// Il metodo aggiorna il GameObject da selezionare dal EventSystem se il controller è collegato, quando quest'ultimo risulta null
    /// </summary>
    private void aggiornaObjectSelected()
    {
        if (Utility.gamePadConnesso())
            if (EventSystem.current.currentSelectedGameObject == null)
                if (pannelloSeiSicuroAperto)
                    EventSystem.current.SetSelectedGameObject(pannelloSeiSicuro.GetComponentsInChildren<Button>()[1].gameObject);
                else
                    EventSystem.current.SetSelectedGameObject(ingredientiBottoniFake[0].GetComponentsInChildren<Transform>()[4].gameObject);
    }

    /// <summary>
    /// Il metodo permette di disattivare i bottoni per navigare nei menu in base alla pagina visualizzata (Ultima pagina disattiva il bottone avanti, prima pagina disattiva il bottone indietro)
    /// </summary>
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

    /// <summary>
    /// Il metodo aggiorna i bottoni della pagina del carosello
    /// </summary>
    public void aggiornaBottoniPaginaCarosello()
    {
        if (ingredientiBottoniFake == null)
            ingredientiBottoniFake = creaIstanzeBottoniFakeNeiPannelli();

        aggiornaValoriBottoniFake();
    }

    /// <summary>
    /// Il metodo permette di inzializzare tutti i bottoni necessari per il pannelo negozio
    /// </summary>
    /// <returns>List bottoni generati aggiornati</returns>
    private Button [] creaIstanzeBottoniFakeNeiPannelli()
    {
        Button[] output = new Button[Costanti.numeroBottoniNellaPaginaNegozio];

        int i = 0;
        while (i < Costanti.numeroPannelliXElementiNellaPaginaNegozio)
        {
            Button [] temp = inizializzaPannelloXElementiVuoto();
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

    /// <summary>
    /// Il metodo permette di inizializzare e linkare i bottoni nel pannello
    /// </summary>
    /// <returns>List button aggiornata</returns>
    private Button[] inizializzaPannelloXElementiVuoto()
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

    /// <summary>
    /// Il metodo permette di aggiungere i bottoni generari al pannello 
    /// </summary>
    /// <param name="pannelloXElementiTemp">GameObject pannelloXElementiTemp da aggiungere gli elementi</param>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp bottone da aggiungere al pannello</param>
    private void aggiungiBottoneFakeIngredientiAlPannelloXElementi(GameObject pannelloXElementiTemp, Button singoloIngredienteTemp)
    {
        aggiungiSingoloIngredienteAPanelloXElementi(singoloIngredienteTemp, pannelloXElementiTemp);
    }

    /// <summary>
    /// Il metodo permette di aggiungere i bottoni generari al pannello 
    /// </summary>
    /// <param name="pannelloXElementi">GameObject pannelloXElementiTemp da aggiungere gli elementi</param>
    /// <param name="singoloIngrediente">Button singoloIngredienteTemp bottone da aggiungere al pannello</param>
    private void aggiungiSingoloIngredienteAPanelloXElementi(Button singoloIngrediente, GameObject pannelloXElementi)
    {
        singoloIngrediente.gameObject.transform.SetParent(pannelloXElementi.transform, false);
    }
    /// <summary>
    /// Il metodo permette di aggiungere il pannello X Elementi al pannello principale del negozio
    /// </summary>
    /// <param name="pannelloXElementiTemp">GameObject pannelloXElementiTemp da aggiungere al pannello principale</param>
    private void aggiungiPannelloXElementiAllaSchermata(GameObject pannelloXElementiTemp)
    {
        pannelloXElementiTemp.gameObject.transform.SetParent(pannelloNegozio.transform, false);
        pannelloXElementiTemp.SetActive(true);
    }

    /// <summary>
    /// Il metodo aggiorna la lista dei bottoni degli ingredienti
    /// </summary>
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
        EventSystem.current.SetSelectedGameObject(ingredientiBottoniFake[0].GetComponentsInChildren<Transform>()[4].gameObject);
    }

    /// <summary>
    /// Il metodo permette di attivare tutti i bottoni nel pannello negozio
    /// </summary>
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

    /// <summary>
    /// Il metodo permmette di trovare l'indice degli ingredienti da inserire
    /// </summary>
    /// <param name="numeroIngredientiInseritiFinoAdOra">int numero ingredienti aggiunti fino a quel momento</param>
    /// <returns>int indice piatto da inserire</returns>
    private int trovaIndicePiattoDaInserire(int numeroIngredientiInseritiFinoAdOra)
    {
        int indice = (ultimaPaginaVisualizzata * Costanti.numeroBottoniNellaPaginaNegozio) + numeroIngredientiInseritiFinoAdOra;

        if (indice != Costanti.databaseIngredienti.Count)
            return indice;

        return -1;
    }

    /// <summary>
    /// Il metodo permette di popolare tutti gli elementi negli ingredienti
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp da modificare</param>
    /// <param name="ingrediente">Ingrediente da inserire nel bottone le sue info</param>
    /// <returns>Button passato in input modificato</returns>
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

    /// <summary>
    /// Il metodo permette di rimuovere tutti i listener dagli elementi presenti nel bottone e sottobottoni presenti nel bottone in input
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp da modificare</param>
    private void rimuoviTuttiIVecchiListener(Button singoloIngredienteTemp)
    {
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAumentaQuantita].onClick.RemoveAllListeners();
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneDiminuisciQuantita].onClick.RemoveAllListeners();
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAggiungiIngredienteAlCarrello].onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Il metodo permette di aggiornare il testo del bottone passato in input
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp bottone da modificare il testo</param>
    /// <param name="nomeIngrediente">string nome dell'ingrediente</param>
    /// <param name="costoIngrediente">string costo dell'ingrediente</param>
    private void modificaTesto(Button singoloIngredienteTemp, string nomeIngrediente, string costoIngrediente)
    {
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Costanti.coloreIngredienti + nomeIngrediente + Costanti.fineColore;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Costo: " + costoIngrediente;
        singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = "0";
    }

    /// <summary>
    /// Il metodo permette di aggiungere i Listener agli ingredienti + e -
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp bottone da aggiungere listener</param>
    /// <param name="costoIngrediente">float costoIngrediente bottone</param>
    private void aggiungiGestioneBottoniQuantita(Button singoloIngredienteTemp, float costoIngrediente)
    {
        aggiungiGestioneBottoneQuantitaDiminuisci(singoloIngredienteTemp);

        aggiungiGestioneBottoneQuantitaAumenta(singoloIngredienteTemp, costoIngrediente);
    }

    /// <summary>
    /// Il metodo permette di gestire la diminuzione della quantità dell'ingredietne selezionato
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp per la gestione del bottone diminuisci</param>
    private void aggiungiGestioneBottoneQuantitaDiminuisci(Button singoloIngredienteTemp)
    {
        //bottone diminuisci quantita 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneDiminuisciQuantita].onClick.AddListener(() => {
            int quantitaSelezionata = System.Int32.Parse(
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text
            );

            

            if (quantitaSelezionata > 0)
            {
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaSelezionata - 1).ToString();
            }

            quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            attivaDisattivaBottoneCompra(singoloIngredienteTemp, quantitaSelezionata);
            attivaDisattivaBottoniPiuMenoSeServe(singoloIngredienteTemp, quantitaSelezionata, 0);
        });
    }

    /// <summary>
    /// Il metodo permette di gestire la quantità dell'ingrediente passato in input
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp ingrediente per il controllo</param>
    /// <param name="costoIngrediente">float costoIngrediente</param>
    private void aggiungiGestioneBottoneQuantitaAumenta(Button singoloIngredienteTemp, float costoIngrediente)
    {
        //bottone aumenta quantita 
        singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAumentaQuantita].onClick.AddListener(() => {
            int quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);

            

            if (giocatore.soldi - prezzoDaPagare - (costoIngrediente * (quantitaSelezionata + 1)) >= 0)
                singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = (quantitaSelezionata + 1).ToString();

            quantitaSelezionata = System.Int32.Parse(singoloIngredienteTemp.GetComponentsInChildren<TextMeshProUGUI>()[2].text);
            attivaDisattivaBottoneCompra(singoloIngredienteTemp, quantitaSelezionata);
            attivaDisattivaBottoniPiuMenoSeServe(singoloIngredienteTemp, quantitaSelezionata, costoIngrediente);
        });
    }

    /// <summary>
    /// Il metodo permette di disattivare il tasto più o meno per la gestione della quantità se il tasto
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp per disattivare i bottoni + e -</param>
    /// <param name="quantitaSelezionata">int quantità selezionata al momento</param>
    /// <param name="costoIngrediente">float costoIngrediente dell'ingrediente</param>
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
        if (singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneDiminuisciQuantita].interactable == false)
            EventSystem.current.SetSelectedGameObject(singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAumentaQuantita].gameObject);
    }

    /// <summary>
    /// Il metodo aggiorna il Listener al bottone passato in input
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp da aggiungere Listener</param>
    /// <param name="ingrediente"></param>
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

    /// <summary>
    /// Il metodo disattiva il bottone compra se la quantità presente nel bottone è uguale a 0
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button singoloIngredienteTemp bottone ingrediente da controllare</param>
    /// <param name="quantita">int quantità del ingrediente impostata</param>
    private void attivaDisattivaBottoneCompra(Button singoloIngredienteTemp, int quantita)
    {
        if (quantita > 0)
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAggiungiIngredienteAlCarrello].interactable = true;
        else
            singoloIngredienteTemp.GetComponentsInChildren<Button>()[Costanti.posizioneBottoneAggiungiIngredienteAlCarrello].interactable = false;
    }

    /// <summary>
    /// Il metodo di aggiornare l'immagine dell'ingrediente con quella corretta
    /// </summary>
    /// <param name="singoloIngredienteTemp">Button dell'ingrediente</param>
    /// <param name="ingrediente">Ingrediente ingrediente del bottone</param>
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

    /// <summary>
    /// Il metodo permette di aprire il pannello sei sicuro per l'aggiunto dell'ingrediente al carrello
    /// </summary>
    public void apriPannelloSeiSicuro()
    {
        if (inNegozio)
        {
            testoPannelloSeiSicuro.text = "Sei sicuro di voler aggiungere al carrello " + Costanti.coloreIngredienti + ingredienteAttualmenteSelezionato.nome + Costanti.fineColore + " x" + quantitaAttualmenteSelezionata.ToString();
            EventSystem.current.SetSelectedGameObject(pannelloSeiSicuro.GetComponentsInChildren<Button>()[1].gameObject);
        }
        else
        {
            testoPannelloSeiSicuro.text = creaStringaPannelloSeiSicuroCarrello();
            EventSystem.current.SetSelectedGameObject(pannelloSeiSicuro.GetComponentsInChildren<Button>()[1].gameObject);
        }

        bottoneAvantiPannelloNegozio.interactable = false;
        bottoneIndietroPannelloNegozio.interactable = false;
        pannelloSeiSicuro.SetActive(true);
        testoEsc.gameObject.SetActive(false);
        aggiornamentoGraficaTasti();
        pannelloSeiSicuroAperto = true;
    }

    /// <summary>
    /// Il metodo inizializza il pannello Sei Sicuro per l'acquista degli elementi nel carello
    /// </summary>
    /// <returns>string per il pannello sei sicuro per l'acquisto nel carrello</returns>
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

    /// <summary>
    /// Il metodo permette di convertire in quantità il numero degli elementi presenti nel carrello
    /// </summary>
    /// <returns>List di OggettiQuantità</returns>
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

    /// <summary>
    /// Il metodo permette di disattivare il bottone passato in input
    /// </summary>
    /// <param name="indicePrimoIngredienteDaDisattivare">int indice ingrediente da disattivare</param>
    private void disattivaIBottoniSuccessivi(int indicePrimoIngredienteDaDisattivare)
    {
        while (indicePrimoIngredienteDaDisattivare < Costanti.numeroBottoniNellaPaginaNegozio)
        {
            ingredientiBottoniFake[indicePrimoIngredienteDaDisattivare].gameObject.SetActive(false);
            indicePrimoIngredienteDaDisattivare++;
        }
    }

    //METODI DEI BOTTONI DEL PANNELLO SEI SICURO

    /// <summary>
    /// Il metodo permette di aggiungere gli ingredienti al carrello
    /// </summary>
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
            EventSystem.current.SetSelectedGameObject(ingredientiBottoniFake[0].GetComponentsInChildren<Transform>()[4].gameObject);
            if (carrello.Count > 0)
                bottoneCarrello.interactable = true;
            else
                bottoneCarrello.interactable = false;
        }
    }

    /// <summary>
    /// Il metodo permette di acquistare gli ingredienti presenti nel carrello
    /// </summary>
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
            EventSystem.current.SetSelectedGameObject(ingredientiBottoniFake[0].GetComponentsInChildren<Transform>()[4].gameObject);
        }
    }

    /// <summary>
    /// Il metodo imposta la variabile booleana inNegozio su false
    /// </summary>
    public void setInNegozioToInCarrello()
    {
        inNegozio = false;
    }

    /// <summary>
    /// Il metodo permette di resettare il valore della quantità selezionata da tutti i piatti
    /// </summary>
    public void resetQuantitaTuttiBottoni()
    {
        foreach (Button ingrediente in ingredientiBottoniFake)
        {
            ingrediente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = "0";
        }
    }

    /// <summary>
    /// Il metodo permette di bloccare l'operazione di acquisto
    /// </summary>
    public void annullaCompraIngrediente()
    {
        ingredienteAttualmenteSelezionato = null;
        quantitaAttualmenteSelezionata = 0;
        chiudiPannelloSeiSicuro();
        EventSystem.current.SetSelectedGameObject(ingredientiBottoniFake[0].GetComponentsInChildren<Transform>()[4].gameObject);
    }

    /// <summary>
    /// Il metodo permette di chiudere il pannello sei sicuro, resettando i valori
    /// </summary>
    private void chiudiPannelloSeiSicuro()
    {
        testoPannelloSeiSicuro.text = "You werent supposed to be able to get here you know";

        pannelloSeiSicuro.SetActive(false);

        if (ingredientiBottoniFake != null)
            foreach (Button ingrediente in ingredientiBottoniFake)
                attivaDisattivaBottoneCompra(ingrediente, 0);
        if (carrello.Count > 0)
            bottoneCarrello.interactable = true;
        else
            bottoneCarrello.interactable = false;
        pannelloSeiSicuroAperto = false;

        if (!inNegozio)
        {
            inNegozio = true;
        }
        resetQuantitaTuttiBottoni();

        disattivaBottoniAvantiDietroSeServe();
        foreach (Button temp in ingredientiBottoniFake)
            attivaDisattivaBottoniPiuMenoSeServe(temp, 0, prezzoDaPagare);
    }

    //GESTIONE PANNELLO E RELATIVI

    /// <summary>
    /// Il metodo permette di aprire il pannelo negozio
    /// </summary>
    /// <param name="giocatorePassato">Player giocatorePassato</param>
    public void apriPannelloNegozio(Player giocatorePassato)
    {
        giocatore = giocatorePassato;
        animazioneNPCParlante();
        pannelloAperto = true;
        canvasPannelloNegozio.SetActive(true);
        aggiornaBottoniPaginaCarosello();
        chiudiPannelloSeiSicuro();
       // EventSystem.current.SetSelectedGameObject(ingredientiBottoniFake[0].gameObject);
        soldiGiocatore.text = Costanti.coloreVerde + "Denaro: " + Costanti.fineColore + giocatore.soldi.ToString("0.00");
        resetSituazioneCarello();
        EventSystem.current.SetSelectedGameObject(ingredientiBottoniFake[0].GetComponentsInChildren<Transform>()[4].gameObject);
        aggiornamentoGraficaTasti();
    }
    
    /// <summary>
    /// Il metodo permette di resettare le variabili nel pannello carrello
    /// </summary>
    private void resetSituazioneCarello()
    {
        //reset delle cose nel carrello
        prezzoDaPagare = 0;
        carrello = new List<Ingrediente>();
        testoTotaleCarello.text = Costanti.coloreVerde + "Totale Carrello: " + Costanti.fineColore + 0.ToString("0.00");
        Destroy(pannelloXElementi);
    }

    /// <summary>
    /// Il metodo permette la chiusura del pannello del negozio
    /// </summary>
    public void chiudiPannelloNegozio()
    {
        pannelloAperto = false;
        canvasPannelloNegozio.SetActive(false);
        guiInGame.aggiornaValoreSoldi(giocatore.soldi);
        animazioneNPCIdle();
    }

    /// <summary>
    /// Il metodo restituisce la variabile booleana pannelloAperto per il controllo del pannello principale
    /// </summary>
    /// <returns>bool pannelloSeiSicuroAperto, True: Pannello Principale aperto, False: Pannello Principale non aperto</returns>
    public bool getPannelloAperto()
    {
        return pannelloAperto;
    }

    /// <summary>
    /// Il metodo restituisce la variabile booleana pannelloSeiSicuroAperto per il controllo del medesimo pannello
    /// </summary>
    /// <returns>bool pannelloSeiSicuroAperto, True: Pannello Sei Sicuro aperto, False: Pannello Sei Sicuro non aperto</returns>
    public bool getPannelloConfermaAperto()
    {
        return pannelloSeiSicuroAperto;
    }


    /// <summary>
    /// Il metodo permette di avviare l'animazione del negoziante mentre è inquadrato
    /// </summary>
    public void animazioneNPCInquadrato()
    {
        animazione.SetBool("inquadrato", true);
    }


    /// <summary>
    /// Il metodo permette di avviare l'animazione del negoziante fermo
    /// </summary>
    public void animazioneNPCIdle()
    {
        animazione.SetBool("parlante", false);
        animazione.SetBool("inquadrato", false);
    }

    /// <summary>
    /// Il metodo permette di avviare l'animazione del negoziante mentre parla
    /// </summary>
    private void animazioneNPCParlante()
    {
        animazione.SetBool("parlante", true);
    }
}
