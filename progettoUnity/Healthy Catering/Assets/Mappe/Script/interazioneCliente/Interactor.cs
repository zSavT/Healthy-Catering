using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactor : MonoBehaviour
{
    [SerializeField] private MenuAiuto menuAiuto;

    [Header("Interazione NPC")]
    [SerializeField] private LayerMask layerUnityNPC = 6;              //layer utilizzato da Unity per le categorie di oggetto

    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform posizioneCamera;
    [SerializeField] private Transform posizioneCameraMenuCliente;
    [SerializeField] private GameObject pannelloMenuECliente;
    [SerializeField] private HudInGame hud;

    [Header("Interazione Magazzino")]
    [SerializeField] private PannelloMagazzino magazzino;

    [Header("Eventi")]
    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;
    //trigger per la scritta dell'interazione
    [SerializeField] private UnityEvent inquadratoNPC;
    [SerializeField] private UnityEvent uscitaRangeMenu;

    private Vector3 posizioneCameraOriginale;

    private bool menuApribile;
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
        interazioneUtenteNPC();
    }

    public void menuApribileOnOff()
    {
        menuApribile = !menuApribile;
    }

    private void interazioneUtenteNPC()
    {
        if (menuApribile)
        {
            if (NPCInteragibilePuntato())
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
                    magazzino.attivaPannello();
                    playerStop.Invoke();
                    PuntatoreMouse.abilitaCursore();
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
        }
    }


    private bool pcPuntato()
    {
        RaycastHit pcInquadrato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out pcInquadrato, 2, layerUnityNPC))
        {
            Debug.Log("CheckComputer");
            // se l'oggetto visualizzato è interagibile
            if (pcInquadrato.collider.GetComponent<PannelloMagazzino>() != false)
            {
                Debug.Log("Computer puntato");
                return true;
            }
        }
        return false;
    }

    private bool NPCInteragibilePuntato()
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