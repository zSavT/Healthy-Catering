using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSaGiocareFPS : MonoBehaviour
{

    [SerializeField] GameObject pannello;
    // 0 = nessuno selezionato
    // 1 = si
    // -1 = no
    public static int siOno = 0;

    [SerializeField] private UnityEvent playerRiprendiMovimento;

    void Start()
    {
        pannello.SetActive(false);
        siOno = 0;
    }

    public void apriPannelloPlayerSaGiocareFPS()
    {
        pauseGame();
        pannello.SetActive(true);
        //playerStop.Invoke () chiamato in progresso tutorial
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
    }

    public void chiudiPannelloSi()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();
        siOno = 1;
        resumeGame();
    }

    public void chiudiPannelloNo()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();
        siOno = -1;
        resumeGame();
    }

    public static bool siOnoSettato ()
    {
        return siOno != 0;
    }

    public static int getSiOno()
    {
        return siOno;
    }

    /// <summary>
    /// Metodo per ripristinare lo scorrere del tempo in gioco
    /// </summary>
    void resumeGame()
    {
        Time.timeScale = 1f; //sblocca il tempo
    }

    /// <summary>
    /// Metodo per bloccare lo scorrere del tempo in gioco.
    /// </summary>
    void pauseGame()
    {
        Time.timeScale = 0f; //blocca il tempo
    }

}
