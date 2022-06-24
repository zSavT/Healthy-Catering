using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoreClienti : MonoBehaviour
{
    [SerializeField] private GameObject[] vettoreClienti;


    private void Start()
    {
        vettoreClienti[0].SetActive(true);
    }

    public void attivaClienteSuccessivo()
    {
        vettoreClienti[Interactable.numeroCliente].SetActive(true);
    }

}
