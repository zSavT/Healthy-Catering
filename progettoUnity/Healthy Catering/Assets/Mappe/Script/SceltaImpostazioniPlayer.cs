using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using Wilberforce;

/// <summary>
/// Classe per la gestione delle impostazioni presenti nel menu del profilo utente per la modifica e selezione.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu per la scelta delle impostazioni del player.
/// </para>
/// </summary>
public class SceltaImpostazioniPlayer : MonoBehaviour
{

    [SerializeField] private GameObject elementiGenereNeutro;
    [SerializeField] private GameObject tastoIndietro;
    [SerializeField] private TMP_InputField inputFieldNomeGiocatore;
    [SerializeField] private GameObject nomeGiaPreso;
    [SerializeField] private GameObject elementiSalvataggio;
    [SerializeField] private GameObject elementiConferma;
    [SerializeField] private Button bottoneSalva;
    [SerializeField] private Camera cameraGioco;
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
        PlayerPrefs.SetInt("primoAvvio", 0);
        PuntatoreMouse.abilitaCursore();
        nomeGiocatoreScritto = "";
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        player = new List<Player>();
        genereNeutroScelto = false;
        disattivaElementi();
        nomiPlayer();
        controlloEsistenzaProfiliPlayer();
    }

    /// <summary>
    /// Disattiva tutti gli elementi presenti.
    /// </summary>
    private void disattivaElementi()
    {
        nomeGiaPreso.SetActive(false);
        elementiGenereNeutro.SetActive(false);
        elementiSalvataggio.SetActive(false);
        elementiConferma.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        controlloNomeEsistente();
    }

    /// <summary>
    /// 
    /// </summary>
    public void nomiPlayer()
    {
        List<Player> listaPlayer = Database.getDatabaseOggetto(new Player());
        if (listaPlayer != null)
        {
            for (int i = 0; i < listaPlayer.Count; i++)
            {
                nomiPlayerPresenti.Add(listaPlayer[i].nome);
            }
        }
    }


    /// <summary>
    /// Controlla se il nome inserito dall'utente, corrisponde con uno già presente nella lista.
    /// </summary>
    private void controlloNomeEsistente()
    {
        if (nomeGiocatoreScritto != "")
        {
            bottoneSalva.interactable = true;
            if (PlayerSettings.caricaPrimoAvvio() == 1)
            {
                foreach (string temp in nomiPlayerPresenti)
                {
                    if (temp.ToUpper() == nomeGiocatoreScritto.ToUpper())
                    {
                        nomeGiaPreso.SetActive(true);
                        bottoneSalva.interactable = false;
                        break;
                    }
                    else
                    {
                        nomeGiaPreso.SetActive(false);
                        bottoneSalva.interactable = true;
                    }
                }
            }
        }  
        else
        {
            bottoneSalva.interactable = false;
        }
    }

    /// <summary>
    /// Inizializza la lista dei nomi dei player presenti.
    /// </summary>
    private void aggiuntaNomiPresentiInLista()
    {
        for (int i = 0; i < player.Count; i++)
        {
            nomiPlayerPresenti.Add(player[i].nome);
        }
    }

    /// <summary>
    /// Legge i player presenti, se esistono attiva il tasto per tornare indietro, altrimenti lo disattiva.
    /// </summary>
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

    /// <summary>
    /// Controlla se sono presenti player nel database.
    /// </summary>
    /// <returns><br><strong>True</strong>: Presente almeno un player nel database.<br><strong>False</strong>: Non presente almeno un player nel database.</br></br></returns>
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

    /// <summary>
    /// Legge da file tutti i player presenti e li salva nella lista.
    /// </summary>
    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto(new Player());
        
    }

    /// <summary>
    /// Attiva il tasto indietro.
    /// </summary>
    private void attivaTastoIndietro()
    {
        tastoIndietro.SetActive(true);
    }

    /// <summary>
    /// Disattiva il tasto indietro.
    /// </summary>
    private void disattivaTastoIndietro()
    {
        tastoIndietro.SetActive(false);
    }

    /// <summary>
    /// Salva tutte le impostazioni fatte dal giocatore, salva il giocatore su file<br></br>
    /// Se la scena è stata caricata dal livello tutorial, dopo il salvataggio ritorna al livello tutorial.<br></br>
    /// In caso contrario carica la scena del menu principale. Utilizza 
    /// <see cref="PlayerSettings"/>.
    /// </summary>
    public void salvaImpostazioni()
    {
        PlayerSettings.salvaPrimoAvvio();
        PlayerSettings.salvaNomePlayerGiocante(nomeGiocatoreScritto);
        PlayerSettings.salvaGenereGiocatore(nomeGiocatoreScritto, sceltaGenere);
        PlayerSettings.salvaColorePelle(nomeGiocatoreScritto, sceltaColorePelle);
        if (genereNeutroScelto)
        {
            PlayerSettings.salvaGenereModello3D(nomeGiocatoreScritto, sceltaModelloPlayer);
        }
        Database.salvaNuovoOggettoSuFile(new Player(nomeGiocatoreScritto));
        if(!PlayerSettings.profiloUtenteCreato)
        {
            PlayerSettings.profiloUtenteCreato = true;
            SelezioneLivelli.caricaLivelloCitta();
        } else
        {
            SelezioneLivelli.caricaMenuPrincipale();
        }
    }

    /// <summary>
    /// Salva localmente il nome scelto dal giocatore.
    /// </summary>
    /// <param name="testo">Nome scritto dal giocatore.</param>
    public void leggiInputNomeScritto(string testo)
    {
        nomeGiocatoreScritto = testo;
    }

    /// <summary>
    /// Salva localmente il valore della pelle scelto dal giocatore.
    /// </summary>
    /// <param name="indice">Indice del dropdown scelta colore pelle modello.<br><strong>0: Caucasico<br>1: Asiatico</br><br>2: Afro</br></strong></br></param>
    public void setPellePlayer(int indice)
    {
        sceltaColorePelle = indice;
    }

    /// <summary>
    /// Metodo che salva localmente il valore della scelta del genere del modello del giocatore.
    /// </summary>
    /// <param name="indice">Indice scela del modello player<br><strong>0: Maschio<br>1: Femmina</br></strong></br></param>
    public void setSceltaModelloGiocatore(int indice)
    {
        sceltaModelloPlayer = indice;
    }

    /// <summary>
    /// Metodo che salva localmente la scelta del genere scelto dal giocatore e controlla se il genere scelto sia quello neutro.<br></br>
    /// In caso affermativo attiva il dropdown per la scelta del genere del modello, altrimenti lo disattiva.
    /// </summary>
    /// <param name="indiceScelta">Indice dropdown del genere<br><strong>0: Maschio<br>1: Femmina</br><br>2: Neutro</br></strong></br></param>
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
