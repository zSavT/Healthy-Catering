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
        nomeGiocatore = PlayerSettings.caricaNomePlayerGiocante();
        attivaModelloGenere();
        setTexturePelle();
    }

    private void attivaModelloGenere()
    {
        if (PlayerSettings.caricaGenereModello3D(nomeGiocatore) == 0)
        {
            modelloFemminile.SetActive(false);
            modelloMaschile.SetActive(true);
        }
        else if (PlayerSettings.caricaGenereModello3D(nomeGiocatore) == 1)
        {
            modelloFemminile.SetActive(true);
            modelloMaschile.SetActive(false);
        }
    }

    private void setTexturePelle()
    {
        if(PlayerSettings.caricaColorePelle(nomeGiocatore) == 0)
        {
            GetComponentInChildren<Renderer>().material = textureBianco; 
        } else if (PlayerSettings.caricaColorePelle(nomeGiocatore) == 1 )
        {
            GetComponentInChildren<Renderer>().material = textureNero;
        }
        else if (PlayerSettings.caricaColorePelle(nomeGiocatore) == 2)
        {
            GetComponentInChildren<Renderer>().material =  textureMulatto;
        }
    }

}
