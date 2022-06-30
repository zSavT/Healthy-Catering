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
    [SerializeField] private GameObject pannelloPrincipaleMenuCliente;
    [SerializeField] private GameObject pannelloMenu;
    [SerializeField] private GameObject pannelloCliente;

    [SerializeField] private GameObject bottonePiatto; //= GameObject.FindGameObjectWithTag("BottonePiatto");
    [SerializeField] private GameObject pannelloPiatti; //= GameObject.FindGameObjectWithTag("PannelloPiatti");

    [Header("Pannello ingredienti piatto")]
    [SerializeField] private GameObject pannelloIngredientiPiatto;
    public static bool pannelloIngredientiPiattoAperto;

    [Header("Pannello ingredienti giusti sbagliati")]
    [SerializeField] private GameObject pannelloIngredientiGiustiSbagliati;
    [SerializeField] private AudioSource suonoPiattoScorretto;
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

    [Header("Gestione aggiornamento piatti")]
    private Button[] bottoniPiatti; 
    List<Piatto> piatti;
    private bool pannelloAggiornato;

    void Start()
    {
        clienteServito = false;
        pannelloIngredientiPiatto.SetActive(false);
        pannelloConfermaPiatto.SetActive(false);
        pannelloIngredientiGiustiSbagliati.SetActive(false);
        piatti = Database.getDatabaseOggetto(new Piatto());
        generaBottoniPiatti();
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

    private void apriMenuCliente()
    {
        pannelloMenu.SetActive(true);
        pannelloCliente.SetActive(true);
    }

    private void chiudiMenuCliente()
    {
        pannelloMenu.SetActive(false);
        pannelloCliente.SetActive(false);
    }

    public void apriPannelloMenuCliente()
    {
        pannelloPrincipaleMenuCliente.SetActive(true);
        apriMenuCliente();
    }

    public void ChiudiPannelloMenuCliente()
    {
        pannelloPrincipaleMenuCliente.SetActive(false);
        chiudiMenuCliente();
    }

    public void setCliente(int idClientePuntato, Player giocatorePartita, Interactable controlleNPCPuntato)
    {
        apriPannelloMenuCliente();
        cliente = Database.getDatabaseOggetto(new Cliente())[idClientePuntato];
        giocatore = giocatorePartita;
        controllerAnimazioneCliente = controlleNPCPuntato;
        caricaClienteInPanello(cliente);
        aggiornaBottoniPiatti();         // in questo momento i bottoniPiatti non sono ancora stati popolati e quindi questa chiamata fallisce. 
    }

    private void generaBottoniPiatti()
    {
        List <Button> bottoniPiattiTemp = new List<Button>();
        bottoniPiatti = new Button[piatti.Count];

        foreach (Piatto piatto in piatti)
        {
            bottoniPiattiTemp.Add(generaBottonePiatto(piatto, bottonePiatto));
        }

        int i = 0;
        foreach (Button bottonePiatto in bottoniPiattiTemp)
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

            bottoniPiatti[i] = bottoneTemp.GetComponent<Button>();
            i++;
        }

        Destroy(bottonePiatto);

        aggiornaBottoniPiatti();
    }

    private void aggiornaBottoniPiatti()
    {
        if(bottoniPiatti != null)
        {
            int i = 0;
            foreach (Button bottonePiatto in bottoniPiatti)
            {
                if (!piatti[i].piattoInInventario(giocatore.inventario))
                {
                    bottonePiatto.interactable = false;
                }
                else
                {
                    bottonePiatto.interactable = true;
                }
                i++;
            }
        }

        print(giocatore.stampaInventario());
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
        giocatore.aggiungiDiminuisciPunteggio(affinita, piattoSelezionato.calcolaNutriScore(databaseIngredienti), piattoSelezionato.calcolaCostoEco(databaseIngredienti), PlayerSettings.livelloSelezionato);
        giocatore.aggiornaInventario(piattoSelezionato.listaIdIngredientiQuantita, false);

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
        livelloProgresso.servitoCliente(giocatore.punteggio[PlayerSettings.livelloSelezionato]);
        animazioni(affinitaPatologiePiatto, affinitaDietaPiatto, guadagno);

        aggiornaBottoniPiatti();
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
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Utility.grassetto + Utility.getStringaConCapitalLetterIniziale(cliente.nome) + Utility.fineGrassetto;
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[1].text = Utility.grassetto + "Dieta: " + Utility.fineGrassetto + Utility.coloreDieta + Utility.getStringaConCapitalLetterIniziale(Dieta.IdDietaToDietaString(cliente.dieta)) + Utility.fineColore;
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = Utility.grassetto + "Patologie: " + Utility.fineGrassetto + Utility.colorePatologia + Patologia.listIdToListPatologie(cliente.listaIdPatologie) + Utility.fineColore;
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
            chiudiMenuCliente();
        }

    }

    private void chiudiPannelloIngredientiPiatto()
    {
        if (pannelloIngredientiPiatto != null)
        {
            pannelloIngredientiPiatto.SetActive(false);
            pannelloIngredientiPiattoApertoChiuso();
            apriMenuCliente();
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
            chiudiMenuCliente();
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
            apriMenuCliente();
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
            EscPerUscireTesto.SetActive(true);
            apriMenuCliente();
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
            suonoPiattoScorretto.Play();
            pannelloIngredientiGiustiSbagliati.SetActive(true);
            pannelloIngredientiGiustiSbagliatiApertoChiuso();
            chiudiMenuCliente();
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

        testoIngredientiGiusti.color = new Color32(104, 176, 60, 255);
        testoIngredientiGiusti.text = Ingrediente.listIngredientiToStringa (ingredientiCompatibili);

        testoIngredientiSbagliatiDieta.color = new Color32(255, 8, 10, 255);
        testoIngredientiSbagliatiDieta.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliDieta);

        testoIngredientiSbagliatiPatologia.color = new Color32(255, 8, 10, 255);
        testoIngredientiSbagliatiPatologia.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliPatologia);
    }

    private void chiudiPannelloIngredientiGiustiSbagliati()
    {
        if (pannelloIngredientiGiustiSbagliati != null)
        {
            pannelloIngredientiGiustiSbagliati.SetActive(false);
            pannelloIngredientiGiustiSbagliatiApertoChiuso();
            apriMenuCliente();
            controllerAnimazioneCliente.animazioneCamminata();
            clienteServito = true;
            chiusuraInterazioneCliente.Invoke();
            controllerAnimazioneCliente.animazioneScontenta();
        }
    }
}
