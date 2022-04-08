using System.Collections.Generic;
using System;
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

    public override string ToString()
    {
        return "Patologia:" + "\n\t" + this.nome + "\n" + "Descrizione: " + "\n\t" + this.descrizione + "\n" + "Fine patologia " + this.nome;
    }

    ~Patologia()
    {

    }

    public static List<int> getNewListaIdPatologieFromUtente(string output)
    {
        List<string> patologieInput = fillListaPatologieStringhe(output);
        //returna una lista vuota di interi (id) se la lista data dall'utente è vuota
        if (patologieInput.Count == 0)
        {
            return new List<int>();
        }
        return convertiListaPatologieStringToListaIdPatologia(patologieInput);
    }

    private static List<string> fillListaPatologieStringhe(string output)
    {
        Console.WriteLine(output);

        List<string> patologieInput = new List<string>();
        while (true)
        {
            string patologiaTemp = Console.ReadLine();
            if ((patologiaTemp.ToLower().Equals("no")) || (patologiaTemp.ToLower().Equals("fine")))
                break;
            patologieInput.Add(patologiaTemp);
        }

        return patologieInput;
    }

    private static List<int> convertiListaPatologieStringToListaIdPatologia(List<string> listaPatolgie)
    {
        List<int> patologieConvertite = new List<int>();

        foreach (string patologiaString in listaPatolgie)
        {
            try
            {
                patologieConvertite.Add(Patologia.patologiaStringToIdPatologia(patologiaString));
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
            DatabasePatologie ??= Database.getDatabaseOggetto(new Patologia());

            foreach (int id in ids)
            {
                Patologia temp = idToPatologia(id, DatabasePatologie);
                if (temp.idPatologia != -1)
                    idsString = idsString + "\n\t" + temp.nome + "\n";
            }
        }
        return idsString;
    }

    private static Patologia idToPatologia(int id, List<Patologia> databasePatologie = null)
    {
        if (id == -1)
            return new Patologia();

        databasePatologie ??= Database.getDatabaseOggetto(new Patologia());

        foreach (Patologia patologia in databasePatologie)
            if (id == patologia.idPatologia)
                return patologia;

        throw new Exception("Patologia non trovata idToPatologia");
    }

    private List<Patologia> idListToPatologieList(List<int> idList, List<Patologia> databasePatologie)
    {
        databasePatologie ??= Database.getDatabaseOggetto(new Patologia());

        idList = idList.Distinct().ToList(); //rimuove eventuali duplicati
        List<Patologia> listaPatologie = new List<Patologia>();

        foreach (int id in idList)
            foreach (Patologia patologia in databasePatologie)
                if (id == patologia.idPatologia)
                    listaPatologie.Add(patologia);

        return listaPatologie;
    }

    public static List<int> getListIdTutteLePatologie(List<Patologia> databasePatologie = null)
    {
        databasePatologie ??= Database.getDatabaseOggetto(new Patologia());

        List<int> output = new List<int>();

        foreach (Patologia patologia in databasePatologie)
            output.Add(patologia.idPatologia);

        return output;
    }
}