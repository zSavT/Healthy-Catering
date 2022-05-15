using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using Wilberforce;

public class SelezioneProfiloUtenteEsistente : MonoBehaviour
{

    [SerializeField] private GameObject elementiGenereNeutro;
    [SerializeField] private TMP_Dropdown dropDownListaPlayer;
    [SerializeField] private TMP_Dropdown dropDownGenere;
    [SerializeField] private TMP_Dropdown dropDownColorePelle;
    [SerializeField] private TMP_Dropdown dropDownModello3D;
    [SerializeField] private Camera camera;
    private List<Player> player;
    private List<string> nomiPlayerPresenti;
    private string nomeSelezionato = "";
    private int sceltaGenere;
    private int sceltaColorePelle;
    private int sceltaModelloPlayer;
    bool genereNeutroScelto = false;


    // Start is called before the first frame update
    void Start()
    {
        camera.GetComponent<Colorblind>().Type = PlayerPrefs.GetInt("daltonismo");
        player = new List<Player>();
        nomiPlayerPresenti = new List<string>();
        // letturaNomiUtenti();
        aggiuntaNomiDropdown();
        nomeSelezionato = dropDownListaPlayer.options[dropDownListaPlayer.value].text;
        dropDownGenere.value = caricaGenereGiocatore(nomeSelezionato);
        dropDownColorePelle.value = caricaColorePelle(nomeSelezionato);
        dropDownModello3D.value = caricaGenereModello(nomeSelezionato);
    }

    public void refreshValori()
    {
        dropDownGenere.value = caricaGenereGiocatore(nomeSelezionato);
        dropDownColorePelle.value = caricaColorePelle(nomeSelezionato);
        dropDownModello3D.value = caricaGenereModello(nomeSelezionato);
    }

    public void salvaImpostazioni()
    {
        salvaNomePlayerGiocante(nomeSelezionato);
        salvaGenereGiocatore(nomeSelezionato, sceltaGenere);
        salvaColorePelle(nomeSelezionato, sceltaColorePelle);
        if (genereNeutroScelto)
        {
            salvaGenereModello(nomeSelezionato, sceltaModelloPlayer);
        }
        SceneManager.LoadScene(0);
    }

    public void setSceltaModelloGiocatore(int indice)
    {
        sceltaModelloPlayer = indice;
    }

    public void setGenerePlayer(int indiceScelta)
    {
        sceltaGenere = indiceScelta;
        if (indiceScelta == 2)
        {
            genereNeutroScelto = true;
            elementiGenereNeutro.SetActive(true);
        }
        else
        {
            genereNeutroScelto = false;
            elementiGenereNeutro.SetActive(false);
        }
    }

    public void setPellePlayer(int indice)
    {
        sceltaColorePelle = indice;
    }


    public void salvaGenereModello(string nomeGiocatore, int scelta)
    {

        PlayerPrefs.SetInt(nomeGiocatore + "_modello", scelta);

    }

    public void salvaNomePlayerGiocante(string nomeInserito)
    {

        PlayerPrefs.SetString("PlayerName", nomeInserito);

    }

    public void salvaColorePelle(string nomeGiocatore, int scelta)
    {

        PlayerPrefs.SetInt(nomeGiocatore + "_pelle", scelta);

    }

    public void salvaGenereGiocatore(string nomeGiocatore, int scelta)
    {
        PlayerPrefs.SetInt(nomeGiocatore + "_genere", scelta);
        if (scelta == 0 || scelta == 1)
        {
            salvaGenereModello(nomeGiocatore, scelta);
        }
    }


    public int caricaGenereModello(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_modello");
    }


    public int caricaColorePelle(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_pelle");
    }


    public int caricaGenereGiocatore(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_genere");
    }

    public void indiceSceltaNomeUtente(int indice)
    {
        nomeSelezionato = dropDownListaPlayer.options[dropDownListaPlayer.value].text;
    }

    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto<Player>(new Player());
    }


    private bool presentePlayer()
    {
        if (player.Count > 0)
        {
            print("pieno");
            return true;
        }
        else
        {
            print("vuoto");
            return false;
        }
        print("ue");
    }

    private void aggiuntaNomiDropdown()
    {
        /*
        if(presentePlayer())
        {
            for (int i = 0; i < player.Count; i++)
            {
                nomiPlayerPresenti.Add(player[i].nome);
            }
        }
        */
        nomiPlayerPresenti.Add("sav");
        nomiPlayerPresenti.Add("pippo");
        nomiPlayerPresenti.Add("nicola");
        dropDownListaPlayer.AddOptions(nomiPlayerPresenti);
    }


}
