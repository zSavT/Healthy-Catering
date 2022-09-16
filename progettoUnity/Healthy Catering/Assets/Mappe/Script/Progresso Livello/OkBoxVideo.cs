using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class OkBoxVideo : MonoBehaviour
{
    [SerializeField] private GameObject pannello;
    [SerializeField] private TextMeshProUGUI titolo;
    [SerializeField] private TextMeshProUGUI testo;
    [SerializeField] private Image immagineOGIF;

    public static readonly int WASD = 0;
    public static readonly int salto = 1;
    public static readonly int sprint = 2;
    public static readonly int parlaZio = 3;
    public static readonly int vaiAlRistorante = 4;
    public static readonly int meccanicheServireCompatibile = 5;
    public static readonly int meccanicheServireNonCompatibile = 6;
    public static readonly int finitiIngredienti = 7;
    public static readonly int doveEIlNegozio = 8;
    public static readonly int interazioneNPC = 9;
    public static readonly int apriRicettario = 10;
    public static readonly int apriMenuAiuto = 11;

    public static bool WASDmostrato = false;
    public static bool saltoMostrato = false;
    public static bool sprintMostrato = false;
    public static bool parlaZioMostrato = false;
    public static bool vaiAlRistoranteMostrato = false;
    public static bool meccanicheServireCompatibileMostrato = false;
    public static bool meccanicheServireNonCompatibileMostrato = false;
    public static bool finitiIngredientiMostrato = false;
    public static bool doveEIlNegozioMostrato = false;
    public static bool interazioneNPCMostrato = false;
    public static bool apriRicettarioMostrato = false;
    public static bool apriMenuAiutoMostrato = false;
    public static int indiceCorrente = 0;


    List<string> titoli = new List<string>
    {
        "Camandi base per il movimento",
        "Comando Salto",
        "Comando Sprint",
        "Parla con tuo zio",
        "Entra nel ristorante",
        "Servire un piatto idoneo ad un cliente",
        "Servire un piatto non idoneo ad un cliente",
        "Come controllare il magazzino del ristorante",
        "Rifornire il Magazzino visitando il Negozio",
        "Acquisire informazioni interagendo con i passanti",
        "Aprire il ricettario",
        "Aprire il menu aiuto"
    };

    List<string> testi = new List<string>
    {
        "Per muoverti all'interno del gioco premi “" + Costanti.coloreVerde + "W" + Costanti.fineColore + "” per andare avanti, “" + Costanti.coloreVerde + "S" + Costanti.fineColore + "” per andare indietro, “" + Costanti.coloreVerde + "A" + Costanti.fineColore + "” per andare a sinistra, “" + Costanti.coloreVerde + "D" + Costanti.fineColore + "” per andare a destra. Il giocatore si muove verso la direzione inquadrata con il “" + Costanti.coloreVerde + "mouse" + Costanti.fineColore + "”. Ora prova a muoverti premendo “" + Costanti.coloreVerde + "W" + Costanti.fineColore + "” , “" + Costanti.coloreVerde + "S" + Costanti.fineColore + "” , “" + Costanti.coloreVerde + "A" + Costanti.fineColore + "” , “" + Costanti.coloreVerde + "D" + Costanti.fineColore + "” di seguito.",
        "Per saltare all'interno del gioco premi il tasto “" + Costanti.coloreVerde + "Spazio" + Costanti.fineColore + "”. Ora prova a saltare per proseguire.",
        "Per correre all'interno del gioco premi il tasto “" + Costanti.coloreVerde + "Shift" + Costanti.fineColore + "”. Ora prova a correre.",
        "Per interagire con una persona premi “" + Costanti.coloreVerde + "E" + Costanti.fineColore + "”. Raggiungi tuo zio e interagisci con lui.",
        "Per entrare nel ristorante premi “" + Costanti.coloreVerde + "E" + Costanti.fineColore + "” una volta inquadrata la porta con il mouse. Fai lo stesso per uscire. Ora entra nel ristorante.",
        "Per servire un cliente al bancone inquadralo e premi il tasto “" + Costanti.coloreVerde + "E" + Costanti.fineColore + "”. Per scegliere un " + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "selezionalo dal menu sulla sinistra. È importante servire un " + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "idoneo alle caratteristiche del cliente, ovvero la " + Costanti.coloreDieta + "dieta " + Costanti.fineColore + "e le " + Costanti.colorePatologia + "patologie" + Costanti.fineColore + ". Servire piatti idonei permette di ricevere dei bonus. Ora prova a servire un piatto idoneo al cliente al bancone.",
        "Servire un "  + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "non idoneo comporta delle penalità al punteggio e non si riceveranno bonus in denaro. I bonus e i malus vengono calcolati in base alla compatibilità del piatto e ai suoi volori <color=#B5D99C>nutriScore</color> e <color=#B5D99C>costoEco</color>. Consulta il " + Costanti.coloreVerde + "menu aiuto " + Costanti.fineColore + "per ulteriori informazioni. Ora prova a serive un piatto non idoneo al cliente al bancone.",
        "Controlla il <color=#B5D99C>magazzino</color> per tener d'occhio quali ingredienti sono disponibili per la realizzazione dei piatti. Servendo un piatto, diminuiscono nel magazzino le quantità di ingredienti che in esso figurano. Puoi verificare lo stato del magazzino dal <color=#B5D99C>PC</color> presente in <color=#B5D99C>ufficio</color> attraverso il programma “<color=#B5D99C>MyInventory</color>”. Ora raggiungi l'ufficio e controlla lo stato del magazzino.",
        "Per fare rifornimenti di " + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "visita il " + Costanti.coloreVerde + "negozio " + Costanti.fineColore + "dove acquistare gli "  + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "necessari a realizzare altri " + Costanti.colorePiatti + "piatti " + Costanti.fineColore + "con i soldi guadagnati. Nel <color=#B5D99C>negozio</color> seleziona la quantità dell'"+ Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " che ti interessa, aggiungila al carrello e poi prosegui con l'acquisto finale. Ora esci dal <color=#B5D99C>ristorante</color>, raggiungi il <color=#B5D99C>negozio</color> e compra un " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + ".",
        "Interagire con i <color=#B5D99C>passanti</color> in giro per la città ti permette di ottenere <color=#B5D99C>suggerimenti</color> utili per servire i " + Costanti.colorePiatti + "piatti " + Costanti.fineColore + "migliori, sia dal punto di vista dell'affinità con le " + Costanti.colorePatologia + "patologie" + Costanti.fineColore + " che da quello del<color=#B5D99C> nutriScore</color> e del<color=#B5D99C> costoEco</color>. Ora prova a parlare con una persona.",
        "Puoi utilizzare il <color=#B5D99C>ricettario</color> quando vuoi ed in ogni punto della mappa, premi il tasto “" + Costanti.coloreVerde + "R" + Costanti.fineColore + "” per visualizzarlo. Puoi utilizzare il " + Costanti.coloreVerde + "ricettario" + Costanti.fineColore + " per visionare le quantità degli " + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "di un " + Costanti.colorePiatti + "piatto " + Costanti.fineColore + ", oltre a poter visionare il <color=#B5D99C>nutriScore</color> e <color=#B5D99C>costoEco</color> nella scheda tecnica del " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " o " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + ".",
        "Puoi consultare il <color=#B5D99C>menu aiuto</color> quando vuoi ed in ogni punto della mappa, premi il tasto “" + Costanti.coloreVerde + "H" + Costanti.fineColore + "” per visualizzarlo. Puoi ri-visionare tutte le informazioni e le meccaniche del gioco mostrate in precedenza.",
    };

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;

    private Animazione animazione;
    List<string> nomiAnimazioni = new List<string>
    {
        "wasd",
        "salto",
        "shift",
        "interazioneZio",
        "entrareRistorante",
        "clienteCompatibile",
        "clienteNonCompatibile",
        "visualizzareMagazzino",
        "negozio",
        "interazionePassanti",
        "ricettario",
        "menuAiuto",
    };

    void Start()
    {
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
        animazione = immagineOGIF.GetComponent<Animazione>();
        WASDmostrato = false;
        saltoMostrato = false;
        sprintMostrato = false;
        parlaZioMostrato = false;
        vaiAlRistoranteMostrato = false;
        meccanicheServireCompatibileMostrato = false;
        meccanicheServireNonCompatibileMostrato = false;
        finitiIngredientiMostrato = false;
        doveEIlNegozioMostrato = false;
        interazioneNPCMostrato = false;
        apriRicettarioMostrato = false;
        apriMenuAiutoMostrato = false;
        indiceCorrente = 0;
        /*
        for (int i = 0; i < nomiAnimazioni.Count; i++)
        {
            cambiaImmagine(i);
        }
        */
    }

    public void apriOkBoxVideo(int posizione)
    {
        playerStop.Invoke();
        indiceCorrente = posizione;
        Interactor.menuApribile = false;
        pannello.SetActive(true);
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();

        if (posizione < titoli.Count)
        {
            titolo.text = titoli[posizione];
            testo.text = testi[posizione];
        }
        else
        {
            titolo.text = "errore";
            testo.text = "";
        }
        cambiaImmagine(posizione);
    }

    public void chiudiOkBoxVideo()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();

        titolo.text = "";
        testo.text = "";
        Interactor.menuApribile = true;
        if(indiceCorrente == parlaZio)
        {
            parlaZioMostrato = true;
        }
        if (indiceCorrente == vaiAlRistorante)
        {
            vaiAlRistoranteMostrato = true;
        }
        if (indiceCorrente == doveEIlNegozio)
        {
            doveEIlNegozioMostrato = true;
        }
    }

    private void cambiaImmagine(int posizione)
    {
        animazione.caricaAnimazione("immaginiOGifOkBoxVideo", nomiAnimazioni [posizione], "default");
    }
}
