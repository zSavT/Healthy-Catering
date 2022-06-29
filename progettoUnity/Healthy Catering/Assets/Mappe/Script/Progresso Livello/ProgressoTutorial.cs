using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    

    private bool videoInRiproduzione;
    private bool skipVideo;

    private List<string> scritteDaMostrare;
    //tiene conto del progresso del giocatore praticamente
    private int numeroScritteMostrate;
    //3 va sostituito con la posizione nella lista delle scritte che devono uscire solo se sa gia giocare agli fps
    private readonly int posizioneScritteDaMostrareSaGiocare = 3;

    private bool finitoTutorial;

    private Player giocatore = null;
    [SerializeField] private Interactor interazioniPlayer;
    [SerializeField] private ProgressoLivello progressoLivelloClassico;

    [SerializeField] private OkBoxVideo okBoxVideo;

    [SerializeField] private PlayerSaGiocareFPS playerSaGiocareFPS;
    private bool saGiocareSettato = false;
    [SerializeField] private UnityEvent playerStop;
    private int siOno = 0;

    OggettoQuantita<int> ingredienteInPiu = new OggettoQuantita<int>(10, 1);//mango

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
            "Vai a parlare con tuo <color=#B5D99C>Zio</color>.",
            "Raggiungi il <color=#B5D99C>Ristorante</color>.",
            "Servi un <color=#B5D99C>Cliente</color> con un piatto idoneo.",
            "Servi un <color=#B5D99C>Cliente</color> con un piatto non idoneo.",
            "Controlla il <color=#B5D99C>Magazzino</color>.",
            "Compra <color=#B5D99C>Ingredienti</color> dal negozio.",
            "Chiedi informazioni alle  <color=#B5D99C>Persone</color>."
        };

        numeroScritteMostrate = 0;
        
        finitoTutorial = false;

    }

    private void Update()
    {
    //    print(saGiocareSettato);
        if (!saGiocareSettato)
        {
            if (PlayerSaGiocareFPS.siOnoSettato())
            {
                siOno = PlayerSaGiocareFPS.getSiOno();
                
                if (siOno == 1)
                    numeroScritteMostrate = posizioneScritteDaMostrareSaGiocare;
                else if (siOno == -1)
                    numeroScritteMostrate = 0;

                if (siOno != 0)
                    saGiocareSettato = true;
            }
            else
            {
                playerStop.Invoke();
                playerSaGiocareFPS.apriPannelloPlayerSaGiocareFPS();
            }
        }
        else
        {
            for (int i = 0; i < scritteDaMostrare.Count; i++)
            {
                if (i == numeroScritteMostrate)
                {
                    setObiettivoTesto(scritteDaMostrare[i]);
                    giocatore = interazioniPlayer.getPlayer();
                }

                if (numeroScritteMostrate == 0)
                {
                    if (!OkBoxVideo.WASDmostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.WASD);
                        OkBoxVideo.WASDmostrato = true;
                    }

                    if (CheckTutorial.checkWASDeMouse()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 1)
                {
                    if (!OkBoxVideo.saltoMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.salto);
                        OkBoxVideo.saltoMostrato = true;
                    }

                    if (CheckTutorial.checkSalto()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 2)
                {
                    if (!OkBoxVideo.sprintMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.sprint);
                        OkBoxVideo.sprintMostrato = true;
                    }

                    if (CheckTutorial.checkSprint()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 3)
                {
                    if (!OkBoxVideo.parlaZioMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.parlaZio);
                        OkBoxVideo.parlaZioMostrato = true;
                    }

                    if (CheckTutorial.checkParlaConZio()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 4)
                {
                    if (!OkBoxVideo.vaiAlRistoranteMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.vaiAlRistorante);
                        OkBoxVideo.vaiAlRistoranteMostrato = true;
                    }

                    if (CheckTutorial.checkVaiRistorante()) { 
                        numeroScritteMostrate++;
                        giocatore.setInventarioLivello(0);
                    }
                }
                else if (numeroScritteMostrate == 5)
                {
                    if (!OkBoxVideo.meccanicheServireCompatibileMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.meccanicheServireCompatibile);
                        OkBoxVideo.meccanicheServireCompatibileMostrato = true;
                    }

                    if (CheckTutorial.checkIsAllaCassa()) //TODO implementazione
                        if (giocatore != null)
                            if (CheckTutorial.checkServitoPiattoCompatibile(giocatore)) { 
                                numeroScritteMostrate++;
                                giocatore.setInventarioLivello(0.5);
                            }
                }
                else if (numeroScritteMostrate == 6)
                {
                    if (!OkBoxVideo.meccanicheServireNonCompatibileMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.meccanicheServireNonCompatibile);
                        OkBoxVideo.meccanicheServireNonCompatibileMostrato = true;
                    }

                    if (CheckTutorial.checkIsAllaCassa()) //TODO implementazione
                        if (giocatore != null)
                            if (CheckTutorial.checkServitoPiattoNonCompatibile(giocatore))
                            {
                                numeroScritteMostrate++;
                            }
                }

                else if (numeroScritteMostrate == 7)
                {
                    if (!OkBoxVideo.finitiIngredientiMostrato)
                    {
                        PannelloMagazzino.pannelloMagazzinoApertoPerTutorial = false;
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.finitiIngredienti);
                        OkBoxVideo.finitiIngredientiMostrato  = true;
                    }

                    if (CheckTutorial.checkVistoMagazzino()) { numeroScritteMostrate++; }

                    //nel magazzino dovremmo mettere un ingrediente che non e' presente nella ricetta ne del 
                    //Piatto compatibile ne in quella del piatto non compatibile, cosi che quando il giocatore 
                    //apre il magazzino non sia vuoto del tutto, se no sembra che il magazzino abbia solo la 
                    //funzione di avvisarti che non hai più ingredienti
                    //magari possiamo cambiare la scritta a "il magazzino sarebbe cosi se ci fossero degli
                    //ingredienti" e poi far scomparire l'ingrediente temp che abbiamo inserito dopo 5 secondi
                }
                else if (numeroScritteMostrate == 8)
                {
                    if (!OkBoxVideo.doveEIlNegozioMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.doveEIlNegozio);
                        OkBoxVideo.doveEIlNegozioMostrato = true;
                    }
                    
                    if (CheckTutorial.checkIsNelNegozio()) //TODO implementazione
                        if (CheckTutorial.checkCompratiIngredienti(giocatore)) { numeroScritteMostrate++; };
                }
                else if (numeroScritteMostrate == 9)
                {
                    if (!OkBoxVideo.interazioneNPCMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(OkBoxVideo.interazioneNPC);
                        OkBoxVideo.interazioneNPCMostrato = true;
                    }
                    
                    if (CheckTutorial.checkParlatoConNPC())
                    {
                        numeroScritteMostrate++;
                        finitoTutorial = true;
                    }
                }
                else
                {
                    setObiettivoTesto("");//resetto il testo dell'obbiettivo 
                }
            }

        }

        
        if (finitoTutorial)
        {
            obbiettivo1Testo.gameObject.SetActive(false);
            inTutorial = false;
            progressoLivelloClassico.attivaSoloObbiettivi();
            iTween.Destroy(this.gameObject);
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
    }

    private void setObiettivoTesto (string output)
    {
        obbiettivo1Testo.text = output;
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
