using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnityNPC = 6;              //layer utilizzato da Unity per le categorie di oggetto

    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione

    //trigger per la scritta dell'interazione
    public UnityEvent inquadratoNPC;
    public UnityEvent uscitaRangeMenu;

    public GameObject NPC;
    public GameObject Player;
    public CameraGiocatore mainCamera;

    public UnityEvent playerStop;
    public UnityEvent playerRiprendiMovimento;

    public Transform posizioneCamera;
    public static bool pannelloAperto;

    public GameObject pannelloCliente;

    void Start()
    {
        chiudiPannello();
        pannelloAperto = false;
    }

    // Update is called once per frame
    void Update()
    {
        interazioneUtenteNPC();
    }

    private void interazioneUtenteNPC()
    {
        if (NPCInteragibilePuntato ())
        {
            inquadratoNPC.Invoke();
            

            if (Input.GetKeyDown(tastoInterazione))
            {
                interazioneCliente(/*Cliente.nomeClienteToCliente (NPC.tag)//ora gli passo solo un numero*/0, Player, NPC);      
            }
        }

        else
        {
            uscitaRangeMenu.Invoke();
            if (pannelloAperto)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    esciDaInterazioneCliente();
                }
            }
        }
    }

    private bool NPCInteragibilePuntato()
    {
        RaycastHit NPCpuntato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCpuntato, 2, layerUnityNPC))
        {
            // se l'oggetto visualizzato è interagibile
            if (NPCpuntato.collider.GetComponent<Interactable>() != false)
            {
                return true;
            }
        }
        return false;
    }
    private void esciDaInterazioneCliente()
    {
        playerRiprendiMovimento.Invoke();
        chiudiPannello();

        ritornaAllaPosizioneNormale();

        PuntatoreMouse.disabilitaCursore();
        mainCamera.lockUnlockVisuale();
    }

    private void ritornaAllaPosizioneNormale()
    {
        var nuovaPosizione = posizioneCamera.position;
        nuovaPosizione = nuovaPosizione + (Vector3.up * 1.47f);//TODO sostituire il 1.47f con il valore dell'altezza della camera alla partenza
        mainCamera.transform.position = nuovaPosizione;
    }

    private void pannelloApertoChiuso()
    {
        pannelloAperto = !pannelloAperto;
    }

    private void interazioneCliente(/*Cliente clienteTemp*/ int clienteTemp, GameObject Player, GameObject NPC)
    {
        muoviCameraPerInterazioneCliente();

        playerStop.Invoke();

        apriPannello();


        mainCamera.lockUnlockVisuale();
        PuntatoreMouse.abilitaCursore();
    }

    private void muoviCameraPerInterazioneCliente()
    {
        //prende la posizione della camera
        Vector3 playerPos = mainCamera.transform.position;

        //prende la posizione dell'npc
        Vector3 newDestination = NPC.transform.position;

        //modifico la destinazione in base alla posizione dell'npc
        newDestination = newDestination + (Vector3.left * 3.5f);//left va cambiato in base alla posizione degli npc nel ristornate
        newDestination = newDestination + (Vector3.up * 2.5f);

        //cambio la posizione della camera
        mainCamera.transform.position = Vector3.Lerp(playerPos, newDestination, 100f);
    }

    private void apriPannello()
    {
        if (pannelloCliente != null)
        {
            Vector3 newDestination = NPC.transform.position;
            pannelloCliente.transform.position = newDestination;

            pannelloCliente.SetActive(true);
        }
        pannelloApertoChiuso();
    }

    private void chiudiPannello()
    {
        if (pannelloCliente != null)
        {
            pannelloCliente.SetActive(false);
        }
        pannelloApertoChiuso();
    }
}