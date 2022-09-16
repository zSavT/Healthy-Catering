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
        "Utilizza i tasti \"" + Costanti.coloreVerde + "W" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "A" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "S" + Costanti.fineColore + "\", " + Costanti.coloreVerde + "D" + Costanti.fineColore + "\" per muoveti rispettivamente in \"" +Costanti.coloreVerde + "Avanti" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "Sinistra" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "Indietro" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "Destra" + Costanti.fineColore + "\". In più premi il tasto \"" + Costanti.coloreVerde + "Spazio" + Costanti.fineColore + "\" per saltare e tieni premuto il tasto \"" + Costanti.coloreVerde + "Shift" + Costanti.fineColore + "\" per correre mentre ti muovi.",
        "Per avviare l'interazione con un " + Costanti.coloreVerde + "cliente" + Costanti.fineColore +" al bancone inquadralo e premi il tasto \"" + Costanti.coloreVerde + "E" + Costanti.fineColore + "\". Per servire i clienti utilizza il mouse e selezionare il " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " da servire al cliente quando richiesto attraverso la relativa schermata.",
        "Scegliere il piatto migliore fra quelli disponibili ti permette di aumentare il tuo denaro e il tuo punteggio, così da poter superare il livello. Nel caso dovessi servire ad un <color=#B5D99C>cliente</color> con una certa " + Costanti.colorePatologia + "patologia" + Costanti.fineColore + " e/o " + Costanti.coloreDieta + "dieta"  + Costanti.fineColore + " un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore + " dove è presente un " + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " non compatibile con essa, verrà mostrato un pop up in cui potrai visualizzare quali degli " + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " del " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " sono compatibili con la " + Costanti.colorePatologia + "patologia" + Costanti.fineColore + " e quali no.",
        "Stesso discorso per un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore + " non compatibile con la " + Costanti.coloreDieta + "dieta" + Costanti.fineColore + " del " + Costanti.coloreVerde + "cliente" + Costanti.fineColore + ". Servire un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore +" non idoneo comporta delle penalità al punteggio e non si riceveranno bonus in denaro. I bonus e i malus vengono calcolati in base alla compatibilità del "+ Costanti.colorePiatti+ "piatto" + Costanti.fineColore + " e ai suoi volori <color=#B5D99C>nutriScore</color> e <color=#B5D99C>costoEco</color>.",
        "Sarà possibile scegliere solo i " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " per i quali " + Costanti.coloreVerde + "sono disponibili tutti gli ingredienti nelle quantità necessarie" + Costanti.fineColore + "; quindi, si dovrà tenere conto degli ingredienti disponibili nel proprio magazzino e comprare gli ingredienti mancanti. É possibile visionoare lo stato del magazzino aprendo il programma \"" + Costanti.coloreVerde + "MyInventory" + Costanti.fineColore + "\", accessibile nel computer presente nell'ufficio del ristorante.",
        "I bonus della vendita del " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " è calcolato come segue: prezzo finale = prezzo base (somma dei costi di tutti gli ingredienti + 10%) + bonus affinità (+5% se il piatto è affine al cliente, -5% se non lo è) + extra bonus pari al 3%, 2% o 1% se il " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " servito è rispettivamente il primo, secondo o terzo migliore servibile. Il puntegggio invece parte da 100 se viene servito un piatto affine, -10 altrimenti. Vengono aggiunti bonus in base al <color=#B5D99C>nutriScore</color> e al <color=#B5D99C>costoEco</color> calcolati come segue:",
        "Interagire con gli <color=#B5D99C>NPC</color> in giro per la città permetterà di ottenere <color=#B5D99C>suggerimenti</color> utili per servire piatti migliori, sia dal punto di vista dell’affinità le patologie che dal punto di vista del " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore +  " e dell'" + Costanti.coloreVerde +  "costoEco" + Costanti.fineColore + ".",
        "Per acquistare nuovi " + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "per il ristorante, bisogna recarsi al " + Costanti.coloreVerde + "negozio" + Costanti.fineColore + ". Interagendo con il negoziante, sarà possibile scegliere gli ingredienti da acquistare con i soldi a disposizione.",
        "Sul " + Costanti.coloreVerde + "ricettario" + Costanti.fineColore + " è possibile visionare tutte le ricette per i " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " e gli " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " presenti nel gioco. In più è possibile visionare anche la scheda tecnica di un "  + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " e di un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore +" per visionare le sue caratteristiche (" + Costanti.coloreVerde + "costoEco" + Costanti.fineColore + "- " + Costanti.coloreVerde + "nutriScore " + Costanti.fineColore + "- breve descrizione).",
        "Ad inizio livello verrano visualizzati i criteri per terminare un livello, come per esempio servire un numero X di clienti e raggiungere un punteggio pari a Y. É possibile visionare il progresso degli obbiettivi nella sezione obbiettivi in alto a destra. Una volta superati entrambi gli obbiettivi, il livello sarà considerato terminato e si avvia la schermata di fine livello dopo è possibile avere un breve riepilogo e tornare al menu principale. Il game over, invece, avviene se dopo X clienti serviti, non si raggiungono gli obbiettivi prefissati, oppure non si ha a disposizione il denaro per acquisire nuovi ingredienti. In tal caso, si avvia la schermata di game Over, dove è possibile tornare al menu principale."
    };

    private Animazione animazione;
    List<string> nomiAnimazioni = new List<string>
    {
        "movimenti",
        "interazioneConIClientiPrimaParte",
        "interazioneConIClienti",
        "interazioneConIClientiSecondaParte",
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
        ultimaPosizione = 0;
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
