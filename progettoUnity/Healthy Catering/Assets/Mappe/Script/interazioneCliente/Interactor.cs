using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactor : MonoBehaviour
{
    [SerializeField] private ProgressoLivello progresso;
    [Header("Interazione Cliente")]
    [SerializeField] private LayerMask layerUnityNPC = 6;              //layer utilizzato da Unity per le categorie di oggetto
    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform posizioneCamera;
    [SerializeField] private Transform posizioneCameraMenuCliente;
    [SerializeField] private GameObject pannelloMenuECliente;
    [SerializeField] private PannelloMenu pannelloMenuCliente;
    [SerializeField] private Gui guiInGame;

    [Header("Interazione Negozio")]
    [SerializeField] private PannelloNegozio negozio;
    [SerializeField] private AudioSource suonoNegozio;

    [Header("Interazione Magazzino")]
    [SerializeField] private PannelloMagazzino magazzino;

    [Header("Interazione NPC passivi")]
    [SerializeField] private LayerMask layerUnityNPCPassivi = 7;
    [SerializeField] private InterazionePassanti interazionePassanti;
    private InteractableNPCPassivi npcPassivo;

    [Header("Interazione Ricettario")]
    [SerializeField] private Ricettario ricettarioScript;
    [SerializeField] private KeyCode testoRicettario;
    [SerializeField] private LayerMask ricettario;

    [Header("Menu Aiuto")]
    [SerializeField] private MenuAiuto menuAiuto;
    [SerializeField] private KeyCode tastoMenuAiuto;


    [Header("Teleport")]
    [SerializeField] private LayerMask layerUnityTeleport = 9;
    public Transform destinazioneTeleport;
    [SerializeField] private Transform posizioneInizialePlayer;
    public static bool nelRistorante = false;

    [Header("Eventi")]
    public UnityEvent playerStop;
    public UnityEvent playerRiprendiMovimento;
    //trigger per la scritta dell'interazione
    [SerializeField] private UnityEvent inquadratoNPC;
    [SerializeField] private UnityEvent uscitaRangeMenu;

    public static bool menuApribile;                                                      //se il menu opzioni è aperto, le interezioni sono disattivate
    public static bool pannelloAperto;
    private int IDClientePuntato;
    private Player giocatore;

    private Interactable npc;

    private int livelloAttuale = PlayerSettings.livelloSelezionato;

    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        posizioneCamera = this.gameObject.GetComponentsInChildren<Transform>()[1];
        nelRistorante = false;
        this.gameObject.transform.position = posizioneInizialePlayer.transform.position;
        try
        {
            giocatore = Database.getPlayerDaNome(PlayerSettings.caricaNomePlayerGiocante());
            giocatore.punteggio[livelloAttuale] = 0;
            if (livelloAttuale == 0)
            {
                livelloAttuale = PlayerSettings.livelloSelezionato;
                giocatore.soldi = 15f;
            }     
            else
            {
                giocatore.setInventarioLivello(livelloAttuale);
                giocatore.soldi = 30f;
            }    
            guiInGame.aggiornaValoreSoldi(giocatore.soldi);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            PlayerSettings.profiloUtenteCreato = false;
            SelezioneLivelli.caricaMenuCreazioneProfiloUtente();
        }
        chiudiPannello();
        pannelloAperto = false;
        menuApribile = true;
        progresso.setGiocatore(giocatore);
    }

    void Update()
    {
        interazioneUtenteConNPCVari();
    }

    public void menuApribileOnOff()
    {
        menuApribile = !menuApribile;
    }

    public void menuNonApribile()
    {
        menuApribile = false;
    }

    private void interazioneUtenteConNPCVari()
    {
        if (menuApribile)
        {
            gestisciOggettiPuntatiEMenuAribiliConTasto();

            gestioneChiusureMenu();
        }
    }
    private void gestisciOggettiPuntatiEMenuAribiliConTasto()
    {
        if (NPCClientePuntato())
        {
            gestisciNPCpuntato();
        }
        else if (pcPuntato())
        {
            gestisciPCpuntato();
        }
        else if (NPCPassantePuntato())
        {
            gestisciNPCpassantePuntato();
        }
        else if (negoziantePuntato())
        {
            gestiscinegoziantePuntato();
        }
        else if (portaPuntata())
        {
            gestisciPortaPuntata();
        }
        else if (Input.GetKeyDown(testoRicettario) && nuovaSchermataApribile())
        {
            gestisciAperturaRicettario();
        }
        else if (Input.GetKeyDown(tastoMenuAiuto) && nuovaSchermataApribile())
        {
            gestisciAperturaMenuAiuto();
        }
        else
        {
            negozio.animazioneNPCIdle();
            uscitaRangeMenu.Invoke();
            if (pannelloAperto)
            {
                if (!PannelloMenu.pannelloIngredientiPiattoAperto && !PannelloMenu.pannelloConfermaPiattoAperto && !PannelloMenu.pannelloIngredientiGiustiSbagliatiAperto)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        esciDaInterazioneCliente();
                    }
                }
            }
        }
    }

    private bool NPCClientePuntato()
    {
        RaycastHit NPCpuntato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCpuntato, 2, layerUnityNPC))
        {
            // se l'oggetto visualizzato è interagibile
            if (NPCpuntato.collider.GetComponent<Interactable>() != false)
            {
                IDClientePuntato = NPCpuntato.collider.GetComponent<Interactable>().IDCliente;
                npc = NPCpuntato.collider.GetComponent<Interactable>();
                ; return true;
            }
        }
        return false;
    }

    private void gestisciNPCpuntato()
    {
        if (npc.raggiuntoBancone)
        {
            inquadratoNPC.Invoke();
            if (Input.GetKeyDown(tastoInterazione))
            {
                interazioneCliente(IDClientePuntato);
            }
        }
        else
        {
            uscitaRangeMenu.Invoke();
        }
    }

    private void interazioneCliente(int IDCliente)
    {
        muoviCameraPerInterazioneCliente();

        playerStop.Invoke();

        pannelloMenuCliente.setCliente(IDClientePuntato, giocatore, npc);

        PuntatoreMouse.abilitaCursore();
        pannelloApertoChiuso();
    }

    private void muoviCameraPerInterazioneCliente()
    {
        mainCamera.transform.position = posizioneCameraMenuCliente.transform.position;
    }

    private void pannelloApertoChiuso()
    {
        pannelloAperto = !pannelloAperto;
    }

    private void gestisciPCpuntato()
    {
        inquadratoNPC.Invoke();
        if (Input.GetKeyDown(tastoInterazione) && !magazzino.getPannelloMagazzinoAperto())
        {
            magazzino.apriPannelloMagazzino(giocatore);
            playerStop.Invoke();
            PuntatoreMouse.abilitaCursore();
            CambioCursore.cambioCursoreNormale();
        }
    }

    private void gestisciNPCpassantePuntato()
    {
        inquadratoNPC.Invoke();
        if (Input.GetKeyDown(tastoInterazione) && !(interazionePassanti.getPannelloInterazionePassantiAperto()))
        {
            interazionePassanti.apriPannelloInterazionePassanti(npcPassivo.transform.parent.name);
            npcPassivo.animazioneParlata(gameObject.transform);
            playerStop.Invoke();
            PuntatoreMouse.abilitaCursore();
            CambioCursore.cambioCursoreNormale();
        }
    }

    private void gestiscinegoziantePuntato()
    {
        inquadratoNPC.Invoke();
        negozio.animazioneNPCInquadrato();
        if (Input.GetKeyDown(tastoInterazione) && !(negozio.getPannelloAperto()))
        {
            negozio.apriPannelloNegozio(giocatore);
            playerStop.Invoke();
            PuntatoreMouse.abilitaCursore();
            CambioCursore.cambioCursoreNormale();
        }
    }

    private void gestisciPortaPuntata()
    {
        inquadratoNPC.Invoke();
        if (Input.GetKeyDown(tastoInterazione) && !(negozio.getPannelloAperto()))
        {
            suonoNegozio.Play();
            this.gameObject.transform.position = destinazioneTeleport.transform.position;
            nelRistorante = !nelRistorante;
            Debug.Log(nelRistorante);
        }
    }
    
    private bool nuovaSchermataApribile()
    {
        return (
            !negozio.getPannelloAperto()
            &&
            !pannelloMenuCliente.getPannelloMenuClienteAperto()
            &&
            !ricettarioScript.getRicettarioAperto()
            &&
            !menuAiuto.getPannelloMenuAiutoAperto()
        );
    }

    private void gestisciAperturaRicettario()
    {
        playerStop.Invoke();
        ricettarioScript.apriRicettario();
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
    }

    private void gestisciAperturaMenuAiuto()
    {
        playerStop.Invoke();
        menuAiuto.apriPannelloMenuAiuto();
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
    }

    private void gestioneChiusureMenu()
    {
        if (negozio.getPannelloAperto() && !negozio.getPannelloConfermaAperto())
        {
            gestisciChiusuraNegozio();
        }
        if (interazionePassanti.getPannelloInterazionePassantiAperto())
        {
            gestisciChiusuraPannelloPassanti();
        }
        if (ricettarioScript.getRicettarioAperto())
        {
            gestisciChiusuraRicettario();
        }
        if (menuAiuto.getPannelloMenuAiutoAperto())
        {
            gestisciChiusuraMenuAiuto();
        }
    }

    private void gestisciChiusuraNegozio()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            negozio.chiudiPannelloNegozio();
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }

    private void gestisciChiusuraPannelloPassanti()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            interazionePassanti.chiudiPannelloInterazionePassanti();
            npcPassivo.stopAnimazioneParlata(gameObject.transform);
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }

    private void gestisciChiusuraRicettario()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ricettarioScript.chiudiRicettario();
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }

    
            

private void gestisciChiusuraMenuAiuto()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuAiuto.chiudiPannelloMenuAiuto();
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }

    private bool NPCPassantePuntato()
    {
        RaycastHit NPCPassivoInquadrato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCPassivoInquadrato, 3, layerUnityNPCPassivi))
        {
            if (NPCPassivoInquadrato.collider.GetComponent<InteractableNPCPassivi>() != false)
            {
                npcPassivo = NPCPassivoInquadrato.collider.GetComponent<InteractableNPCPassivi>();
                return true;
            }

        }
        return false;
    }

    private bool negoziantePuntato()
    {
        RaycastHit pcInquadrato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out pcInquadrato, 2, layerUnityNPC))
        {
            // se l'oggetto visualizzato è interagibile
            if (pcInquadrato.collider.GetComponent<PannelloNegozio>() != false)
            {
                return true;
            }
        }
        return false;
    }

    public void esciDaInterazionePC()
    {
        playerRiprendiMovimento.Invoke();

        ritornaAllaPosizioneNormale();
        CambioCursore.cambioCursoreNormale();
        PuntatoreMouse.disabilitaCursore();
        magazzino.chiudiPannelloMagazzino();
    }

    private bool portaPuntata()
    {
        RaycastHit portaPuntata;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out portaPuntata, 2, layerUnityTeleport))
        {
            // se l'oggetto visualizzato è interagibile
            if (portaPuntata.collider.GetComponent<InteractablePorta>() != false)
            {
                destinazioneTeleport = portaPuntata.collider.GetComponent<InteractablePorta>().posizioneTeleport;
                return true;
            }
        }
        return false;
    }

    private bool pcPuntato()
    {
        RaycastHit pcInquadrato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out pcInquadrato, 2, layerUnityNPC))
        {
            // se l'oggetto visualizzato è interagibile
            if (pcInquadrato.collider.GetComponent<PannelloMagazzino>() != false)
            {
                return true;
            }
        }
        return false;
    }

    public void esciDaInterazioneCliente()
    {
        chiudiPannello();
        playerRiprendiMovimento.Invoke();

        ritornaAllaPosizioneNormale();
        CambioCursore.cambioCursoreNormale();
        PuntatoreMouse.disabilitaCursore();
        if (!PannelloMenu.clienteServito)
        {
            guiInGame.bloccaAnimazioniParticellari();
        } 
        else
        {
            guiInGame.aggiornaValorePunteggio(giocatore.punteggio[livelloAttuale]);
            
            guiInGame.aggiornaValoreSoldi(giocatore.soldi);
        }
    }

    private void ritornaAllaPosizioneNormale()
    {
        mainCamera.transform.position = posizioneCamera.position;
    }

    private void chiudiPannello()
    {
        pannelloMenuCliente.ChiudiPannelloMenuCliente();
        pannelloApertoChiuso();
    }

    public Player getPlayer()
    {
        return giocatore;
    }
}