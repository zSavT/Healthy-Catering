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
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //material.mainTexture = textureBianco;
        } else if (PlayerPrefs.GetInt(nomeGiocatore + "_pelle") == 1 )
        {
            //material.mainTexture = textureNero;
        }
        else if (PlayerPrefs.GetInt(nomeGiocatore + "_pelle") == 2)
        {
            //material.mainTexture = textureMulatto;
        }
    }
}
