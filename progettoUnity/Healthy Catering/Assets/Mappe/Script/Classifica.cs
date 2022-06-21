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
    private List<Player> listaPlayer;
    private int numeroGiocatoriDaVisualizzare = 5;

    void Start()
    {
        listaPlayer = Database.getDatabaseOggetto(new Player());
        listaPunteggioLivello0.text = "";
        popolaClassificaLivello0();
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
    }

    /// <summary>
    /// Carica la scena del menu principale
    /// </summary>
    public void menuPrincipale()
    {
        SelezioneLivelli.caricaMenuPrincipale();
    }

    private void popolaClassificaLivello0()
    {
        listaPlayer.Sort();
        int i = 0;
        while(i < numeroGiocatoriDaVisualizzare)
        {
            if(i < listaPlayer.Count)
            {
                listaPunteggioLivello0.text = listaPunteggioLivello0.text + (i+1).ToString() + " " + listaPlayer[i].nome + " " + listaPlayer[i].punteggio[0] + "\n\n";
            } else
            {
                listaPunteggioLivello0.text = listaPunteggioLivello0.text + (i + 1).ToString() + " AAA " + 0.ToString() + "\n\n";
            }
            i++;
        }
    }
}



