using UnityEngine;

public class ModelloPlayer : MonoBehaviour
{

    [SerializeField] private GameObject modelloFemminile;
    [SerializeField] private GameObject modelloMaschile;
    [SerializeField] private Material textureBianco;
    [SerializeField] private Material textureNero;
    [SerializeField] private Material textureMulatto;
    string nomeGiocatore = "sav";           //inzializata così solo per test


    // Start is called before the first frame update
    void Start()
    {
        attivaModelloGenere();
        setTexturePelle();
    }

    // Update is called once per frame
    void Update()
    {
        print(PlayerPrefs.GetInt(nomeGiocatore + "_pelle"));
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
}
