using System;
using UnityEngine;

/// <summary>
/// Classe per aggiungere in maniera automatica lo script del cambio cursore agli oggetti con il Tag "Interagibile".<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// A qualsiasi oggetto nella scena.
/// </para>
/// </summary>
public class InizializzaElementiInteragibiliCursore : MonoBehaviour
{
    void Start()
    {
        GameObject[] oggettiInteragibili = GameObject.FindGameObjectsWithTag("Interagibile");
        foreach (GameObject elemento in oggettiInteragibili)
        {
            elemento.AddComponent(Type.GetType("CambioCursore"));
        }
    }

}
