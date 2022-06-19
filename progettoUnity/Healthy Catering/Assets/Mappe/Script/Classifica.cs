using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wilberforce;

/// <summary>
/// Classe per la gestione della classifica del gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello dove sono presenti gli elementi della classifica (Va bene in realtà qualsiasi oggetto).
/// </para>
/// </summary>
public class Classifica : MonoBehaviour
{
    [SerializeField] private Camera cameraGioco;
    [SerializeField] private TextMeshProUGUI listaPunteggioLivello0;
    [SerializeField] private TextMeshProUGUI listaPunteggioLivello1;
    [SerializeField] private TextMeshProUGUI listaPunteggioLivello2;


    void Start()
    {
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
    }

    /// <summary>
    /// Carica la scena del menu principale
    /// </summary>
    public void menuPrincipale()
    {
        SelezioneLivelli.caricaMenuPrincipale();
    }
}
