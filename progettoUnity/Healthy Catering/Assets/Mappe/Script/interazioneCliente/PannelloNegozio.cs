using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelloNegozio : MonoBehaviour
{
    [SerializeField] private GameObject pannelloNegozio;
    private bool pannelloAperto = false;

    // Start is called before the first frame update
    void Start()
    {
        pannelloAperto = false;
        pannelloNegozio.SetActive(false);
    }

    public void apriPannelloNegozio()
    {
        pannelloAperto = true;
        pannelloNegozio.SetActive(true);
    }

    public void chiudiPannelloNegozio()
    {
        pannelloAperto = false;
        pannelloNegozio.SetActive(false);
    }

    public bool getPannelloAperto()
    {
        return pannelloAperto;
    }


}
