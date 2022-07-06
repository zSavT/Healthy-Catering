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
        "Per muoverti all'interno del gioco premi “" + Utility.coloreVerde + "W" + Utility.fineColore + "” per andare avanti, “" + Utility.coloreVerde + "S" + Utility.fineColore + "” per andare indietro, “" + Utility.coloreVerde + "A" + Utility.fineColore + "” per andare a sinistra, “" + Utility.coloreVerde + "D" + Utility.fineColore + "” per andare a destra. Il giocatore si muove verso la direzione inquadrata con il “" + Utility.coloreVerde + "mouse" + Utility.fineColore + "”. Ora prova a muoverti per proseguire.",
        "Per saltare all'interno del gioco premi il tasto “" + Utility.coloreVerde + "Spazio" + Utility.fineColore + "”. Ora prova a saltare per proseguire.",
        "Per correre all'interno del gioco premi il tasto “" + Utility.coloreVerde + "Shift" + Utility.fineColore + "”. Ora prova a correre.",
        "Per interagire con una persona premi “" + Utility.coloreVerde + "E" + Utility.fineColore + "”. Raggiungi tuo zio e interagisci con lui.",
        "Per entrare nel ristorante premi “" + Utility.coloreVerde + "E" + Utility.fineColore + "” una volta inquadrata la porta con il mouse. Fai lo stesso per uscire. Ora entra nel ristorante.",
        "Per servire un cliente al bancone inquadralo e premi il tasto “" + Utility.coloreVerde + "E" + Utility.fineColore + "”. Per scegliere un<color=#B5D99C> piatto</color> selezionalo dal menu sulla sinistra. È importante servire un piatto idoneo alle caratteristiche del cliente, ovvero la dieta e le patologie. Servire piatti idonei permette di ricevere dei bonus. Ora prova a servire un piatto idoneo al cliente al bancone.",
        "Servire un <color=#B5D99C>piatto</color> non idoneo comporta delle penalità al punteggio e non si riceveranno bonus in denaro. I bonus e i malus vengono calcolati in base alla compatibilità del piatto e ai suoi volori <color=#B5D99C>nutriScore</color> e <color=#B5D99C>costoEco</color>. Consulta il menu aiuto per ulteriori informazioni. Ora prova a serive un piatto non idoneo al cliente al bancone.",
        "Controlla il <color=#B5D99C>magazzino</color> per tener d'occhio quali ingredienti sono disponibili per la realizzazione dei piatti. Servendo un piatto, diminuiscono nel magazzino le quantità di ingredienti che in esso figurano. Puoi verificare lo stato del magazzino dal <color=#B5D99C>PC</color> presente in <color=#B5D99C>ufficio</color> attraverso il programma “<color=#B5D99C>MyInventory</color>”. Ora raggiungi l'ufficio e controlla lo stato del magazzino.",
        "Per fare rifornimenti di ingredienti visita il negozio dove acquistare gli ingredienti necessari a realizzare altri piatti con i soldi guadagnati. Ora raggiungi il negozio e compra un ingrediente.",
        "Interagire con i <color=#B5D99C>passanti</color> in giro per la città ti permette di ottenere <color=#B5D99C>suggerimenti</color> utili per servire piatti migliori, sia dal punto di vista dell'affinità con le patologie che da quello del<color=#B5D99C> nutriScore</color> e del<color=#B5D99C> costoEco</color>. Ora prova a parlare con una persona.",
        "Puoi utilizzare il ricettario quando vuoi ed in ogni punto della mappa, premi il tasto " + Utility.coloreVerde + "R " + Utility.fineColore + "per visualizzarlo.",
        "Puoi consultare il menu aiuto quando vuoi ed in ogni punto della mappa, premi il tasto " + Utility.coloreVerde + "H " + Utility.fineColore + "per visualizzarlo.",
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
        /*
        for (int i = 0; i < nomiAnimazioni.Count; i++)
        {
            cambiaImmagine(i);
        }
        */
    }

    public void apriOkBoxVideo(int posizione)
    {
        Interactor.menuApribile = false;
        pannello.SetActive(true);
        playerStop.Invoke();
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
    }

    private void cambiaImmagine(int posizione)
    {
        animazione.caricaAnimazione("immaginiOGifOkBoxVideo", nomiAnimazioni [posizione], "default");
    }

    /// <summary>
    /// Metodo per ripristinare lo scorrere del tempo in gioco
    /// </summary>
    void resumeGame()
    {
        Time.timeScale = 1f; //sblocca il tempo
    }

    /// <summary>
    /// Metodo per bloccare lo scorrere del tempo in gioco.
    /// </summary>
    void pauseGame()
    {
        Time.timeScale = 0f; //blocca il tempo
    }
}
