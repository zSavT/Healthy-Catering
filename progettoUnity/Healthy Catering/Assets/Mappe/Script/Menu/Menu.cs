using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Wilberforce;
using TMPro;

/// <summary>
/// Classe per la gestione delle impostazioni presenti nel menu iniziale del Gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu principale del gioco.
/// </para>
/// </summary>
public class Menu : MonoBehaviour
{
    [SerializeField] private Camera cameraGioco;
    [SerializeField] private TextMeshProUGUI testoVersioneGioco;
    [SerializeField] private TextMeshProUGUI companyName;
    [SerializeField] private GameObject elementiCrediti;
    [SerializeField] private GameObject elementiMenuPrincipale;
    [SerializeField] private GameObject elementiProfiloNonEsistente;
    private List<Player> player = new List<Player>();
    //serve per eliminare altri elementi in visualilzzazione
    [SerializeField] private UnityEvent clickCrediti;             

    void Start()
    {
        gameVersion();
        //disattivo a priori, per non visualizzarli in caso di errori di lettura dei nomi utenti ed evitare lo schermo occupato tutto da scritte
        elementiProfiloNonEsistente.SetActive(false);               
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        letturaNomiUtenti();
        if (!presentePlayer())
        {
            elementiProfiloNonEsistente.SetActive(true);
            elementiMenuPrincipale.SetActive(false);
        } else
        {
            elementiProfiloNonEsistente.SetActive(false);
        }
        elementiCrediti.SetActive(false);
        CambioCursore.cambioCursoreNormale();
    }

    void Update()
    {
        attivaDisattivaLivelli();
    }

    /// <summary>
    /// Metodo per inzializzare la lista dei player presenti nel database.
    /// </summary>
    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto(new Player());
    }

    /// <summary>
    /// Metodo per controllare se sono presenti o meno dei player nel database.
    /// </summary>
    /// <returns>True: � presente almeno un player, false: non esiste alcun player</returns>
    private bool presentePlayer()
    {
        return player.Count > 0;
    }

    /// <summary>
    /// Metodo per caricare la scena della modifica e selezione del profilo utente.
    /// </summary>
    public void caricaSelezioneModificaProfilo()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// Metodo per caricare la scena della creazione del profilo utente.
    /// </summary>
    public void caricaCreazioneProfilo()
    {
        SceneManager.LoadScene(5);
    }

    //metodo per triggerare a mano i livelli, da eliminare poi.
    /// <summary>
    /// Metodo per attivare i livelli tramite cheatcode
    /// </summary>
    private void attivaDisattivaLivelli()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(PlayerSettings.caricaProgressoLivello1() == 0)
            {
                PlayerSettings.salvaProgressoLivello1(true);
                print("livello 1 attivato");
            } else
            {
                print("livello 1 disattivato");
                PlayerSettings.salvaProgressoLivello1(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (PlayerSettings.caricaProgressoLivello2() == 0)
            {
                PlayerSettings.salvaProgressoLivello2(true);
                print("livello 2 attivato");
            }
            else
            {
                print("livello 2 disattivato");
                PlayerSettings.salvaProgressoLivello2(false);
            }
        }
    }

    /// <summary>
    /// Metodo per caricare la scena del menu iniziale.
    /// </summary>
    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Attiva l'evento per visualizzare i credit del gioco.
    /// </summary>
    public void crediti()
    {
        clickCrediti.Invoke();
    }

    /// <summary>
    /// Metodo per caricare la scena della selezione livelli.
    /// </summary>
    public void menuSelezioneLivelli()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// Metodo per aprire la scena del menu opzioni.
    /// </summary>
    public void menuOpzioni()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Metodo per chiudere il gioco.
    /// </summary>
    public void chiudi()
    {
        Application.Quit();
    }

    /// <summary>
    /// Metodo per aggiornare il testo della versione del gioco e il nome della societ�.
    /// </summary>
    private void gameVersion()
    {
        testoVersioneGioco.text = testoVersioneGioco.text + Application.version;
        companyName.text = companyName.text + " " + Application.companyName;
    }

    /// <summary>
    /// Metodo che carica la scena della classifica
    /// </summary>
    public void classifica()
    {
        SelezioneLivelli.caricaClassifica();
    }
}
