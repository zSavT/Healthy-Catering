using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuAiuto : MonoBehaviour
{
    private bool pannelloMenuAiutoAperto;

    [SerializeField] private GameObject pannelloMenuAiuto;
    [SerializeField] private TextMeshProUGUI titoloTestoAiuto;
    [SerializeField] private TextMeshProUGUI testoAiuto;
    [SerializeField] private Button tastoAvanti;
    [SerializeField] private Button tastoIndietro;
    [SerializeField] private TextMeshProUGUI testoNumeroPannelloAttuale;
    [SerializeField] private Image immagineSchermata;

    private int ultimaPosizione = 0;//cosi la prima volta apre il primo messaggio

    private List<string> titoliMessaggiDiAiuto = new List<string>
    {
        "Comandi Movimento del gioco:",
        "Interazione con i clienti:",
        "Scelta del piatto - Parte 1:",
        "Scelta del piatto - Parte 2:",
        "Gestione magazzino:",
        "Come vengono calcolati I bonus:",
        "Interazione con gli NPC:",
        "Rifornirsi di ingredienti dal negozio:",
        "Visionare il ricettario:",
        "Criteri di vittoria e game Over:",
    };
    
    private List<string> messaggiDiAiuto = new List<string>
    {
        "Per potersi muovere all'interno del gioco, bisogna utilizzare i tasti \"" + Utility.coloreVerde + "W" + Utility.fineColore + "\", \"" + Utility.coloreVerde + "A" + Utility.fineColore + "\", \"" + Utility.coloreVerde + "S" + Utility.fineColore + "\", " + Utility.coloreVerde + "D" + Utility.fineColore + "\" per muoversi rispettivamente in \"" +Utility.coloreVerde + "Avanti" + Utility.fineColore + "\", \"" + Utility.coloreVerde + "Sinistra" + Utility.fineColore + "\", \"" + Utility.coloreVerde + "Indietro" + Utility.fineColore + "\", \"" + Utility.coloreVerde + "Destra" + Utility.fineColore + "\". In più premendo il tasto \"" + Utility.coloreVerde + "Spazio" + Utility.fineColore + "\" è possibile saltare. Premendo il tasto \"" + Utility.coloreVerde + "Shift" + Utility.fineColore + "\" è possibile correre.",
        "Per avviare l'interazione con un cliente al bancone, inquadralo e premi il tasto \"" + Utility.coloreVerde + "E" + Utility.fineColore + "\". Per poter interagire con i clienti si dovrà utilizzare il mouse e selezionare il " + Utility.colorePiatti + "piatto" + Utility.fineColore + " che si vuole servire al cliente quando richiesto attraverso la relativa schermata.",
        "Scegliere il piatto migliore fra quelli disponibili, permette al giocatore di aumentare il suo denaro e il suo punteggio così da poter superare il livello. Nel caso si dovesse servire ad un <color=#B5D99C>cliente</color> con una " + Utility.colorePatologia + "patologia" + Utility.fineColore + " X un " + Utility.colorePiatti+ "piatto" + Utility.fineColore + " dove è presente un " + Utility.coloreIngredienti + "ingrediente" + Utility.fineColore + " non compatibile con essa verrà mostrato un pop up dal quale sarà possibile visualizzare quali degli " + Utility.coloreIngredienti + "ingrediente" + Utility.fineColore + " del " + Utility.colorePiatti + "piatto" + Utility.fineColore + " sono compatibili con la " + Utility.colorePatologia + "patologia" + Utility.fineColore + " e quali no.",
        "Stesso discorso per un " + Utility.colorePiatti+ "piatto" + Utility.fineColore + " non compatibile con la " + Utility.coloreDieta + "dieta" + Utility.fineColore + " del " + Utility.coloreVerde + "cliente" + Utility.fineColore + ". Servire un " + Utility.colorePiatti+ "piatto" + Utility.fineColore +" non idoneo comporta delle penalità al punteggio e non si riceveranno bonus in denaro. I bonus e i malus vengono calcolati in base alla compatibilità del "+ Utility.colorePiatti+ "piatto" + Utility.fineColore + " e ai suoi volori <color=#B5D99C>nutriScore</color> e <color=#B5D99C>costoEco</color>.",
        "Sarà possibile scegliere solo i " + Utility.colorePiatti + "piatti" + Utility.fineColore + " per i quali " + Utility.coloreVerde + "sono disponibili tutti gli ingredienti nelle quantità necessarie" + Utility.fineColore + "; quindi, si dovrà tenere conto degli ingredienti disponibili nel proprio magazzino e comprare gli ingredienti mancanti. É possibile visionoare lo stato del magazzino aprendo il programma \"" + Utility.coloreVerde + "MyInventory" + Utility.fineColore + "\", accessibile nel computer presente nell'ufficio del ristorante.",
        "I bonus della vendita " + Utility.colorePiatti + "piatto" + Utility.fineColore + " è calcolato nel seguente modo: Prezzo finale = prezzo base (somma dei costi di tutti gli ingredienti + 10%) + bonus affinità (+5% se il piatto è affine al cliente - 5% se non è affine) + extra bonus pari al 3% 2% 1% rispettivamente se il " + Utility.colorePiatti + "piatto" + Utility.fineColore + " servito è il primo, secondo o terzo migliore " + Utility.colorePiatti + " piatto " + Utility.fineColore + " servibile. Il puntegggio, invece, parte da 100 se viene servito un piatto affine, -10 altrimenti, vengono aggiunti bonus in base al <color=#B5D99C>nutriScore</color> e al <color=#B5D99C>costoEco</color> calcolato come segue:",
        "Interagire con gli <color=#B5D99C>NPC</color> in giro per la città permetterà di ottenere <color=#B5D99C>suggerimenti</color> utili per servire piatti migliori, sia dal punto di vista dell’affinità le patologie che dal punto di vista del " + Utility.coloreVerde + "nutriScore" + Utility.fineColore +  " e dell'" + Utility.coloreVerde +  "costoEco" + Utility.fineColore + ".",
        "Per acquistare nuovi " + Utility.coloreIngredienti + "ingredienti " + Utility.fineColore + "per il ristorante, bisogna recarsi al " + Utility.coloreVerde + "negozio" + Utility.fineColore + ". Interagendo con il negoziante, sarà possibile scegliere gli ingredienti da acquistare con i soldi a disposizione.",
        "Sul " + Utility.coloreVerde + "ricettario" + Utility.fineColore + " è possibile visionare tutte le ricette per i " + Utility.colorePiatti + "piatti" + Utility.fineColore + " e gli " + Utility.coloreIngredienti + "ingredienti" + Utility.fineColore + " presenti nel gioco. In più è possibile visionare anche la scheda tecnica di un "  + Utility.coloreIngredienti + "ingrediente" + Utility.fineColore + " e di un " + Utility.colorePiatti+ "piatto" + Utility.fineColore +" per visionare le sue caratteristiche (" + Utility.coloreVerde + "costoEco" + Utility.fineColore + "- " + Utility.coloreVerde + "nutriScore " + Utility.fineColore + "- breve descrizione).",
        "Ad inizio livello verrano visualizzati i criteri per terminare un livello, come per esempio servire un numero X di clienti e raggiungere un punteggio pari a Y. É possibile visionare il progresso degli obbiettivi nella sezione obbiettivi in alto a destra. Una volta superati entrambi gli obbiettivi, il livello sarà considerato terminato e si avvia la schermata di fine livello dopo è possibile avere un breve riepilogo e tornare al menu principale. Il game over, invece, avviene se dopo X clienti serviti, non si raggiungono gli obbiettivi prefissati, oppure non si ha a disposizione il denaro per acquisire nuovi ingredienti. In tal caso, si avvia la schermata di game Over, dove è possibile tornare al menu principale."
    };

    private Animazione animazione;
    List<string> nomiAnimazioni = new List<string>
    {
        "movimenti",
        "interazioneConIClientiPrimaParte",
        "interazioneConIClienti",
        "vuota",
        "visualizzareMagazzino",
        "bonus",
        "interazionePassanti",
        "negozio",
        "ricettario",
        "vuota",
    };

    public static bool apertoMenuAiuto = false;//TUTORIAL

    void Start()
    {
       
        pannelloMenuAiuto.SetActive(false);

        pannelloMenuAiutoAperto = false;

        tastoAvanti.onClick.AddListener(mostraProssimoMessaggioDiAiuto);
        tastoIndietro.onClick.AddListener(mostraPrecedenteMessaggioDiAiuto);

        testoNumeroPannelloAttuale.text = 1.ToString();

        gestisciBottoniAvantiDietro(ultimaPosizione);

        animazione = immagineSchermata.GetComponent<Animazione>();
        /*
        for (int i = 0; i < nomiAnimazioni.Count; i++)
        {
            cambiaImmagineSchermataAiuto(i);
        }
        */
    }

    public void apriPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(true);
        pannelloMenuAiutoAperto = true;

        titoloTestoAiuto.text = titoliMessaggiDiAiuto[ultimaPosizione];
        testoAiuto.text = messaggiDiAiuto[ultimaPosizione];
        cambiaImmagineSchermataAiuto(ultimaPosizione);

        apertoMenuAiuto = true; //TUTORIAL
    }

    public void chiudiPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(false);
        pannelloMenuAiutoAperto = false;
    }

    public bool getPannelloMenuAiutoAperto()
    {
        return pannelloMenuAiutoAperto;
    }

    void mostraProssimoMessaggioDiAiuto()
    {
        int prossimaPosizione = ultimaPosizione;
        if (ultimaPosizione != messaggiDiAiuto.Count - 1)
        {
            prossimaPosizione = ultimaPosizione + 1;
        }
        titoloTestoAiuto.text = titoliMessaggiDiAiuto[prossimaPosizione];
        testoAiuto.text = messaggiDiAiuto[prossimaPosizione];
        cambiaImmagineSchermataAiuto(prossimaPosizione);

        ultimaPosizione = prossimaPosizione;//aggiorno l'ultima posizione

        aggiornaTestoNumeroPannelloAttuale(ultimaPosizione);

        gestisciBottoniAvantiDietro(ultimaPosizione);
    }

    private void mostraPrecedenteMessaggioDiAiuto()
    {
        int precedentePosizione = ultimaPosizione;
        if (ultimaPosizione != 0)
        {
            precedentePosizione = ultimaPosizione - 1;
        }
        titoloTestoAiuto.text = titoliMessaggiDiAiuto[precedentePosizione];
        testoAiuto.text = messaggiDiAiuto[precedentePosizione];
        cambiaImmagineSchermataAiuto(precedentePosizione);

        ultimaPosizione = precedentePosizione;//aggiorno l'ultima posizione

        aggiornaTestoNumeroPannelloAttuale(ultimaPosizione);

        gestisciBottoniAvantiDietro(ultimaPosizione);
    }

    private void cambiaImmagineSchermataAiuto(int ultimaPosizione)
    {
        animazione.caricaAnimazione("immaginiAiuto", nomiAnimazioni[ultimaPosizione], "default");
    }

    private void aggiornaTestoNumeroPannelloAttuale(int ultimaPosizione)
    {
        testoNumeroPannelloAttuale.text = (ultimaPosizione + 1).ToString();
    }

    private void gestisciBottoniAvantiDietro(int posizione)
    {
        //tasto avanti
        if (posizione == messaggiDiAiuto.Count - 1)
        {
            tastoAvanti.interactable = false;
        }
        else
        {
            tastoAvanti.interactable = true;
        }

        //tasto indietro
        if (posizione == 0)
        {
            tastoIndietro.interactable = false;
        }
        else
        {
            tastoIndietro.interactable = true;
        }
    }
}
