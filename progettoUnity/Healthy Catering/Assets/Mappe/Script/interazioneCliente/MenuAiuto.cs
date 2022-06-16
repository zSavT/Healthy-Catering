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
    [SerializeField] private GameObject menuOpzioni;

    private int ultimaPosizione = 0;//cosi la prima volta apre il primo messaggio

    private List<string> titoliMessaggiDiAiuto = new List<string>
    {
        "Movimenti:",
        "Interazione con I clienti:",
        "Scelta del piatto:",
        "Gestione del denaro:",
        "Gestione magazzino:",
        "Come vengono calcolati I bonus:",
        "Interazione con gli NPC:"
    };
    private List<string> messaggiDiAiuto = new List<string>
    {
        "Per potersi muovere all'interno del gioco, bisogna utilizzare i tasti <color=#B5D99C>W, A, S, D</color> per muoversi rispettivamente in <color=#B5D99C>\"Avanti\", \"Sinistra\", \"Indietro\", \"Destra\"</color>.",
        "Per poter interagire con i clienti si dovrà utilizzare il mouse come puntatore e selezionare il <color=#B5D99C>piatto</color> che si vuole servire al cliente quando richiesto attraverso la relativa schermata.",
        "Scegliere il piatto migliore fra quelli disponibili, permette al giocatore di aumentare il suo denaro e il suo punteggio così da poter superare il livello. Nel caso dovesse servire ad un <color=#B5D99C>cliente</color> con una <color=#B5D99C>patologia</color> X un piatto dove è presente un ingrediente non compatibile con essa verrà mostrato un pop up dal quale sarà possibile visualizzare quali degli ingredienti del piatto sono compatibili con la patologia e quali no.",
        "<color=#B5D99C>Più saranno affini i piatti che verranno serviti</color> più incrementerà il denaro del giocatore e più ingredienti potrà compare dal negozio.",
        "Sarà possibile scegliere <color=#B5D99C>solo i piatti</color> per i quali <color=#B5D99C>sono disponibili tutti gli ingredienti nelle quantità necessarie</color>; quindi, si dovrà tenere conto degli ingredienti disponibili nel proprio magazzino e comprare gli ingredienti mancanti.",
        "I <color=#B5D99C>bonus denaro</color> e il <color=#B5D99C>punteggio</color> vengono calcolati in base all’affinità del cliente che si sta servendo ed ai parametri del nutriScore e l’ecoScore:\n1. Per quanto riguarda il guadagno monetario verrà assegnato:\n\t• Un bonus del dieci percento sulla somma dei costi dei singoli ingredienti usati, a prescindere dall’affinità\n\t• Un bonus del cinque percento se il piatto è affine alla patologia e alla dieta del cliente\n\t• Ci sarà inoltre una sanzione, sempre del cinque percento nel caso in cui il piatto non fosse affine\n2. Per quanto riguarda il punteggio invece ci sarà:\n\t• Un punteggio base di 100 punti se il piatto è affine e, ed in tal caso verranno inoltre assegnati i bonus in base al nutriScore e all’ecoScore che partono da -10 percento nel caso peggiore e + 10 percento <u>nel</u> caso migliore; questi 2 bonus verranno calcolati per tutti e 2 gli indicatori quindi, per esempio: nel caso in cui il giocatore dovesse decidere di servire un piatto con il nutriScore più alto ma allo stesso tempo questo piatto inquina molto il punteggio rimarrà invariato.",
        "Interagire con gli <color=#B5D99C>NPC</color> in giro per la città permetterà di ottenere <color=#B5D99C>suggerimenti</color> utili per servire piatti migliori, sia dal punto di vista dell’affinità le patologie che dal punto di vista del nutriScore e dell’ecoScore.",
    };

    void Start()
    {
        pannelloMenuAiuto.SetActive(false);

        pannelloMenuAiutoAperto = false;

        tastoAvanti.onClick.AddListener(mostraProssimoMessaggioDiAiuto);
        tastoIndietro.onClick.AddListener(mostraPrecedenteMessaggioDiAiuto);

        testoNumeroPannelloAttuale.text = 1.ToString();

        gestisciBottoniAvantiDietro(ultimaPosizione);
    }

    public void apriPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(true);
        menuOpzioni.SetActive(false);
        pannelloMenuAiutoAperto = true;

        titoloTestoAiuto.text = titoliMessaggiDiAiuto[ultimaPosizione];
        testoAiuto.text = messaggiDiAiuto[ultimaPosizione];
        cambiaImmagineSchermataAiuto(ultimaPosizione);
    }

    public void chiudiPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(false);
        menuOpzioni.SetActive(true);
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
        Sprite nuovaImmagine = Resources.Load<Sprite>("immaginiAiuto/" + "immagineAiuto" + (ultimaPosizione + 1).ToString());
        if (nuovaImmagine == null)
        {
            nuovaImmagine = Resources.Load<Sprite>("immaginiAiuto/immagineAiutoDefault");
        }
        immagineSchermata.sprite = nuovaImmagine;
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
