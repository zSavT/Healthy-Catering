using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerSaGiocareFPS : MonoBehaviour
{

    [SerializeField] GameObject pannello;
    [SerializeField] GameObject bottoneNo;
    private GameObject ultimoElementoSelezionato;
    // 0 = nessuno selezionato
    // 1 = si
    // -1 = no
    public static int siOno = 0;
    public static bool pannelloSaGiocareAperto = false;

    [SerializeField] private UnityEvent playerRiprendiMovimento;

    void Start()
    {
        pannello.SetActive(false);
        pannelloSaGiocareAperto = false;
        siOno = 0;
    }

    private void Update()
    {
        if (Utility.gamePadConnesso())
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(bottoneNo);
    }

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    /// <summary>
    /// Il metodo controlla e gestiscisce le periferiche di Input 
    /// </summary>
    /// <param name="device"></param>
    /// <param name="change"></param>
    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                // New Device.
                break;
            case InputDeviceChange.Disconnected:
                ultimoElementoSelezionato = EventSystem.current.currentSelectedGameObject;
                break;
            case InputDeviceChange.Reconnected:
                aggioraEventSystemPerControllerConnesso(ultimoElementoSelezionato);
                break;
            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }
    }

    /// <summary>
    /// Il metodo imposta come elemento selzionato dell'EventSystem l'oggetto passato in input
    /// </summary>
    /// <param name="elementoDaSelezionare">GameObject da impostare come elemento selezionato</param>
    private void aggioraEventSystemPerControllerConnesso(GameObject elementoDaSelezionare)
    {
        if (Utility.gamePadConnesso())
            EventSystem.current.SetSelectedGameObject(elementoDaSelezionare);
    }

    public void apriPannelloPlayerSaGiocareFPS()
    {
        pauseGame();
        pannello.SetActive(true);
        //playerStop.Invoke () chiamato in progresso tutorial
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
        pannelloSaGiocareAperto = true;
    }

    public void chiudiPannelloSi()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();
        siOno = 1;
        pannelloSaGiocareAperto = false;
        resumeGame();
    }

    public void chiudiPannelloNo()
    {
        PuntatoreMouse.disabilitaCursore();
        pannello.SetActive(false);
        playerRiprendiMovimento.Invoke();
        siOno = -1;
        pannelloSaGiocareAperto = false;
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

    public void distruggiOggetto()
    {
        Destroy(this.gameObject, 2f);
    }
}
