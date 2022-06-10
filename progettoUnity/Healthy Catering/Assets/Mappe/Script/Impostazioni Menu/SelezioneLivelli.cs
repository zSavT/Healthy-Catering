using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using Wilberforce;

public class SelezioneLivelli : MonoBehaviour
{

    /*
     * Menu Principale = 0
     * Menu Opzioni = 1
     * Citta Livello 0 = 2                  
     * Menu Selezione livello = 3
     * Menu Selezione Profilo Utente = 4
     * Menu Creazione Profilo Utente = 5
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

    public static void caricaMenuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    public static void caricaMenuOpzioni()
    {
        SceneManager.LoadScene(1);
    }

    public static void caricaMenuSelezioneLivello()
    {
        SceneManager.LoadScene(3);
    }

    public static void caricaMenuSelezioneProfiloUtente()
    {
        SceneManager.LoadScene(4);
    }

    public static void caricaMenuCreazioneProfiloUtente()
    {
        SceneManager.LoadScene(5);
    }

    public static void caricaLivelloCitta()
    {
        SceneManager.LoadScene(2);
    }
}

