using UnityEngine;

/// <summary>
/// Classe per la gestione delle impostazioni del scelte dal player.
/// </summary>
public class PlayerSettings : MonoBehaviour
{
    /// <summary>
    /// Variabile utilizzata per controllo e catch di eventuali errori nella creazione e caricamento dei livelli e utilizzato in
    /// <see cref="ControlloMouse"/>
    /// </summary>
    public static bool profiloUtenteCreato = true;
    /// <summary>
    /// Variabile da utilizzare per conoscere l'indice del livello selezionato, inizializzata in <see cref="SelezioneLivelli"/> 
    /// nel metodo <seealso cref="SelezioneLivelli.playGame(int)"/>
    /// </summary>
    public static int livelloSelezionato = 0;


    /// <summary>
    /// Retta tutti i valori pesenti nel database del playerPref
    /// </summary>
    public static void resetTuttiValori()
    {
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// Metodo che rimuove tutte le chiavi del giocatore passato in input
    /// </summary>
    /// <param name="nomeUtente">Nome giocatore da elimianre</param>
    public static void rimuoviChiaviProfiloUtente(string nomeUtente)
    {
        PlayerPrefs.DeleteKey(nomeUtente + "_Daltonismo");
        PlayerPrefs.DeleteKey(nomeUtente + "_genere");
        PlayerPrefs.DeleteKey(nomeUtente + "_livello2");
        PlayerPrefs.DeleteKey(nomeUtente + "_livello1");
        PlayerPrefs.DeleteKey(nomeUtente + "_livello0");
        PlayerPrefs.DeleteKey(nomeUtente + "_livello1");
        PlayerPrefs.DeleteKey(nomeUtente + "_primoAvvioSensibilita");
        PlayerPrefs.DeleteKey(nomeUtente + "_modello");
        PlayerPrefs.DeleteKey(nomeUtente + "_pelle");
    }

    /// <summary>
    /// Caricare variabile per confermare che è stato avviato per la prima volta il gioco (e non il livello) e quindi non è stato mai creato un player.<br></br>
    /// <strong>Usato in</strong> <see cref="SceltaImpostazioniPlayer.controlloNomeEsistente"/>
    /// </summary>
    /// <returns>0 è la prima volta che si avvia il livello, 1 è stato già avviato una volta.</returns>
    public static int caricaPrimoAvvio()
    {
        return PlayerPrefs.GetInt("primoAvvio");
    }

    /// <summary>
    /// Salva impostazioni primo avvio.
    /// </summary>
    public static void salvaPrimoAvvio()
    {
        PlayerPrefs.SetInt("primoAvvio", 1);
    }

    /// <summary>
    /// Carica variabile per controllo del primo avvio per settare il valore della sensibilità del mouse.
    /// </summary>
    /// <returns>0 è la prima volta che si avvia il livello, 1 è stato già avviato una volta.</returns>
    public static int caricaPrimoAvvioSettaggiSensibilita()
    {
        return PlayerPrefs.GetInt(caricaNomePlayerGiocante() + "_primoAvvioSensibilita");
    }

    /// <summary>
    /// Salva variabile per controllo del primo avvio per settare il valore della sensibilità del mouse.
    /// </summary>
    public static void salvaPrimoAvvioSettaggiSensibilita()
    {
        PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_primoAvvioSensibilita", 1);
    }
    //PROFILO UTENTE

    //GENERE MODELLO

    /// <summary>
    /// Salva scelta del genere modello 3D del giocatore.<br></br>
    /// <strong>0: Modello Maschile</strong><br></br>
    /// <strong>1: Modello Femminile</strong>
    /// </summary>
    /// <param name="nomeGiocatore">Nome del giocatore giocante attivo</param>
    /// <param name="scelta">Valore indice scelta del dropdown</param>
    public static void salvaGenereModello3D(string nomeGiocatore, int scelta)
    {
        PlayerPrefs.SetInt(nomeGiocatore + "_modello", scelta);
    }

    /// <summary>
    /// Carica scelta del genere modello 3D del giocatore.
    /// </summary>
    /// <param name="nomeGiocatore">Nome del giocatore giocante attivo</param>
    /// <returns><strong>0: Modello Maschile</strong><br></br><strong>1: Modello Femminile</strong></returns>
    public static int caricaGenereModello3D(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_modello");
    }

    //NOME GIOCATORE

    /// <summary>
    /// Metodo per salvare il nome del giocatore giocante in quel momento.<br></br>
    /// Utilizzato in <see cref="SelezioneProfiloUtenteEsistente"/> e <see cref="SceltaImpostazioniPlayer"/>
    /// </summary>
    /// <param name="nomeInserito">Nome inserito nell'input field</param>
    public static void salvaNomePlayerGiocante(string nomeInserito)
    {
        PlayerPrefs.SetString("PlayerName", nomeInserito);
    }

    /// <summary>
    /// Metodo che restituisce il nome del giocatore selezionato in quel momento.
    /// </summary>
    /// <returns><strong>Stringa</strong> contenente il nome del profilo giocatore attivo in quel momento.</returns>
    public static string caricaNomePlayerGiocante()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

    //PROGRESSI LIVELLI

    //LIVELLO 1

    /// <summary>
    /// Salva se il livello 1 è stato completato o meno per il profilo del giocatore giocante.
    /// </summary>
    /// <param name="completato">True completato, false non completato</param>
    public static void salvaProgressoLivello1(bool completato)
    {
        if (completato)
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello1", 1);
        else
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello1", 0);
    }

    /// <summary>
    /// Carica se il livello 1 è stato completato o meno per il profilo del giocatore giocante.
    /// </summary>
    /// <returns>1 completato, 0 non completato.</returns>
    public static int caricaProgressoLivello1()
    {
        return PlayerPrefs.GetInt(caricaNomePlayerGiocante() + "_livello1");
    }

    //LIVELLO 2

    /// <summary>
    /// Salva se il livello 2 è stato completato o meno per il profilo del giocatore giocante.
    /// </summary>
    /// <param name="completato">True completato, false non completato</param>
    public static void salvaProgressoLivello2(bool completato)
    {
        if (completato)
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello2", 1);
        else
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello2", 0);
    }

    /// <summary>
    /// Carica se il livello 2 è stato completato o meno per il profilo del giocatore giocante.
    /// </summary>
    /// <returns>1 completato, 0 non completato.</returns>
    public static int caricaProgressoLivello2()
    {
        return PlayerPrefs.GetInt(caricaNomePlayerGiocante() + "_livello2");
    }

    //COLORE PELLE GIOCATORE

    /// <summary>
    /// Salva la scelta del giocatore per il colore della pelle del giocatore. Valore intero per il colore:<br></br>
    /// <strong>0: Caucasico<br>1: Asiatico</br><br>2: Afro</br></strong>
    /// </summary>
    /// <param name="nomeGiocatore">Nome del giocatore attivo in quel momento.</param>
    /// <param name="scelta">Indice scelta colore della pelle.</param>
    public static void salvaColorePelle(string nomeGiocatore, int scelta)
    {

        PlayerPrefs.SetInt(nomeGiocatore + "_pelle", scelta);

    }
    /// <summary>
    /// Salva la scelta del giocatore per il colore della pelle del giocatore. Valore intero per il colore:<br></br>
    /// </summary>
    /// <param name="nomeGiocatore">Nome del giocatore attivo in quel momento.</param>
    /// <returns><strong>0: Caucasico<br>1: Asiatico</br><br>2: Afro</br></strong></returns>
    public static int caricaColorePelle(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_pelle");
    }

    //GENERE GIOCATORE

    /// <summary>
    /// Salva l'impostazione genere del giocatore. Se la scelta del genere è diversa da neutro, allora verà salvato anche il genere del modelo<br></br>
    /// Valori possibili:<br></br>
    /// <strong>0: Maschio<br>1: Femmina</br><br>2: Neutro</br></strong>
    /// </summary>
    /// <param name="nomeGiocatore">Nome del giocatore attivo in quel momento.</param>
    /// <param name="scelta">Indice scelta del giocatore.</param>
    public static void salvaGenereGiocatore(string nomeGiocatore, int scelta)
    {
        PlayerPrefs.SetInt(nomeGiocatore + "_genere", scelta);
        if (scelta == 0 || scelta == 1)
        {
            salvaGenereModello3D(nomeGiocatore, scelta);
        }
    }

    /// <summary>
    /// Carica il genere del scelto dal giocatore.
    /// </summary>
    /// <param name="nomeGiocatore">Nome del giocatore giocante.</param>
    /// <returns><strong>0: Maschio<br>1: Femmina</br><br>2: Neutro</br></strong></returns>
    public static int caricaGenereGiocatore(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_genere");
    }

    //IMPOSTAZIONI GIOCO

    //FOV

    /// <summary>
    /// Metodo che salva il valore scelto per il fov del gioco.
    /// </summary>
    /// <param name="fov">Valore scelto fov dello slider.</param>
    public static void salvaImpostazioniFov(float fov)
    {
        PlayerPrefs.SetFloat("fov", fov);
    }

    /// <summary>
    /// Metodo che carica il valore fov del giocatore.
    /// </summary>
    /// <returns>Valore fov impostato dal giocatore</returns>
    public static float caricaImpostazioniFov()
    {
        return PlayerPrefs.GetFloat("fov");
    }

    //SENSIBILITA' MOUSE

    /// <summary>
    /// Metodo che salva il valore della sensibilità scelta dal giocatore.
    /// </summary>
    /// <param name="sensibilita">Valore della sensibilità scelta dal giocatore nello slider</param>
    public static void salvaImpostazioniSensibilita(float sensibilita)
    {
        PlayerPrefs.SetFloat("sensibilita", sensibilita);
    }

    /// <summary>
    /// Metodo che carica il valore della sensibilità salvata in precedenza dal giocatore.
    /// </summary>
    /// <returns>Valore della sensibilità</returns>
    public static float caricaImpostazioniSensibilita()
    {
        return PlayerPrefs.GetFloat("sensibilita");
    }

    //RISOLUZIONE

    /// <summary>
    /// Metodo che salva l'indice della scelta della risoluzione del giocatore.
    /// </summary>
    /// <param name="indiceRisoluzione">Indice del dropdown delle risoluzioni disponibili.</param>
    public static void salvaImpostazioniRisoluzione(int indiceRisoluzione)
    {
        PlayerPrefs.SetInt("risoluzione", indiceRisoluzione);
    }

    /// <summary>
    /// Metodo che carica l'indice della scelta della risoluzione del giocatore.
    /// </summary>
    /// <returns>Indice scelta indice dropdown risoluzione</returns>
    public static int caricaImpostazioniRisoluzione()
    {
        return PlayerPrefs.GetInt("risoluzione");
    }

    /// <summary>
    /// Metodo che restituisce se le impostazioni delle risoluzione, sono state salvate per la prima volta
    /// </summary>
    /// <returns>0: Mai salvate, 1: Salvate</returns>
    public static int caricaImpostazioniPrimoAvvioRisoluzione()
    {
        return PlayerPrefs.GetInt("risoluzionePrimoAvvio");
    }

    /// <summary>
    /// Metodo che salva se le impostazioni delle risoluzione, sono state salvate per la prima volta
    /// </summary>
    /// <param name="val">Valore da settare.<br>0: Settate, 1: Settate</br></param>
    public static void salvaImpostazioniPrimoAvvioRisoluzione(int val)
    {
        PlayerPrefs.SetInt("risoluzionePrimoAvvio", val);
    }

    //IMPOSTAZIONI DALTONISMO

    /// <summary>
    /// Metodo che salva l'indice della scelta dell'impostazioni del daltonismo.<para>I valori possibili sono:<br></br>
    /// <strong>0: Disattiva<br>1: Protanopia</br><br>2: Deuteranopia</br><br>3: Tritanopia</br></strong>
    /// </para>
    /// </summary>
    /// <param name="indiceDaltonismo">Indice scelta dropdown del daltonismo</param>
    public static void salvaImpostazioniDaltonismo(int indiceDaltonismo)
    {
        PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_Daltonismo", indiceDaltonismo);
    }

    /// <summary>
    /// Metodo che carica l'indice della scelta fatta in precedenza dal giocatore per il daltonismo.
    /// </summary>
    /// <returns><strong>0: Disattiva<br>1: Protanopia</br><br>2: Deuteranopia</br><br>3: Tritanopia</br></strong></returns>
    public static int caricaImpostazioniDaltonismo()
    {
        return PlayerPrefs.GetInt(caricaNomePlayerGiocante() + "_Daltonismo");
    }
    
    //VOLUME PRINCIPALE GIOCO

    /// <summary>
    /// Metodo che salva il valore dello slider del volume della musica.
    /// </summary>
    /// <param name="volume">Valore del volume del gioco scelto dal giocatore nello slider</param>
    public static void salvaImpostazioniVolumeMusica(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    /// <summary>
    /// Metodo che carica il valore dello slider del volume della musica.
    /// </summary>
    /// <returns>Valore del volume scelto</returns>
    public static float caricaImpostazioniVolumeMusica()
    {
        return PlayerPrefs.GetFloat("volume");
    }

    /// <summary>
    /// Metodo che salva le impostazioni audio dei suoni del gioco
    /// </summary>
    /// <param name="volume">Variabile dei suoni</param>
    public static void salvaImpostazioniVolumeSuoni(float volume)
    {
        PlayerPrefs.SetFloat("volumeSuoni", volume);
    }

    /// <summary>
    /// Metodo che restituisce il valore dei suoni salvato in memoria.
    /// </summary>
    /// <returns>Valore volume suoni</returns>
    public static float caricaImpostazioniVolumeSuoni()
    {
        return PlayerPrefs.GetFloat("volumeSuoni");
    }

    //IMPOSTAZIONI FULL SCREEN

    /// <summary>
    /// Metodo che salva l'impostazione dello schermo intero scelto dal giocatori.
    /// </summary>
    /// <param name="fullScreen"><strong>True</strong> toggle su true<br><strong>False</strong> toggle su false</br></param>
    public static void salvaImpostazioniFullScreen(bool fullScreen)
    {
        if (fullScreen)
        {
            PlayerPrefs.SetInt("fullScreen", 0);        //attivo
        }
        else
        {
            PlayerPrefs.SetInt("fullScreen", 1);            //disattivo
        }
    }

    /// <summary>
    /// Metodo che carica il valore del fullscreen salvato in precedenza.
    /// </summary>
    /// <returns><strong>True</strong> toggle su true, fullscreen attivo.<br><strong>False</strong> toggle su false, fullscreen disattivato.</br></returns>
    public static bool caricaImpostazioniFullScreen()
    {
        if (PlayerPrefs.GetInt("fullScreen") == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //IMPOSTAZIONI FRAMERATE

    /// <summary>
    /// Metodo che permette di salvare l'impostazione sul Framerate libero scelto dall'utente.
    /// </summary>
    /// <param name="framerateLibero"><strong>True</strong> toggle su true, Framerate libero attivo.<br><strong>False</strong> toggle su false, Framerate libero disattivato.</br></param>
    public static void salvaImpostazioniFramerateLibero(bool framerateLibero)
    {
        if (framerateLibero)
        {
            PlayerPrefs.SetInt("framerateLibero", 0);        //attivo
        }
        else
        {
            PlayerPrefs.SetInt("framerateLibero", 1);            //disattivo
        }

    }

    /// <summary>
    /// Metodo che permette di caricare l'impostazione sul Framerate libero scelto dall'utente.
    /// </summary>
    /// <returns><strong>True</strong> toggle su true, Framerate libero attivo.<br><strong>False</strong> toggle su false, Framerate libero disattivato.</br></returns>
    public static bool caricaImpostazioniFramerateLibero()
    {
        if (PlayerPrefs.GetInt("framerateLibero") == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
