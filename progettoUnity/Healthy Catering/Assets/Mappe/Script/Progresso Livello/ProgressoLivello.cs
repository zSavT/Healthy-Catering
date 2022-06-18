using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using System.Collections;

/// <summary>
/// Classe che gestisce gli obbiettivi del livello, ovvero il numero di clienti da servire ed il punteggio da raggiungere.
/// </summary>
public class ProgressoLivello : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scrittaObbiettivo;
    [SerializeField] private Color32 coloreRaggiuntoObbiettivo;
    //da eventualmente eliminare se esiste un modo per accedere al punteggio nella classe player
    private int punteggioPlayer;                        
    [Header("Obbiettivo numero Clienti da Servire")]
    [SerializeField] private TextMeshProUGUI obbiettivoUno;
    [SerializeField] private Toggle obbiettivoUnoToogle;
    [SerializeField] private int numeroClientiDaServire = 3;
    private int numeroClientiServiti;
    //testo da visualizzare, da modificare le stringhe se si volesse aggiungere un colore alle parole
    private string testoObbietivo1;                                         
    private bool obbiettivoUnoRaggiunto = false;

    [Header("Obbiettivo Punteggio da Raggiungere")]
    [SerializeField] private TextMeshProUGUI obbiettivoDue;
    [SerializeField] private Toggle obbiettivoDueToogle;
    [SerializeField] private int punteggioMassimo = 200;
    //testo da visualizzare, da modificare le stringhe se si volesse aggiungere un colore alle parole
    private string testoObbietivo2;                                        
    private bool obbiettivoDueRaggiunto = false;

    [Header("Fine Livello")]
    //pannello che mostra le scritte di fine livello ed il bottone per tornare al menu iniziale, quando attivo, parte in automatico l'animazione impostasta con il file .anim
    [SerializeField] private GameObject schermataFineLivello;         
    //Testo da inizializzare con il valore del punteggio del giocatore raggiunto a fine livello.
    [SerializeField] private TextMeshProUGUI valorePunteggioPlayer;
    public UnityEvent disattivaElementiFineLivello;


    private void Start()
    {
        //disattivare la schermata per evitare che l'animazione parti fin da subito (N.B. L'animazione è impostata per avviarsi all'attivazione dell'oggetto per semplicità è per dover scrivere molti meno controlli)
        schermataFineLivello.SetActive(false);
        valorePunteggioPlayer.gameObject.SetActive(false);
        //Se il livello è il livello tutorial la schermata obbiettivi non si attiva (da attivare successivamente)
        if (PlayerSettings.livelloSelezionato != -1)
        {
            testoObbietivo1 = "Servire " + numeroClientiDaServire + " clienti. Clienti serviti: " + numeroClientiServiti + "/" + numeroClientiDaServire;
            obbiettivoUno.text = testoObbietivo1;
            testoObbietivo2 = "Raggiungi un punteggio pari a " + punteggioMassimo + ". Punteggio attuale " + 0 + "/" + punteggioMassimo;
            obbiettivoDue.text = testoObbietivo2;
        } 
    }

    private void Update()
    {
        //controllo costante dell'obbiettivi raggiunti, eventualmente questo controllo può essere spostato altrove in base a come verrà strutturato il gioco successivamente.
        if(obbiettiviRaggiunti())
        {
            attivazioneSchermataFineLivello();
        }
    }

    /// <summary>
    /// Aggiornamento dei parametri per il controllo degli obbiettivi.<br></br>
    /// Il testo degll'obbiettivi si aggiorna in automatico i valori aggiornati.
    /// </summary>
    /// <param name="punteggio">Punteggio raggiunto dal giocatore</param>
    public void servitoCliente(int punteggio)
    {
        punteggioPlayer = punteggio;
        numeroClientiServiti++;
        testoObbietivo1 = "Servire " + numeroClientiDaServire + " clienti. Clienti serviti: " + numeroClientiServiti + "/" + numeroClientiDaServire;
        obbiettivoUno.text = testoObbietivo1;
        testoObbietivo2 = testoObbietivo2 = "Raggiungi un punteggio pari a " + punteggioMassimo + ". Punteggio attuale " + punteggio + "/" + punteggioMassimo;
        obbiettivoDue.text = testoObbietivo2;
        controlloProgressiObbiettivo(punteggio);
    }

    /// <summary>
    /// Controllo e aggiornamento degli obbiettivi del livello.<br></br>
    /// Se un obbiettivo è stato raggiunto, il testo si colora con il <strong>coloreRaggiuntoObbiettivo</strong> ed il Toggle si setta su True.
    /// </summary>
    /// <param name="punteggio">Punteggio raggiunto dal giocatore</param>
    private void controlloProgressiObbiettivo(int punteggio)
    {
        if(numeroClientiServiti == numeroClientiDaServire)
        {
            obbiettivoUnoToogle.isOn = true;
            obbiettivoUno.color = coloreRaggiuntoObbiettivo;
            obbiettivoUnoRaggiunto = true;
        }
        if (punteggio >= punteggioMassimo)
        {
            obbiettivoDueToogle.isOn = true;
            obbiettivoDue.color = coloreRaggiuntoObbiettivo;
            obbiettivoDueRaggiunto = true;
        } else
        {
            //Solo l'obbiettivo due si può resettare perchè il punteggio può diminuire ma il numero dei clienti serviti no
            obbiettivoDueToogle.isOn = false;
            obbiettivoDue.color = Color.white;
        }
    }

    /// <summary>
    /// Controllo se entrambi gli obbiettivi sono stati raggiunti.
    /// </summary>
    /// <returns>Restituisce true se entrambi gli obbiettivi sono stati raggiunti, falso se anche uno dei due obbiettivi non è stato raggiunto.</returns>
    public bool obbiettiviRaggiunti()
    {
        if (obbiettivoUnoRaggiunto && obbiettivoDueRaggiunto)
            return true;
        else
            return false;
    }


    //Il metodo eventualmente può essere eliminato per inserire il suo contenuto altrove, oppure può essere espanso in base alle necessità.
    private void attivazioneSchermataFineLivello()
    {
        schermataFineLivello.SetActive(true);
        valorePunteggioPlayer.gameObject.SetActive(true);
        valorePunteggioPlayer.text = "Punteggio raggiunto: " + punteggioPlayer.ToString();
        disattivaElementiFineLivello.Invoke();
        PuntatoreMouse.abilitaCursore();
        disattivaObbiettivi();
       // GameObject.FindObjectOfType<Camera>().transform.position = new Vector3(0, 4000, 0);       //sposta la telecamera in ciealo
    }


    private void disattivaObbiettivi()
    {
        obbiettivoUno.gameObject.SetActive(false);
        obbiettivoUnoToogle.gameObject.SetActive(false);
        obbiettivoDue.gameObject.SetActive(false);
        obbiettivoDueToogle.gameObject.SetActive(false);
        scrittaObbiettivo.gameObject.SetActive(false);
    }

    /// <summary>
    /// Metodo che salva il progresso livello e carica il menu Iniziale
    /// </summary>
    public void tornaAlMenuPrincipale()
    {
        PlayerSettings.salvaProgressoLivello1(true);
        SelezioneLivelli.caricaMenuPrincipale();
    }
}
