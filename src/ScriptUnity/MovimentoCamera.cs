using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoCamera : MonoBehaviour
{
    public Transform PosizioneCamera;

    // Update is called once per frame
    void Update()
    {
        transform.position = PosizioneCamera.position;
    }
}
