using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe dedicata alla gestione del tutorial del livello 0
/// </summary>
public class ProgressoTutorial : MonoBehaviour
{
    private bool inTutorial;

    [Header("Video tutorial")]
    [SerializeField] private GameObject canvasVideoTutorial;

    [Header("Obbiettivi Tutorial")]
    //testo obbiettivo da cambiare di volta in volta
    [SerializeField] private TextMeshProUGUI obbiettivo1Testo;
    [SerializeField] private Toggle obbiettivo1Toggle;
    /*
    [SerializeField] private TextMeshProUGUI obbiettivo2Testo;
    [SerializeField] private TextMeshProUGUI obbiettivo2Toggle;
    */

    private bool videoInRiproduzione;
    private bool skipVideo;

    private List<string> scritteDaMostrare;
    //tiene conto del progresso del giocatore praticamente
    private int numeroScritteMostrate;
    //3 va sostituito con la posizione nella lista delle scritte che devono uscire solo se sa gia giocare agli fps
    private readonly int posizioneScritteDaMostrareSaGiocare = 3;


    [SerializeField] private GameObject okBoxVideo;
    [SerializeField] private TextMeshProUGUI testoOkBoxVideo;
    [SerializeField] private UnityEngine.Video.VideoPlayer videoOkBoxVideo; // non so sicuro sia quello l'oggetto giusto per questa cosa

    private CheckTutorial checkTuorial;
    private bool finitoTutorial;

    private Player giocatore = null;

    private void Start()
    { 
        print("iniziato tutorial");
        riproduciVideo(); 

        inTutorial = true;
        canvasVideoTutorial.SetActive(true);
        //le disattivo per attivarle solo nel momento opportuno - Questi elementi sono nel loro specifico pannello, che va attivato poi quando serve.
        attivaObbiettiviTutorial();

        scritteDaMostrare = new List<string> /*qui vanno inserite le varie scritte per bene*/
        {
            "Premi <color=#B5D99C>W,A,S,D</color> per camminare.",
            "Premi <color=#B5D99C>Spazio</color> per saltare.",
            "Premi <color=#B5D99C>Shift</color> per correre.",
            "parla zio",
            "Raggiungi il <color=#B5D99C>Ristorante</color>.",
            "servito piatto compatibile e non",
            "controlla il magazzino",
            "compra ingredienti",
            "interagito con npc"
        };

        numeroScritteMostrate = 0;
        
        finitoTutorial = false;
    }

    private void Update()
    {
        /*
        if (videoInRiproduzione)
        {
            if (skipVideo) // si aggiorna con 
            {
                stopRiproduzioneVideo();
                //dopo che ha riprodotto il video gli chiedo se deve skippare la prima parte del tutorial
                skipTutorialComandi = chiediSeSaHaGiocatoFPS();
            }
        }
        else
        {*/
            for (int i = 0; i < scritteDaMostrare.Count; i++)
            {
                if (i == numeroScritteMostrate)
                {
                    setobiettivoTesto(scritteDaMostrare[i]);
                }

                if (numeroScritteMostrate == 0)
                {
                    if (CheckTutorial.checkWASDeMouse()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 1)
                {
                    if (CheckTutorial.checkSalto()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 2)
                {
                    if (CheckTutorial.checkSprint()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 3)
                {
                    if (CheckTutorial.checkParlaConZio()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 4)
                {
                    if (CheckTutorial.checkVaiRistorante()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 5)
                {
                    //mostraOkBoxVideo("spiegazione meccaniche per servire");
                    //forse qui serve un if "è alla cassa prima? cosi da poter aggiornare la scritta meglio"
                    if (true)
                        if (giocatore != null)
                            if (CheckTutorial.checkServitoPiattoCompatibileENon(giocatore)) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 6)
                {
                    //mostraOkBoxVideo("sono finiti gli ingredienti per fare i piatti");
                    if (CheckTutorial.checkVistoMagazzino()) { numeroScritteMostrate++; }

                    //nel magazzino dovremmo mettere un ingrediente che non e' presente nella ricetta ne del 
                    //Piatto compatibile ne in quella del piatto non compatibile, cosi che quando il giocatore 
                    //apre il magazzino non sia vuoto del tutto, se no sembra che il magazzino abbia solo la 
                    //funzione di avvisarti che non hai più ingredienti
                    //magari possiamo cambiare la scritta a "il magazzino sarebbe cosi se ci fossero degli
                    //ingredienti" e poi far scomparire l'ingrediente temp che abbiamo inserito dopo 5 secondi
                }
                else if (numeroScritteMostrate == 7)
                {
                    //mostraOkBoxVideo("mostra dov'e' il negozio");
                    //forse qui serve un if "è nel negozio prima? cosi da poter aggiornare la scritta meglio"
                    if (true)
                        if (CheckTutorial.checkCompratiIngredienti()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 8)
                {
                    //mostraOkBoxVideo("puoi interagire con gli npc");
                    if (CheckTutorial.checkParlatoConNPC())
                    {
                        numeroScritteMostrate++;
                        finitoTutorial = true;
                    }
                }
                else
                {
                    setobiettivoTesto("");//resetto il testo dell'obbiettivo 
            }
            }
        //}
        
        if (finitoTutorial)
        {
            setobiettivoTesto("Hai finito il tutorial!!!");
            inTutorial = false;
        }
        
    }

    public void setGiocatore (Player player)
    {
        giocatore = player;
    }

    private void riproduciVideo()
    {
        /*
        canvasVideoTutorial.SetActive(true);

        //metodo per riprodurre il video, per ora ho solo attivato il canvas che contiene gli elementi per riprodurlo

        */
        videoInRiproduzione = true;
        print("ciao video");
    }

    public void saltaVideo()
    {
        //TODO metodo da chiamare dal bottone per saltare il video
        skipVideo = true;
    }

    private void stopRiproduzioneVideo()
    {
        //TODO
    }

    private bool chiediSeSaHaGiocatoFPS()
    {
        //TODO
        return false;
    }

    /// <summary>
    /// Metodo che attiva il TextMeshProUGUI dell'obbiettivo e quello del toggle<br></br>
    /// </summary>
    public void attivaObbiettiviTutorial()
    {
        obbiettivo1Testo.gameObject.SetActive(true);
        obbiettivo1Toggle.gameObject.SetActive(true);
    }

    private void setobiettivoTesto (string output)
    {
        obbiettivo1Testo.text = output;
    }

    private void mostraOkBoxVideo(string output, string nomeVideoOGifDaRiprodurre = null)
    {
        //TODO (il metodo e tutto il resto)

        nomeVideoOGifDaRiprodurre ??= "";

        // poi li disattiviamo dal bottone relativo che chiama il metodo scritto sotto
        okBoxVideo.SetActive(true);
        //attiva puntatore mouse

        testoOkBoxVideo.text = output;

        if (!nomeVideoOGifDaRiprodurre.Equals(""))
        {
            videoOkBoxVideo.Play();
        }
    }

    public void disattivaOkBoxVideo()
    {
        okBoxVideo.SetActive(false);
        //disattiva puntatore mouse
    }

    /*
    questi 3 metodi non servono più con il sistema implementato
    /// <summary>
    /// cambia colore testo obbiettivo in verde e imposta il toogle su vero.
    /// Il codice verde è 181, 216, 156, 255<br></br>
    /// </summary>
    private void setObbiettivoCompletato()
    {
        obbiettivoTesto.color = new Color32(181, 216, 156, 255);
        obbiettivoToggle.isOn = true;
    }
    
    /// <summary>
    /// Cambia il colore in bianco del testo e setta il toogle su falso<br></br>
    /// </summary>
    private void resetObbiettivoCompletato()
    {
        obbiettivoTesto.color = Color.white;
        obbiettivoToggle.isOn = false;
    }
    /// <summary>
    /// Imposta la variabile di skip del tutorial su true
    /// </summary>
    public void skipComandiTutorial()
    {
        skipTutorialComandi = true;
    }
    */
}
