using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelloMagazzino : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMagazzino;
    [SerializeField] private Image sfondoImmaginePC;
    [SerializeField] private GameObject sottoPannelloMostraInventario3Elementi;


    private void Start()
    {
        pannelloMagazzino.SetActive(false);
    }


    public void attivaPannello()
    {
        pannelloMagazzino.SetActive(true);
        popolaSchermata();
    }

    private void popolaSchermata()
    {
        print(sottoPannelloMostraInventario3Elementi.GetComponentInChildren<Button>().name);
    }

    public void cambiaSfondo()
    {
        sfondoImmaginePC.sprite = Resources.Load<Sprite>("SchermataMagazzino");
    }

}
