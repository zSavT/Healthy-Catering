using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpzioniModello3D : MonoBehaviour
{
    private float r;
    private float target;
    private void Start()
    {
        
    }

    private void Update()
    {
        float angolo = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref r, 0.1f);
        transform.rotation = Quaternion.Euler(0, angolo, 0);
        if (target >= 360)
            target = 0;
        target = target + 10;

    }

    public void rotazioneModello3D(float angolo)
    {
        target = angolo;
    }
}
