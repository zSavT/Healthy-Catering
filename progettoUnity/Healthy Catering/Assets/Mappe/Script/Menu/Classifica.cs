using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Wilberforce;

/// <summary>
/// Classe per la gestione della classifica del gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello dove sono presenti gli elementi della classifica (Va bene in realt� qualsiasi oggetto).
/// </para>
/// </summary>
public class Classifica : MonoBehaviour
{
    [Header("Lista Classifica")]
    [SerializeField] private TextMeshProUGUI listaPunteggioLivello0;
    [SerializeField] private TextMeshProUGUI listaPunteggioLivello1;
    [SerializeField] private TextMeshProUGUI listaPunteggioLivello2;
    [Header("Altro")]
    [SerializeField] private GameObject bottoneIndietro;
    private Camera cameraGioco;
    private List<Player> listaPlayer;
    private int numeroGiocatoriDaVisualizzare = 5;

    void Start()
    {
        cameraGioco = FindObjectOfType<Camera>();
        azzeraTextElementi();
        popolaClassificaLivello0();
        popolaClassificaLivello1();
        popolaClassificaLivello2();
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
    }

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void Update()
    {
        if (Utility.gamePadConnesso())
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(bottoneIndietro);
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
                break;
            case InputDeviceChange.Reconnected:
                aggioraEventSystemPerControllerConnesso(bottoneIndietro);
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

    /// <summary>
    /// Elimina il testo presente nelle liste punteggio.
    /// </summary>
    private void azzeraTextElementi()
    {
        listaPunteggioLivello0.text = string.Empty;
        listaPunteggioLivello1.text = string.Empty;
        listaPunteggioLivello2.text = string.Empty;
    }

    /// <summary>
    /// Carica la scena del menu principale
    /// </summary>
    public void menuPrincipale()
    {
        SelezioneLivelli.caricaMenuPrincipale();
    }

    /// <summary>
    /// Aggiorna il testo della lista dei punteggio dei giocatori per il livello 0
    /// </summary>
    private void popolaClassificaLivello0()
    {
        PlayerSettings.livelloSelezionato = 0;
        listaPlayer = Player.getListaSortata ();
        int i = 0;
        while (i < numeroGiocatoriDaVisualizzare)
        {
            if (i < listaPlayer.Count)
            {
                listaPunteggioLivello0.text = listaPunteggioLivello0.text + (i + 1).ToString() + " " + listaPlayer[i].nome + " " + listaPlayer[i].punteggio[0] + "\n\n";
            }
            else
            {
                listaPunteggioLivello0.text = listaPunteggioLivello0.text + (i + 1).ToString() + "\n\n";
            }
            i++;
        }
    }

    /// <summary>
    /// Aggiorna il testo della lista dei punteggio dei giocatori per il livello 1
    /// </summary>
    private void popolaClassificaLivello1()
    {
        PlayerSettings.livelloSelezionato = 1;
        listaPlayer = Player.getListaSortata ();
        int i = 0;
        while (i < numeroGiocatoriDaVisualizzare)
        {
            if (i < listaPlayer.Count)
            {
                listaPunteggioLivello1.text = listaPunteggioLivello1.text + (i + 1).ToString() + " " + listaPlayer[i].nome + " " + listaPlayer[i].punteggio[1] + "\n\n";
            }
            else
            {
                listaPunteggioLivello1.text = listaPunteggioLivello1.text + (i + 1).ToString() + "\n\n";
            }
            i++;
        }
    }

    /// <summary>
    /// Aggiorna il testo della lista dei punteggio dei giocatori per il livello 2
    /// </summary>
    private void popolaClassificaLivello2()
    {
        PlayerSettings.livelloSelezionato = 2;
        listaPlayer = Player.getListaSortata ();
        
        int i = 0;
        while (i < numeroGiocatoriDaVisualizzare)
        {
            if (i < listaPlayer.Count)
            {
                listaPunteggioLivello2.text = listaPunteggioLivello2.text + (i + 1).ToString() + " " + listaPlayer[i].nome + " " + listaPlayer[i].punteggio[2] + "\n\n";
            }
            else
            {
                listaPunteggioLivello2.text = listaPunteggioLivello2.text + (i + 1).ToString() + "\n\n";
            }
            i++;
        }
    }
}


