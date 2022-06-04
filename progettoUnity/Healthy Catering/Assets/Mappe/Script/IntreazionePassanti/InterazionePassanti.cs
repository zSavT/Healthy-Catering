using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InterazionePassanti : MonoBehaviour
{
    [SerializeField] private GameObject pannelloInterazionePassanti;
    private bool pannelloInterazionePassantiAperto;
    [SerializeField] private TextMeshProUGUI testoInterazionePassanti;

    private List<string> tutteLeScritte;

    private void Start()
    {
        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
        
        getTutteLeScritteInterazione();
    }

    private void Update()
    {
    }

    private void getTutteLeScritteInterazione() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "stringheInterazioniPassanti.txt");

        tutteLeScritte = File.ReadAllLines(filePath).ToList();
    }

    public void apriPannelloInterazionePassanti()
    { 
        pannelloInterazionePassanti.SetActive(true);
        testoInterazionePassanti.text = trovaScrittaDaMostrare();
        pannelloInterazionePassantiAperto = true;
    }

    public bool getPannelloInterazionePassantiAperto()
    {
        return pannelloInterazionePassantiAperto;
    }

    public void chiudiPannelloInterazionePassanti()
    {
        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
    }

    private string trovaScrittaDaMostrare()
    {
        string output = "";
        
        return output;
    }
}
