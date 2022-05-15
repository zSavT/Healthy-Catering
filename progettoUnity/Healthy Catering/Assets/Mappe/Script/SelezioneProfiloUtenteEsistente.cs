using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelezioneProfiloUtenteEsistente : MonoBehaviour
{


    [SerializeField] private TMP_Dropdown dropDownListaPlayer;
    private List<Player> player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        List<string> nomiPlayerPresenti = null;
        for (int i = 0; i < player.Count; i++)
        {
            nomiPlayerPresenti.Add(player[i].nome);
        }

        dropDownListaPlayer.AddOptions(nomiPlayerPresenti);
    }
}
