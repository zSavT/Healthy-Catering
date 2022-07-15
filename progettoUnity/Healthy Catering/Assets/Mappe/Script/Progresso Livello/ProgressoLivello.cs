using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Classe che gestisce gli obbiettivi del livello, ovvero il numero di clienti da servire ed il punteggio da raggiungere.
/// </summary>
public class ProgressoLivello : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scrittaObbiettivo;
    [SerializeField] private Color32 coloreRaggiuntoObbiettivo;
    //da eventualmente eliminare se esiste un modo per accedere al punteggio nella classe player
    private int punteggioPlayer;
    private Player giocatore;
    [SerializeField] Interactor interazioni;
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

    [Header("Lista obbiettivi livello")]
    [SerializeField] private GameObject pannelloObbiettiviInizioLivello;
    [SerializeField] private TextMeshProUGUI titoloObbiettiviInizioLivello;
    [SerializeField] private TextMeshProUGUI listaObbiettiviInizioLivello;
    [SerializeField] private TextMeshProUGUI listaCriteriGameOver;
    [SerializeField] private UnityEvent playerStop;

    [Header("Fine Livello")]
    //pannello che mostra le scritte di fine livello ed il bottone per tornare al menu iniziale, quando attivo, parte in automatico l'animazione impostasta con il file .anim
    [SerializeField] private GameObject schermataFineLivello;         
    //Testo da inizializzare con il valore del punteggio del giocatore raggiunto a fine livello.
    [SerializeField] private TextMeshProUGUI valorePunteggioPlayer;
    [SerializeField] private TextMeshProUGUI titoloSchermataFineLivello;
    [SerializeField] private AudioSource suonoVittoria;
    public UnityEvent disattivaElementiFineLivello;
    [Header("GameOver")]
    public int numeroDiClientiMassimi = 10;
    private bool gameOver = false;
    private int minimoSoldi = 5;
    [SerializeField] private ParticleSystem particellare1;
    [SerializeField] private ParticleSystem particellare2;
    [SerializeField] private AudioSource suonoGameOver;


    private void Start()
    {
        gameOver = false;
        //disattivare la schermata per evitare che l'animazione parti fin da subito (N.B. L'animazione � impostata per avviarsi all'attivazione dell'oggetto per semplicit� � per dover scrivere molti meno controlli)
        schermataFineLivello.SetActive(false);
        numeroClientiServiti = 0;
        punteggioPlayer = 0;
        valorePunteggioPlayer.gameObject.SetActive(false);
        //Se il livello � il livello tutorial la schermata obbiettivi non si attiva (da attivare successivamente)
        if (PlayerSettings.livelloSelezionato != 0)
        {
            disattivaObbiettiviETesto();
            attivaPannelloRiepiloghiObbiettivi();
        } else
        {
            disattivaSoloObbiettivi();
        }
        punteggioPlayer = 0;
    }

    private void Update()
    {
        //controllo costante dell'obbiettivi raggiunti, eventualmente questo controllo pu� essere spostato altrove in base a come verr� strutturato il gioco successivamente.
        if(obbiettiviRaggiunti())
        {
            attivazioneSchermataFineLivello();
        }
        if(!ProgressoTutorial.inTutorial)
            controlloGameOver();
    }

    /// <summary>
    /// Attiva il pannello del riepilogo livello iniziale.
    /// </summary>
    public void attivaPannelloRiepiloghiObbiettivi()
    {
        playerStop.Invoke();
        PuntatoreMouse.abilitaCursore();
        pannelloObbiettiviInizioLivello.SetActive(true);
        listaObbiettiviInizioLivello.text = "Servire " + numeroClientiDaServire + " clienti.\nRaggiungere il punteggio: " + punteggioMassimo + ".";
        listaCriteriGameOver.text = "Denaro inferiore a " + minimoSoldi + " e nessuna possibilità di servire almeno un piatto.\nServiti " + numeroDiClientiMassimi + " clienti non avendo raggiunto un punteggio pari o superiore a " + punteggioMassimo;
    }

    /// <summary>
    /// Disattiva gli obbiettivi del gioco e gli inizalizza
    /// </summary>
    public void disattivaPannelloRiepiloghiObbiettiviEInizializzaVolori()
    {
        PuntatoreMouse.disabilitaCursore();
        pannelloObbiettiviInizioLivello.SetActive(false);
        attivaSoloObbiettivi();
        if(PlayerSettings.livelloSelezionato == 0)
        {
            punteggioPlayer = giocatore.punteggio[0];
        }
        valoriInizialiTesto();
    }


    /// <summary>
    /// Controllo del gameOver della partita
    /// </summary>
    private void controlloGameOver()
    {
        if( (soldiFiniti() && !giocatore.piattiRealizzabiliConInventario()) || numeroClientiServiti == numeroDiClientiMassimi)
        {
            gameOver = true;
            settaggiSchermataFineLivelloGameOver();
            attivazioneSchermataFineLivello();
        }
    }

    /// <summary>
    /// Imposta la schermata di fine livello per il gameOver.
    /// </summary>
    private void settaggiSchermataFineLivelloGameOver()
    {
        titoloSchermataFineLivello.text = "Hai perso!";
        var main1 = particellare1.main;
        var main2 = particellare2.main;
        main1.playOnAwake = false;
        main2.playOnAwake = false;
        suonoVittoria.gameObject.SetActive(false);
        suonoGameOver.gameObject.SetActive(true);
    }


    /// <summary>
    /// Metodo per il controllo del criterio del gameOver del giocatore
    /// </summary>
    /// <returns>True: Il giocatore ha meno del minimo dei soldi. <br>False: Il giocatore ha più del minimo dei soldi.</br></returns>
    private bool soldiFiniti()
    {
        if(giocatore.soldi > minimoSoldi)
        {
            return false;
        } else
        {
            return true;
        }
        
    }

    /// <summary>
    /// Metodo set per il giocatore.
    /// </summary>
    /// <param name="giocatore">Giocatore</param>
    public void setGiocatore(Player giocatore)
    {
        this.giocatore = giocatore;
    }


    /// <summary>
    /// Inizializza i valori iniziali degli obbiettivi nella gui.
    /// </summary>
    private void valoriInizialiTesto()
    {
        testoObbietivo1 = "Servire " + numeroClientiDaServire + " clienti. Clienti serviti: " + numeroClientiServiti + "/" + numeroClientiDaServire;
        obbiettivoUno.text = testoObbietivo1;
        testoObbietivo2 = "Raggiungi un punteggio pari a " + punteggioMassimo + ". Punteggio attuale " + punteggioPlayer + "/" + punteggioMassimo;
        obbiettivoDue.text = testoObbietivo2;
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
    /// Se un obbiettivo � stato raggiunto, il testo si colora con il <strong>coloreRaggiuntoObbiettivo</strong> ed il Toggle si setta su True.
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
            //Solo l'obbiettivo due si pu� resettare perch� il punteggio pu� diminuire ma il numero dei clienti serviti no
            obbiettivoDueToogle.isOn = false;
            obbiettivoDue.color = Color.white;
        }
    }

    /// <summary>
    /// Controllo se entrambi gli obbiettivi sono stati raggiunti.
    /// </summary>
    /// <returns>Restituisce true se entrambi gli obbiettivi sono stati raggiunti, falso se anche uno dei due obbiettivi non � stato raggiunto.</returns>
    public bool obbiettiviRaggiunti()
    {
        if (obbiettivoUnoRaggiunto && obbiettivoDueRaggiunto)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Metodo che attiva la schermata di fine livello sia per il gameOver che per la vittoria.
    /// </summary>
    private void attivazioneSchermataFineLivello()
    {
        schermataFineLivello.SetActive(true);
        valorePunteggioPlayer.gameObject.SetActive(true);
        if(gameOver)
        {
            if ((soldiFiniti() && !giocatore.piattiRealizzabiliConInventario()))
            {
                valorePunteggioPlayer.text = "Punteggio raggiunto: " + punteggioPlayer.ToString() + "\nHai perso perchè non puoi servire alcun piatto con l'inventario attuale e il denaro è sotto il valore di " + minimoSoldi + ".";
            }
            else
            {
                valorePunteggioPlayer.text = "Punteggio raggiunto: " + punteggioPlayer.ToString() + "\nHai perso perchè non hai raggiunto l'obbiettivo del punteggio entro i " + numeroDiClientiMassimi + " clienti serviti.";
            }
        } else
        {
            valorePunteggioPlayer.text = "Punteggio raggiunto: " + punteggioPlayer.ToString() + ", complimenti!";
        }


        disattivaElementiFineLivello.Invoke();
        PuntatoreMouse.abilitaCursore();
        disattivaObbiettiviETesto();
       // GameObject.FindObjectOfType<Camera>().transform.position = new Vector3(0, 4000, 0);       //sposta la telecamera in ciealo
    }


    /// <summary>
    /// Disattiva gli elementi degli obbiettivi.
    /// </summary>
    private void disattivaObbiettiviETesto()
    {
        obbiettivoUno.gameObject.SetActive(false);
        obbiettivoUnoToogle.gameObject.SetActive(false);
        obbiettivoDue.gameObject.SetActive(false);
        obbiettivoDueToogle.gameObject.SetActive(false);
        scrittaObbiettivo.gameObject.SetActive(false);
    }

    /// <summary>
    /// Disattiva tutti gli elementi tranne la scritta Obbiettivo
    /// </summary>
    private void disattivaSoloObbiettivi()
    {
        obbiettivoUno.gameObject.SetActive(false);
        obbiettivoUnoToogle.gameObject.SetActive(false);
        obbiettivoDue.gameObject.SetActive(false);
        obbiettivoDueToogle.gameObject.SetActive(false);
    }

    /// <summary>
    /// Attiva solo la sezione degli obbiettivi del gioco (non la scritta obbiettivi verde).
    /// </summary>
    public void attivaSoloObbiettivi()
    {
        obbiettivoUno.gameObject.SetActive(true);
        obbiettivoUnoToogle.gameObject.SetActive(true);
        obbiettivoDue.gameObject.SetActive(true);
        obbiettivoDueToogle.gameObject.SetActive(true);
    }

    /// <summary>
    /// Metodo che salva il progresso livello e carica il menu Iniziale
    /// </summary>
    public void tornaAlMenuPrincipale()
    {
        Debug.Log(PlayerSettings.livelloSelezionato);
        if(!gameOver)
        {
            Database.aggiornaDatabaseOggetto(aggiornaGiocatore());
            if(PlayerSettings.livelloSelezionato == 0)
            {
                PlayerSettings.salvaProgressoLivello1(true);
            }
            if (PlayerSettings.livelloSelezionato == 1)
            {
                PlayerSettings.salvaProgressoLivello2(true);
                Debug.Log("Ue");
                Debug.Log(PlayerSettings.caricaProgressoLivello2());
            }
            
        }
        Interactable.numeroCliente = 0;
        SelezioneLivelli.caricaMenuPrincipale();
    }

    /// <summary>
    /// Legge da file la lista dei player presenti e poi aggiorna il punteggio del giocatore.
    /// </summary>
    /// <returns>Lista giocatori aggiornata</returns>
    private List<Player> aggiornaGiocatore()
    {
        List<Player> listaPlayer = Database.getDatabaseOggetto(new Player());
        int i = 0;
        foreach(Player temp in listaPlayer)
        {
            if(temp.nome == giocatore.nome)
            {
                if(temp.punteggio[PlayerSettings.livelloSelezionato] < giocatore.punteggio[PlayerSettings.livelloSelezionato])
                    listaPlayer[i].punteggio[PlayerSettings.livelloSelezionato] = punteggioPlayer;
                break;
            }
            i++;
        }
        return listaPlayer;
    }
}
