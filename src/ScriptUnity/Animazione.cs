using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *   Script da inserire sul modello 3D
 */


public class Animazione : MonoBehaviour
{
    public Animator anim;                                               //Da collegare l'animazione su Unity

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("vertical", Input.GetAxis("Vertical"));
    }
}
