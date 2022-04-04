using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *   Lo script va inserito nell'oggetto vuoto che contiene l'effettiva camera del gioco
 */
public class MovimentoCamera : MonoBehaviour
{
    public Transform PosizioneCamera;           //va collegata in Unity all'oggetto posizione camera su Unity

    // Update is called once per frame
    void Update()
    {
        transform.position = PosizioneCamera.position;
    }
}
