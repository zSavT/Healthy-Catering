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

    public static string listIdToListPatologie(List<int> ids)
    {
        string patologieString = "";

        if (ids.Count > 0)
        { 
            foreach (int id in ids)
            {
                Patologia temp = idToPatologia(id);
                if (temp.idPatologia != -1)
                    patologieString += "\n\t" + temp.nome + "\n";
            }
        }

        return patologieString;
    }

    private static Patologia idToPatologia(int id)
    {
        if (id == -1)
            return new Patologia();

        foreach (Patologia patologia in Costanti.databasePatologie)
            if (id == patologia.idPatologia)
                return patologia;

        throw new Exception("Patologia non trovata idToPatologia");
    }

    private List<Patologia> idListToPatologieList(List<int> idList)
    {
        idList = idList.Distinct().ToList(); //rimuove eventuali duplicati
        List<Patologia> listaPatologie = new List<Patologia>();

        foreach (int id in idList)
            foreach (Patologia patologia in Costanti.databasePatologie)
                if (id == patologia.idPatologia)
                    listaPatologie.Add(patologia);

        return listaPatologie;
    }

    public static List<int> getListIdTutteLePatologie()
    {
        List<int> output = new List<int>();

        foreach (Patologia patologia in Costanti.databasePatologie)
            output.Add(patologia.idPatologia);

        return output;
    }
}