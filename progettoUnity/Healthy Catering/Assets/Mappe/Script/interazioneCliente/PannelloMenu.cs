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

    [SerializeField] private GameObject bottonePiatto;
    [SerializeField] private GameObject pannelloPiatti;
    private bool pannelloMenuAperto;

    [Header("Pannello ingredienti piatto")]
    [SerializeField] private GameObject pannelloIngredientiPiatto;
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
    [SerializeField] private GameObject EscPerUscireTesto; //Lo imposto come GameObject e non come testo, perch� mi interessa solo attivarlo disattivarlo velocemente
    public UnityEvent chiusuraInterazioneCliente;
    private List<string> blackListPiatti = new List<string>();

    [Header("Gestione aggiornamento piatti")]
    private Button[] bottoniPiatti;
    List<Piatto> piatti;

    void Start()
    {
        blackListPiatti.Add("Frittura di pesce");
        blackListPiatti.Add("Patatine fritte");
        clienteServito = false;
        pannelloIngredientiPiatto.SetActive(false);
        pannelloConfermaPiatto.SetActive(false);
        pannelloIngredientiGiustiSbagliati.SetActive(false);
        piatti = Database.getDatabaseOggetto(new Piatto());
        generaBottoniPiatti();
    }

    void Update()
    {
        if (pannelloIngredientiPiattoAperto)
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
        pannelloMenuAperto = false;
    }

    public bool getPannelloMenuClienteAperto()
    {
        return pannelloMenuAperto;
    }

    public void apriPannelloMenuCliente()
    {
        pannelloMenuAperto = true;
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
        List<Button> bottoniPiattiTemp = new List<Button>();
        bottoniPiatti = new Button[piatti.Count];

        foreach (Piatto piatto in piatti)
        {
            bottoniPiattiTemp.Add(generaBottonePiatto(piatto, bottonePiatto));
        }
        Destroy(bottonePiatto);

        int i = 0;
        foreach (Button bottonePiatto in bottoniPiattiTemp)
        {
            Button bottoneTemp;
            bottoneTemp = (Instantiate(bottonePiatto, pannelloPiatti.transform, false) as Button);
            bottoneTemp.transform.SetParent(pannelloPiatti.transform);

            bottoneTemp.GetComponent<Button>().onClick.AddListener(() => {
                selezionaPiatto(bottoneTemp, piatti, cliente);
            });

            //in posizione 0 c'e' il bottone per selezionare il piatto
            //e in posizione 1 c'e' il bottone per vedere gli ingredienti
            Button bottoneMostraIngredienti = bottoneTemp.GetComponentsInChildren<Button>()[1];
            bottoneMostraIngredienti.onClick.AddListener(() => {
                cambiaPannelloIngredientiPiattoConPiatto(bottoneMostraIngredienti);
                apriPannelloIngredientiPiatto();
            });

            bottoniPiatti[i] = bottoneTemp;
            i++;
        }

        aggiornaBottoniPiatti();
    }

    private void aggiornaBottoniPiatti()
    {
        if (bottoniPiatti != null)
        {
            List<Button> piattiDisponibili = new List<Button>();
            List<Button> piattiNonDisponibili = new List<Button>();

            int i = 0;
            foreach (Button bottonePiatto in bottoniPiatti)
            {

                bottonePiatto.transform.SetParent(null, true);

                if (!(Piatto.nomeToPiatto(bottonePiatto.name)).piattoInInventario(giocatore.inventario))
                {
                    bottonePiatto.interactable = false;
                    piattiNonDisponibili.Add(bottonePiatto);
                }
                else
                {
                    bottonePiatto.interactable = true;
                    piattiDisponibili.Add(bottonePiatto);
                }
                i++;
            }

            piattiDisponibili.AddRange(piattiNonDisponibili);
            bottoniPiatti = piattiDisponibili.ToArray();

            foreach (Button temp in bottoniPiatti)
            {
                temp.transform.SetParent(pannelloPiatti.transform);
            }
        }

    }

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
            if (cliente.listaIdPatologie.Contains(0) || cliente.listaIdPatologie.Contains(1))
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
        animazioni(affinitaPatologiePiatto, affinitaDietaPiatto, guadagno);
        aggiornaBottoniPiatti();
    }

    private void setPannelloConfermaConNomePiatto(string nomePiatto)
    {
        apriPannelloConfermaPiatto();
        testoConfermaPiatto.text = "Sei sicuro di voler servire il piatto: \n" + Costanti.colorePiatti + nomePiatto + Costanti.fineColore;
    }

    private void cambiaPannelloIngredientiPiattoConPiatto(Button bottoneMostraIngredienti)
    {
        Piatto piattoSelezionato = Piatto.getPiattoFromNomeBottone(bottoneMostraIngredienti.name);

        string ingredientiPiatto = piattoSelezionato.getListaIngredientiQuantitaToString();

        //piatto
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingredienti nel piatto " + Costanti.colorePiatti + piattoSelezionato.nome + Costanti.fineColore;
        //Ingredienti
        pannelloIngredientiPiatto.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Ingredienti:\n" + Costanti.coloreIngredienti + ingredientiPiatto + Costanti.fineColore;
    }

    private void animazioni(bool affinitaPatologiePiatto, bool affinitaDietaPiatto, float guadagno)
    {
        if (affinitaPatologiePiatto && affinitaDietaPiatto)
        {
            controllerAnimazioneCliente.animazioneContenta();
        }
    }

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

        return output;
    }

    private void caricaClienteInPanello(Cliente cliente)
    {
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[0].text = Costanti.grassetto + Utility.getStringaConCapitalLetterIniziale(cliente.nome) + Costanti.fineGrassetto;
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[1].text = Costanti.grassetto + "Dieta: " + Costanti.fineGrassetto + Costanti.coloreDieta + Utility.getStringaConCapitalLetterIniziale(Dieta.IdDietaToDietaString(cliente.dieta)) + Costanti.fineColore;
        pannelloCliente.GetComponentsInChildren<TextMeshProUGUI>()[2].text = Costanti.grassetto + "Patologie: " + Costanti.fineGrassetto + Costanti.colorePatologia + Patologia.listIdToListPatologie(cliente.listaIdPatologie) + Costanti.fineColore;
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
            suonoVoceNegativo.PlayDelayed(0.1f);
            pannelloIngredientiGiustiSbagliati.SetActive(true);
            pannelloIngredientiGiustiSbagliatiApertoChiuso();
            chiudiMenuCliente();
        }

    }

    private void caricaIngredientiInPannelloIngredientiGiustiSbagliati(Piatto piattoSelezionato, Cliente cliente, bool piattoInBlackList)
    {
        titoloIngredientiGiustiSbagliati.text = "Compatibilità ingredienti del piatto " + Costanti.colorePiatti + piattoSelezionato.nome + Costanti.fineColore + "\nper il cliente " + Utility.getStringaConCapitalLetterIniziale(cliente.nome);

        List<Ingrediente> ingredientiPiattoSelezionato = piattoSelezionato.getIngredientiPiatto();

        //ogni volta che calcolo gli ingredienti non compatibili li rimuovo dalla lista generale degli ingredienti
        //in questo modo non vengono aggiunti alle altre liste a prescindere
        List<Ingrediente> ingredientiNonCompatibiliPatologia = cliente.getListaIngredientiNonCompatibiliPatologie(ingredientiPiattoSelezionato);
        foreach (Ingrediente item in ingredientiNonCompatibiliPatologia) ingredientiPiattoSelezionato.Remove(item);

        List<Ingrediente> ingredientiNonCompatibiliDieta = cliente.getListaIngredientiNonCompatibiliDieta(ingredientiPiattoSelezionato);
        foreach (Ingrediente item in ingredientiNonCompatibiliDieta) ingredientiPiattoSelezionato.Remove(item);

        //nella lista degli ingredienti piatto selezionato ci sono solo gli ingredienti che vanno bene ora
        List<Ingrediente> ingredientiCompatibili = ingredientiPiattoSelezionato;

        testoIngredientiGiusti.color = new Color32(104, 176, 60, 255);
        testoIngredientiGiusti.text = Ingrediente.listIngredientiToStringa(ingredientiCompatibili);

        testoIngredientiSbagliatiDieta.color = new Color32(255, 8, 10, 255);
        testoIngredientiSbagliatiDieta.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliDieta);
        testoIngredientiSbagliatiPatologia.color = new Color32(255, 8, 10, 255);
        if (piattoInBlackList)
        {
            testoIngredientiSbagliatiPatologia.text = "La frittura non è adatta alla patologia del cliente.";
        }
        else
        {
            testoIngredientiSbagliatiPatologia.text = Ingrediente.listIngredientiToStringa(ingredientiNonCompatibiliPatologia);
        }
    }

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
