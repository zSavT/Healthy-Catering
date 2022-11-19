using System;
using System.Collections.Generic;

public class Database
{
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

    public static Player getPlayerDaNome(string nomePlayer)
    {
        foreach (Player player in Costanti.databasePlayer)
        {
            if (nomePlayer == player.nome)
            {
                Player temp = player;
                return temp;
            }
        }
        throw new Exception("Non è stato trovato nessuno Player con il nome: " + nomePlayer);
    }
}