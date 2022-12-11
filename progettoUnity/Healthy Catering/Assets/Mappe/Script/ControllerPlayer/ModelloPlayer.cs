using UnityEngine;

/// <summary>
/// Classe per la gestione del modello 3D del giocatore.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore del Player.
/// </para>
/// </summary>
public class ModelloPlayer : MonoBehaviour
{
    [Header("Modello 3D")]
    [SerializeField] private GameObject modelloFemminile;
    [SerializeField] private GameObject modelloMaschile;

    [Header("Texture pelle modello 3D")]
    //EVENTUALMENTE POSSONO ESSERE TOLTI SE CARICHIAMO LE TEXTURE DALLA CARTELLA RISORSE
    [SerializeField] private Material textureBianco;
    [SerializeField] private Material textureNero;
    [SerializeField] private Material textureMulatto;
    string nomeGiocatore = string.Empty;

    void Start()
    {
        nomeGiocatore = PlayerSettings.caricaNomePlayerGiocante();
        attivaModelloGenere();
        setTexturePelle();
    }

    /// <summary>
    /// Metodo per controllare il genere del modello del giocatore
    /// </summary>
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

    /// <summary>
    /// Metodo per impostare il colore della pelle del modello 3D del giocatore
    /// </summary>
    private void setTexturePelle()
    {
        if (PlayerSettings.caricaColorePelle(nomeGiocatore) == 0)
        {
            GetComponentInChildren<Renderer>().material = textureBianco;
        }
        else if (PlayerSettings.caricaColorePelle(nomeGiocatore) == 1)
        {
            GetComponentInChildren<Renderer>().material = textureNero;
        }
        else if (PlayerSettings.caricaColorePelle(nomeGiocatore) == 2)
        {
            GetComponentInChildren<Renderer>().material = textureMulatto;
        }
    }

    /// <summary>
    /// Restituisce il GameObject del modello del giocatore attivo.
    /// </summary>
    /// <returns>GameObject del modello del giocatore attivo</returns>
    public GameObject getModelloAttivo()
    {
        if (PlayerSettings.caricaGenereModello3D(PlayerSettings.caricaNomePlayerGiocante()) == 1)
            return modelloFemminile;
        else
            return modelloMaschile;
    }
}
