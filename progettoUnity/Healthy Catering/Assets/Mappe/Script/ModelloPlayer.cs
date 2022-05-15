using UnityEngine;

public class ModelloPlayer : MonoBehaviour
{

    [SerializeField] private GameObject modelloFemminile;
    [SerializeField] private GameObject modelloMaschile;
    [SerializeField] private Material textureBianco;
    [SerializeField] private Material textureNero;
    [SerializeField] private Material textureMulatto;
    string nomeGiocatore = "";           


    // Start is called before the first frame update
    void Start()
    {
        nomeGiocatore = caricaNomePlayerGiocante();
        attivaModelloGenere();
        setTexturePelle();
    }

    private void attivaModelloGenere()
    {
        print(PlayerPrefs.GetInt(nomeGiocatore + "_modello"));
        if (PlayerPrefs.GetInt(nomeGiocatore + "_modello") == 0)
        {
            modelloFemminile.SetActive(false);
            modelloMaschile.SetActive(true);
        }
        else if (PlayerPrefs.GetInt(nomeGiocatore + "_modello") == 1)
        {
            modelloFemminile.SetActive(true);
            modelloMaschile.SetActive(false);
        }
    }

    private void setTexturePelle()
    {
        if(PlayerPrefs.GetInt(nomeGiocatore + "_pelle") == 0)
        {
            GetComponentInChildren<Renderer>().material = textureBianco; 
        } else if (PlayerPrefs.GetInt(nomeGiocatore + "_pelle") == 1 )
        {
            GetComponentInChildren<Renderer>().material = textureNero;
        }
        else if (PlayerPrefs.GetInt(nomeGiocatore + "_pelle") == 2)
        {
            GetComponentInChildren<Renderer>().material =  textureMulatto;
        }
    }

    private string caricaNomePlayerGiocante()
    {
        return PlayerPrefs.GetString("PlayerName");
    }
}
