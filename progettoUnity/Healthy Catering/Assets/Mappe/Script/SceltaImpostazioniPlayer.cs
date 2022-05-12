using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceltaImpostazioniPlayer : MonoBehaviour
{

    [SerializeField] private GameObject elementiGenereNeutro;
    private string nomeGiocatoreScritto;
    private int sceltaGenere;
    private int sceltaColorePelle;
    private int sceltaModelloPlayer;

    // Start is called before the first frame update
    void Start()
    {
        elementiGenereNeutro.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(nomeGiocatoreScritto);
    }

    public void leggiInputNomeScritto(string testo)
    {
        nomeGiocatoreScritto = testo;
    }

    public void salvaImpostazioni()
    {
        salvaNomeGiocatore(nomeGiocatoreScritto);
        salvaGenereGiocatore(nomeGiocatoreScritto, sceltaGenere);
        salvaColorePelle(nomeGiocatoreScritto, sceltaColorePelle);
        salvaGenereModello(nomeGiocatoreScritto, sceltaModelloPlayer);
    }

    public void dropdownGenere(int indiceScelta)
    {
        if(indiceScelta == 2)
        {
            elementiGenereNeutro.active = true;
            PlayerPrefs.SetInt(nomeGiocatoreScritto + "_genere", indiceScelta);
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

    public void salvaColorePelle(string nomeGiocatore, int scelta)
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
            if (scelta == 0 || scelta == 1)
            {
                PlayerPrefs.SetInt(nomeGiocatore + "_modello", scelta);
            }
        }
    }

    public int caricaGenereGiocatore(string nomeGiocatore)
    {
         return PlayerPrefs.GetInt(nomeGiocatore + "_genere");
    }

}
