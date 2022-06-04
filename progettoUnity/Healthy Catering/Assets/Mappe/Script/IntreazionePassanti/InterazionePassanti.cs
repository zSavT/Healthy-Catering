using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InterazionePassanti : MonoBehaviour
{
    [SerializeField] private GameObject pannelloInterazionePassanti;
    [SerializeField] private TextMeshProUGUI testoInterazionePassanti;

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;

    private void Start()
    {
        pannelloInterazionePassanti.SetActive(false);
    }

    private void Update()
    {
        
    }

    public void apriPannelloInterazionePassanti()
    { 
        pannelloInterazionePassanti.SetActive(true);
        testoInterazionePassanti.text = trovaScrittaDaMostrare();

    }

    private string trovaScrittaDaMostrare()
    {
        return "ciao";
    }
}
