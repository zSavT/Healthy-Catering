using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceltaImpostazioniPlayer : MonoBehaviour
{

    [SerializeField] private GameObject elementiGenereNeutro;
    private string nomeGiocatoreScritto;

    // Start is called before the first frame update
    void Start()
    {
        elementiGenereNeutro.active = false;
    }

    // Update is called once per frame
    void Update()
    {
       // print(nomeGiocatoreScritto);
    }

    public void leggiInputNomeScritto(string testo)
    {
        nomeGiocatoreScritto = testo;
    }

    public void dropdownGenere(int indiceScelta)
    {
        if(indiceScelta == 2)
        {
            elementiGenereNeutro.active = true;
        } else
        {
            elementiGenereNeutro.active = false;
        }
    }

    public void salvaGenereModello(string nomeGiocatore, int scelta)
    {
        if (PlayerPrefs.HasKey(nomeGiocatore + "_modello"))
        {
            PlayerPrefs.SetInt(nomeGiocatore + "_modello", scelta);
        }
    }

    public int caricaGenereModello(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_modello");
    }

    public void salvaNomeGiocatore(string nomeInserito)
    {
        if (PlayerPrefs.HasKey("PlayerName_" + nomeInserito))
        {
            PlayerPrefs.SetString("PlayerName_" + nomeInserito, nomeInserito);
        }
    }

    public void saltaColorePelle(string nomeGiocatore, int scelta)
    {
        if (PlayerPrefs.HasKey(nomeGiocatore + "_pelle"))
        {
            PlayerPrefs.SetInt(nomeGiocatore + "_pelle", scelta);
        }
    }


    public int caricaColorePelle(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_pelle");
    }

    public void salvaGenereGiocatore(string nomeGiocatore, int scelta)
    {
        if(PlayerPrefs.HasKey(nomeGiocatore + "_genere"))
        {
            PlayerPrefs.SetInt(nomeGiocatore + "_genere", scelta);
        }
    }

    public int caricaGenereGiocatore(string nomeGiocatore)
    {
         return PlayerPrefs.GetInt(nomeGiocatore + "_genere");
    }

}
