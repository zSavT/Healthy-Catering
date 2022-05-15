using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SceltaImpostazioniPlayer : MonoBehaviour
{

    [SerializeField] private GameObject elementiGenereNeutro;
    [SerializeField] private TMP_Dropdown dropDownGenereModello;
    [SerializeField] private GameObject tastoIndietro;
    [SerializeField] private TMP_InputField inputFieldNomeGiocatore;
    [SerializeField] private GameObject nomeGiaPreso;
    [SerializeField] private Button bottoneSalva;
    private List<Player> player;
    private List<string> nomiPlayerPresenti;
    private string nomeGiocatoreScritto;
    private int sceltaGenere;
    private int sceltaColorePelle;
    private int sceltaModelloPlayer;
    bool genereNeutroScelto = false;

    // Start is called before the first frame update
    void Start()
    {
        player = new List<Player>();
        nomiPlayerPresenti = new List<string>();
        genereNeutroScelto = false;
        nomeGiaPreso.SetActive(false);
        elementiGenereNeutro.SetActive(false);
        controlloEsistenzaProfiliPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        controlloNomeEsistente();
        //print(nomeGiocatoreScritto);
        //dropDownGenereModello.value = caricaGenereModello(nomeGiocatoreScritto);
    }




    private void controlloNomeEsistente()
    {
        nomiPlayerPresenti.Add("pippo");
        if (nomeGiocatoreScritto != "")
        {
            if (nomiPlayerPresenti.Contains(nomeGiocatoreScritto))
            {
                nomeGiaPreso.SetActive(true);
                bottoneSalva.interactable = false; 
            } else
            {
                nomeGiaPreso.SetActive(false);
                bottoneSalva.interactable = true;
            }
        }
    }

    private void aggiuntaNomiPresentiInLista()
    {
        for (int i = 0; i < player.Count; i++)
        {
            nomiPlayerPresenti.Add(player[i].nome);
        }
    }


    private void controlloEsistenzaProfiliPlayer()
    {
        letturaNomiUtenti();
        if (presentePlayer())
        {
            aggiuntaNomiPresentiInLista();
            attivaTastoIndietro();

        } else
        {
            disattivaTastoIndietro();
        }
    }

    private bool presentePlayer()
    {
        if (player.Count > 0)
        {
            print("pieno");
            return true;
        } else
        {
            print("vuoto");
            return false;
        }
    }

    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto<Player>(new Player());
        
    }

    private void attivaTastoIndietro()
    {
        tastoIndietro.SetActive(true);
    }

    private void disattivaTastoIndietro()
    {
        tastoIndietro.SetActive(false);
    }

    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    public void leggiInputNomeScritto(string testo)
    {
        nomeGiocatoreScritto = testo;
    }

    public void setPellePlayer(int indice)
    {
        sceltaColorePelle = indice;
    }

    public void salvaImpostazioni()
    {
        salvaNomeGiocatore(nomeGiocatoreScritto);
        salvaGenereGiocatore(nomeGiocatoreScritto, sceltaGenere);
        salvaColorePelle(nomeGiocatoreScritto, sceltaColorePelle);
        if (genereNeutroScelto)
        {
            salvaGenereModello(nomeGiocatoreScritto, sceltaModelloPlayer);
        }
    }

    public void dropdownGenere(int indiceScelta)
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


    public void salvaGenereModello(string nomeGiocatore, int scelta)
    {

        PlayerPrefs.SetInt(nomeGiocatore + "_modello", scelta);

    }

    public int caricaGenereModello(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_modello");
    }

    public void salvaNomeGiocatore(string nomeInserito)
    {

        PlayerPrefs.SetString("PlayerName_" + nomeInserito, nomeInserito);

    }

    public void salvaColorePelle(string nomeGiocatore, int scelta)
    {

        PlayerPrefs.SetInt(nomeGiocatore + "_pelle", scelta);

    }


    public int caricaColorePelle(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_pelle");
    }

    public void salvaGenereGiocatore(string nomeGiocatore, int scelta)
    {
        PlayerPrefs.SetInt(nomeGiocatore + "_genere", scelta);
        if (scelta == 0 || scelta == 1)
        {
            salvaGenereModello(nomeGiocatore, scelta);
        }
    }

    public int caricaGenereGiocatore(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_genere");
    }

    public void setSceltaModelloGiocatore(int indice)
    {
        sceltaModelloPlayer = indice;
    }

}
