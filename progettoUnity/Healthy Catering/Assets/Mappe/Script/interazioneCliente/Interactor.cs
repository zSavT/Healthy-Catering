using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnityNPC = 6;              //layer utilizzato da Unity per le categorie di oggetto

    private UnityEvent onInteract;                                  //attributo per l'invocazione di eventi all'interno di Unity    
    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione

    [Header("CrossHair")]
    public RawImage crossHair;                                      //riferimento allo sprit del crossHair
    public Color32 coloreNormale;                                   //colore base del crossHair
    public Color32 coloreInterazione;                               //colore del crossHair quando viene in contatto con un entità interagibile

    //trigger per la scritta dell'interazione
    public UnityEvent inquadratoNPC;
    public UnityEvent uscitaRangeMenu;

    public GameObject NPC;
    public GameObject Player;
    public Camera mainCamera;

    public UnityEvent playerStop;
    public UnityEvent playerRiprendiMovimento;

    void Start()
    {
        crossHair.color = coloreNormale;
    }

    // Update is called once per frame
    void Update()
    {
        interazioneUtenteNPC();
    }

    private void interazioneUtenteNPC()
    {
        RaycastHit NPCpuntato;
        //se inquadrato dal player(tramite l'ausilio della camera)
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCpuntato, 2, layerUnityNPC))
        {
            //  l'oggetto visualizzato ha è interagibile
            if (NPCpuntato.collider.GetComponent<Interactable>() != false)
            {
                onInteract = NPCpuntato.collider.GetComponent<Interactable>().onInteract; //richiama l'evento

                crossHair.color = coloreInterazione;//cambia colore crosshar

                inquadratoNPC.Invoke();

                if (Input.GetKeyDown(tastoInterazione))
                { 
                    print("premuto tasto interazione");
                    interazioneCliente(/*Cliente.nomeClienteToCliente (NPC.tag)//ora gli passo solo un numero*/0, Player, NPC);
                    
                }


            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("ok");
                playerRiprendiMovimento.Invoke();
            }
            crossHair.color = coloreNormale;
            uscitaRangeMenu.Invoke();
        }
    }

    private void interazioneCliente(/*Cliente clienteTemp*/ int clienteTemp, GameObject Player, GameObject NPC)
    {
        muoviCameraPerInterazioneCliente();

        playerStop.Invoke();


        


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
}