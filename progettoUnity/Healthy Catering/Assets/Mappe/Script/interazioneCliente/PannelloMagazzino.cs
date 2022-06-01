using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelloMagazzino : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMagazzino;
    [SerializeField] private Image sfondoImmaginePC;

    private void Start()
    {
        pannelloMagazzino.SetActive(false);
    }


    public void attivaPannello()
    {
        pannelloMagazzino.SetActive(true);
    }


    public void cambiaSfondo()
    {
        sfondoImmaginePC.sprite = Resources.Load<Sprite> ("SchermataMagazzino");
    }

}
