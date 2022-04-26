using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Slider sliderCaricamento;        //slider del caricamento della partita
    [SerializeField] private UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione
    [SerializeField] private UnityEvent clickCrediti;             //serve per eliminare altri elementi in visualilzzazione

    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }


    public void playGame(int sceneIndex)
    {
        allAvvio.Invoke();
        StartCoroutine(caricamentoAsincrono(sceneIndex));
        allAvvio.Invoke();
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

    public void crediti()
    {
        clickCrediti.Invoke();
    }

    public void menuOpzioni()
    {
        SceneManager.LoadScene(1);
    }
    public void chiudi()
    {
        Application.Quit();
    }
}
