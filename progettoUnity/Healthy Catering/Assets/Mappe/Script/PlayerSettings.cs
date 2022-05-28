using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static bool profiloUtenteCreato = true;

    //PROFILO UTENTE


    //GENERE MODELLO
    public static void salvaGenereModello3D(string nomeGiocatore, int scelta)
    {

        PlayerPrefs.SetInt(nomeGiocatore + "_modello", scelta);

    }

    public static int caricaGenereModello3D(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_modello");
    }

    //NOME GIOCATORE
    public static void salvaNomePlayerGiocante(string nomeInserito)
    {

        PlayerPrefs.SetString("PlayerName", nomeInserito);

    }

    public static string caricaNomePlayerGiocante()
    {
        return PlayerPrefs.GetString("PlayerName");
    }
    
    //PROGRESSI LIVELLI

    //LIVELLO 1

    public static void salvaProgressoLivello1(bool completato)
    {
        if (completato)
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello1", 1);
        else
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello1", 0);
    }

    public static int caricaProgressoLivello1()
    {
        return PlayerPrefs.GetInt(caricaNomePlayerGiocante() + "_livello1");
    }

    //LIVELLO 2

    public static void salvaProgressoLivello2(bool completato)
    {
        if (completato)
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello2", 1);
        else
            PlayerPrefs.SetInt(caricaNomePlayerGiocante() + "_livello2", 0);
    }

    public static int caricaProgressoLivello2()
    {
        return PlayerPrefs.GetInt(caricaNomePlayerGiocante() + "_livello2");
    }

    //COLORE PELLE GIOCATORE
    public static void salvaColorePelle(string nomeGiocatore, int scelta)
    {

        PlayerPrefs.SetInt(nomeGiocatore + "_pelle", scelta);

    }

    public static int caricaColorePelle(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_pelle");
    }

    //GENERE GIOCATORE
    public static void salvaGenereGiocatore(string nomeGiocatore, int scelta)
    {
        PlayerPrefs.SetInt(nomeGiocatore + "_genere", scelta);
        if (scelta == 0 || scelta == 1)
        {
            salvaGenereModello3D(nomeGiocatore, scelta);
        }
    }

    public static int caricaGenereGiocatore(string nomeGiocatore)
    {
        return PlayerPrefs.GetInt(nomeGiocatore + "_genere");
    }

    //IMPOSTAZIONI GIOCO



    //FOV
    public static void salvaImpostazioniFov(float fov)
    {
        PlayerPrefs.SetFloat("fov", fov);
    }

    public static float caricaImpostazioniFov()
    {
        return PlayerPrefs.GetFloat("fov");
    }

    //SENSIBILITA' MOUSE
    public static void salvaImpostazioniSensibilita(float sensibilita)
    {
        PlayerPrefs.SetFloat("sensibilita", sensibilita);
    }


    public static float caricaImpostazioniSensibilita()
    {
        return PlayerPrefs.GetFloat("sensibilita");
    }

    //RISOLUZIONE
    public static void salvaImpostazioniRisoluzione(int indiceRisoluzione)
    {
        PlayerPrefs.SetInt("risoluzione", indiceRisoluzione);
    }

    public static int caricaImpostazioniRisoluzione()
    {
        return PlayerPrefs.GetInt("risoluzione");
    }

    //IMPOSTAZIONI DALTONISMO
    public static void salvaImpostazioniDaltonismo(int indiceDaltonismo)
    {
        PlayerPrefs.SetInt("daltonismo", indiceDaltonismo);
    }

    public static int caricaImpostazioniDaltonismo()
    {
        return PlayerPrefs.GetInt("daltonismo");
    }

    public static void salvaImpostazioniVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    //VOLUME PRINCIPALE GIOCO
    public static float caricaImpostazioniVolume()
    {
        return PlayerPrefs.GetFloat("volume");
    }

    //IMPOSTAZIONI FULL SCREEN
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
