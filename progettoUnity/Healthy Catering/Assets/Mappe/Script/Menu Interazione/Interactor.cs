using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

/// <summary>
/// Classe principale per gestire le azioni del giocatore<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore Player in game.
/// </para>
/// </summary>
public class Interactor : MonoBehaviour
{
    private ControllerInput controllerInput;
    [SerializeField] private ProgressoLivello progresso;
    [Header("Interazione Cliente")]
    private Camera mainCamera;
    private Transform posizioneCamera;
    [SerializeField] private LayerMask layerUnityNPC = 6;              //layer utilizzato da Unity per le categorie di oggetto
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
    [SerializeField] private LayerMask ricettario;

    [Header("Menu Aiuto")]
    [SerializeField] private MenuAiuto menuAiuto;

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
        inizializzaValori();
    }

    void Update()
    {
        interazioneUtenteConNPCVari();
    }

    /// <summary>
    /// Disattiva il controller alla eliminazione dell'oggetto
    /// </summary>
     private void OnDestroy()
    {
        controllerInput.Disable();
    }

    /// <summary>
    /// Il metodo inizializza tutti i valori della classe
    /// </summary>
    private void inizializzaValori()
    {
        Costanti.spriteTastiera = Resources.Load<TMP_SpriteAsset>("tastiTastiera");
        Costanti.spriteXbox = Resources.Load<TMP_SpriteAsset>("tastiXbox");
        controllerInput = new ControllerInput();
        controllerInput.Enable();
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
        catch (Exception e)
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

    /// <summary>
    /// Il metodo inverte il valore di verità della variabile menuApribile
    /// </summary>
    public void menuApribileOnOff()
    {
        menuApribile = !menuApribile;
    }

    /// <summary>
    /// Il metodo disattiva il valore di verità della variabile menuApribile
    /// </summary>
    public void menuNonApribile()
    {
        menuApribile = false;
    }

    /// <summary>
    /// Il metodo richiama tutti i metodi per i controlli degli elementi interagibili dal playler
    /// </summary>
    private void interazioneUtenteConNPCVari()
    {
        if (menuApribile)
        {
            gestisciOggettiPuntatiEMenuAribiliConTasto();

            gestioneChiusureMenu();
        }
    }

    /// <summary>
    /// Il metodo controlla e gestisce gli input per l'interazione
    /// </summary>
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
        else if (controllerInput.Player.Ricettario.IsPressed() && nuovaSchermataApribile())
        {
            gestisciAperturaRicettario();
        }
        else if (controllerInput.Player.MenuAiuto.IsPressed() && nuovaSchermataApribile())
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
                    if (controllerInput.Player.UscitaMenu.IsPressed())
                    {
                        esciDaInterazioneCliente();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Il metodo controlla se il player punta un cliente
    /// </summary>
    /// <returns>booleano npc puntato</returns>
    private bool NPCClientePuntato()
    {
        RaycastHit NPCpuntato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCpuntato, 2, layerUnityNPC))
        {
            // se l'oggetto visualizzato è interagibile
            if (NPCpuntato.collider.GetComponent<Interactable>() != false)
            {
                IDClientePuntato = NPCpuntato.collider.GetComponent<Interactable>().getIdCliente();
                npc = NPCpuntato.collider.GetComponent<Interactable>();
                ; return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Il metodo permette di gestire tutta l'interazione con interazione con il cliente
    /// </summary>
    private void gestisciNPCpuntato()
    {
        if (npc.getClienteRaggiuntoBancone())
        {
            inquadratoNPC.Invoke();
            if (controllerInput.Player.Interazione.IsPressed())
            {
                interazioneCliente(IDClientePuntato);
            }
        }
        else
        {
            uscitaRangeMenu.Invoke();
        }
    }

    /// <summary>
    /// Il metodo permette di avviare l'interazione con il cliente aprendo l'apposito menu
    /// </summary>
    /// <param name="IDCliente">int id del cliente servito</param>
    private void interazioneCliente(int IDCliente)
    {
        muoviCameraPerInterazioneCliente();

        playerStop.Invoke();

        pannelloMenuCliente.setCliente(IDClientePuntato, giocatore, npc);

        PuntatoreMouse.abilitaCursore();
        pannelloApertoChiuso();
    }

    /// <summary>
    /// Il metodo permette di muovere la telecamera
    /// </summary>
    private void muoviCameraPerInterazioneCliente()
    {
        mainCamera.transform.position = posizioneCameraMenuCliente.transform.position;
    }

    /// <summary>
    /// Il metodo inverte il valore booleano della variabile pannelloAperto
    /// </summary>
    private void pannelloApertoChiuso()
    {
        pannelloAperto = !pannelloAperto;
    }

    /// <summary>
    /// Il metodo gestice l'interazione con il PC del magazzino per l'interazione
    /// </summary>
    private void gestisciPCpuntato()
    {
        inquadratoNPC.Invoke();
        if (controllerInput.Player.Interazione.IsPressed() && !magazzino.getPannelloMagazzinoAperto())
        {
            magazzino.apriPannelloMagazzino(giocatore);
            playerStop.Invoke();
            PuntatoreMouse.abilitaCursore();
            CambioCursore.cambioCursoreNormale();
        }
    }

    /// <summary>
    /// Il metodo gestisce gli NPC passivi per l'interazione
    /// </summary>
    private void gestisciNPCpassantePuntato()
    {
        inquadratoNPC.Invoke();
        if (controllerInput.Player.Interazione.IsPressed() && !(interazionePassanti.getPannelloInterazionePassantiAperto()) && !(menuAiuto.getPannelloMenuAiutoAperto()) && !(ricettarioScript.getRicettarioAperto()))
        {
            interazionePassanti.apriPannelloInterazionePassanti(npcPassivo.transform.parent.name);
            npcPassivo.animazioneParlata(gameObject.transform);
            playerStop.Invoke();
            PuntatoreMouse.abilitaCursore();
            CambioCursore.cambioCursoreNormale();
        }
    }

    /// <summary>
    /// Il metodo gestice interazione con il negoziante
    /// </summary>
    private void gestiscinegoziantePuntato()
    {
        inquadratoNPC.Invoke();
        negozio.animazioneNPCInquadrato();
        if (controllerInput.Player.Interazione.IsPressed() && !(negozio.getPannelloAperto()))
        {
            negozio.apriPannelloNegozio(giocatore);
            playerStop.Invoke();
            PuntatoreMouse.abilitaCursore();
            CambioCursore.cambioCursoreNormale();
        }
    }

    /// <summary>
    /// Il metodo gestice il teletrasporto dentro e fuori dal ristorante
    /// </summary>
    private void gestisciPortaPuntata()
    {
        inquadratoNPC.Invoke();
        if (controllerInput.Player.Interazione.IsPressed() && !(negozio.getPannelloAperto()))
        {
            suonoNegozio.Play();
            this.gameObject.transform.position = destinazioneTeleport.transform.position;
            nelRistorante = !nelRistorante;
        }
    }
    
    /// <summary>
    /// Il metodo controlla se una nuova schermata è apribile controllando che tutte le altre sono chiuse
    /// </summary>
    /// <returns>booleano, True: Si può aprire una nuova schermata, False: Non è possibile aprire una nuova schermata</returns>
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

    /// <summary>
    /// Il metodo permette di gestire l'apertura del menu ricettario disattivando gli elementi grafici e bloccando il movimento del giocatore
    /// </summary>
    private void gestisciAperturaRicettario()
    {
        playerStop.Invoke();
        ricettarioScript.apriRicettario();
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
    }

    /// <summary>
    /// Il metodo gestice l'apertura del menu aiuto disattivando gli elementi grafici e il movimento del player
    /// </summary>
    private void gestisciAperturaMenuAiuto()
    {
        playerStop.Invoke();
        menuAiuto.apriPannelloMenuAiuto();
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
    }

    /// <summary>
    /// Il metodo permette di gestire la chisura di tutti i menu
    /// </summary>
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

    /// <summary>
    /// Il metodoo permette di uscire dal menu interazione con NPC passivi
    /// </summary>
    private void gestisciChiusuraNegozio()
    {
        if (controllerInput.Player.UscitaMenu.IsPressed())
        {
            negozio.chiudiPannelloNegozio();
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }

    /// <summary>
    /// Il metodoo permette di uscire dal menu interazione con NPC passivi
    /// </summary>
    private void gestisciChiusuraPannelloPassanti()
    {
        if (controllerInput.Player.UscitaMenu.IsPressed())
        {
            interazionePassanti.chiudiPannelloInterazionePassanti();
            npcPassivo.stopAnimazioneParlata(gameObject.transform);
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }


    /// <summary>
    /// Il metodoo permette di uscire dal menu ricettario
    /// </summary>
    private void gestisciChiusuraRicettario()
    {
        if (controllerInput.Player.UscitaMenu.IsPressed())
        {
            ricettarioScript.chiudiRicettario();
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }

    /// <summary>
    /// Il metodoo permette di uscire dal menu aiuto
    /// </summary>
    private void gestisciChiusuraMenuAiuto()
    {
        if (controllerInput.Player.UscitaMenu.IsPressed())
        {
            menuAiuto.chiudiPannelloMenuAiuto();
            PuntatoreMouse.disabilitaCursore();
            playerRiprendiMovimento.Invoke();
        }
    }

    /// <summary>
    /// Il metodo controlla se il giocatore sta puntando un NPC passivo
    /// </summary>
    /// <returns>booleano, True: NPC passivo puntato, False: NPC passivo non puntato</returns>
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

    /// <summary>
    /// Il metodo controlla se il giocatore sta puntando il negoziante
    /// </summary>
    /// <returns>booleano, True: negoziante puntato, False: negoziante non puntato</returns>
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

    /// <summary>
    /// Il metodo permette la chiusura dell'interfaccia dell'interazione con il PC, aggiornando i valori della gui e ri-attivando i movimenti del player
    /// </summary>
    public void esciDaInterazionePC()
    {
        playerRiprendiMovimento.Invoke();

        ritornaAllaPosizioneNormale();
        CambioCursore.cambioCursoreNormale();
        PuntatoreMouse.disabilitaCursore();
        magazzino.chiudiPannelloMagazzino();
    }

    /// <summary>
    /// Il metodo controlla se il giocatore sta puntando la porta del ristorante
    /// </summary>
    /// <returns>booleano, True: porta puntato, False: porta non puntato</returns>
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

    /// <summary>
    /// Il metodo controlla se il giocatore sta puntando il PC del magazzino
    /// </summary>
    /// <returns>booleano, True: PC puntato, False: PC non puntato</returns>
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

    /// <summary>
    /// Il metodo permette la chiusura dell'interfaccia con l'interazione con il cliente, aggiornando i valori della gui e ri-attivando i movimenti del player
    /// </summary>
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

    /// <summary>
    /// Il metodo resetta alla posizione originale la telecamera del giocatore
    /// </summary>
    private void ritornaAllaPosizioneNormale()
    {
        mainCamera.transform.position = posizioneCamera.position;
    }

    /// <summary>
    /// Il metodo chiude il pannello Menu Cliente
    /// </summary>
    private void chiudiPannello()
    {
        pannelloMenuCliente.ChiudiPannelloMenuCliente();
        pannelloApertoChiuso();
    }

    /// <summary>
    /// Il metodo restituisce il riferimento della classe Player attiva
    /// </summary>
    /// <returns>Player attivo</returns>
    public Player getPlayer()
    {
        return giocatore;
    }
}