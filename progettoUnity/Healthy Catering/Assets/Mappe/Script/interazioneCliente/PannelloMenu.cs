using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

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
    [SerializeField] private GameObject pannelloMenu;
    [SerializeField] private GameObject pannelloCliente;

    [SerializeField] private GameObject bottonePiatto; //= GameObject.FindGameObjectWithTag("BottonePiatto");
    [SerializeField] private GameObject pannelloPiatti; //= GameObject.FindGameObjectWithTag("PannelloPiatti");

    [Header("Pannello ingredienti piatto")]
    [SerializeField] private GameObject pannelloIngredientiPiatto;
    public static bool pannelloIngredientiPiattoAperto;

    [Header("Pannello ingredienti giusti sbagliati")]
    [SerializeField] private GameObject pannelloIngredientiGiustiSbagliati;
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
    [SerializeField] private GameObject EscPerUscireTesto; //Lo imposto come GameObject e non come testo, perch� mi interessa solo attivarlo disattivarlo velocemente
    public UnityEvent chiusuraInterazioneCliente;


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

    //TODO divisione in metodi
    private void generaBottoniPiatti(Cliente cliente)
    {
        List<Piatto> piatti = Database.getDatabaseOggetto(new Piatto());
        List<Button> bottoniPiatti = new List<Button>();

        foreach (Piatto piatto in piatti)
        {
            bottoniPiatti.Add(generaBottonePiatto(piatto, bottonePiatto));
        }

        foreach (Button bottonePiatto in bottoniPiatti)
        {
            GameObject bottoneTemp = new GameObject();
            bottoneTemp = (Instantiate(bottonePiatto, pannelloPiatti.transform, false) as Button).gameObject;
            bottoneTemp.transform.SetParent(pannelloPiatti.transform);

            bottoneTemp.GetComponent<Button>().onClick.AddListener(() => {
                selezionaPiatto(bottoneTemp, piatti, cliente);
            });

            //in posizione 0 c'e' il bottone per selezionare il piatto
            //e in posizione 1 c'e' il bottone per vedere gli ingredienti
            Button bottoneMostraIngredienti = bottoneTemp.GetComponentsInChildren<Button>()[1];
            bottoneMostraIngredienti.onClick.AddListener(() => {
                cambiaPannelloIngredientiPiattoConPiatto(bottoneMostraIngredienti, piatti);
                apriPannelloIngredientiPiatto();
            });
        }

        Destroy(bottonePiatto);
    }

    private void selezionaPiatto(GameObject bottone, List<Piatto> piatti, Cliente cliente)
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

    public void confermaPiattoDaBottone()
    {
        List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());

        bool affinitaPatologiePiatto = piattoSelezionato.checkAffinitaPatologiePiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.listaIdPatologie);
        bool affinitaDietaPiatto = piattoSelezionato.checkAffinitaDietaPiatto(piattoSelezionato.listaIdIngredientiQuantita, cliente.dieta);
        bool affinita = affinitaPatologiePiatto && affinitaDietaPiatto;
        float guadagno = piattoSelezionato.calcolaCostoConBonus(affinita, piattoSelezionato.calcolaCostoBase(databaseIngredienti));

        chiudiPannelloConfermaPiatto();

        giocatore.guadagna(guadagno);
        giocatore.aggiungiDiminuisciPunteggio(affinita, piattoSelezionato.calcolaNutriScore(databaseIngredienti), piattoSelezionato.calcolaCostoEco(databaseIngredienti));
        
        if (!affinita)
        {
            caricaIngredientiInPannelloIngredientiGiustiSbagliati(piattoSelezionato, cliente, databaseIngredienti);
            apriPannelloIngredientiGiustiSbagliati();
        }
        else
        {
            clienteServito = true;
            chiusuraInterazioneCliente.Invoke();
        }
        livelloProgresso.servitoCliente(giocatore.punteggio);
        animazioni(affinitaPatologiePiatto, affinitaDietaPiatto, guadagno);
    }

    private void setPannelloConfermaConNomePiatto(string nomePiatto)
    {
        apriPannelloConfermaPiatto();
        testoConfermaPiatto.text = "Sei sicuro di voler servire il piatto: \n" + Utility.colorePiatti + nomePiatto + Utility.fineColore;
    }

    private void cambiaPannelloIngredientiPiattoConPiatto(Button bottoneMostraIngredienti, List<Piatto> piatti)
    {
        Piatto piattoSelezionato = Piatto.getPiattoFromNomeBottone(bottoneMostraIngredienti.name, piatti);

        string ingredientiPiatto = piattoSelezionato.getListaIngredientiQuantitaToString();

        //piatto
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingredienti nel piatto " + Utility.colorePiatti + piattoSelezionato.nome + Utility.fineColore;
        //Ingredienti
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Ingredienti:\n" + Utility.coloreIngredienti + ingredientiPiatto + Utility.fineColore;
    }

    private void animazioni(bool affinitaPatologiePiatto, bool affinitaDietaPiatto, float guadagno)
    {
        if(affinitaPatologiePiatto && affinitaDietaPiatto)
        {
            controllerAnimazioneCliente.animazioneContenta();
        }
    }

    private Button generaBottonePiatto(Piatto piatto, GameObject bottonePiatto)
    {
        GameObject outputGameObject = (GameObject)Instantiate(bottonePiatto);

        Button output = outputGameObject.GetComponent<Button>();
        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Utility.colorePiatti + piatto.nome + Utility.fineColore;
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

        return output;
    }

    private void caricaClienteInPanello(Cliente cliente)
    {
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Utility.getStringaConCapitalLetterIniziale(cliente.nome);
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Dieta: " + Utility.coloreDieta + Utility.getStringaConCapitalLetterIniziale(Dieta.IdDietaToDietaString(cliente.dieta)) + Utility.fineColore;
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = "Patologie: " + Utility.colorePatologia + Patologia.listIdToListPatologie(cliente.listaIdPatologie) + Utility.fineColore;
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

    private void caricaIngredientiInPannelloIngredientiGiustiSbagliati(Piatto piattoSelezionato, Cliente cliente, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente());

        titoloIngredientiGiustiSbagliati.text = "Ingredienti del piatto " + Utility.colorePiatti + piattoSelezionato.nome + Utility.fineColore +"\nche vanno bene e non per: " + Utility.getStringaConCapitalLetterIniziale(cliente.nome);

        List<Ingrediente> ingredientiPiattoSelezionato = piattoSelezionato.getIngredientiPiatto(databaseIngredienti);

        //ogni volta che calcolo gli ingredienti non compatibili li rimuovo dalla lista generale degli ingredienti
        //in questo modo non vengono aggiunti alle altre liste a prescindere
        List<Ingrediente> ingredientiNonCompatibiliPatologia = cliente.getListaIngredientiNonCompatibiliPatologie(ingredientiPiattoSelezionato, databaseIngredienti);
        foreach (Ingrediente item in ingredientiNonCompatibiliPatologia) ingredientiPiattoSelezionato.Remove(item);
        
        List<Ingrediente> ingredientiNonCompatibiliDieta = cliente.getListaIngredientiNonCompatibiliDieta(ingredientiPiattoSelezionato, databaseIngredienti);
        foreach (Ingrediente item in ingredientiNonCompatibiliDieta) ingredientiPiattoSelezionato.Remove(item);
        
        //nella lista degli ingredienti piatto selezionato ci sono solo gli ingredienti che vanno bene ora
        List<Ingrediente> ingredientiCompatibili = ingredientiPiattoSelezionato;

        testoIngredientiGiusti.color = new Color32(182, 216, 156, 255);
        testoIngredientiGiusti.text = Ingrediente.listIngredientiToStringa (ingredientiCompatibili);

        testoIngredientiSbagliatiDieta.color = new Color32(255, 102, 102, 255);
        testoIngredientiSbagliatiDieta.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliDieta);

        testoIngredientiSbagliatiPatologia.color = new Color32(255, 102, 102, 255);
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
