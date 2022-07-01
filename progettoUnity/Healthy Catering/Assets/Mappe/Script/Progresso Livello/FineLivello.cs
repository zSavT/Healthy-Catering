using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Wilberforce;

public class FineLivello : MonoBehaviour
{
    private readonly int livelloFinale = 2;

    [SerializeField] GameObject pannello;
    [SerializeField] TextMeshProUGUI scrittaFineLivello;
    [SerializeField] Button bottoneProssimoLivello;
    [SerializeField] TextMeshProUGUI scrittaBottoneProssimoLivello;

    [SerializeField] private UnityEvent playerStop;
    /*
    [Header ("cambio livello")]
    [SerializeField] private Slider sliderCaricamento;        //slider del caricamento della partita
    [SerializeField] private UnityEvent allAvvio;             //serve per eliminare altri elementi in visualilzzazione
    */

    void Start()
    {
        pannello.SetActive(false);
    }

    public void apriPannello(int numeroLivello, Player giocatore)
    {
        pannello.SetActive(true);

        playerStop.Invoke();
        PuntatoreMouse.abilitaCursore();
        CambioCursore.cambioCursoreNormale();
        
        setScrittaFineLivello(numeroLivello, giocatore);
        setBottoneProssimoLivello(numeroLivello);
    }

    private void setScrittaFineLivello(int numeroLivello, Player giocatore)
    {
        print(giocatore == null);
        scrittaFineLivello.text = "Congratulazioni " + giocatore.nome + "\nhai finito ";
        if (numeroLivello == 0)
        {
            scrittaFineLivello.text += "il tutorial.";
        }
        else if (numeroLivello == 1)
        {
            scrittaFineLivello.text += "il primo livello.";
        }
        else if (numeroLivello == livelloFinale)
        {
            scrittaFineLivello.text += "l'ultimo livello.";
        }
        scrittaFineLivello.text += "\nCon un punteggio di " + giocatore.punteggio;
        
        if (numeroLivello != livelloFinale)
        {
            scrittaFineLivello.text += "\n\nOra decidi se vuoi tornare al menu principale o proseguire";
        }
        else
        {

            scrittaFineLivello.text += "\n\nOra puoi tornare al menu principale";
        }
    }

    private void setBottoneProssimoLivello(int numeroLivello)
    {
        if (numeroLivello != livelloFinale)
        {
            scrittaBottoneProssimoLivello.text = "Prosegui al livello " + (numeroLivello + 1).ToString();
        }
        else
        {
            bottoneProssimoLivello.interactable = false;
            scrittaBottoneProssimoLivello.text = "Hai finito l'ultimo livello del gioco";
        }
    }

    /*
    public void tornaAlMenuPrincipale()
    {
        print("torna a menu principale");
        StartCoroutine(caricamentoAsincrono(0));
    }

    IEnumerator caricamentoAsincrono(int sceneIndex)
    {
        allAvvio.Invoke();
        yield return new WaitForSecondsRealtime(2f);
        AsyncOperation caricamento = SceneManager.LoadSceneAsync(sceneIndex);

        while (!caricamento.isDone)
        {
            float progresso = Mathf.Clamp01(caricamento.progress / .9f);
            sliderCaricamento.value = progresso;
            yield return null;
        }
    }
    */
}
