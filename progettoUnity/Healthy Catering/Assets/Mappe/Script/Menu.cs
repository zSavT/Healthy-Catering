using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione




    public void PlayGame(int sceneIndex)
    {
        allAvvio.Invoke();
        StartCoroutine(CaricamentoAsincrono(sceneIndex));
        allAvvio.Invoke();
    }

    IEnumerator CaricamentoAsincrono(int sceneIndex)
    {
        AsyncOperation caricamento = SceneManager.LoadSceneAsync(sceneIndex);

        while (!caricamento.isDone)
        {
            float progresso = Mathf.Clamp01(caricamento.progress / .9f);
            Debug.Log(caricamento.progress);
            yield return null;
        }
    }

    public void chiudi()
    {
        Application.Quit();
    }

    public void menuOpzioni()
    {
        SceneManager.LoadScene(1);
    }

    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }
}
