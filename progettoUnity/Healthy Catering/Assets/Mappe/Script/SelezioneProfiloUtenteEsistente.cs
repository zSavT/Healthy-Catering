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
        camera.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        player = new List<Player>();
        nomiPlayerPresenti = new List<string>();
        // letturaNomiUtenti();
        aggiuntaNomiDropdown();
        dropDownListaPlayer.value = indiceNomeGiocatoreInLista(PlayerSettings.caricaNomePlayerGiocante());
        nomeSelezionato = dropDownListaPlayer.options[dropDownListaPlayer.value].text;
        dropDownGenere.value = PlayerSettings.caricaGenereGiocatore(nomeSelezionato);
        dropDownColorePelle.value = PlayerSettings.caricaColorePelle(nomeSelezionato);
        dropDownModello3D.value = PlayerSettings.caricaGenereModello3D(nomeSelezionato);
        attivaDisattivaImpostazioniGenereNeutro();
        refreshValori();
    }

    private int indiceNomeGiocatoreInLista(string nome)
    {
        int indice = 0;
        while(indice<dropDownListaPlayer.options.Count)
        {
            if(dropDownListaPlayer.options[dropDownListaPlayer.value].text == nome)
            {
                break;
            }
            indice++;
        }
        return indice;
    }

    public void refreshValori()
    {
        dropDownGenere.value = PlayerSettings.caricaGenereGiocatore(nomeSelezionato);
        dropDownColorePelle.value = PlayerSettings.caricaColorePelle(nomeSelezionato);
        dropDownModello3D.value = PlayerSettings.caricaGenereModello3D(nomeSelezionato);
    }

    public void salvaImpostazioni()
    {
        PlayerSettings.salvaNomePlayerGiocante(nomeSelezionato);
        PlayerSettings.salvaGenereGiocatore(nomeSelezionato, sceltaGenere);
        PlayerSettings.salvaColorePelle(nomeSelezionato, sceltaColorePelle);
        if (genereNeutroScelto)
        {
            PlayerSettings.salvaGenereModello3D(nomeSelezionato, sceltaModelloPlayer);
        } else
        {
            PlayerSettings.salvaGenereModello3D(nomeSelezionato, sceltaGenere);
        }
        SceneManager.LoadScene(0);
    }


    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto<Player>(new Player());
    }


    private bool presentePlayer()
    {
        if (player.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    public void setSceltaModelloGiocatore(int indice)
    {
        sceltaModelloPlayer = indice;
    }

    private void attivaDisattivaImpostazioniGenereNeutro()
    {
        if (PlayerSettings.caricaGenereModello3D(nomeSelezionato) == 2)
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

    private void attivaDisattivaImpostazioniGenereNeutro(int indice)
    {
        if (indice == 2)
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

    public void setGenerePlayer(int indiceScelta)
    {
        sceltaGenere = indiceScelta;
        attivaDisattivaImpostazioniGenereNeutro(sceltaGenere);
    }

    public void setPellePlayer(int indice)
    {
        sceltaColorePelle = indice;
    }

    public void indiceSceltaNomeUtente(int indice)
    {
        nomeSelezionato = dropDownListaPlayer.options[dropDownListaPlayer.value].text;
        refreshValori();
    }

}
