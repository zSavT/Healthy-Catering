using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Classe per gestire il Menu Magazzino<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject da puntare per attivare il Menu Magazzino (PC)
/// </para>
/// </summary>
public class PannelloMagazzino : MonoBehaviour
{
    [Header ("Gestione pannello PC")]
    [SerializeField] private GameObject pannelloMagazzino;
    private bool pannelloMagazzinoAperto;
    [SerializeField] private Image sfondoImmaginePC;
    [SerializeField] private Button tastoX;
    [SerializeField] private Button tastoMyInventory;

    public static bool pannelloMagazzinoApertoPerTutorial = false;

    [Header("Suoni PC")]
    [SerializeField] private AudioSource suonoAperturaPC;
    [SerializeField] private AudioSource suonoChiusuraPC;

    [Header ("Pannello mostra inventario")]
    //mi serve per settare il parent dell'oggetto sotto a questo oggetto, poi se la vede unity a sistemarli all'interno della schermata
    [SerializeField] private GameObject pannelloMostraInventario;
    
    [SerializeField] private GameObject pannelloXElementi;
    private GameObject copiaPannelloXElementi;
    private int numeroPannelliXElementiPresenti = 1;
    
    [SerializeField] private GameObject pannelloInventarioCanvas;

    [SerializeField] private TextMeshProUGUI testoInventarioVuoto;

    [Header ("Bottone ingrediente")]
    private Button bottoneIngredienteTemplate;

    [Header ("Altro")]
    [SerializeField] private PannelloMostraRicette pannelloMostraRicette;
    private MovimentoPlayer movimentoPlayer;

    private Player giocatore;

    private List<GameObject> pannelliXElementiAttivi = new List<GameObject>();

    private ControllerInput controllerInput;
    private int indicePannello = 0;

    private void Start()
    {
        indicePannello = 0;
        pannelloMagazzino.SetActive(false);
        cambiaSfondoDesktop();
        

        //creo una copia del bottone template
        bottoneIngredienteTemplate = Instantiate(pannelloXElementi.GetComponentInChildren<Button>());
        //poi lo elimino dal pannello cosi che non ci sia piu' (non posso eliminare l'instanza successivamente siccome ci sono piu' pannelli 
        //al posto di uno solo come in menu (interazione cliente))
        pannelloXElementi = rimuoviTuttiFigliDaPannello(pannelloXElementi);

        copiaPannelloXElementi = Instantiate(pannelloXElementi);
    }

    private void OnEnable()
    {
        controllerInput = new ControllerInput();
        controllerInput.UI.Enable();
    }

    private void OnDisable()
    {
        controllerInput.UI.Disable();
    }


    private void OnDestroy()
    {
        controllerInput.Disable();
    }

    private void Update()
    {
        if(pannelloMagazzinoAperto)
            resetObjectSelected();
    }

    /// <summary>
    /// Il metodo resetta il gameObject da selezionare (Con EventSystem) quando il gamepad è connesso in base alla sezione attualmente attiva
    /// </summary>
    private void resetObjectSelected()
    {
        if (Utility.gamePadConnesso())
            if (EventSystem.current.currentSelectedGameObject == null)
                if (Utility.qualsiasiTastoPremuto(controllerInput))
                {
                    if (!pannelloInventarioCanvas.gameObject.activeSelf && pannelloMostraRicette.gameObject.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(pannelloMostraRicette.gameObject.GetComponentsInChildren<Transform>()[5].gameObject);
                    }
                    if (pannelloInventarioCanvas.gameObject.activeSelf && pannelloMostraInventario.activeSelf && !pannelloMostraRicette.gameObject.activeSelf && !giocatore.inventarioVuoto())
                    {
                        setEventSystemPrimoElemento();

                    } else if (!pannelloInventarioCanvas.gameObject.activeSelf && !pannelloMostraRicette.gameObject.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(tastoMyInventory.gameObject);
                    }
                }
    }

    /// <summary>
    /// Il metodo cambia immagine del pannello del magazzino con la visualizzazione del desktop ed attiva la versione del desktop corretta per il rapporto utilizzato
    /// </summary>
    public void cambiaSfondoDesktop()
    {
        if (AspectRatio(Screen.height, Screen.width) == "4:3" || AspectRatio(Screen.height, Screen.width) == "3:4")
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SfondoBasePC4_3");
        else
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SfondoBasePC");
        if(pannelloMagazzino.activeSelf)
            EventSystem.current.SetSelectedGameObject(tastoMyInventory.gameObject);
    }

    /// <summary>
    /// Metodo che permette di calcolare l'aspect ratio del gioco.
    /// </summary>
    /// <param name="a">Altezza risoluzione</param>
    /// <param name="b">Larghezza risoluzione</param>
    /// <returns>Rapporto Altezza/risoluzione</returns>
    private string AspectRatio(int a, int b)
    {
        int r;
        int oa = a;
        int ob = b;
        while (b != 0)
        {
            r = a % b;
            a = b;
            b = r;
        }
        return (oa / a).ToString() + ":" + (ob / a).ToString();
    }

    /// <summary>
    /// Il metodo rimuove tutti i figli presenti nel GameObject passato in input
    /// </summary>
    /// <param name="pannello">GameObject pannello da elimianare i figli all'intero</param>
    /// <returns></returns>
    private GameObject rimuoviTuttiFigliDaPannello(GameObject pannello)
    {
        foreach (Transform child in pannello.transform)
        {
            Destroy(child.gameObject);
        }

        return pannello;//non sono sicuro sia necessario il return del pannello, se non serve poi lo togliamo
    }

    /// <summary>
    /// Il metodo apre il pannello magazzino correttamente
    /// <param name="player">Player giocatore</param>
    /// <param name="controllerMovimento">MovimentoPlayer controllerMovimento per bloccare gli input non voluti</param>
    public void apriPannelloMagazzino(Player player, MovimentoPlayer controllerMovimento)
    {
        resetPannelloMagazzino();
        giocatore = player;
        movimentoPlayer = controllerMovimento;
        pannelloInventarioCanvas.SetActive(false);

        popolaSchermata();

        setSchermataInizialePC();
        if (Utility.gamePadConnesso())
            movimentoPlayer.bloccaMovimento();
    }

    /// <summary>
    /// Il metodo permette di impostare la schermata iniziale del pannello magazzino
    /// </summary>
    private void setSchermataInizialePC()
    {
        CambioCursore.cambioCursoreNormale();
        pannelloMagazzino.SetActive(true);
        pannelloMagazzinoAperto = true;
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        cambiaSfondoDesktop();
        EventSystem.current.SetSelectedGameObject(tastoMyInventory.gameObject);
        suonoAperturaPC.Play();
    }

    /// <summary>
    /// Il metodo chiude il pannello magazzino correttamente 
    /// </summary>
    public void chiudiPannelloMagazzino()
    {
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        pannelloMagazzino.SetActive(false);
        pannelloMagazzinoAperto = false;
        suonoChiusuraPC.Play();
        resetPannelloMagazzino();
        if (indicePannello == 0)
            indicePannello++;
        indicePannello++;
        if (Utility.gamePadConnesso())
            controllerInput.Player.Salto.Disable();

    }

    /// <summary>
    /// Il metodo permette di resettare la situazione degli oggetti del pannello del magazzino
    /// </summary>
    private void resetPannelloMagazzino()
    {
        foreach (GameObject pannelloXElementiTemp in pannelliXElementiAttivi)
        {
            Destroy(pannelloXElementiTemp);
        }

        pannelloXElementi = rimuoviTuttiFigliDaPannello(pannelloXElementi);
        pannelliXElementiAttivi = new List<GameObject>();

    }

    /// <summary>
    /// Il metodo restiuisce la variabile pannelloMagazzinoAperto
    /// </summary>
    /// <returns>bool pannelloMagazzinoAperto True: Il pannello è aperto, False: Il pannello è chiuso</returns>
    public bool getPannelloMagazzinoAperto()
    {
        return pannelloMagazzinoAperto;
    }

    /// <summary>
    /// Il metodo cambia immagine del pannello del magazzino con la visualizzazione del magazzino ed attiva la versione del magazzino corretta per il rapporto utilizzato
    /// </summary>
    public void cambiaSfondoMagazzino()
    {
        pannelloMagazzinoApertoPerTutorial = true;
        if (AspectRatio(Screen.height, Screen.width) == "4:3" || AspectRatio(Screen.height, Screen.width) == "3:4")
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SchermataMagazzino4_3");
        else
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SchermataMagazzino");
        if(!giocatore.inventarioVuoto())
        {
            Transform[] lista = pannelloMostraInventario.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Button>().GetComponentsInChildren<Transform>();
            EventSystem.current.SetSelectedGameObject(lista[3].gameObject);
        } else
            EventSystem.current.SetSelectedGameObject(tastoX.gameObject);

    }

    /// <summary>
    /// Il metodo imposta EventSystem con il primo elemento della lista di oggetti
    /// </summary>
    public void setEventSystemPrimoElemento()
    {
        Transform[] lista = pannelloMostraInventario.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Button>().GetComponentsInChildren<Transform>();
        EventSystem.current.SetSelectedGameObject(lista[3].gameObject);
    }

    /// <summary>
    /// Il metodo inizializza tutti gli oggetti presenti nel pannello magazzino in base alla situazione dell'inventario del player
    /// </summary>
    private void popolaSchermata()
    {   
        if (!giocatore.inventarioVuoto())
        {
            List<OggettoQuantita<int>> inventario = giocatore.inventario;
            pannelloXElementi.SetActive(true);

            int numeroBottoniAggiuntiFinoAdOraInPannelloXElementi = 0;

            foreach (OggettoQuantita<int> oggettoDellInventario in inventario)
            {
                if(oggettoDellInventario.quantita != 0)
                {
                    Button bottoneDaAggiungereTemp = creaBottoneConValoriIngrediente(oggettoDellInventario, bottoneIngredienteTemplate);
                    
                    bottoneDaAggiungereTemp.transform.SetParent(pannelloXElementi.transform, false);
                    numeroBottoniAggiuntiFinoAdOraInPannelloXElementi++;
                    if ((numeroBottoniAggiuntiFinoAdOraInPannelloXElementi > Costanti.bottoniMassimiPerPannelloXElementi)
                        &&
                        (oggettoDellInventario != inventario[inventario.Count - 1])) // se e' diverso dall'ultimo elemento, previene che venga creato un pannello vuoto
                    {
                        pannelliXElementiAttivi.Add(pannelloXElementi);
                        aggiungiPannelloXElementi();
                        numeroBottoniAggiuntiFinoAdOraInPannelloXElementi = 0;
                    }
                }
            }

            if (!testoInventarioVuoto.text.Equals(""))
                testoInventarioVuoto.text = "";

        }
        else
        {
            testoInventarioVuoto.text = Costanti.testoInventarioVuotoString;
            pannelloXElementi.SetActive(false);
            EventSystem.current.SetSelectedGameObject(tastoX.gameObject);
        }
    }

    /// <summary>
    /// Il metodo crea ed inizializza i bottoni per gli ingredienti presenti
    /// </summary>
    /// <param name="oggettoDellInventario">OggettoQuantita<int> lista di id degli ingredienti presenti nell'invetario del giocatore</param>
    /// <param name="bottoneIngredienteTemplate">Button da inizializzare</param>
    /// <returns></returns>
    private Button creaBottoneConValoriIngrediente(OggettoQuantita<int> oggettoDellInventario, Button bottoneIngredienteTemplate)
    {
        Ingrediente ingrediente = Ingrediente.idToIngrediente(oggettoDellInventario.oggetto);

        Button output = Instantiate(bottoneIngredienteTemplate);
        output.name = ingrediente.nome;

        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingrediente: " + Costanti.coloreIngredienti + ingrediente.nome + Costanti.fineColore;
        output.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Quantità presente: " + oggettoDellInventario.quantita.ToString();

        output.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            pannelloMostraRicette.apriPannelloMostraRicette(ingrediente);
            pannelloInventarioCanvas.SetActive(false);
        });

        return output;
    }

    /// <summary>
    /// Il metodo aggiunge gli elementi al sottopannello per contenenre gli ingredienti
    /// </summary>
    private void aggiungiPannelloXElementi()
    {
        GameObject nuovoPannelloXElementi = Instantiate(copiaPannelloXElementi);
        nuovoPannelloXElementi = rimuoviTuttiFigliDaPannello(nuovoPannelloXElementi);

        nuovoPannelloXElementi.name = "SottoPannelloMostraInventarioXElementi" + numeroPannelliXElementiPresenti.ToString();
        nuovoPannelloXElementi.transform.SetParent(pannelloMostraInventario.transform, false);
        numeroPannelliXElementiPresenti++;

        //ora la variabile di prima viene usata per il nuovo pannello
        pannelloXElementi = nuovoPannelloXElementi;
    }
}
