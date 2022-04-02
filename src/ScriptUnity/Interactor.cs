using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{

    public LayerMask layerUnity = 6;
    UnityEvent onInteract;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit colpito;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out colpito, 2, layerUnity))
        {
            if(colpito.collider.GetComponent<Interactable>() != false)
            {
                onInteract = colpito.collider.GetComponent<Interactable>().onInteract;
                if(Input.GetKeyDown(KeyCode.E))
                {
                    onInteract.Invoke();
                    Debug.Log(colpito.collider.name);
                }
            }
            
        }
        
    }
}
