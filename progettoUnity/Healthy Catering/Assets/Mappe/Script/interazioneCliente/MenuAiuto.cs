using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuAiuto : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMenuAiuto;
    [SerializeField] private TextMeshProUGUI testoAiuto;
    [SerializeField] private Button tastoAvanti;
    [SerializeField] private Button tastoIndietro;

    private int ultimaPosizione = 0;//cosi la prima volta apre il primo messaggio
    private List<string> messaggiDiAiuto = new List<string>
    {
        "- Movimenti: Per potersi muovere all'interno del gioco, bisogna utilizzare i tasti W, A, S, D per muoversi rispettivamente in \"Avanti\", \"Sinistra\", \"Indietro\", \"Destra\"."
        "- Interazione con I clienti: Per poter interagire con i clienti si dovrà utilizzare il mouse come puntatore e selezionare il piatto che si vuole servire al cliente quando richiesto attraverso la relativa schermata."
        "- Scelta del piatto: Scegliere il piatto migliore fra quelli disponibili, permette al giocatore di aumentare il suo denaro e il suo punteggio così da poter superare il livello. Nel caso dovesse servire ad un cliente con una patologia X un piatto dove è presente un ingrediente non compatibile con essa verrà mostrato un pop up dal quale sarà possibile visualizzare quali degli ingredienti del piatto sono compatibili con la patologia e quali no."
        "- Gestione del denaro: Più saranno affini i piatti che verranno serviti più incrementerà il denaro del giocatore e più ingredienti potrà compare dal negozio."
        "- Gestione magazzino: Sarà possibile scegliere solo i piatti per i quali sono disponibili tutti gli ingredienti nelle quantità necessarie; quindi, si dovrà tenere conto degli ingredienti disponibili nel proprio magazzino e comprare gli ingredienti mancanti."
        "- Come vengono calcolati I bonus: I bonus denaro e il punteggio vengono calcolati in base all’affinità del cliente che si sta servendo ed ai parametri del nutriScore e l’ecoScore:\n1.per quanto riguarda il guadagno monetario verrà assegnato:\n\ta.un bonus del dieci percento sulla somma dei costi dei singoli ingredienti usati, a prescindere dall’affinità\n\tb.un bonus del cinque percento se il piatto è affine alla patologia e alla dieta del cliente\n\tc.ci sarà inoltre una sanzione, sempre del cinque percento nel caso in cui il piatto non fosse affine\n2.per quanto riguarda il punteggio invece ci sarà:\n\ta.un punteggio base di 100 punti se il piatto è affine e, ed in tal caso verranno inoltre assegnati i bonus in base al nutriScore e all’ecoScore che partono da -10 percento nel caso peggiore e + 10 percento nel caso migliore; questi 2 bonus verranno calcolati per tutti e 2 gli indicatori quindi, per esempio: nel caso in cui il giocatore dovesse decidere di servire un piatto con il nutriScore più alto ma allo stesso tempo questo piatto inquina molto il punteggio rimarrà invariato."
        "- Interazione con gli NPC: interagire con gli NPC in giro per la città permetterà di ottenere suggerimenti utili per servire piatti migliori, sia dal punto di vista dell’affinità le patologie che dal punto di vista del nutriScore e dell’ecoScore."
    };


    void Start()
    {
        pannelloMenuAiuto.SetActive(false);
        tastoAvanti.onClick.AddListener(() => {
            mostraProssimoMessaggioDiAiuto();
        });
        tastoIndietro.onClick.AddListener(() => {
            mostraPrecedenteMessaggioDiAiuto();
        });
    }

    void apriPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(true);
        testoAiuto.text = messaggiDiAiuto[ultimaPosizione];
    }

    void mostraProssimoMessaggioDiAiuto()
    {
        int prossimaPosizione = ultimaPosizione + 1;
        if (ultimaPosizione == messaggiDiAiuto.Count - 1)
        {
            prossimaPosizione = 0;
        }
        testoAiuto.text = messaggiDiAiuto[prossimaPosizione];

        ultimaPosizione = prossimaPosizione;//aggiorno l'ultima posizione
    }

    void mostraPrecedenteMessaggioDiAiuto()
    {
        int precedentePosizione = ultimaPosizione - 1;
        if (ultimaPosizione == 0)
        {
            precedentePosizione = messaggiDiAiuto.Count -1;
        }
        testoAiuto.text = messaggiDiAiuto[precedentePosizione];

        ultimaPosizione = precedentePosizione;//aggiorno l'ultima posizione
    }
}
