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
    protected static bool oggettoGiaPresente<Oggetto>(Oggetto oggetto, List<Oggetto> databaseOggetto = null)
    {
        databaseOggetto ??= getDatabaseOggetto(oggetto);

        if (databaseOggetto.Count > 0)
            foreach (Oggetto singoloOggetto in databaseOggetto)
                if (singoloOggetto.Equals(oggetto))
                    return true;

        return false;
    }
	
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

    public static void aggiornaDatabaseOggetto <Oggetto> (List<Oggetto> nuovoDatabaseOggetto)
    {
        Serializza.salvaOggettiSuFile(nuovoDatabaseOggetto);
    }

    public static int getNewIntFromUtente(string output)
    {
        Console.WriteLine(output);

        bool numeroValido = false;

        while (!numeroValido)
        {
            string input = Console.ReadLine();
            numeroValido = int.TryParse(input, out int numero);
            if (numeroValido)
                return numero;
            Console.WriteLine($"{input} non è un numero");
        }

        return 0;
    }

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