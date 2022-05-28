using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PannelloMenu : MonoBehaviour
{

    //TODO prendere il player che sta giocando al posto del primo nel database
    private Player giocatore;

    [SerializeField] private GameObject pannelloIngredientiPiatto;
    public static bool pannelloIngredientiPiattoAperto;

    [SerializeField] private GameObject pannelloIngredientiGiustiSbagliati;
    public static bool pannelloIngredientiGiustiSbagliatiAperto;

    //TODO aggiornare con il cliente vero
    private Cliente cliente;

    [SerializeField] GameObject pannelloConfermaPiatto;
    public static bool pannelloConfermaPiattoAperto;

    private Piatto piattoSelezionato;
    [SerializeField] private GameObject pannelloCliente;
    [SerializeField] private GameObject pannelloMenu;

    //DA EVENTUALMENTE TOGLIERE

    [SerializeField] private GameObject Player;
    [SerializeField] private TextMeshProUGUI testoConfermaPiatto;
    [SerializeField] private GameObject EscPerUscireTesto; //Lo imposto come GameObject e non come testo, perch� mi interessa solo attivarlo disattivarlo velocemente
    private Interactable controllerAnimazioneCliente;

    //PannelloIngredientiGiustiSbagliati 
    [SerializeField] private TextMeshProUGUI titoloIngredientiGiustiSbagliati;
    [SerializeField] private TextMeshProUGUI testoIngredientiGiusti;
    [SerializeField] private TextMeshProUGUI testoIngredientiSbagliatiDieta;
    [SerializeField] private TextMeshProUGUI testoIngredientiSbagliatiPatologia;

    public UnityEvent chiusuraInterazioneCliente;
    public static bool clienteServito;

    void Start()
    {
        clienteServito = false;
        pannelloIngredientiPiatto.SetActive(false);
        pannelloConfermaPiatto.SetActive(false);
        pannelloIngredientiGiustiSbagliati.SetActive(false);
        generaBottoniPiatti(cliente);
    }

    void Update()
    {
        if(pannelloIngredientiPiattoAperto)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                chiudiPannelloIngredientiPiatto();
            }
        }

        if (pannelloIngredientiGiustiSbagliatiAperto)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                chiudiPannelloIngredientiGiustiSbagliati();
            }
        }
    }

    public void setCliente(int idClientePuntato, Player giocatorePartita, Interactable controlleNPCPuntato)
    {
        cliente = Database.getDatabaseOggetto(new Cliente())[idClientePuntato];
        giocatore = giocatorePartita;
        controllerAnimazioneCliente = controlleNPCPuntato;

        caricaClienteInPanello(cliente);
    }

    private void generaBottoniPiatti(Cliente cliente)
    {
        //Button bottonePiattoPrefab = GameObject.FindGameObjectWithTag("BottonePiatto").GetComponent <Button>();
        GameObject bottonePiattoPrefab = GameObject.FindGameObjectWithTag("BottonePiatto");

        //bottonePiattoPrefab.gameObject.SetActive(false);

        GameObject pannelloPiatti = GameObject.FindGameObjectWithTag("PannelloPiatti");
        List<Button> bottoniPiatti = new List<Button>();

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


            //in posizione 0 c'� il bottone per selezionare il piatto
            //e in posizione 1 c'� il bottone per vedere gli ingredienti
            Button bottoneMostraIngredienti = bottoneTemp.GetComponentsInChildren<Button>()[1];
            bottoneMostraIngredienti.onClick.AddListener(() => {
                cambiaPannelloIngredientiPiattoConPiatto(bottoneMostraIngredienti, piatti);
                apriPannelloIngredientiPiatto();
            });
        }

        Destroy(bottonePiattoPrefab);
    }

    void selezionaPiatto(GameObject bottone, List<Piatto> piatti, Cliente cliente)
    {
        foreach (Piatto piatto in piatti)
        {
            if (bottone.name.Contains(piatto.nome))//contains perch� viene aggiunta la stringa "(Clone)" nel gameobject
            {
                piattoSelezionato = piatto;
                break;
            }
        }

        setPannelloConfermaConNomePiatto(piattoSelezionato.nome);
    }

    public void confermaPiattoDaBottone()
    {
        chiudiPannelloConfermaPiatto();

        List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());

        bool affinitaPatologiePiatto = piattoSelezionato.checkAffinitaPatologiePiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.listaIdPatologie);
        bool affinitaDietaPiatto = piattoSelezionato.checkAffinitaDietaPiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.dieta);
        bool affinita = affinitaPatologiePiatto && affinitaDietaPiatto;
        float guadagno = piattoSelezionato.calcolaCostoConBonus(affinita, piattoSelezionato.calcolaCostoBase(databaseIngredienti));

        giocatore.guadagna(guadagno);

        giocatore.aggiungiDiminuisciPunteggio(affinita, piattoSelezionato.calcolaNutriScore(databaseIngredienti), piattoSelezionato.calcolaCostoEco(databaseIngredienti));
        if (!affinita)
        {
            caricaIngredientiInPannelloIngredientiGiustiSbagliati(piattoSelezionato, cliente, databaseIngredienti);
            apriPannelloIngredientiGiustiSbagliati();
        }else
        {
            clienteServito = true;
            chiusuraInterazioneCliente.Invoke();
        }
        animazioni(affinitaPatologiePiatto, affinitaDietaPiatto, guadagno);
    }

    void setPannelloConfermaConNomePiatto(string nomePiatto)
    {
        //pannelloConfermaPiatto.GetComponentsInChildren<Button>()[0] = bottone si, in posizione 1 c'� quello del no
        apriPannelloConfermaPiatto();
        testoConfermaPiatto.text = "Sei sicuro di voler servire il piatto: \n" + nomePiatto;
    }

    void cambiaPannelloIngredientiPiattoConPiatto(Button bottoneMostraIngredienti, List<Piatto> piatti)
    {
        Piatto piattoSelezionato = Piatto.getPiattoFromNomeBottone(bottoneMostraIngredienti.name, piatti);

        string ingredientiPiatto = piattoSelezionato.getListaIngredientiQuantitaToString();

        //piatto
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingredienti nel piatto " + piattoSelezionato.nome + ":";
        //Ingredienti
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Ingredienti:\n" + ingredientiPiatto;
    }

    void animazioni(bool affinitaPatologiePiatto, bool affinitaDietaPiatto, float guadagno)
    {
        //@zSavT qui puoi inserire le animazioni,
        //la bool affinitaPatologiePiatto � per sapere se il piatto andava bene per la patologia,
        //l'altra affinit� � ovviamente per la dieta,
        //il guadagno sono i soldi che sta guadagnando il giocatore,
        //non so se vuoi mettere un'animazione per questa cosa e se nel caso la vuoi regolare per la quantit� guadagnata ma nel caso ce l'hai,
        //se vuoi togliere l'unica chiamata a questo metodo � nel metodo selezionaPiatto quindi ti basta rimuovere il parametro da li
        if(affinitaPatologiePiatto && affinitaDietaPiatto)
        {
            controllerAnimazioneCliente.animazioneContenta();
        } else
        {
            
        }
        
    }

    private Button generaBottonePiatto(Piatto piatto, GameObject bottonePiattoPrefab)
    {
        GameObject outputGameObject = (GameObject)Instantiate(bottonePiattoPrefab);


        Button output = outputGameObject.GetComponent<Button>();
        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = piatto.nome;
        output.GetComponentsInChildren<TextMeshProUGUI>()[1].text = piatto.calcolaCostoBase().ToString();

        Sprite nuovaImmagine = Resources.Load<Sprite>("ImmaginePiatto1");//TODO aggiungere immagine in base al nome del piatto e nominare gli sprite delle immagini dei piatti con i nomi dei piatti
        output.GetComponentsInChildren<Image>()[1].sprite = nuovaImmagine;

        output.name = piatto.nome;


        //in posizione 0 c'� il bottone per selezionare il piatto
        //e in posizione 1 c'� il bottone per vedere gli ingredienti
        output.GetComponentsInChildren<Button>()[1].name = "Ingredienti " + piatto.nome;

        return output;
    }

    private void caricaClienteInPanello(Cliente cliente)
    {
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Utility.getStringaConCapitalLetterIniziale(cliente.nome);
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Dieta: " + Utility.getStringaConCapitalLetterIniziale(Dieta.IdDietaToDietaString(cliente.dieta));
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = Patologia.listIdToListPatologie(cliente.listaIdPatologie);
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
            pannelloMenu.SetActive(false);
            pannelloCliente.SetActive(false);
        }

    }

    private void chiudiPannelloIngredientiPiatto()
    {
        if (pannelloIngredientiPiatto != null)
        {
            pannelloIngredientiPiatto.SetActive(false);
            pannelloIngredientiPiattoApertoChiuso();
            pannelloMenu.SetActive(true);
            pannelloCliente.SetActive(true);
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
            pannelloMenu.SetActive(false);
            pannelloCliente.SetActive(false);
            pannelloConfermaPiattoApertoChiuso();
            EscPerUscireTesto.SetActive(false);
        }

    }

    private void chiudiPannelloConfermaPiatto()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(false);
            pannelloConfermaPiattoApertoChiuso();
            pannelloMenu.SetActive(true);
            pannelloCliente.SetActive(true);
            EscPerUscireTesto.SetActive(true);
            controllerAnimazioneCliente.animazioneCamminata();
        }
    }

    public void chiudiPannelloConfermaPiattoDopoNO()
    {
        if (pannelloConfermaPiatto != null)
        {
            pannelloConfermaPiatto.SetActive(false);
            pannelloConfermaPiattoApertoChiuso();
            pannelloMenu.SetActive(true);
            EscPerUscireTesto.SetActive(true);
            pannelloCliente.SetActive(true);
        }
    }

    private void pannelloIngredientiGiustiSbagliatiApertoChiuso()
    {
        pannelloIngredientiGiustiSbagliatiAperto = !pannelloIngredientiGiustiSbagliatiAperto;
    }

    private void apriPannelloIngredientiGiustiSbagliati()
    {
        if (pannelloIngredientiGiustiSbagliati != null)
        {
            
            pannelloIngredientiGiustiSbagliati.SetActive(true);
            pannelloIngredientiGiustiSbagliatiApertoChiuso();
            pannelloMenu.SetActive(false);
            pannelloCliente.SetActive(false);
        }

    }

    void caricaIngredientiInPannelloIngredientiGiustiSbagliati(Piatto piattoSelezionato, Cliente cliente, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente());

        titoloIngredientiGiustiSbagliati.text = "Ingredienti del piatto " + piattoSelezionato.nome + "\nche vanno bene e non per: " + cliente.nome;

        List<Ingrediente> ingredientiPiattoSelezionato = piattoSelezionato.getIngredientiPiatto(databaseIngredienti);

        //ogni volta che calcolo gli ingredienti non compatibili li rimuovo dalla lista generale degli ingredienti
        //in questo modo non vengono aggiunti alle altre liste a prescindere
        List<Ingrediente> ingredientiNonCompatibiliPatologia = cliente.getListaIngredientiNonCompatibiliPatologie(ingredientiPiattoSelezionato, databaseIngredienti);
        foreach (Ingrediente item in ingredientiNonCompatibiliPatologia) ingredientiPiattoSelezionato.Remove(item);
        
        List<Ingrediente> ingredientiNonCompatibiliDieta = cliente.getListaIngredientiNonCompatibiliDieta(ingredientiPiattoSelezionato, databaseIngredienti);
        foreach (Ingrediente item in ingredientiNonCompatibiliDieta) ingredientiPiattoSelezionato.Remove(item);
        
        //nella lista degli ingredienti piatto selezionato ci sono solo gli ingredienti che vanno bene ora
        List<Ingrediente> ingredientiCompatibili = ingredientiPiattoSelezionato;

        testoIngredientiGiusti.color = Color.green;
        testoIngredientiGiusti.text = Ingrediente.listIngredientiToStringa (ingredientiCompatibili);

        testoIngredientiSbagliatiDieta.color = Color.red;
        testoIngredientiSbagliatiDieta.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliDieta);

        testoIngredientiSbagliatiPatologia.color = Color.red;
        testoIngredientiSbagliatiPatologia.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliPatologia);
    }

    private void chiudiPannelloIngredientiGiustiSbagliati()
    {
        if (pannelloIngredientiGiustiSbagliati != null)
        {
            pannelloIngredientiGiustiSbagliati.SetActive(false);
            pannelloIngredientiGiustiSbagliatiApertoChiuso();
            pannelloCliente.SetActive(true);
            pannelloMenu.SetActive(true);
            controllerAnimazioneCliente.animazioneCamminata();
            clienteServito = true;
            chiusuraInterazioneCliente.Invoke();
            controllerAnimazioneCliente.animazioneScontenta();
        }
    }
}
