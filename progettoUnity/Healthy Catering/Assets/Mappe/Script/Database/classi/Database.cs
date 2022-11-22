using System;
using System.Collections.Generic;

public class Database
{
    public static void Main (){
        
    }

    //Get database e oggetti
    public static List<Oggetto> getDatabaseOggetto<Oggetto>(Oggetto oggetto)
    {
        return Serializza.leggiOggettiDaFile<Oggetto>(Serializza.getJsonPath(oggetto));
    }



    public static Oggetto getUltimoOggettoAggiuntoAlDatabase<Oggetto>(Oggetto oggetto, List<Oggetto> databaseOggetto = null)
    {
        databaseOggetto ??= getDatabaseOggetto(oggetto);

        return databaseOggetto[databaseOggetto.Count - 1];
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
	
	public static bool isDatabaseOggettoVuoto<Oggetto> (Oggetto oggetto, List<Oggetto> databaseOggetto = null)
    {
        databaseOggetto ??= Database.getDatabaseOggetto(oggetto);

        return databaseOggetto.Count == 0;
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

    //Get nuovi valori
    public static int getNewId<Oggetto>(Oggetto oggetto)
    {
        List<Oggetto> databaseOggetto = getDatabaseOggetto(oggetto);

        string nomeTipoOggetto = Serializza.getNomeTipo(databaseOggetto).ToLower();

        //prendo l'id dell'ultimo oggetto aggiunto al database(quindi all'indice dimensioneLista - 1) e gli aggiungo 1
        if (nomeTipoOggetto.Equals("ingrediente")){
            Ingrediente temp = (Ingrediente) Convert.ChangeType (databaseOggetto[databaseOggetto.Count - 1], typeof (Ingrediente));
            return temp.idIngrediente + 1;
        }
        else if (nomeTipoOggetto.Equals("patologia")){
            Patologia temp = (Patologia) Convert.ChangeType (databaseOggetto[databaseOggetto.Count - 1], typeof (Patologia));
            return temp.idPatologia + 1;
        }
        else
            throw new Exception("La classe dell'oggetto che mi hai passato non ha una propietà id");
    }

    public static string getNewStringaFromUtente(string output)
    {
        Console.WriteLine(output);
        return Console.ReadLine();
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

    protected static float getNewFloatFromUtente(string output)
    {
        Console.WriteLine(output);

        bool numeroValido = false;

        while (!numeroValido)
        {
            string input = Console.ReadLine();
            numeroValido = Double.TryParse(input, out double numero);
            if (numeroValido)
                return (float)numero;
            Console.WriteLine($"{input} non è un numero reale");
        }

        return 0;
    }


    private static void pulisciDatabase()
    {
        List<Ingrediente> databaseIngredienti = getDatabaseOggetto(new Ingrediente());
        if (databaseIngredienti.Count > 0)
            if (databaseIngredienti[0].idIngrediente == -1)
            {
                databaseIngredienti.RemoveAt(0);
                Serializza.salvaOggettiSuFile(databaseIngredienti);
            }

        List<Patologia> databasePatologie = getDatabaseOggetto(new Patologia());
        if (databasePatologie.Count > 0)
            if (databasePatologie[0].idPatologia == -1)
            {
                databasePatologie.RemoveAt(0);
                Serializza.salvaOggettiSuFile(databasePatologie);
            }
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