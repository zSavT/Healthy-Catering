using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(2);
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
