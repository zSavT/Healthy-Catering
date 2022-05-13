using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceltaImpostazioniPlayer : MonoBehaviour
{

    [SerializeField] private GameObject elementiGenereNeutro;
    [SerializeField] private Dropdown dropDownGenereModello;
    private string nomeGiocatoreScritto;
    private int sceltaGenere;
    private int sceltaColorePelle;
    private int sceltaModelloPlayer;
    bool genereNeutroScelto = false;

    // Start is called before the first frame update
    void Start()
    {
        genereNeutroScelto = false;
        elementiGenereNeutro.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        //print(nomeGiocatoreScritto);
        //dropDownGenereModello.value = caricaGenereModello(nomeGiocatoreScritto);
    }

    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
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
        print(caricaGenereGiocatore(nomeGiocatoreScritto));
        print(caricaGenereModello(nomeGiocatoreScritto));
        if (genereNeutroScelto)
        {
            salvaGenereModello(nomeGiocatoreScritto, sceltaModelloPlayer);
        }
        print(caricaGenereGiocatore(nomeGiocatoreScritto));
        print(caricaGenereModello(nomeGiocatoreScritto));
        print(nomeGiocatoreScritto);
        print(PlayerPrefs.HasKey(nomeGiocatoreScritto + "_mod"));
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
