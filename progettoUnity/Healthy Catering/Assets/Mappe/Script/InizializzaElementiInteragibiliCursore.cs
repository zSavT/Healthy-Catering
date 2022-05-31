using System;
using UnityEngine;

public class InizializzaElementiInteragibiliCursore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] oggettiInteragibili = GameObject.FindGameObjectsWithTag("Interagibile");
        foreach (GameObject elemento in oggettiInteragibili)
        {
            elemento.AddComponent(Type.GetType("CambioCursore"));
        }
    }

}
