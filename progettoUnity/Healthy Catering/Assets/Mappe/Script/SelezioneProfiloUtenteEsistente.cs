using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class SelezioneProfiloUtenteEsistente : MonoBehaviour
{


    [SerializeField] private TMP_Dropdown dropDownListaPlayer;
    private List<Player> player;
    private List<string> nomiPlayerPresenti;
    private string nomeSelezionato = "";


    // Start is called before the first frame update
    void Start()
    {
        player = new List<Player>();
        nomiPlayerPresenti = new List<string>();
        // letturaNomiUtenti();
        aggiuntaNomiDropdown();
        nomeSelezionato = dropDownListaPlayer.options[dropDownListaPlayer.value].text;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void salvaImpostazioni()
    {
        salvaNomePlayerGiocante(nomeSelezionato);
        SceneManager.LoadScene(0);
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
        dropDownListaPlayer.AddOptions(nomiPlayerPresenti);
    }

    public void salvaNomePlayerGiocante(string nomeInserito)
    {

        PlayerPrefs.SetString("PlayerName", nomeInserito);

    }
}
