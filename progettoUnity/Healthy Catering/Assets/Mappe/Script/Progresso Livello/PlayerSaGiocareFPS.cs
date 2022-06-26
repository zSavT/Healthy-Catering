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
    }

    public void apriPannelloPlayerSaGiocareFPS()
    {
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
    }

    public void chiudiPannelloNo()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();
        siOno = -1;
    }

    public static int getSiOno()
    {
        return siOno;
    }

}
