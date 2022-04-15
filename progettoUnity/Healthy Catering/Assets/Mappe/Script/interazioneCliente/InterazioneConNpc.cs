using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterazioneConNpc : MonoBehaviour
{
    //
    private GameObject triggeringNpc;
    //check se il player è nell'area per il trigger
    private bool triggering;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (triggering)
        {
            print("Player is triggering with " + triggeringNpc);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC") // se entro nell'area di trigger dell'npc
        {
            triggering = true; //metto la variabile che mi dice che sono nell'area di trigger a true
            triggeringNpc = other.gameObject; //e setto l'npc che sto triggerando nella variabile relativa
        }
    }

    private void OnTriggerExit(Collider other)
    {//stessa cosa di su ma al contrario
        if (other.tag == "NPC") 
        {
            triggering = false;
            triggeringNpc = null;
        }
    }
}
