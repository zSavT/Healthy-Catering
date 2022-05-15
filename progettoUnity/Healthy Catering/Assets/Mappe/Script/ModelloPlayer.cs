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
        if (caricaModelloGenereGiocatore() == 0)
        {
            modelloFemminile.SetActive(false);
            modelloMaschile.SetActive(true);
        }
        else if (caricaModelloGenereGiocatore() == 1)
        {
            modelloFemminile.SetActive(true);
            modelloMaschile.SetActive(false);
        }
    }

    private void setTexturePelle()
    {
        if(caricaColorePelleGiocatore() == 0)
        {
            GetComponentInChildren<Renderer>().material = textureBianco; 
        } else if (caricaColorePelleGiocatore() == 1 )
        {
            GetComponentInChildren<Renderer>().material = textureNero;
        }
        else if (caricaColorePelleGiocatore() == 2)
        {
            GetComponentInChildren<Renderer>().material =  textureMulatto;
        }
    }

    private int caricaModelloGenereGiocatore()
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_modello");
    }

    private int caricaColorePelleGiocatore()
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_pelle");
    }

    private string caricaNomePlayerGiocante()
    {
        return PlayerPrefs.GetString("PlayerName");
    }
}
