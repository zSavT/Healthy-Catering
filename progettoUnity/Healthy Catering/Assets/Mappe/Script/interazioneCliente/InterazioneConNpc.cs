using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class InterazioneConNpc : MonoBehaviour
{
    private GameObject triggeringNpc;
    //check se il player è nell'area per il trigger
    private bool triggering;

    //testo che deve apparire per l'interazione
    public GameObject npcText;

    public UnityEvent premutoF;
    public UnityEvent uscitaMenu;


    private void Start()
    {
        
    }

    private void Update()
    {
        if (triggering)
        {
            //!npcText.activeSelf
            npcText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                print("oke"); // qui devo mettere lo script per far apparire i pannelli con menu e dati cliente+
                premutoF.Invoke();
            }
        }
        else
        {
            uscitaMenu.Invoke();
            npcText.SetActive(false);
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
