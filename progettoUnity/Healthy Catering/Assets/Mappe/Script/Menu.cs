using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wilberforce;

public class Menu : MonoBehaviour
{
    [SerializeField] private UnityEvent clickCrediti;             //serve per eliminare altri elementi in visualilzzazione
    [SerializeField] private Camera camera;


    void Start()
    {
        camera.GetComponent<Colorblind>().Type  = PlayerPrefs.GetInt("daltonismo");
    }


    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }


    public void crediti()
    {
        clickCrediti.Invoke();
    }

    public void menuSelezioneLivelli()
    {
        SceneManager.LoadScene(3);
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
