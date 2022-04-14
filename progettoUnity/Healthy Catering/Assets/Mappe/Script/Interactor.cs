using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnity = 6;              //layer utilizzato da Unity per le categorie di oggetto
    private UnityEvent onInteract;                                  //attributo per l'invocazione di eventi all'interno di Unity    
    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione
    // Start is called before the first frame update
    [Header("CrossHair")]
    public RawImage crossHair;                                      //riferimento allo sprit del crossHair
    public Color32 coloreNormale;                                   //colore base del crossHair
    public Color32 coloreInterazione;                               //colore del crossHair quando viene in contatto con un entità interagibile
    void Start()
    {
        crossHair.color = coloreNormale;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit colpito;
         //se inquadrato dal player(tramite l'ausilio della camera)
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out colpito, 2, layerUnity)) 
        {
            if(colpito.collider.GetComponent<Interactable>() != false)
            {
                onInteract = colpito.collider.GetComponent<Interactable>().onInteract;
                crossHair.color = coloreInterazione;
                if (Input.GetKeyDown(tastoInterazione))
                {
                    /*
                        Quello che c'è qui dentro viene eseguito quando il player inquadra l'oggetto e preme il comando.
                        Debug.Log(colpito.collider.name);       //per test in game
                    */
                    onInteract.Invoke();
                }
            }
        } else
        {
            crossHair.color = coloreNormale;
        }
    }
}
