using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interazione : MonoBehaviour
{

    public LayerMask layerUnity = 6;
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
            Debug.Log(colpito.collider.name);
        }
        
        
    }
}
