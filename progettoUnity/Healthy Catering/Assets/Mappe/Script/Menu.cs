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

    void Update()
    {
        attivaDisattivaLivelli();
    }

    private void attivaDisattivaLivelli()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(PlayerPrefs.GetInt("livello1") == 0)
            {
                PlayerPrefs.SetInt("livello1", 1);
                print("livello 1 attivato");
            } else
            {
                print("livello 1 disattivato");
                PlayerPrefs.SetInt("livello1", 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (PlayerPrefs.GetInt("livello2") == 0)
            {
                PlayerPrefs.SetInt("livello2", 1);
                print("livello 2 attivato");
            }
            else
            {
                print("livello 2 disattivato");
                PlayerPrefs.SetInt("livello2", 0);
            }
        }

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
