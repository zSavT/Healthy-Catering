using System;
using System.Collections.Generic;

public class Database
{

    //Get database e oggetti
    /// <summary>
    /// Il metodo permette di ottenere il database della classe passata
    /// </summary>
    /// <typeparam name="Oggetto">Oggetto oggetto da trovare il dataset</typeparam>
    /// <param name="oggetto">Oggetto oggetto da trovare il dataset</param>
    /// <returns>List database oggetto</returns>
    public static List<Oggetto> getDatabaseOggetto<Oggetto>(Oggetto oggetto)
    {
        return Serializza.leggiOggettiDaFile<Oggetto>(Serializza.getJsonPath(oggetto));
    }


    //Check e salva oggetti

    /// <summary>
    /// Il metodo controlla se la lista di oggetti passati è già presente su file o meno
    /// </summary>
    /// <typeparam name="Oggetto">Oggetto generico delle classi</typeparam>
    /// <param name="oggetto">Oggetto da controlla se già presente nel database</param>
    /// <param name="databaseOggetto">Database da controllare se presente oggetto passato in input</param>
    /// <returns></returns>
    protected static bool oggettoGiaPresente<Oggetto>(Oggetto oggetto, List<Oggetto> databaseOggetto = null)
    {
        databaseOggetto ??= getDatabaseOggetto(oggetto);

        if (databaseOggetto.Count > 0)
            foreach (Oggetto singoloOggetto in databaseOggetto)
                if (singoloOggetto.Equals(oggetto))
                    return true;

        return false;
    }
	
    /// <summary>
    /// Il metodo permette di salvare l'oggetto passato in input su file
    /// </summary>
    /// <typeparam name="Oggetto">Oggetto della classe database</typeparam>
    /// <param name="oggetto">oggetto da salvare</param>
    /// <param name="databaseOggetto">database da stampare su file</param>
    public static void salvaNuovoOggettoSuFile<Oggetto>(Oggetto oggetto, List<Oggetto> databaseOggetto = null)
    {
        databaseOggetto ??= getDatabaseOggetto(oggetto);

        if (!(oggettoGiaPresente(oggetto, databaseOggetto)))
        {
            if (!(databaseOggetto.Contains(oggetto)))
                databaseOggetto.Add(oggetto);

            Serializza.salvaOggettiSuFile(databaseOggetto);
        }
    }

    /// <summary>
    /// Il metodo permette di agigornare un file presente già nel database
    /// </summary>
    /// <typeparam name="Oggetto">Oggetto della classe database</typeparam>
    /// <param name="nuovoDatabaseOggetto">lista di oggetti da salvare su file</param>
    public static void aggiornaDatabaseOggetto <Oggetto> (List<Oggetto> nuovoDatabaseOggetto)
    {
        Serializza.salvaOggettiSuFile(nuovoDatabaseOggetto);
    }

    /// <summary>
    /// Il metodo permette di ottenere il player corrispondente al nome del player passato in input
    /// </summary>
    /// <param name="nomePlayer">string nome del player</param>
    /// <param name="databasePlayer">database player da verificare se presente quello passato in input</param>
    /// <returns></returns>
    /// <exception cref="Exception">Eccezione player non trovato</exception>
    public static Player getPlayerDaNome(string nomePlayer, List<Player> databasePlayer = null)
    {
        databasePlayer ??= Database.getDatabaseOggetto(new Player());

        foreach (Player player in databasePlayer)
        {
            if (nomePlayer == player.nome)
            {
                Player temp = player;
                return temp;
            }
        }
        throw new Exception("Non è stato trovato nessuno Player con il nome: " + nomePlayer);
    }


    //METODO NON USATI MA EVENTUALMENTE UTILI


    /// <summary>
    /// Il metodo restituisce la lista cliente suddivisi per patologia passata in input
    /// </summary>
    /// <param name="idexPatologia">int id patologia da verificare</param>
    /// <returns>Lista di clienti con la patologia passata in input</returns>
    public static List<Cliente> getListaClientiPerPatologia(int idexPatologia)
    {
        List<Cliente> databaseClienti = Costanti.databaseClienti;
        for(int i = 0; i < databaseClienti.Count; i++)
        {
            if (!databaseClienti[i].listaIdPatologie.Contains(idexPatologia))
            {
                databaseClienti.Remove(databaseClienti[i]);
            }
        }
        return databaseClienti;
    }
}