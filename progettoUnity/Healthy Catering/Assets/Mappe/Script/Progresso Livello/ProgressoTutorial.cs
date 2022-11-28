using System.Collections;
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
    public static bool inTutorial;
    [SerializeField] private Gui guiInGame;
    [Header("Video tutorial")]
    [SerializeField] private GameObject canvasVideoTutorial;

    [Header("Obbiettivi Tutorial")]
    //testo obbiettivo da cambiare di volta in volta
    [SerializeField] private TextMeshProUGUI obbiettivo1Testo;

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
    [SerializeField] private MovimentoPlayer moviemnto;

    [SerializeField] IndicatoreDistanza indicatoreDistanza;
    private void Start()
    {
        inTutorial = true;
        saGiocareSettato = false;
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
            "Servi un piatto idoneo al <color=#B5D99C>cliente</color>.",
            "Servi un piatto non idoneo al <color=#B5D99C>cliente</color>.",
            "Controlla il <color=#B5D99C>Magazzino</color>.",
            "Compra <color=#B5D99C>Ingredienti</color> dal negozio.",
            "Chiedi informazioni alle <color=#B5D99C>Persone</color>.",
            "Apri il ricettario con il tasto " + Costanti.coloreVerde + "R" + Costanti.fineColore + ".",
            "Apri il menu aiuto con il tasto " + Costanti.coloreVerde + "H" + Costanti.fineColore + "."
        };

        numeroScritteMostrate = 0;

        finitoTutorial = false;
    }

    private void Update()
    {
        if (!saGiocareSettato)
        {
            if (PlayerSaGiocareFPS.siOnoSettato())
            {
                siOno = PlayerSaGiocareFPS.getSiOno();

                if (siOno == 1)
                {
                    numeroScritteMostrate = posizioneScritteDaMostrareSaGiocare;
                    OkBoxVideo.indiceCorrente = posizioneScritteDaMostrareSaGiocare;
                }
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
                        okBoxVideo.apriOkBoxVideo(Costanti.WASD);
                        OkBoxVideo.WASDmostrato = true;
                    }

                    if (CheckTutorial.checkWASDeMouse()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 1)
                {
                    if (!OkBoxVideo.saltoMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.salto);
                        OkBoxVideo.saltoMostrato = true;
                    }

                    if (CheckTutorial.checkSalto()) { if(moviemnto.perTerra) numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 2)
                {
                    if (!OkBoxVideo.sprintMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.sprint);
                        OkBoxVideo.sprintMostrato = true;
                    }

                    if (CheckTutorial.checkSprint()) { numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 3)
                {
                    if (!OkBoxVideo.parlaZioMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.parlaZio);
                        InterazionePassanti.parlatoConZio = false;
                    }
                    if(OkBoxVideo.parlaZioMostrato && !Interactor.nelRistorante)
                    {
                        indicatoreDistanza.setTarget("zio");
                    } else
                    {
                        indicatoreDistanza.setTarget("reset");
                    }
                    if (CheckTutorial.checkParlaConZio()) { indicatoreDistanza.setTarget("reset"); numeroScritteMostrate++; }
                }
                else if (numeroScritteMostrate == 4)
                {
                    if (!OkBoxVideo.vaiAlRistoranteMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.vaiAlRistorante);

                    } else
                    {
                        indicatoreDistanza.setTarget("ristorante");
                    }

                    if (CheckTutorial.checkVaiRistorante())
                    {
                        giocatore.setInventarioLivello(0);
                        numeroScritteMostrate++;
                    }
                }
                else if (numeroScritteMostrate == 5)
                {
                    indicatoreDistanza.setTarget("reset");
                    if (!OkBoxVideo.meccanicheServireCompatibileMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.meccanicheServireCompatibile);
                        OkBoxVideo.meccanicheServireCompatibileMostrato = true;
                    }

                    if (CheckTutorial.checkIsAllaCassa()) //TODO implementazione
                        if (giocatore != null)
                            if (CheckTutorial.checkServitoPiattoCompatibile(giocatore))
                            {
                                guiInGame.aggiornaValorePunteggioSenzaAnimazione(giocatore.punteggio[0]);
                                giocatore.setInventarioLivello(0.5);
                                numeroScritteMostrate++;
                            }
                }
                else if (numeroScritteMostrate == 6)
                {
                    if (!OkBoxVideo.meccanicheServireNonCompatibileMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.meccanicheServireNonCompatibile);
                        OkBoxVideo.meccanicheServireNonCompatibileMostrato = true;
                    }

                    if (CheckTutorial.checkIsAllaCassa()) //TODO implementazione
                        if (giocatore != null)
                            if (CheckTutorial.checkServitoPiattoNonCompatibile(giocatore))
                            {
                                guiInGame.aggiornaValorePunteggioSenzaAnimazione(giocatore.punteggio[0]);
                                numeroScritteMostrate++;
                            }
                }

                else if (numeroScritteMostrate == 7)
                {
                    if (!OkBoxVideo.finitiIngredientiMostrato)
                    {
                        PannelloMagazzino.pannelloMagazzinoApertoPerTutorial = false;
                        okBoxVideo.apriOkBoxVideo(Costanti.finitiIngredienti);
                        OkBoxVideo.finitiIngredientiMostrato = true;
                    }

                    if (CheckTutorial.checkVistoMagazzino()) { numeroScritteMostrate++; }

                    //nel magazzino dovremmo mettere un ingrediente che non e' presente nella ricetta ne del 
                    //Piatto compatibile ne in quella del piatto non compatibile, cosi che quando il giocatore 
                    //apre il magazzino non sia vuoto del tutto, se no sembra che il magazzino abbia solo la 
                    //funzione di avvisarti che non hai pi� ingredienti
                    //magari possiamo cambiare la scritta a "il magazzino sarebbe cosi se ci fossero degli
                    //ingredienti" e poi far scomparire l'ingrediente temp che abbiamo inserito dopo 5 secondi
                }
                else if (numeroScritteMostrate == 8)
                {
                    if (!OkBoxVideo.doveEIlNegozioMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.doveEIlNegozio);
                    }
                    if(!Interactor.nelRistorante && OkBoxVideo.doveEIlNegozioMostrato)
                    {
                        indicatoreDistanza.setTarget("negozio");
                    } else
                    {
                        indicatoreDistanza.setTarget("reset");
                    }
                    if (CheckTutorial.checkIsNelNegozio()) //TODO implementazione
                        if (CheckTutorial.checkCompratiIngredienti(giocatore)) { numeroScritteMostrate++; };
                }
                else if (numeroScritteMostrate == 9)
                {
                    if (!OkBoxVideo.interazioneNPCMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.interazioneNPC);
                        OkBoxVideo.interazioneNPCMostrato = true;

                        indicatoreDistanza.setTarget("reset");
                    }

                    if (CheckTutorial.checkParlatoConNPC())
                    {
                        Ricettario.apertoRicettario = false;
                        numeroScritteMostrate++;
                    }
                }
                else if (numeroScritteMostrate == 10)
                {
                    if (!OkBoxVideo.apriRicettarioMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.apriRicettario);
                        OkBoxVideo.apriRicettarioMostrato = true;
                    }

                    if (CheckTutorial.checkMostratoRicettario()) { MenuAiuto.apertoMenuAiuto = false; numeroScritteMostrate++; };
                }
                else if (numeroScritteMostrate == 11)
                {

                    if (!OkBoxVideo.apriMenuAiutoMostrato)
                    {
                        okBoxVideo.apriOkBoxVideo(Costanti.apriMenuAiuto);
                        OkBoxVideo.apriMenuAiutoMostrato = true;
                    }

                    if (CheckTutorial.checkMostratoMenuAiuto()) { numeroScritteMostrate++; };
                }
                else
                {
                    finitoTutorial = true;
                    setObiettivoTesto("");//resetto il testo dell'obbiettivo 
                }
            }

        }


        if (finitoTutorial)
        {
            obbiettivo1Testo.gameObject.SetActive(false);
            giocatore.setInventarioLivello(1);
            giocatore.soldi = 30f;
            inTutorial = false;
            progressoLivelloClassico.attivaPannelloRiepiloghiObbiettivi();
            iTween.Destroy(this.gameObject);
        }

    }

    /// <summary>
    /// Metodo che attiva il TextMeshProUGUI dell'obbiettivo e quello del toggle<br></br>
    /// </summary>
    public void attivaObbiettiviTutorial()
    {
        obbiettivo1Testo.gameObject.SetActive(true);
    }

    private void setObiettivoTesto(string output)
    {
        obbiettivo1Testo.text = output;
    }
}