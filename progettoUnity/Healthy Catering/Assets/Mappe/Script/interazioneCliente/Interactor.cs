using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

/*
 * 
 *      RICORDATI DI METTE LE NOTE PER LE ANIMAZIONI!
 * 
 * 
 */
public class Interactor : MonoBehaviour
{
    [Header("Interazione NPC")]
    [SerializeField] private LayerMask layerUnityNPC = 6;              //layer utilizzato da Unity per le categorie di oggetto

    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione

    //trigger per la scritta dell'interazione
    [SerializeField] private UnityEvent inquadratoNPC;
    [SerializeField] private UnityEvent uscitaRangeMenu;

    [SerializeField] private GameObject NPC;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;

    [SerializeField] private Transform posizioneCamera;
    public static bool pannelloAperto;

    [SerializeField] private GameObject pannelloCliente;
    private Vector3 posizioneCameraOriginale;
    private bool menuApribile;

    private bool bottoniGenerati;
    
    //TODO prendere il player che sta giocando al posto del primo nel database
    [SerializeField] private Player giocatore = Database.getDatabaseOggetto (new Player ())[0];

    [SerializeField] private GameObject pannelloIngredientiPiatto;
    private bool pannelloIngredientiPiattoAperto;

    //TODO aggiornare con il cliente vero
    private Cliente cliente = Database.getDatabaseOggetto(new Cliente())[1];

    [SerializeField] GameObject pannelloConfermaPiatto;
    private bool pannelloConfermaPiattoAperto;
    private bool confermaSI;

    private Piatto piattoSelezionato;

    void Start()
    {
        chiudiPannello();
        pannelloAperto = false;
        chiudiPannelloIngredientiPiatto();
        pannelloIngredientiPiattoAperto = false;
        chiudiPannelloConfermaPiatto();
        pannelloConfermaPiattoAperto = false;
        confermaSI = false;

        posizioneCameraOriginale = mainCamera.transform.position;
        menuApribile = true;
        bottoniGenerati = false;
    }

    // Update is called once per frame
    void Update()
    {
        interazioneUtenteNPC();
    }

    public void menuApribileOnOff()
    {
        menuApribile = !menuApribile;
    }

    private void interazioneUtenteNPC()
    {
        if (menuApribile)
        {
            if (NPCInteragibilePuntato())
            {
                inquadratoNPC.Invoke();


                if (Input.GetKeyDown(tastoInterazione))
                {
                    interazioneCliente(Player, NPC);
                }
            }

            else
            {
                uscitaRangeMenu.Invoke();
                print("pannello aperto: " + pannelloAperto.ToString());
                print("pannello ingredienti aperto: " + pannelloIngredientiPiattoAperto.ToString());
                if (pannelloAperto && !pannelloIngredientiPiattoAperto)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        esciDaInterazioneCliente();
                    }
                }
                if (pannelloIngredientiPiattoAperto)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        chiudiPannelloIngredientiPiatto();
                        apriPannello();
                    }
                }
            }
        }
    }

    private bool NPCInteragibilePuntato()
    {
        RaycastHit NPCpuntato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCpuntato, 2, layerUnityNPC))
        {
            // se l'oggetto visualizzato è interagibile
            if (NPCpuntato.collider.GetComponent<Interactable>() != false)
            {
                return true;
            }
        }
        return false;
    }
    private void esciDaInterazioneCliente()
    {
        chiudiPannello();
        playerRiprendiMovimento.Invoke();
        

        ritornaAllaPosizioneNormale();

        PuntatoreMouse.disabilitaCursore();
    }

    private void ritornaAllaPosizioneNormale()
    {
        mainCamera.transform.position = posizioneCamera.position;
    }

    private void pannelloApertoChiuso()
    {
        pannelloAperto = !pannelloAperto;
    }

    private void interazioneCliente(GameObject Player, GameObject NPC)
    {
        muoviCameraPerInterazioneCliente();

        playerStop.Invoke();

        apriPannello();//Riguardo il TODO di sopra, anche qui, ovviamente, va cambiato


        PuntatoreMouse.abilitaCursore();
    }

    private void muoviCameraPerInterazioneCliente()
    {
        //prende la posizione della camera
        Vector3 playerPos = mainCamera.transform.position;

        //prende la posizione dell'npc
        Vector3 newDestination = NPC.transform.position;

        //modifico la destinazione in base alla posizione dell'npc
        newDestination = newDestination + (Vector3.left * 3.5f);//left va cambiato in base alla posizione degli npc nel ristornate
        newDestination = newDestination + (Vector3.up * 2.5f);

        //cambio la posizione della camera
        mainCamera.transform.position = Vector3.Lerp(playerPos, newDestination, 100f);
    }

    private void apriPannello()
    {
        if (pannelloCliente != null)
        {
            Vector3 newDestination = NPC.transform.position;
            pannelloCliente.transform.position = newDestination;

            pannelloCliente.SetActive(true);
            pannelloApertoChiuso();
        }
        if (!bottoniGenerati) { 
            generaBottoniPiatti(cliente);
            bottoniGenerati = true;
        }
        caricaClienteInPanello(cliente);

    }

    private void chiudiPannello()
    {
        if (pannelloCliente != null)
        {
            pannelloCliente.SetActive(false);
            pannelloApertoChiuso();
        }
    }
    
    private void generaBottoniPiatti(Cliente cliente)
    {
        //Button bottonePiattoPrefab = GameObject.FindGameObjectWithTag("BottonePiatto").GetComponent <Button>();
        GameObject bottonePiattoPrefab = GameObject.FindGameObjectWithTag("BottonePiatto");

        //bottonePiattoPrefab.gameObject.SetActive(false);

        GameObject pannelloPiatti = GameObject.FindGameObjectWithTag("PannelloPiatti");
        List <Button> bottoniPiatti = new List<Button> ();

        List<Piatto> piatti = Database.getDatabaseOggetto(new Piatto());

        foreach (Piatto piatto in piatti)
        {
            bottoniPiatti.Add(generaBottonePiatto(piatto, bottonePiattoPrefab));
        }

        foreach (Button bottonePiatto in bottoniPiatti)
        { 
            GameObject bottoneTemp = new GameObject();
            bottoneTemp = (Instantiate(bottonePiatto, pannelloPiatti.transform, false) as Button).gameObject;
            bottoneTemp.transform.SetParent(pannelloPiatti.transform);
            
            bottoneTemp.GetComponent<Button>().onClick.AddListener(() => {
                selezionaPiatto(bottoneTemp, piatti, cliente);
            });


            //in posizione 0 c'è il bottone per selezionare il piatto
            //e in posizione 1 c'è il bottone per vedere gli ingredienti
            Button bottoneMostraIngredienti = bottoneTemp.GetComponentsInChildren<Button>()[1];
            bottoneMostraIngredienti.onClick.AddListener(() => {
                chiudiPannello();
                print("ciao");
                cambiaPannelloIngredientiPiattoConPiatto(bottoneMostraIngredienti, piatti);
                apriPannelloIngredientiPiatto();
            });
        }

        Destroy(bottonePiattoPrefab);
    }

    void selezionaPiatto(GameObject bottone, List <Piatto> piatti, Cliente cliente)
    {
        chiudiPannello();
        
        foreach (Piatto piatto in piatti)
        {
            if (bottone.name.Contains(piatto.nome))//contains perché viene aggiunta la stringa "(Clone)" nel gameobject
            {
                piattoSelezionato = piatto;
                break;
            }
        }

        setPannelloConfermaConNomePiatto(piattoSelezionato.nome);
    }

    public void confermaPiattoDaBottone()
    {
        confermaSI = true;
        chiudiPannelloConfermaPiatto();
        esciDaInterazioneCliente();

        List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());

        bool affinitaPatologiePiatto = piattoSelezionato.checkAffinitaPatologiePiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.listaIdPatologie);
        bool affinitaDietaPiatto = piattoSelezionato.checkAffinitaDietaPiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.dieta);
        bool affinita = affinitaPatologiePiatto && affinitaDietaPiatto;
        float guadagno = piattoSelezionato.calcolaCostoConBonus(affinita, piattoSelezionato.calcolaCostoBase(databaseIngredienti));

        giocatore.guadagna(guadagno);

        giocatore.aggiungiDiminuisciPunteggio(affinita, piattoSelezionato.calcolaNutriScore(databaseIngredienti), piattoSelezionato.calcolaCostoEco(databaseIngredienti));

        animazioni(affinitaPatologiePiatto, affinitaDietaPiatto, guadagno);

        confermaSI = false;

        print(giocatore.soldi.ToString());
    }

    void setPannelloConfermaConNomePiatto (string nomePiatto)
    {
        //pannelloConfermaPiatto.GetComponentsInChildren<Button>()[0] = bottone si, in posizione 1 c'è quello del no
        apriPannelloConfermaPiatto();
        pannelloConfermaPiatto.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Sei sicuro di voler servire il piatto: \n" + nomePiatto;
    }

    void cambiaPannelloIngredientiPiattoConPiatto(Button bottoneMostraIngredienti, List <Piatto> piatti)
    {
        Piatto piattoSelezionato = new Piatto();
        foreach (Piatto piatto in piatti)
        {
            if (bottoneMostraIngredienti.name.Contains(piatto.nome))//contains perché viene aggiunta la stringa ingredienti nel nome del bottone
            {
                piattoSelezionato = piatto;
                break;
            }
        }

        string ingredientiPiatto = piattoSelezionato.getListaIngredientiQuantitaToString();
        
        //piatto
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingredienti nel piatto " + piattoSelezionato.nome + ":";
        //Ingredienti
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Ingredienti:\n" + piattoSelezionato.getListaIngredientiQuantitaToString ();
        /*
        //esc per uscire
        print(pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[2].text);
        */
    }

    void animazioni (bool affinitaPatologiePiatto, bool affinitaDietaPiatto, float guadagno)
    {
        //@zSavT qui puoi inserire le animazioni,
        //la bool affinitaPatologiePiatto è per sapere se il piatto andava bene per la patologia,
        //l'altra affinità è ovviamente per la dieta,
        //il guadagno sono i soldi che sta guadagnando il giocatore,
        //non so se vuoi mettere un'animazione per questa cosa e se nel caso la vuoi regolare per la quantità guadagnata ma nel caso ce l'hai,
        //se vuoi togliere l'unica chiamata a questo metodo è nel metodo selezionaPiatto quindi ti basta rimuovere il parametro da li
    }

    private Button generaBottonePiatto(Piatto piatto, GameObject bottonePiattoPrefab) 
    {
        GameObject outputGameObject = (GameObject)Instantiate(bottonePiattoPrefab);
        Button output = outputGameObject.GetComponent<Button>();

        output.GetComponentsInChildren<TextMeshProUGUI>() [0].text = piatto.nome;
        output.GetComponentsInChildren<TextMeshProUGUI>() [1].text = piatto.calcolaCostoBase().ToString();

        Sprite nuovaImmagine = Resources.Load<Sprite>("ImmaginePiatto1");//TODO aggiungere immagine in base al nome del piatto e nominare gli sprite delle immagini dei piatti con i nomi dei piatti
        output.GetComponentsInChildren<Image>()[1].sprite = nuovaImmagine;

        output.name = piatto.nome;


        //in posizione 0 c'è il bottone per selezionare il piatto
        //e in posizione 1 c'è il bottone per vedere gli ingredienti
        output.GetComponentsInChildren<Button>()[1].name = "Ingredienti " + piatto.nome;

        return output;
    }

    private void caricaClienteInPanello(Cliente cliente)
    {
        GameObject pannelloCliente = GameObject.FindGameObjectWithTag("PannelloCliente");

        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Utility.getStringaConCapitalLetterIniziale(cliente.nome);
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>() [1].text = "Dieta: " + Utility.getStringaConCapitalLetterIniziale(Dieta.IdDietaToDietaString(cliente.dieta));
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>() [2].text = Patologia.listIdToListPatologie(cliente.listaIdPatologie);
    }

    private void pannelloIngredientiPiattoApertoChiuso()
    {
        pannelloIngredientiPiattoAperto = !pannelloIngredientiPiattoAperto;
    }

    private void apriPannelloIngredientiPiatto()
    {
        if (pannelloIngredientiPiatto != null)
        {
            pannelloIngredientiPiatto.SetActive(true);
            pannelloIngredientiPiattoApertoChiuso();
        }
        
    }

    private void chiudiPannelloIngredientiPiatto()
    {
        if (pannelloIngredientiPiatto != null)
        {
            pannelloIngredientiPiatto.SetActive(false);
            pannelloIngredientiPiattoApertoChiuso();
        }
    }

    private void pannelloConfermaPiattoApertoChiuso()
    {
        pannelloConfermaPiattoAperto = !pannelloConfermaPiattoAperto;
    }

    private void apriPannelloConfermaPiatto()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(true);
            pannelloConfermaPiattoApertoChiuso();
        }

    }

    private void chiudiPannelloConfermaPiatto()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(false);
            pannelloConfermaPiattoApertoChiuso();
        }
    }

    public void chiudiPannelloConfermaPiattoDopoNO()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(false);
            pannelloConfermaPiattoApertoChiuso();
            apriPannello();
        }
    }

}