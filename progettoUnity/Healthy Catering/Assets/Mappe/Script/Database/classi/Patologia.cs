using System;
using System.Collections.Generic;
using UnityEngine;

public class Patologia 
{
    public int idPatologia = -1;

    public string nome = "";
    public string descrizione = "";

    public Patologia(int idPatologia, string nome, string descrizione)
    {
        this.idPatologia = idPatologia;
        this.nome = nome;
        this.descrizione = descrizione;
    }

    public Patologia()
    {
        this.idPatologia = -1;
        this.nome = "";
        this.descrizione = "";
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Patologia))
        {
            return false;
        }
        return (this.nome.Equals(((Patologia)obj).nome))
            && (this.descrizione.Equals(((Patologia)obj).descrizione)
            && (this.idPatologia.Equals(((Patologia)obj).idPatologia)));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "Patologia:" + "\n\t" + this.nome + "\n" + "Descrizione: " + "\n\t" + this.descrizione + "\n" + "Fine patologia " + this.nome;
    }

    ~Patologia()
    {

    }

    /// <summary>
    /// Il metodo converte la lista di ID di patologie passata in input in una stringa formatta
    /// </summary>
    /// <param name="ids">lista ID (int) patologie</param>
    /// <param name="DatabasePatologie">database patologie da controllare</param>
    /// <returns>stringa formatta</returns>
    public static string listIdToListPatologie(List<int> ids, List<Patologia> DatabasePatologie = null)
    {
        string idsString = "";

        if (ids.Count > 0)
        {
            //se non lo prendo prima viene ricreato ogni volta che viene chiamato il metodo idToPatologia
            DatabasePatologie ??= Costanti.databasePatologie;

            foreach (int id in ids)
            {
                Patologia temp = idToPatologia(id, DatabasePatologie);
                if (temp.idPatologia != -1)
                    idsString = idsString + temp.nome + "\n";
            }
        }
        else
            idsString = "nessuna patologia";
        return idsString;
    }

    /// <summary>
    /// Il metodo permette di convertire ID passato in input in Patologia
    /// </summary>
    /// <param name="id">int ID da controllare</param>
    /// <param name="databasePatologie">database patologie da controllare</param>
    /// <returns>Patologia corrispondente all'ID passato</returns>
    /// <exception cref="Exception">ID patologia non presente</exception>
    private static Patologia idToPatologia(int id, List<Patologia> databasePatologie = null)
    {
        if (id == -1)
            return new Patologia();

        databasePatologie ??= Costanti.databasePatologie;

        foreach (Patologia patologia in databasePatologie)
            if (id == patologia.idPatologia)
                return patologia;

        throw new Exception("Patologia non trovata idToPatologia");
    }

    /// <summary>
    /// Il metodo permette di ricevere la lista di ID di tutte le patologie esistenti
    /// </summary>
    /// <param name="databasePatologie">database patologia da controllare</param>
    /// <returns>list ID (int) patologie esistenti</returns>
    public static List<int> getListIdTutteLePatologie(List<Patologia> databasePatologie = null)
    {
        databasePatologie ??= Costanti.databasePatologie;

        List<int> output = new List<int>();

        foreach (Patologia patologia in databasePatologie)
            output.Add(patologia.idPatologia);

        return output;
    }

    

    /// <summary>
    /// Il metodo restituisce una lista di stringhe contenente il nome delle patologie presenti nel database
    /// </summary>
    /// <param name="databasePatologie">database patologie</param>
    /// <returns>List di string di patologie nel database</returns>
    public static List<string> getListStringNomePatologie(List<Patologia> databasePatologie)
    {
        List<string> output = new List<string>();
        if (databasePatologie.Count > 0 && databasePatologie != null)
        {
            foreach (Patologia patologia in databasePatologie)
                output.Add(patologia.nome);
        }
        return output;
    }

    /// <summary>
    /// Il metodo restituisce una lista di stringhe contenente il nome delle patologie presenti nel database caricato
    /// </summary>
    /// <returns>List di string di patologie nel database</returns>
    public static List<string> getListStringNomePatologie()
    {
        List<string> output = new List<string>();

        foreach (Patologia patologia in Costanti.databasePatologie)
            output.Add(patologia.nome);

        return output;
    }

    public static Patologia getPatologiaDaID(int id)
    {
        foreach (Patologia patologia in Costanti.databasePatologie)
            if (patologia.idPatologia.Equals(id))
                return patologia;
        return null;
    }

    public static Patologia getPatologiaDaNome(string nome)
    {
        Patologia temp = null;
        foreach (Patologia patologia in Costanti.databasePatologie)
            if (patologia.nome.Equals(nome))
            {
                temp = patologia;
            }

        return temp;
    }
}