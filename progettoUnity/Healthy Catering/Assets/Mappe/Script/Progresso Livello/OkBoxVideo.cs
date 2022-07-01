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
    
    List<string> titoli = new List<string>
    {
        "Camandi base per il movimento",
        "Comando per Salto",
        "Comando per Sprint",
        "Parla con tuo zio",
        "Raggiungi il ristorante",
        "Come servire un piatto idoneo ad un cliente",
        "Come servire un piatto non idoneo ad un cliente",
        "Come controllare il magazzino del ristorante",
        "Rifornire il Magazzino visitando il Negozio",
        "Acquisire informazioni interagendo con i passanti"
    };

    List<string> testi = new List<string>
    {
        "Per muoversi all’interno del gioco, bisogna usare rispettivamente " + Utility.coloreVerde + "“W”" + Utility.fineColore + " per andare avanti, " + Utility.coloreVerde + "“S”" + Utility.fineColore + " per andare indietro, " + Utility.coloreVerde + "“A”" + Utility.fineColore + " per andare a sinistra, " + Utility.coloreVerde + "“D”" + Utility.fineColore + " per andare a destra. Il giocatore si muove verso la direzione inquadrata con il " + Utility.coloreVerde + "“mouse”. " + Utility.fineColore + "Ora prova a muoverti per proseguire.",
        "Per saltare all'interno del gioco bisogna premere il tasto" + Utility.coloreVerde + " “Spazio“" + Utility.fineColore + ". Ora prova a saltare per proseguire.",
        "Per correre all'interno del gioco bisogna premere il tasto" + Utility.coloreVerde + " “Shift“" + Utility.fineColore + ". Ora prova a correre.",
        "Raggiungi tuo zio e interagisci con lui. Per interagire con una persona bisogna premere " + Utility.coloreVerde + "“E“" + Utility.fineColore + ".",
        "Per entrare o uscire nel ristorante bisogna premere " + Utility.coloreVerde + "“E“" + Utility.fineColore + " una volta inquadrato la porta con il mouse. Ora entra nel ristorante.",
        "Per servire un cliente al bancone, bisogna inquadrarlo e premere il tasto " + Utility.coloreVerde + " “E“" + Utility.fineColore + ". Per scegliere un<color=#B5D99C> piatto</color> bisogna selezionarlo dal menu sulla sinistra. É importante servire un piatto idoneo alle caratteristiche del cliente, ovvero la dieta e le patologie. Servire piatti idonei permette di ricevere dei bonus. Ora prova a servire un piatto idoneo al cliente al bancone.",
        "Servire un <color=#B5D99C> piatto</color> non idoneo, comporta delle penalità al punteggio e non si riceveranno bonus in denaro. I bonus e i malus, vengono calcolati in base alla compatibilità del piatto ed al volore dei <color=#B5D99C> nutriScore</color> e <color=#B5D99C> costoEco</color> di questi ultimi. Ora prova a serive un piatto non idoneo al cliente al bancone.",
        "É importante controllare il <color=#B5D99C> magazzino</color> del negozio, per controllare quali ingredienti sono disponibili per poter realizzare i piatti. Infatti servire un piatto, scala dal magazzino la quantità necessaria degli ingredienti. É possibile visionare lo stato del magazzino nel <color=#B5D99C> PC</color> presente in <color=#B5D99C> ufficio</color>. Ora raggiungi l'ufficio e controlla lo stato del magazzino dall'apposito programma.",
        "Per fare rifornimenti di ingredienti, bisogna visitare il negozio ed acquistare gli ingredienti desiderati per realizzare nuovi piatti, acquistandoli con i soldi guadagnati dal ristorante. Ora raggiungi il negozio e compra un ingrediente.",
        "Interagire con gli <color=#B5D99C>NPC</color> in giro per la città permette di ottenere <color=#B5D99C>suggerimenti</color> utili per servire piatti migliori, sia dal punto di vista dell'affinità le patologie che dal punto di vista del <color=#B5D99C> nutriScore</color> e dell'<color=#B5D99C> costoEco</color>. Ora prova a parlare con una persona."
    };

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;

    void Start()
    {
        pannello.SetActive(false);
        titolo.text = "";
        testo.text = "";
        setImmagineDefault();
    }

    public void apriOkBoxVideo(int posizione)
    {
        pauseGame();
        pannello.SetActive(true);
        playerStop.Invoke();
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();

        if (posizione < titoli.Count)
        {
            titolo.text = titoli[posizione];
            testo.text = testi[posizione];
            cambiaImmagine(posizione);
        }
        else
        {
            titolo.text = "errore";
            testo.text = "";
            setImmagineDefault();
        }
    }

    public void chiudiOkBoxVideo()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();

        titolo.text = "";
        testo.text = "";
        resumeGame();
    }

    private void cambiaImmagine(int posizione)
    {
        Sprite nuovaImmagine = Resources.Load<Sprite>("immaginiOGifOkBoxVideo/" + "immagineOGifOkBoxVideo " + titoli[posizione]);
        if (nuovaImmagine == null)
        {
            setImmagineDefault();
        }
        else
        {
            immagineOGIF.sprite = nuovaImmagine;
        }
    }

    private void setImmagineDefault (){
        immagineOGIF.sprite = Resources.Load<Sprite>("immaginiOGifOkBoxVideo/immagineOGifOkBoxVideoDefault"); 
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
