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

    void Start()
    {
        crossHair.color = coloreNormale;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit NPCpuntato;
         //se inquadrato dal player(tramite l'ausilio della camera)
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCpuntato, 2, layerUnityNPC)) 
        {
            if(NPCpuntato.collider.GetComponent<Interactable>() != false) //l'oggetto visualizzato ha è interagibile
            {
                onInteract = NPCpuntato.collider.GetComponent<Interactable>().onInteract; //richiama l'evento
                crossHair.color = coloreInterazione;//cambia colore crosshari
                
                inquadratoNPC.Invoke();
                
                if (Input.GetKeyDown(tastoInterazione))
                {
                    print("oke");
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
}