using System;
using System.Linq;
using System.Collections.Generic;

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
            && (this.descrizione.Equals(((Patologia)obj).descrizione));
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


    private static List<int> convertiListaPatologieStringToListaIdPatologia(List<string> listaPatolgie)
    {
        List<int> patologieConvertite = new List<int>();

        foreach (string patologiaString in listaPatolgie)
        {
            try
            {
                patologieConvertite.Add(patologiaStringToIdPatologia(patologiaString));
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return patologieConvertite;
    }

    private static int patologiaStringToIdPatologia(string patologia)
    {
        if (patologia == "diabete")
            return 0;
        //TODO aggiungere altre patologie
        else
            throw new InvalidOperationException("Nome patologia non valido");
    }

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
        } else
            idsString = "nessuna patologia";
        return idsString;
    }

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


    public static List<int> getListIdTutteLePatologie(List<Patologia> databasePatologie = null)
    {
        databasePatologie ??= Costanti.databasePatologie;

        List<int> output = new List<int>();

        foreach (Patologia patologia in databasePatologie)
            output.Add(patologia.idPatologia);

        return output;
    }
}