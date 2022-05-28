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

    [SerializeField] private Button bottoneLivello0;
    [SerializeField] private Button bottoneLivello1;
    [SerializeField] private Button bottoneLivello2;
    [SerializeField] private Slider sliderCaricamento;        //slider del caricamento della partita
    [SerializeField] private UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject elementiDomandaUscita;


    // Start is called before the first frame update
    void Start()
    {
        camera.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        if (PlayerSettings.caricaProgressoLivello1() == 1)
        {
            bottoneLivello1.interactable = true;                
        }
        if (PlayerSettings.caricaProgressoLivello2() == 1)
        {
            bottoneLivello2.interactable = true;
        }
        elementiDomandaUscita.SetActive(false);
    }

    public void playGame(int sceneIndex)
    {
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
}

