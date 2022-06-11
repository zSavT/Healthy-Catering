using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactor : MonoBehaviour
{
    [Header("Interazione Cliente")]
    [SerializeField] private LayerMask layerUnityNPC = 6;              //layer utilizzato da Unity per le categorie di oggetto

    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform posizioneCamera;
    [SerializeField] private Transform posizioneCameraMenuCliente;
    [SerializeField] private GameObject pannelloMenuECliente;
    [SerializeField] private HudInGame hud;

    [Header("Interazione Negozio")]
    [SerializeField] private PannelloNegozio negozio;


    [Header("Interazione Magazzino")]
    [SerializeField] private PannelloMagazzino magazzino;

    [Header("Interazione NPC passivi")]
    [SerializeField] private LayerMask layerUnityNPCPassivi = 7;
    [SerializeField] private InterazionePassanti interazionePassanti;
    private InteractableNPCPassivi npcPassivo;  

    [Header("Eventi")]
    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;
    //trigger per la scritta dell'interazione
    [SerializeField] private UnityEvent inquadratoNPC;
    [SerializeField] private UnityEvent uscitaRangeMenu;

    private Vector3 posizioneCameraOriginale;

    private bool menuApribile;                                                      //se il menu opzioni è aperto, le interezioni sono disattivate
    public static bool pannelloAperto;
    private int IDClientePuntato;
    private Player giocatore;

    private Interactable npc;

    void Start()
    {
        try
        {
            giocatore = Database.getPlayerDaNome(PlayerSettings.caricaNomePlayerGiocante());
            giocatore.punteggio = 0;
            giocatore.soldi = 0f;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            PlayerSettings.profiloUtenteCreato = false;
            SelezioneLivelli.caricaMenuCreazioneProfiloUtente();
        }
        chiudiPannello();
        pannelloAperto = false;
        posizioneCameraOriginale = mainCamera.transform.position;
        menuApribile = true;
    }

    void Update()
    {
        interazioneUtenteConNPCVari();
    }

    public void menuApribileOnOff()
    {
        menuApribile = !menuApribile;
    }

    private bool NPCPassantePuntato()
    {
        RaycastHit NPCPassivoInquadrato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCPassivoInquadrato, 3, layerUnityNPCPassivi))
        {
            if(NPCPassivoInquadrato.collider.GetComponent<InteractableNPCPassivi>() != false)
            { 
                npcPassivo = NPCPassivoInquadrato.collider.GetComponent<InteractableNPCPassivi>();
                return true;
            }
            
        }
        return false;
    }

    private void interazioneUtenteConNPCVari()
    {
        if (menuApribile)
        {
            if (NPCClientePuntato())
            {
                inquadratoNPC.Invoke();
                if (Input.GetKeyDown(tastoInterazione))
                {
                    interazioneCliente(IDClientePuntato);
                }
            } else if (pcPuntato())
            {
                inquadratoNPC.Invoke();
                if (Input.GetKeyDown(tastoInterazione))
                {
                    magazzino.apriPannelloMagazzino();
                    playerStop.Invoke();
                    PuntatoreMouse.abilitaCursore();
                    CambioCursore.cambioCursoreNormale();
                }
            }
            else if (NPCPassantePuntato())
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
            } else if (negozianteInquadrato())
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
            else
            {
                uscitaRangeMenu.Invoke();
                if (pannelloAperto)
                {
                    if(!PannelloMenu.pannelloIngredientiPiattoAperto && !PannelloMenu.pannelloConfermaPiattoAperto && !PannelloMenu.pannelloIngredientiGiustiSbagliatiAperto)
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            esciDaInterazioneCliente();
                        }
                        
                    }
                } 
            }
            if(negozio.getPannelloAperto())
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    negozio.chiudiPannelloNegozio();
                    PuntatoreMouse.disabilitaCursore();
                    playerRiprendiMovimento.Invoke();
                }
            }
            if (interazionePassanti.getPannelloInterazionePassantiAperto())
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    interazionePassanti.chiudiPannelloInterazionePassanti();
                    npcPassivo.stopAnimazioneParlata(gameObject.transform);
                    PuntatoreMouse.disabilitaCursore();
                    playerRiprendiMovimento.Invoke();
                }
            }
        }
    }

    private bool negozianteInquadrato()
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
;               return true;
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
            Debug.Log(PannelloMenu.clienteServito);
            hud.bloccaAnimazioniParticellari();
            PannelloMenu.clienteServito = false; //non si può vare il contrario, perchè in caso di apertura consecuitiva del pannello senza servire, la seconda volta risulterà servito
        } 
        else
        {
            hud.aggiornaValorePunteggio(giocatore.punteggio);
            hud.aggiornaValoreSoldi(giocatore.soldi);
        }
    }

    private void ritornaAllaPosizioneNormale()
    {
        mainCamera.transform.position = posizioneCamera.position;
    }

    private void pannelloApertoChiuso()
    {
        pannelloAperto = !pannelloAperto;
    }

    private void interazioneCliente(int IDCliente)
    {
        muoviCameraPerInterazioneCliente();

        playerStop.Invoke();

        apriPannello();

        pannelloMenuECliente.GetComponent<PannelloMenu>().setCliente(IDClientePuntato, giocatore, npc);

        PuntatoreMouse.abilitaCursore();
    }

    private void muoviCameraPerInterazioneCliente()
    {
        mainCamera.transform.position = posizioneCameraMenuCliente.transform.position;
    }

    private void apriPannello()
    {
        if (pannelloMenuECliente != null)
        {
            pannelloMenuECliente.SetActive(true);
        }
        pannelloApertoChiuso();
    }

    private void chiudiPannello()
    {
        if (pannelloMenuECliente != null)
        {
            pannelloMenuECliente.SetActive(false);
        }
        pannelloApertoChiuso();
    }
}