using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using Wilberforce;

/// <summary>
/// Classe per il caricamento dei livelli<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu della scelta dei livelli.
/// </para>
/// </summary>
public class SelezioneLivelli : MonoBehaviour
{

    /*
     * Menu Principale = 0
     * Menu Opzioni = 1
     * Citta Livello 0 = 2                  
     * Menu Selezione livello = 3
     * Menu Selezione Profilo Utente = 4
     * Menu Creazione Profilo Utente = 5
     * Classifica = 6
     */
    [SerializeField] private Camera cameraGioco;
    [SerializeField] private GameObject elementiDomandaUscita;
    [Header("Bottoni Livelli")]
    [SerializeField] private Button bottoneLivello0;
    [SerializeField] private Button bottoneLivello1;
    [SerializeField] private Button bottoneLivello2;
    [Header("Elementi Caricamento Livello")]
    [SerializeField] private Slider sliderCaricamento;        //slider del caricamento della partita
    [SerializeField] private UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione
    [Header("Tutorial")]
    private ProgressoTutorial tutorial;


    // Start is called before the first frame update
    void Start()
    {
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        if (PlayerSettings.caricaProgressoLivello1() == 1)
        {
            bottoneLivello1.interactable = true;
        }
        if (PlayerSettings.caricaProgressoLivello2() == 1)
        {
            bottoneLivello2.interactable = true;
        }
        elementiDomandaUscita.SetActive(false);                                         //disattiva gli elementi della domanda all'uscita per non visualizzarli fin da subito
    }

    /// <summary>
    /// Avvia il caricamento del livello
    /// </summary>
    /// <param name="sceneIndex">Indice scena da caricare</param>
    public void playGame(int sceneIndex)
    {
        if (sceneIndex == 2)
        {
            PlayerSettings.livelloSelezionato = 0;
        } else if (sceneIndex == 6) {
            PlayerSettings.livelloSelezionato = 1;
        } else
        {
            PlayerSettings.livelloSelezionato = 2;
        }
        allAvvio.Invoke();
        StartCoroutine(caricamentoAsincrono(sceneIndex));
    }

    /// <summary>
    /// Carica il livello con la barra di caricamento
    /// </summary>
    /// <param name="sceneIndex">Indice della scena da caricare</param>
    IEnumerator caricamentoAsincrono(int sceneIndex)
    {
        AsyncOperation caricamento = SceneManager.LoadSceneAsync(sceneIndex);

        while (!caricamento.isDone)
        {
            float progresso = Mathf.Clamp01(caricamento.progress / .9f);
            sliderCaricamento.value = progresso;
            yield return null;
        }
    }

    /// <summary>
    /// Carica il menu principale.
    /// </summary>
    public static void caricaMenuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Carica il menu delle opzioni.
    /// </summary>
    public static void caricaMenuOpzioni()
    {
        SceneManager.LoadScene(1);
    }


    /// <summary>
    /// Carica il menu della selezione livelli.
    /// </summary>
    public static void caricaMenuSelezioneLivello()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// Carica il menu della selezione del profilo utente
    /// </summary>
    public static void caricaMenuSelezioneProfiloUtente()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// Carica il menu della creazione del profilo utente.
    /// </summary>
    public static void caricaMenuCreazioneProfiloUtente()
    {
        SceneManager.LoadScene(5);
    }

    /// <summary>
    /// Carica il livello della citta.
    /// </summary>
    public static void caricaLivelloCitta()
    {
        SceneManager.LoadScene(2);

    }

    public static void caricaClassifica()
    {
        SceneManager.LoadScene(6);
    }

}

