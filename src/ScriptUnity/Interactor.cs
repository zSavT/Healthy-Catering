using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnity = 6;              //layer utilizzato da Unity per le categorie di oggetto
    private UnityEvent onInteract;                                  //attributo per l'invocazione di eventi all'interno di Unity    
    [SerializeField] private KeyCode tastoInterazione;              //tasto da premere per invocare l'azione
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit colpito;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out colpito, 2, layerUnity))  //se inquadrato dal player(tramite l'ausilio della camera)
        {
            if(colpito.collider.GetComponent<Interactable>() != false)
            {
                onInteract = colpito.collider.GetComponent<Interactable>().onInteract;
                if(Input.GetKeyDown(tastoInterazione))
                {
                    /*
                        Quello che c'è qui dentro viene eseguito quando il player inquadra l'oggetto e preme il comando.
                        Debug.Log(colpito.collider.name);       //per test in game
                    */
                    onInteract.Invoke();
                }
            }
        }
    }
}
