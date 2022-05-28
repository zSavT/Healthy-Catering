using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using Wilberforce;

public class SceltaImpostazioniPlayer : MonoBehaviour
{

    [SerializeField] private GameObject elementiGenereNeutro;
    [SerializeField] private GameObject tastoIndietro;
    [SerializeField] private TMP_InputField inputFieldNomeGiocatore;
    [SerializeField] private GameObject nomeGiaPreso;
    [SerializeField] private GameObject elementiSalvataggio;
    [SerializeField] private GameObject elementiConferma;
    [SerializeField] private Button bottoneSalva;
    [SerializeField] private Camera camera;
    private List<Player> player = new List<Player>();
    private List<string> nomiPlayerPresenti = new List<string>();
    private string nomeGiocatoreScritto;
    private int sceltaGenere;
    private int sceltaColorePelle;
    private int sceltaModelloPlayer;
    bool genereNeutroScelto = false;

    // Start is called before the first frame update
    void Start()
    {
        nomeGiocatoreScritto = "";
        camera.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        player = new List<Player>();
        nomiPlayer();
        genereNeutroScelto = false;
        nomeGiaPreso.SetActive(false);
        elementiGenereNeutro.SetActive(false);
        controlloEsistenzaProfiliPlayer();
        elementiSalvataggio.SetActive(false);
        elementiConferma.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        controlloNomeEsistente();
    }

    public void nomiPlayer()
    {
        List<Player> listaPlayer = Database.getDatabaseOggetto<Player>(new Player());
        if (listaPlayer != null)
        {
            for (int i = 0; i < listaPlayer.Count; i++)
            {
                nomiPlayerPresenti.Add(listaPlayer[i].nome);
            }
        }
    }

    private void controlloNomeEsistente()
    {
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

    public void controlloEsistenzaProfiliPlayer()
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
            return true;
        } else
        {
            return false;
        }
    }

    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto(new Player());
        
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

    public void salvaImpostazioni()
    {
        PlayerSettings.salvaNomePlayerGiocante(nomeGiocatoreScritto);
        PlayerSettings.salvaGenereGiocatore(nomeGiocatoreScritto, sceltaGenere);
        PlayerSettings.salvaColorePelle(nomeGiocatoreScritto, sceltaColorePelle);
        if (genereNeutroScelto)
        {
            PlayerSettings.salvaGenereModello3D(nomeGiocatoreScritto, sceltaModelloPlayer);
        }
        Database.salvaNuovoOggettoSuFile(new Player(nomeGiocatoreScritto));
    }

    public void leggiInputNomeScritto(string testo)
    {
        nomeGiocatoreScritto = testo;
    }

    public void setPellePlayer(int indice)
    {
        sceltaColorePelle = indice;
    }

    public void setSceltaModelloGiocatore(int indice)
    {
        sceltaModelloPlayer = indice;
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

}
