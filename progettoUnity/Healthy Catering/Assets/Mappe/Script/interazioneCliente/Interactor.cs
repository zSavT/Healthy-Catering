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
                    NPC = GameObject.FindGameObjectWithTag("NPC");
                    Player = GameObject.FindGameObjectWithTag("Player");

                    print("premuto tasto interazione");
                    interazioneCliente(/*Cliente.nomeClienteToCliente (NPC.tag)//ora gli passo solo un numero*/0, Player, NPC);
                    uscitaRangeMenu.Invoke();
                }
            }
        }
        else
        {
            uscitaRangeMenu.Invoke();
            crossHair.color = coloreNormale;
        }
    }

    private void interazioneCliente(/*Cliente clienteTemp*/ int clienteTemp, GameObject Player, GameObject NPC)
    {
        //prende la posizione del player
        Vector3 playerPos = this.Player.transform.position;

        //prende la posizione dell'npc
        Vector3 newDestination = NPC.transform.position;

        //modifico la destinazione in base alla posizione dell'npc
        newDestination = newDestination + (Vector3.left * 4f);

        //cambio la posizione del player
        this.Player.transform.position = Vector3.Lerp(playerPos, newDestination, 100f);
    }
}