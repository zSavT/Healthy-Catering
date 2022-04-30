using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuroInvisibile : MonoBehaviour
{

    [Header("Muri Invisibili")]
    [SerializeField] private LayerMask oceano;
    [SerializeField] private LayerMask muroInvisiible;
    [SerializeField] private Transform posizioneReset;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform triggerTocco;
    [SerializeField] private float tolleranzaAltezzaContatto;
    private bool toccato;



    // Update is called once per frame
    void Update()
    {
        controlloCollisioneAcqua();
    }

    private void controlloCollisioneAcqua()
    {
        toccato = Physics.CheckSphere(triggerTocco.position, tolleranzaAltezzaContatto, oceano);
        if (toccato)
        {
            player.transform.position = posizioneReset.transform.position;
        }
    }
}
