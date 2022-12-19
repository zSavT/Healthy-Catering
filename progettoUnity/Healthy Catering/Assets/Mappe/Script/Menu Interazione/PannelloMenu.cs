using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Classe per gestire il Menu clienti<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject pannello principale MenuCliente
/// </para>
/// </summary>
public class PannelloMenu : MonoBehaviour
{
    //Variabili di supporto e linking
    public static bool clienteServito;
    private Piatto piattoSelezionato;
    private Cliente cliente;
    private Interactable controllerAnimazioneCliente;
    private Player giocatore;
    [SerializeField] private ProgressoLivello livelloProgresso;
    [Header("Pannello menu e pannello cliente")]
    [SerializeField] private GameObject pannelloPrincipaleMenuCliente;
    [SerializeField] private GameObject pannelloMenu;
    [SerializeField] private GameObject pannelloCliente;
    [SerializeField] private GameObject bottonePiatto; 
    [SerializeField] private GameObject pannelloPiatti;
    public static bool pannelloMenuAperto;

    [Header("Pannello ingredienti piatto")]
    [SerializeField] private GameObject pannelloIngredientiPiatto;
    [SerializeField] private TextMeshProUGUI testoEsciMenuIngredientiPiatto;
    public static bool pannelloIngredientiPiattoAperto;

    [Header("Pannello ingredienti giusti sbagliati")]
    [SerializeField] private GameObject pannelloIngredientiGiustiSbagliati;
    [SerializeField] private AudioSource suonoPiattoScorretto;
    [SerializeField] private AudioSource suonoVoceNegativo;
    public static bool pannelloIngredientiGiustiSbagliatiAperto;

    [Header("Pannello conferma piatto")]
    [SerializeField] GameObject pannelloConfermaPiatto;
    public static bool pannelloConfermaPiattoAperto;


    [Header("Elementi Pannello Ingredienti Giusto e Sbagliato")]
    [SerializeField] private TextMeshProUGUI titoloIngredientiGiustiSbagliati;
    [SerializeField] private TextMeshProUGUI testoIngredientiGiusti;
    [SerializeField] private TextMeshProUGUI testoIngredientiSbagliatiDieta;
    [SerializeField] private TextMeshProUGUI testoIngredientiSbagliatiPatologia;

    [Header("Altro")]
    [SerializeField] private TextMeshProUGUI testoConfermaPiatto;
    [SerializeField] private Scrollbar scroll;
    [SerializeField] private ScrollRect scrollReact;
    [SerializeField] private GameObject EscPerUscireTesto; //Lo imposto come GameObject e non come testo, perchè mi interessa solo attivarlo disattivarlo velocemente
    public UnityEvent chiusuraInterazioneCliente;
    private List<string> blackListPiatti = new List<string>();

    [Header("Gestione aggiornamento piatti")]
    private Button[] bottoniPiatti; 
    List<Piatto> piatti;

    private ControllerInput controllerInput;

    private GameObject piattoSelezionatoBottone;

    void Start()
    {
        blackListPiatti.Add("Frittura di pesce");
        blackListPiatti.Add("Patatine fritte");
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        clienteServito = false;
        pannelloIngredientiPiatto.SetActive(false);
        pannelloConfermaPiatto.SetActive(false);
        pannelloIngredientiGiustiSbagliati.SetActive(false);
        piatti = Costanti.databasePiatti;
        generaBottoniPiatti();
    }


    void Update()
    {
        if(pannelloMenuAperto || pannelloPrincipaleMenuCliente.activeSelf)
        {
            if (pannelloIngredientiPiattoAperto)
            {
                if (controllerInput.Player.UscitaMenu.WasPressedThisFrame())
                {
                    chiudiPannelloIngredientiPiatto();
                }
            }

            if (pannelloIngredientiGiustiSbagliatiAperto)
            {
                if (controllerInput.Player.UscitaMenu.WasPerformedThisFrame())
                {
                    chiudiPannelloIngredientiGiustiSbagliati();
                }
            }
            if (pannelloConfermaPiatto.activeSelf)
            {
                if (EventSystem.current.currentSelectedGameObject == null && Utility.gamePadConnesso())
                {
                    EventSystem.current.SetSelectedGameObject(pannelloConfermaPiatto.GetComponentsInChildren<Button>()[1].gameObject);
                }
            }
            if (pannelloPrincipaleMenuCliente.activeSelf)
            {
                if (controllerInput.UI.MostraRicette.WasPressedThisFrame() && !pannelloIngredientiGiustiSbagliatiAperto && !getPannelloConfermaPiattoAperto() && controlloSelectObjectCorretto())
                {
                    cambiaPannelloIngredientiPiattoConPiatto(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Button>());
                    apriPannelloIngredientiPiatto();
                }
                if (Utility.gamePadConnesso())
                {
                    fixSelectObjectCorretto();
                }
                aggiornaImmagineTastiPiatti();
                PlayerSettings.addattamentoSpriteComandi(EscPerUscireTesto.GetComponent<TextMeshProUGUI>());
                PlayerSettings.addattamentoSpriteComandi(testoEsciMenuIngredientiPiatto);
            }
        }
    }


    /// <summary>
    /// Il metodo controlla se il currentObject selezionato dall'eventSystem sia quello corretto presente nella lista piatti
    /// </summary>
    /// <returns>bool True: Object corretto, False, Object non corretto</returns>
    private bool controlloSelectObjectCorretto()
    {
        bool trovato = false;
        foreach (Button temp in bottoniPiatti)
        {
            if (EventSystem.current.currentSelectedGameObject == temp.gameObject)
            {
                trovato = true; break;
            }
        }
        return trovato;
    }

    /// <summary>
    /// Il metodo aggionra gli sprite nei testi presenti nei bottoni dei piatti in base alla tipologia di input
    /// </summary>
    private void aggiornaSpriteBottonePiatti()
    {
        foreach (Button temp in bottoniPiatti)
        {
            PlayerSettings.addattamentoSpriteComandi(temp.GetComponentsInChildren<Button>()[1].GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    /// <summary>
    /// Il metodo aggiorna l'immagine dei tasti nei piatti in base al tipo di input
    /// </summary>
    private void aggiornaImmagineTastiPiatti()
    {
        foreach (Button temp in bottoniPiatti)
        {
            temp.GetComponentsInChildren<Image>()[3].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("Triangolo");
        }
    }

    /// <summary>
    /// Il metodo controlla ed aggiorna l'EventSystem per l'oggetto corretto
    /// </summary>
    private void fixSelectObjectCorretto()
    {
        if (pannelloConfermaPiattoAperto && EventSystem.current.currentSelectedGameObject == null && !pannelloIngredientiGiustiSbagliatiAperto && !pannelloIngredientiPiattoAperto && !pannelloCliente.activeSelf && !pannelloMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(pannelloConfermaPiatto.GetComponentsInChildren<Button>()[1].gameObject);
        } else if (controlloSelectObjectCorretto())
        {
            piattoSelezionatoBottone = EventSystem.current.currentSelectedGameObject;
        } 
        else if ((!controlloSelectObjectCorretto() && !pannelloIngredientiGiustiSbagliatiAperto && !pannelloIngredientiPiattoAperto && !pannelloConfermaPiattoAperto) || EventSystem.current.currentSelectedGameObject == null) 
        {
            EventSystem.current.SetSelectedGameObject(piattoSelezionatoBottone);
        }

    }


    /// <summary>
    /// Il metodo attiva il gameObject del pannello cliente
    /// </summary>
    private void apriMenuCliente()
    {
        pannelloMenu.SetActive(true);
        pannelloCliente.SetActive(true);
        EscPerUscireTesto.SetActive(true);
    }

    /// <summary>
    /// Il metodo disattiva il gameObject del pannello cliente
    /// </summary>
    private void chiudiMenuCliente()
    {
        pannelloMenu.SetActive(false);
        pannelloCliente.SetActive(false);
        pannelloMenuAperto = false;
    }

    /// <summary>
    /// Il metodo restiuisce il bolleano per controllare se il pannello menu cliente è aperto o meno
    /// </summary>
    /// <returns>Booleano pannelloMenuAperto</returns>
    public bool getPannelloMenuClienteAperto()
    {
        return pannelloMenuAperto;
    }

    /// <summary>
    /// Il metodo attiva il pannello principale dell'interazione con il cliente
    /// </summary>
    public void apriPannelloMenuCliente()
    {
        pannelloMenuAperto = true;
        scroll.value = 0;
        scrollReact.verticalNormalizedPosition= 0;
        PlayerSettings.addattamentoSpriteComandi(EscPerUscireTesto.GetComponent<TextMeshProUGUI>());
        pannelloPrincipaleMenuCliente.SetActive(true);
        apriMenuCliente();
    }

    /// <summary>
    /// Il metodo disattiva il pannello principale dell'interazione con il cliente
    /// </summary>
    public void ChiudiPannelloMenuCliente()
    {
        pannelloPrincipaleMenuCliente.SetActive(false);
        chiudiMenuCliente();
    }

    /// <summary>
    /// Il metodo imposta tutte le informazioni del cliente per i controlli
    /// </summary>
    /// <param name="idClientePuntato">int id del cliente puntato</param>
    /// <param name="giocatorePartita">Player classe player del giocatore</param>
    /// <param name="controlleNPCPuntato">Interactable classe del modello del cliente </param>
    public void setCliente(int idClientePuntato, Player giocatorePartita, Interactable controlleNPCPuntato)
    {
        apriPannelloMenuCliente();
        aggiornaBottoniPiatti();
        
        cliente = Costanti.databaseClienti[idClientePuntato];
        caricaClienteInPanello(cliente);
        controllerAnimazioneCliente = controlleNPCPuntato;
        
        giocatore = giocatorePartita;
    }

    /// <summary>
    /// Il metodo genera gli effettivi bottoni del pannello piatti
    /// </summary>
    private void generaBottoniPiatti()
    {
        bottoniPiatti = new Button[piatti.Count];
        List<Button> bottoniPiattiTemp = generaBottoniPiattiTemp();

        int i = 0;
        foreach (Button bottonePiatto in bottoniPiattiTemp)
        {
            Button bottoneTemp;
            bottoneTemp = (Instantiate(bottonePiatto, pannelloPiatti.transform, false) as Button);

            bottoneTemp.GetComponent<Button>().onClick.AddListener(() => {
                selezionaPiatto(bottoneTemp, piatti, cliente);
            });

            //in posizione 0 c'e' il bottone per selezionare il piatto
            //e in posizione 1 c'e' il bottone per vedere gli ingredienti
            bottoneTemp.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
                cambiaPannelloIngredientiPiattoConPiatto(bottoneTemp.GetComponentsInChildren<Button>()[1]);
                apriPannelloIngredientiPiatto();
            });
            PlayerSettings.addattamentoSpriteComandi(bottoneTemp.GetComponentsInChildren<Button>()[1].GetComponentInChildren<TextMeshProUGUI>());
            bottoniPiatti[i] = bottoneTemp;
            i++;
        }

        aggiornaBottoniPiatti();
        foreach (Button bottonePiatto in bottoniPiattiTemp)
        {
            Destroy(bottonePiatto.gameObject);
        }
    }

    /// <summary>
    /// Il metodo genera la lista dei bottoni dei piatti per l'iniizializzazione
    /// </summary>
    /// <returns>List bottoni piatti</returns>
    private List<Button> generaBottoniPiattiTemp()
    {
        List<Button> bottoniPiattiTemp = new List<Button>();

        foreach (Piatto piatto in piatti)
        {
            bottoniPiattiTemp.Add(generaBottonePiatto(piatto, bottonePiatto));
        }
        Destroy(bottonePiatto);

        return bottoniPiattiTemp;
    }


    /// <summary>
    /// Il metodo aggiorna la lista dei piatti disponibili e quelli non disponibili
    /// </summary>
    private void aggiornaBottoniPiatti()
    {
        if (bottoniPiatti != null)
        {
            List<Button> piattiDisponibili = new List<Button>();
            List<Button> piattiNonDisponibili = new List<Button>();

            foreach (Button bottonePiatto in bottoniPiatti)
            {
                bottonePiatto.transform.SetParent(null);

                if (!(Piatto.nomeToPiatto (bottonePiatto.name)).piattoInInventario(giocatore.inventario))
                {
                    bottonePiatto.interactable = true;
                    piattiNonDisponibili.Add(bottonePiatto);
                    bottonePiatto.onClick.RemoveAllListeners();
                    bottonePiatto.GetComponentsInChildren<TextMeshProUGUI>()[3].text = "Piatto non servibile, ingredienti  insufficienti";
                }
                else
                {
                    bottonePiatto.interactable = true;
                    piattiDisponibili.Add(bottonePiatto);
                    bottonePiatto.onClick.AddListener(() => {
                         selezionaPiatto(bottonePiatto, piatti, cliente);
                     });
                    bottonePiatto.GetComponentsInChildren<TextMeshProUGUI>()[3].text = string.Empty;
                }
            }

            aggiungiPiattiAPannelloPiatti(piattiDisponibili, piattiNonDisponibili);
        }
    }

    /// <summary>
    /// Il metodo aggiorna il pannello dei piatti con quelli disponibili e quelli non disponibili
    /// </summary>
    /// <param name="piattiDisponibili">list piatti realizzabili</param>
    /// <param name="piattiNonDisponibili">list piatti non realizzabili</param>
    private void aggiungiPiattiAPannelloPiatti(List <Button> piattiDisponibili, List <Button> piattiNonDisponibili)
    {
        piattiDisponibili.AddRange(piattiNonDisponibili);
        bottoniPiatti = piattiDisponibili.ToArray();

        foreach (Button temp in bottoniPiatti)
        {
            temp.transform.SetParent(pannelloPiatti.transform);
        }
        EventSystem.current.SetSelectedGameObject(piattiDisponibili[0].gameObject);

    }

    /// <summary>
    /// Il metodo controlla quale piatto è selezioato tra la lista di quelli presenti
    /// </summary>
    /// <param name="bottone">Button bottone attualmente selezioato</param>
    /// <param name="piatti">List piatti contenente tutti i piatti esistenti</param>
    /// <param name="cliente">Cliente cliente servito</param>
    private void selezionaPiatto(Button bottone, List<Piatto> piatti, Cliente cliente)
    {
        foreach (Piatto piatto in piatti)
        {
            if ((bottone.name.Contains(piatto.nome)) && (bottone.name.Contains("(Clone)")))//contains perche' viene aggiunta la stringa "(Clone)" nel gameobject
            {
                piattoSelezionato = piatto;
                break;
            }
        }

        setPannelloConfermaConNomePiatto(piattoSelezionato.nome);
    }

    /// <summary>
    /// Il metodo serve per confermare l'acquisto di un piatto
    /// </summary>
    public void confermaPiattoDaBottone()
    {
        clienteServito = true;
        bool affinitaPatologiePiatto = false;
        bool piattoInBlackList = false;
        if (!blackListPiatti.Contains(piattoSelezionato.nome))
        {
            affinitaPatologiePiatto = piattoSelezionato.checkAffinitaPatologiePiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.listaIdPatologie);
        } 
        else
        {
            if(cliente.listaIdPatologie.Contains(0) || cliente.listaIdPatologie.Contains(1))
            {
                piattoInBlackList = true;
                affinitaPatologiePiatto = false;
            }
            else
            {
                affinitaPatologiePiatto = piattoSelezionato.checkAffinitaPatologiePiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.listaIdPatologie);
            }
        }
        bool affinitaDietaPiatto = piattoSelezionato.checkAffinitaDietaPiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.dieta);
        bool affinita = affinitaPatologiePiatto && affinitaDietaPiatto;
        float guadagno = piattoSelezionato.calcolaCostoConBonus(affinita, piattoSelezionato.calcolaCostoBase());

        chiudiPannelloConfermaPiatto();

        giocatore.guadagna(guadagno);
        giocatore.aggiungiDiminuisciPunteggio(affinita, piattoSelezionato.calcolaNutriScore(), piattoSelezionato.calcolaCostoEco(), PlayerSettings.livelloSelezionato);
        giocatore.aggiornaInventario(piattoSelezionato.listaIdIngredientiQuantita, false);

        if (!affinita)
        {
            caricaIngredientiInPannelloIngredientiGiustiSbagliati(piattoSelezionato, cliente, piattoInBlackList);
            apriPannelloIngredientiGiustiSbagliati();
        }
        else
        {
            chiusuraInterazioneCliente.Invoke();
        }
        livelloProgresso.servitoCliente(giocatore.punteggio[PlayerSettings.livelloSelezionato]);
        animazioni(affinitaPatologiePiatto, affinitaDietaPiatto);
        aggiornaBottoniPiatti();
    }

    /// <summary>
    /// Il metodo aggiorna i parametri del pannello conferma piatto
    /// </summary>
    /// <param name="nomePiatto">string nomePiatto nome del piatto da inserire nel titolo del pannello</param>
    private void setPannelloConfermaConNomePiatto(string nomePiatto)
    {
        apriPannelloConfermaPiatto();
        testoConfermaPiatto.text = "Sei sicuro di voler servire il piatto: \n" + Costanti.colorePiatti + nomePiatto + Costanti.fineColore;
    }

    /// <summary>
    /// Il metodo carica i valori del piatto del bottone mostra ingredienti
    /// </summary>
    /// <param name="bottoneMostraIngredienti">Button bottoneMostraIngredienti bottone del ingrediente cliccato per mostrare gli ingredienti</param>
    private void cambiaPannelloIngredientiPiattoConPiatto(Button bottoneMostraIngredienti)
    {
        Piatto piattoSelezionato = Piatto.nomeToPiatto(bottoneMostraIngredienti.name);
        //Piatto piattoSelezionato = Piatto.getPiattoFromNomeBottone(bottoneMostraIngredienti.name);

        string ingredientiPiatto = piattoSelezionato.getListaIngredientiQuantitaToString();

        //piatto
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingredienti nel piatto " + Costanti.colorePiatti + piattoSelezionato.nome + Costanti.fineColore;
        //Ingredienti
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Ingredienti:\n" + Costanti.coloreIngredienti + ingredientiPiatto + Costanti.fineColore;
    }

    /// <summary>
    /// Il metodo avvia l'animazione del cliente dopo aver servito un piatto
    /// </summary>
    /// <param name="affinitaPatologiePiatto">bool affinitaPatologiePiatto True: Piatto affine patologie, False: Piatto non affine con le patologie</param>
    /// <param name="affinitaDietaPiatto">bool affinitaDietaPiatto, True: Piatto affine alla dieta, False: Piatto non affine alla dieta</param>
    private void animazioni(bool affinitaPatologiePiatto, bool affinitaDietaPiatto)
    {
        if(affinitaPatologiePiatto && affinitaDietaPiatto)
        {
            controllerAnimazioneCliente.animazioneContenta();
        }
    }

    /// <summary>
    /// Il metodo genera tutti i bottoni del piatto ed inizializza i loro valori
    /// </summary>
    /// <param name="piatto">Piatto da inizializzare i valori nel pannello</param>
    /// <param name="bottonePiatto">GameObjec del bottone da inizializzare</param>
    /// <returns>Button generato del piatto</returns>
    private Button generaBottonePiatto(Piatto piatto, GameObject bottonePiatto)
    {
        GameObject outputGameObject = (GameObject)Instantiate(bottonePiatto);

        Button output = outputGameObject.GetComponent<Button>();
        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Costanti.colorePiatti + piatto.nome + Costanti.fineColore;
        output.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Costo: " + piatto.calcolaCostoBase().ToString();

        Sprite nuovaImmagine = Resources.Load<Sprite>("immaginiPiatti/" + piatto.nome);
        if (nuovaImmagine == null) 
        { 
            nuovaImmagine = Resources.Load<Sprite>("immaginiPiatti/ImmaginePiattoDefault");
        }
        output.GetComponentsInChildren<Image>()[1].sprite = nuovaImmagine;

        output.name = piatto.nome;

        //in posizione 0 c'e' il bottone per selezionare il piatto
        //e in posizione 1 c'e' il bottone per vedere gli ingredienti
        output.GetComponentsInChildren<Button>()[1].name = "Ingredienti " + piatto.nome;
        output.navigation = outputGameObject.GetComponent<Button>().navigation;
        return output;
    }

    /// <summary>
    /// Il metood permette di inizializzare tutti i valori del cliente nel pannello cliente
    /// </summary>
    /// <param name="cliente">Cliente cliente da leggere i valori</param>
    private void caricaClienteInPanello(Cliente cliente)
    {
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Costanti.grassetto + Utility.getStringaConCapitalLetterIniziale(cliente.nome) + Costanti.fineGrassetto;
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[1].text = Costanti.grassetto + "Dieta: " + Costanti.fineGrassetto + Costanti.coloreDieta + Utility.getStringaConCapitalLetterIniziale(Dieta.IdDietaToDietaString(cliente.dieta)) + Costanti.fineColore;
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = Costanti.grassetto + "Tieni conto che il cliente ha problemi con:\n" + Costanti.fineGrassetto + Costanti.colorePatologia + Patologia.listIdToListPatologie(cliente.listaIdPatologie) + Costanti.fineColore;
    }

    /// <summary>
    /// Il metodo permette di invertire il valore della variabile pannelloIngredientiPiattoAperto
    /// </summary>
    private void pannelloIngredientiPiattoApertoChiuso()
    {
        pannelloIngredientiPiattoAperto = !pannelloIngredientiPiattoAperto;
    }

    /// <summary>
    /// Il metodo restituisce la variabile booleana pannelloIngredientiPiattoAperto
    /// </summary>
    /// <returns>bool pannelloIngredientiPiattoAperto, True: Pannello Aperto, False: Pannello Chiuso</returns>
    public bool getPannelloIngredientiPiattoAperto()
    {
        return pannelloIngredientiPiattoAperto;
    }


    /// <summary>
    /// Il metodo permette di aprire il pannello Ingredienti piatto
    /// </summary>
    private void apriPannelloIngredientiPiatto()
    {
        if (pannelloIngredientiPiatto != null)
        {
            pannelloIngredientiPiatto.SetActive(true);
            PlayerSettings.addattamentoSpriteComandi(testoEsciMenuIngredientiPiatto);
            pannelloIngredientiPiattoApertoChiuso();
            chiudiMenuCliente();
            EscPerUscireTesto.SetActive(false);
        }
    }

    /// <summary>
    /// Il metodo permette chiude il pannello ingredienti Piatto
    /// </summary>
    private void chiudiPannelloIngredientiPiatto()
    {
        if (pannelloIngredientiPiatto != null)
        {
            pannelloIngredientiPiatto.SetActive(false);
            pannelloIngredientiPiattoApertoChiuso();
            apriMenuCliente();
        }
    }


    /// <summary>
    /// Il metodo restiuisce il booleano pannelloConfermaPiattoAperto che controlla sei il panello conferma piatto e aperto o meno
    /// </summary>
    /// <returns>bool pannelloConfermaPiattoAperto</returns>
    public bool getPannelloConfermaPiattoAperto()
    {
        return pannelloConfermaPiattoAperto;
    }

    /// <summary>
    /// Il metodo apre il pannello Conferma Piatto
    /// </summary>
    private void apriPannelloConfermaPiatto()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(true);
            chiudiMenuCliente();
            pannelloConfermaPiattoAperto = true;
            EscPerUscireTesto.SetActive(false);
            EventSystem.current.SetSelectedGameObject(pannelloConfermaPiatto.GetComponentsInChildren<Button>()[0].gameObject);
        }

    }

    /// <summary>
    /// Il metodo chiude il pannello conferma piatto
    /// </summary>
    private void chiudiPannelloConfermaPiatto()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(false);
            pannelloConfermaPiattoAperto = false;
            apriMenuCliente();
            EscPerUscireTesto.SetActive(true);
            controllerAnimazioneCliente.animazioneCamminata();
        }
    }

    /// <summary>
    /// Il metodo disattiva il pannello Conferma Piatto selezionando la scelta "NO"
    /// </summary>
    public void chiudiPannelloConfermaPiattoDopoNO()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(false);
            pannelloConfermaPiattoAperto = false;
            EscPerUscireTesto.SetActive(true);
            apriMenuCliente();
        }
    }


    /// <summary>
    /// Il metodo inverte il valore booleano di "pannelloIngredientiGiustiSbagliatiAperto" che controlla se il pannello Ingredienti Giusti e Sbagliati Aperto
    /// </summary>
    private void pannelloIngredientiGiustiSbagliatiApertoChiuso()
    {
        pannelloIngredientiGiustiSbagliatiAperto = !pannelloIngredientiGiustiSbagliatiAperto;
    } 

    /// <summary>
    /// Il metodo permette di aprire il pannello degli Ingredienti Giusti e Sbagliati correttamente, disattivando gli elementi non necessari.
    /// </summary>
    private void apriPannelloIngredientiGiustiSbagliati()
    {
        if (pannelloIngredientiGiustiSbagliati != null)
        {
            suonoPiattoScorretto.Play();
            suonoVoceNegativo.PlayDelayed(0.1f);
            pannelloIngredientiGiustiSbagliati.SetActive(true);
            pannelloIngredientiGiustiSbagliatiApertoChiuso();
            chiudiMenuCliente();
        }

    }

    /// <summary>
    /// Il metodo permette di caricare tutti gli ingredienti del pannello Ingredienti Giusti e Sbagliati per la visualizzazione degli ingredienti che hanno dato problemi
    /// </summary>
    /// <param name="piattoSelezionato">Il piatto servito al cliente</param>
    /// <param name="cliente">Il cliente a cui è stato servito il piatto</param>
    /// <param name="piattoInBlackList">Controllo se il piatto è nella blacklist dei piatti</param>
    private void caricaIngredientiInPannelloIngredientiGiustiSbagliati(Piatto piattoSelezionato, Cliente cliente, bool piattoInBlackList)
    {
        titoloIngredientiGiustiSbagliati.text = "Compatibilità ingredienti del piatto " + Costanti.colorePiatti + piattoSelezionato.nome + Costanti.fineColore +"\nper il cliente " + Utility.getStringaConCapitalLetterIniziale(cliente.nome);

        List<Ingrediente> ingredientiPiattoSelezionato = piattoSelezionato.getIngredientiPiatto();

        //ogni volta che calcolo gli ingredienti non compatibili li rimuovo dalla lista generale degli ingredienti
        //in questo modo non vengono aggiunti alle altre liste a prescindere
        List<Ingrediente> ingredientiNonCompatibiliPatologia = cliente.getListaIngredientiNonCompatibiliPatologie(ingredientiPiattoSelezionato);
        foreach (Ingrediente item in ingredientiNonCompatibiliPatologia) ingredientiPiattoSelezionato.Remove(item);
        
        List<Ingrediente> ingredientiNonCompatibiliDieta = cliente.getListaIngredientiNonCompatibiliDieta(ingredientiPiattoSelezionato);
        foreach (Ingrediente item in ingredientiNonCompatibiliDieta) ingredientiPiattoSelezionato.Remove(item);
        
        //nella lista degli ingredienti piatto selezionato ci sono solo gli ingredienti che vanno bene ora
        List<Ingrediente> ingredientiCompatibili = ingredientiPiattoSelezionato;

        testoIngredientiGiusti.color = Costanti.coloreTestoIngredientiGiusti;
        testoIngredientiGiusti.text = Ingrediente.listIngredientiToStringa (ingredientiCompatibili);

        testoIngredientiSbagliatiDieta.color = Costanti.coloreTestoIngredientiSbagliatiDieta;
        testoIngredientiSbagliatiDieta.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliDieta);
        testoIngredientiSbagliatiPatologia.color = Costanti.coloreTestoIngredientiSbagliatiPatologia;

        if (piattoInBlackList)
        {
            testoIngredientiSbagliatiPatologia.text = "La frittura non è adatta alla patologia del cliente.";
        } else
        {
            testoIngredientiSbagliatiPatologia.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliPatologia);
        }
    }

    /// <summary>
    /// Il metodo permette di chiudere correttamente il pannello IngredientiGiustiESbagliati
    /// </summary>
    private void chiudiPannelloIngredientiGiustiSbagliati()
    {
        if (pannelloIngredientiGiustiSbagliati != null)
        {
            pannelloIngredientiGiustiSbagliati.SetActive(false);
            pannelloIngredientiGiustiSbagliatiApertoChiuso();
            apriMenuCliente();
            controllerAnimazioneCliente.animazioneCamminata();
            chiusuraInterazioneCliente.Invoke();
            controllerAnimazioneCliente.animazioneScontenta();
        }
    }
}
