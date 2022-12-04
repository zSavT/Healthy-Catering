using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wilberforce;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Classe per la gestione delle impostazioni presenti nel menu della creazione profilo utente<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu della scena della creazione del profilo utente.
/// </para>
/// </summary>
public class SelezioneProfiloUtenteEsistente : MonoBehaviour
{
    [Header("Pannelli Elementi")]
    [SerializeField] private GameObject elementiGenereNeutro;
    [SerializeField] private GameObject elementiDomandaUscita;
    [SerializeField] private GameObject elementiEliminazioneProfilo;
    [Header("Dropdown Elementi")]
    [SerializeField] private TMP_Dropdown dropDownListaPlayer;
    [SerializeField] private TMP_Dropdown dropDownGenere;
    [SerializeField] private TMP_Dropdown dropDownColorePelle;
    [SerializeField] private TMP_Dropdown dropDownModello3D;
    [Header("Altro")]
    [SerializeField] private AudioSource suonoClick;
    private Camera cameraGioco;
    private ControllerInput controllerInput;
    private GameObject ultimoElementoSelezionato;
    private List<Player> players = new List<Player>();
    private List<string> nomiPlayerPresenti = new List<string>();
    private string nomeSelezionato = string.Empty;
    private int sceltaGenere;
    private int sceltaColorePelle;
    private int sceltaModelloPlayer;
    bool genereNeutroScelto = false;


    // Start is called before the first frame update
    void Start()
    {
        inizializzaElementiIniziali();
    }

    private void Update()
    {
        controlloElementoDaSelezionare();
    }

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDisable()
    {
        controllerInput.Disable();
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
    /// Il metodo inizializza tutti gli elementi iniziali per il menu
    /// </summary>
    private void inizializzaElementiIniziali()
    {
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        cameraGioco = FindObjectOfType<Camera>();
        cameraGioco.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        letturaNomiUtenti();
        nomiPlayer();
        aggiuntaNomiDropdown();
        dropDownListaPlayer.value = indiceNomeGiocatoreInLista(PlayerSettings.caricaNomePlayerGiocante());
        nomeSelezionato = dropDownListaPlayer.options[dropDownListaPlayer.value].text;
        dropDownGenere.value = PlayerSettings.caricaGenereGiocatore(nomeSelezionato);
        dropDownColorePelle.value = PlayerSettings.caricaColorePelle(nomeSelezionato);
        dropDownModello3D.value = PlayerSettings.caricaGenereModello3D(nomeSelezionato);
        attivaDisattivaImpostazioniGenereNeutro();
        refreshValori();
        elementiDomandaUscita.SetActive(false);
    }


    /// <summary>
    /// Il metodo se un GamePad è connesso, controlla se l'eventsystem.currentSelectedGameObject risulta nullo ed imposta quello corretto
    /// </summary>
    private void controlloElementoDaSelezionare()
    {
        if (Utility.gamePadConnesso())
            if (EventSystem.current.currentSelectedGameObject == null)
                if (Utility.qualsiasiTastoPremuto(controllerInput))
                    if (elementiDomandaUscita.activeSelf && !elementiEliminazioneProfilo.activeSelf)
                        EventSystem.current.SetSelectedGameObject(elementiDomandaUscita.GetComponentsInChildren<Button>()[1].gameObject);
                   else if (!elementiDomandaUscita.activeSelf && elementiEliminazioneProfilo.activeSelf)
                        EventSystem.current.SetSelectedGameObject(elementiEliminazioneProfilo.GetComponentsInChildren<Button>()[1].gameObject);
                   else if (!elementiDomandaUscita.activeSelf && !elementiEliminazioneProfilo.activeSelf)
                        EventSystem.current.SetSelectedGameObject(dropDownListaPlayer.gameObject);
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
    /// Trova l'indice corrispondente al nome della lista dei players. 
    /// </summary>
    /// <param name="nome">Nome del giocatore selezionato</param>
    /// <returns>Indice della lista dei nomi presenti</returns>
    private int indiceNomeGiocatoreInLista(string nome)
    {
        int indice = 0;
        while(indice<dropDownListaPlayer.options.Count)
        {
            if(dropDownListaPlayer.options[indice].text.CompareTo(nome) == 0)
            {
                return indice;
            }
            indice++;
        }
        return indice;
       
    }

    /// <summary>
    /// Aggiorna i valori degli elementi presenti nella scena con quelli attuali.
    /// </summary>
    public void refreshValori()
    {
        dropDownGenere.value = PlayerSettings.caricaGenereGiocatore(nomeSelezionato);
        dropDownColorePelle.value = PlayerSettings.caricaColorePelle(nomeSelezionato);
        dropDownModello3D.value = PlayerSettings.caricaGenereModello3D(nomeSelezionato);
    }

    /// <summary>
    /// Salva tutte le impostazioni fatte dal giocatore, salva il giocatore su file<br></br>
    /// Carica successivamente il menu principale.
    /// <see cref="PlayerSettings"/>.
    /// </summary>
    public void salvaImpostazioni()
    {
        PlayerSettings.salvaNomePlayerGiocante(nomeSelezionato);
        PlayerSettings.salvaGenereGiocatore(nomeSelezionato, sceltaGenere);
        PlayerSettings.salvaColorePelle(nomeSelezionato, sceltaColorePelle);
        if (genereNeutroScelto)
        {
            PlayerSettings.salvaGenereModello3D(nomeSelezionato, sceltaModelloPlayer);
        } else
        {
            PlayerSettings.salvaGenereModello3D(nomeSelezionato, sceltaGenere);
        }
        SelezioneLivelli.caricaMenuPrincipale();
    }


    /// <summary>
    /// Inizializza la lista dei players presenti nel database.
    /// </summary>
    private void letturaNomiUtenti()
    {
        players = Database.getDatabaseOggetto(new Player());
    }


    /// <summary>
    /// Aggiunge i nomi presenti dalla lista players.
    /// </summary>
    public void nomiPlayer()
    {
        List<Player> players = Database.getDatabaseOggetto(new Player());
        for (int i = 0; i < players.Count; i++)
        {
            nomiPlayerPresenti.Add(players[i].nome);
        }
    }

    /// <summary>
    /// Aggiunge i players presenti nel database nella lista.
    /// </summary>
    private void aggiuntaNomiDropdown()
    {
        dropDownListaPlayer.AddOptions(nomiPlayerPresenti);
    }

    /// <summary>
    /// Metodo che salva localmente il valore della scelta del genere del modello del giocatore.
    /// </summary>
    /// <param name="indice">Indice scela del modello players<br><strong>0: Maschio<br>1: Femmina</br></strong></br></param>
    public void setSceltaModelloGiocatore(int indice)
    {
        suonoClick.Play();
        sceltaModelloPlayer = indice;
    }

    /// <summary>
    /// Attiva o disattiva le impostazioni del genere neutro in base al valore del dropdown del genere.
    /// </summary>
    private void attivaDisattivaImpostazioniGenereNeutro()
    {
        if (dropDownGenere.value == 2)
        {
            genereNeutroScelto = true;
            elementiGenereNeutro.SetActive(true);
        }
        else
        {
            genereNeutroScelto = false;
            elementiGenereNeutro.SetActive(false);
        }
    }

    /// <summary>
    /// Controlla se il genere scelto sia quello neutro.<br></br>
    /// In caso affermativo attiva il dropdown per la scelta del genere del modello, altrimenti lo disattiva.
    /// </summary>
    /// <param name="indice">Indice dropdown del genere<br><strong>0: Maschio<br>1: Femmina</br><br>2: Neutro</br></strong></br></param>
    private void attivaDisattivaImpostazioniGenereNeutro(int indice)
    {
        if (indice == 2)
        {
            genereNeutroScelto = true;
            elementiGenereNeutro.SetActive(true);
        }
        else
        {
            genereNeutroScelto = false;
            elementiGenereNeutro.SetActive(false);
        }
    }


    /// <summary>
    /// Metodo che salva localmente la scelta del genere scelto dal giocatore e controlla se il genere scelto sia quello neutro.<br></br>
    /// In caso affermativo attiva il dropdown per la scelta del genere del modello, altrimenti lo disattiva.
    /// </summary>
    /// <param name="indiceScelta">Indice dropdown del genere<br><strong>0: Maschio<br>1: Femmina</br><br>2: Neutro</br></strong></br></param>
    public void setGenerePlayer(int indiceScelta)
    {
        suonoClick.Play();
        sceltaGenere = indiceScelta;
        attivaDisattivaImpostazioniGenereNeutro(sceltaGenere);
    }


    /// <summary>
    /// Salva localmente il valore della pelle scelto dal giocatore.
    /// </summary>
    /// <param name="indice">Indice del dropdown scelta colore pelle modello.<br><strong>0: Caucasico<br>1: Asiatico</br><br>2: Afro</br></strong></br></param>
    public void setPellePlayer(int indice)
    {
        suonoClick.Play();
        sceltaColorePelle = indice;
    }

    /// <summary>
    /// Inizializza correttamente il valore del profilo utente selezionato.
    /// </summary>
    /// <param name="indice">Indice della scelta dropdown lista nomi profili giocatore.</param>
    public void indiceSceltaNomeUtente(int indice)
    {
        suonoClick.Play();
        nomeSelezionato = dropDownListaPlayer.options[dropDownListaPlayer.value].text;
        refreshValori();
    }

    /// <summary>
    /// Metodo che permette la corretta eliminazione del profilo utente selezionato al momento.<br>
    /// Se il profilo eliminato è anche l'ultimo, verrà caricato il menu principale.</br>
    /// </summary>
    public void eliminazioneProfilo()
    {
        PlayerSettings.rimuoviChiaviProfiloUtente(nomeSelezionato);
        players.Remove(new Player(nomeSelezionato));
        dropDownListaPlayer.ClearOptions();
        nomiPlayerPresenti.Remove(nomeSelezionato);
        Database.aggiornaDatabaseOggetto(players);
        if(players.Count == 0)
        {
            PlayerSettings.salvaNomePlayerGiocante("");
            SelezioneLivelli.caricaMenuPrincipale();
        } else
        {
            nomeSelezionato = players[0].nome;
            aggiuntaNomiDropdown();
            dropDownListaPlayer.value = indiceNomeGiocatoreInLista(PlayerSettings.caricaNomePlayerGiocante());
            refreshValori();
            PlayerSettings.salvaNomePlayerGiocante(nomeSelezionato);
        }
    }

}
