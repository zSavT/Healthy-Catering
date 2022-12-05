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
    [SerializeField] Transform posizioneDaRaggiungereTutorial;
    [SerializeField] private PlayerSaGiocareFPS playerSaGiocareFPS;
    private bool saGiocareSettato = false;
    [SerializeField] private UnityEvent playerStop;
    private int siOno = 0;
    [SerializeField] private MovimentoPlayer movimento;

    [SerializeField] IndicatoreDistanza indicatoreDistanza;

    private ControllerInput controllerInput;

    private void Start()
    {
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        inTutorial = true;
        saGiocareSettato = false;
        //le disattivo per attivarle solo nel momento opportuno - Questi elementi sono nel loro specifico pannello, che va attivato poi quando serve.
        attivaObbiettiviTutorial();

        scritteDaMostrare = new List<string> /*qui vanno inserite le varie scritte per bene*/
        {
            "Premi " + Costanti.tastoWASD + " per camminare e raggiungi il cono stradale.",
            "Premi " + Costanti.tastoSpazio + " per saltare.",
            "Premi " + Costanti.tastoShift + " per correre.",
            "Vai a parlare con tuo " + Costanti.coloreVerde + "zio" + Costanti.fineColore + ".",
            "Raggiungi il " + Costanti.coloreVerde + "Ristorante" + Costanti.fineColore + ".",
            "Servi un piatto idoneo al <color=#B5D99C>cliente</color>.",
            "Servi un piatto non idoneo al <color=#B5D99C>cliente</color>.",
            "Controlla il <color=#B5D99C>Magazzino</color>.",
            "Compra <color=#B5D99C>Ingredienti</color> dal negozio.",
            "Chiedi informazioni alle " + Costanti.coloreVerde + "Persone" + Costanti.fineColore + ".",
            "Apri il ricettario con il tasto " + Costanti.tastoR + ".",
            "Apri il menu aiuto con il tasto " + Costanti.tastoH + "."
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
                movimento.bloccaMovimento();
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
                        playerSaGiocareFPS.distruggiOggetto();
                        movimento.bloccaMovimento();
                        controllerInput.Disable();
                        okBoxVideo.apriOkBoxVideo(Costanti.WASD);
                        OkBoxVideo.WASDmostrato = true;
                    }
                    else if (!OkBoxVideo.okBoxVideoAperto)
                    {
                        if(!MenuInGame.menuAperto)
                        {
                            movimento.sbloccaMovimento();
                            indicatoreDistanza.impostaSizeWayPoint(new Vector3(0.65f, 0.65f, 0.65f));
                            indicatoreDistanza.setTarget("Cono");
                        }
                        if (Interactor.nelRistorante)
                            indicatoreDistanza.setTarget("reset");
                        else
                            indicatoreDistanza.setTarget("Cono");
                        controllerInput.Enable();
                    }
                        
                    if (CheckTutorial.checkWASDeMouse(controllerInput,movimento.gameObject.transform , posizioneDaRaggiungereTutorial)) 
                    { 
                        numeroScritteMostrate++;
                    }
                }
                else if (numeroScritteMostrate == 1)
                {
                    if (!OkBoxVideo.saltoMostrato)
                    {
                        indicatoreDistanza.setTarget("reset");
                        indicatoreDistanza.impostaSizeWayPoint(new Vector3(1, 1, 1));
                        movimento.bloccaMovimento();
                        controllerInput.Disable();
                        okBoxVideo.apriOkBoxVideo(Costanti.salto);

                        OkBoxVideo.saltoMostrato = true;
                    }
                    else if(!OkBoxVideo.okBoxVideoAperto)
                    {
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();
                        controllerInput.Enable();
                    }
                    if (CheckTutorial.checkSalto(controllerInput)) 
                    { 
                        if(movimento.perTerra) 
                           numeroScritteMostrate++; 
                    }
                }
                else if (numeroScritteMostrate == 2)
                {
                    if (!OkBoxVideo.sprintMostrato)
                    {
                        movimento.bloccaMovimento();
                        controllerInput.Disable();
                        okBoxVideo.apriOkBoxVideo(Costanti.sprint);
                        OkBoxVideo.sprintMostrato = true;
                    }
                    else if (!OkBoxVideo.okBoxVideoAperto)
                    {
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();
                        controllerInput.Enable();
                    }
                    if (CheckTutorial.checkSprint(controllerInput)) 
                    { 
                        numeroScritteMostrate++; 
                    }
                }
                else if (numeroScritteMostrate == 3)
                {
                    if (!OkBoxVideo.parlaZioMostrato)
                    {
                        if(playerSaGiocareFPS != null)
                            playerSaGiocareFPS.distruggiOggetto();
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.parlaZio);
                        InterazionePassanti.parlatoConZio = false;
                    }
                    else if (!OkBoxVideo.okBoxVideoAperto)
                    {
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();
                        controllerInput.Enable();
                    }
                    if (OkBoxVideo.parlaZioMostrato && !Interactor.nelRistorante)
                    {
                        indicatoreDistanza.setTarget("zio");
                    } else
                    {
                        indicatoreDistanza.setTarget("reset");
                    }
                    if (CheckTutorial.checkParlaConZio())
                    { 
                        indicatoreDistanza.setTarget("reset"); 
                        numeroScritteMostrate++; 
                    }
                }
                else if (numeroScritteMostrate == 4)
                {
                    if (!OkBoxVideo.vaiAlRistoranteMostrato)
                    {
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.vaiAlRistorante);

                    } else if (!OkBoxVideo.okBoxVideoAperto)
                    {
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();
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
                        iTween.Destroy(posizioneDaRaggiungereTutorial.gameObject);
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.meccanicheServireCompatibile);
                        OkBoxVideo.meccanicheServireCompatibileMostrato = true;
                    } else if (!OkBoxVideo.okBoxVideoAperto)
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();

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
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.meccanicheServireNonCompatibile);
                        OkBoxVideo.meccanicheServireNonCompatibileMostrato = true;
                    }
                    else if (!OkBoxVideo.okBoxVideoAperto)
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();

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
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.finitiIngredienti);
                        OkBoxVideo.finitiIngredientiMostrato = true;
                    } else if (!OkBoxVideo.okBoxVideoAperto)
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();

                    if (CheckTutorial.checkVistoMagazzino())
                    { 
                        numeroScritteMostrate++; 
                    }
                }
                else if (numeroScritteMostrate == 8)
                {
                    if (!OkBoxVideo.doveEIlNegozioMostrato)
                    {
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.doveEIlNegozio);
                    } else if (!OkBoxVideo.okBoxVideoAperto)
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();

                    if (!Interactor.nelRistorante && OkBoxVideo.doveEIlNegozioMostrato)
                    {
                        indicatoreDistanza.setTarget("negozio");
                    } else
                    {
                        indicatoreDistanza.setTarget("reset");
                    }
                    if (CheckTutorial.checkCompratiIngredienti(giocatore)) 
                    { 
                        numeroScritteMostrate++; 
                    }
                }
                else if (numeroScritteMostrate == 9)
                {
                    if (!OkBoxVideo.interazioneNPCMostrato)
                    {
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.interazioneNPC);
                        OkBoxVideo.interazioneNPCMostrato = true;

                        indicatoreDistanza.setTarget("reset");
                    } else if (!OkBoxVideo.okBoxVideoAperto)
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();

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
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.apriRicettario);
                        OkBoxVideo.apriRicettarioMostrato = true;
                    } else if (!OkBoxVideo.okBoxVideoAperto)
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();
                    if (CheckTutorial.checkMostratoRicettario()) 
                    { 
                        MenuAiuto.apertoMenuAiuto = false; 
                        numeroScritteMostrate++;
                    };
                }
                else if (numeroScritteMostrate == 11)
                {

                    if (!OkBoxVideo.apriMenuAiutoMostrato)
                    {
                        movimento.bloccaMovimento();
                        okBoxVideo.apriOkBoxVideo(Costanti.apriMenuAiuto);
                        OkBoxVideo.apriMenuAiutoMostrato = true;
                    }
                    else if (!OkBoxVideo.okBoxVideoAperto)
                        if (!MenuInGame.menuAperto)
                            movimento.sbloccaMovimento();

                    if (CheckTutorial.checkMostratoMenuAiuto()) 
                    { 
                        numeroScritteMostrate++;
                    };
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

    /// <summary>
    /// Il metodo imposta il testo dell'obbiettivo da raggiungere
    /// </summary>
    /// <param name="output">string testo da inserire</param>
    private void setObiettivoTesto(string output)
    {
        obbiettivo1Testo.text = output;
        PlayerSettings.addattamentoSpriteComandi(obbiettivo1Testo);
    }
}