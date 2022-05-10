using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using Wilberforce;

public class SelezioneLivelli : MonoBehaviour
{
    [SerializeField] private Button bottoneLivello0;
    [SerializeField] private Button bottoneLivello1;
    [SerializeField] private Button bottoneLivello2;
    [SerializeField] private Slider sliderCaricamento;        //slider del caricamento della partita
    [SerializeField] private UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione
    [SerializeField] private Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        camera.GetComponent<Colorblind>().Type = PlayerPrefs.GetInt("daltonismo");
        if (PlayerPrefs.GetInt("livello1") == 1)
        {
            bottoneLivello1.interactable = true;                
        }
        if (PlayerPrefs.GetInt("livello2") == 1)
        {
            bottoneLivello2.interactable = true;
        }
    }

    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    public void caricaLivello0()
    {

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
}
