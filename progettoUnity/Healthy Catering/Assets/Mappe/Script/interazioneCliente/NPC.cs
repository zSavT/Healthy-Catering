using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Transform modelloNPC;
    public Transform backgroundPannelloPiatti;

    public PannelloPiattiDisponibili pannelloPiattiDisponibili;

    public string nomePiatto;
    [TextArea(5, 10)]
    public string descrizionePiatto;

    // Start is called before the first frame update
    void Start()
    {
        pannelloPiattiDisponibili = FindObjectOfType <PannelloPiattiDisponibili> ();
    }

    // Update is called once per frame
    void Update()
    {
        //prendo la posizione del modello npc (che sta venendo puntato dalla main camera)
        Vector3 pos = Camera.main.WorldToScreenPoint(modelloNPC.position);
        //e dico al background del pannello dei piatti di mettersi poco più a sinistra
        pos.x -= 200;
        backgroundPannelloPiatti.position = pos;
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<NPC> ().enabled = true;
        FindObjectOfType<PannelloPiattiDisponibili>().EnterRangeOfNPC();

        if ((other.gameObject.tag.Equals ("player")) && (Input.GetKeyDown (KeyCode.F)))
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            pannelloPiattiDisponibili.nome.text = nomePiatto;
            pannelloPiattiDisponibili.descrizione.text = descrizionePiatto;
            //FindObjectOfType<PannelloPiattiDisponibili>().nomeNPC();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //FindObjectOfType<PannelloPiattiDisponibili>().outOfRange();
        this.gameObject.GetComponent<NPC> ().enabled = false;
    }


}
