using UnityEngine;
using TMPro;
using System.Collections;
using System;

/// <summary>
/// Classe per la gestione del HUB del gioco.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello GUI del gioco.
/// </para>
/// </summary>
public class Gui : MonoBehaviour
{


    // CAMBIARE NOME POI IN GUI e NON IN HUB
    [Header("Testo valori")]
    [SerializeField] private TextMeshProUGUI soldiTesto;
    [SerializeField] private TextMeshProUGUI punteggioTesto;

    [Header("Animazioni")]
    [SerializeField] private ParticleSystem animazioneSoldi;
    [SerializeField] private ParticleSystem animazionePunteggioPositiva;
    [SerializeField] private ParticleSystem animazionePunteggioNegativa;

    [Header("Impostazioni per aggiornamento valori graduale")]
    public int CountFPS = 30;
    public float durata = 3f;
    public string formatoNumero = "N0";
    private int valorePrecedentePunteggio = 0;
    private Coroutine CountingCoroutine;


    void start()
    {
        bloccaAnimazioniParticellari();
    }

    /// <summary>
    /// Metodo per bloccare i particellari presenti nella GUI.
    /// </summary>
    public void bloccaAnimazioniParticellari()
    {
        animazioneSoldi.Stop();
        animazionePunteggioPositiva.Stop();
        animazionePunteggioNegativa.Stop();
    }

    /// <summary>
    /// Metodo per aggiornare il valore dei soldi presenti nella GUI.
    /// </summary>
    /// <param name="soldi">Valore soldi raggiunti</param>
    public void aggiornaValoreSoldi(float soldi)
    {
        soldiTesto.text = soldi.ToString("0.00");
        var main = animazioneSoldi.main;
        main.maxParticles = ((int)soldi / 2)+5;
        animazioneSoldi.Play();
    }

    /// <summary>
    /// Metodo per aggiornare i valori nella GUI
    /// </summary>
    /// <param name="punteggio">Valore punteggio raggiunto</param>
    public void aggiornaValorePunteggio(int punteggio)
    {
        Debug.Log(punteggio);
        UpdateText(punteggio, punteggioTesto);
        if(valorePrecedentePunteggio>punteggio)
        {
            var main = animazionePunteggioNegativa.main;
            //per ottenere il valore assoluto ed ottenere il numero di particelle corretto
            main.maxParticles = (Math.Abs(punteggio/2))+5;               
            animazionePunteggioNegativa.Play();
        } else
        {
            var main = animazionePunteggioNegativa.main;
            main.maxParticles = (punteggio / 5)+5;
            animazionePunteggioPositiva.Play();
        }
        valorePrecedentePunteggio = punteggio;
    }


    /// <summary>
    /// Avvia l'aggiornamento dimanico valore int
    /// </summary>
    /// <param name="valoreNuovo">Valore da raggiunggere.</param>
    /// <param name="testo">Testo da aggiornare</param>
    private void UpdateText(int valoreNuovo, TextMeshProUGUI testo)
    {
        if (CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }

        CountingCoroutine = StartCoroutine(CountText(valoreNuovo, testo));
    }

    /// <summary>
    /// Metodo per l'aggiornamento dinamico del testo contenente interi.
    /// </summary>
    /// <param name="valoreNuovo">Valore da raggiungere.</param>
    /// <param name="testo">Testo da aggiornare.</param>
    /// <returns></returns>
    private IEnumerator CountText(int valoreNuovo, TextMeshProUGUI testo)
    {
        WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
        int previousValue = valorePrecedentePunteggio;
        int stepAmount;

        if (valoreNuovo - previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((valoreNuovo - previousValue) / (CountFPS * durata)); // valoreNuovo = -20, previousValue = 0. CountFPS = 30, and durata = 1; (-20- 0) / (30*1) // -0.66667 (ceiltoint)-> 0
        }
        else
        {
            stepAmount = Mathf.CeilToInt((valoreNuovo - previousValue) / (CountFPS * durata)); // valoreNuovo = 20, previousValue = 0. CountFPS = 30, and durata = 1; (20- 0) / (30*1) // 0.66667 (floortoint)-> 0
        }

        if (previousValue < valoreNuovo)
        {
            while (previousValue < valoreNuovo)
            {
                previousValue += stepAmount;
                if (previousValue > valoreNuovo)
                {
                    previousValue = valoreNuovo;
                }

                testo.text = previousValue.ToString(formatoNumero);

                yield return Wait;
            }
        }
        else
        {
            while (previousValue > valoreNuovo)
            {
                previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1
                if (previousValue < valoreNuovo)
                {
                    previousValue = valoreNuovo;
                }

                testo.text = previousValue.ToString(formatoNumero);

                yield return Wait;
            }
        }
        //Se i particellari sono ancora visibili, stoppa i particellari per sincronizzarli con l'aggiornamento del valore
        bloccaAnimazioniParticellari();
    }


}
