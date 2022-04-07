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

    public Patologia (){
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

    public static int patologiaStringToIdPatologia (string patologia){
        if (patologia == "diabete")
            return 0;
        else
            throw new InvalidOperationException ("Nome patologia non valido");
    }

    public static List <int> getNewListaIdPatologieFromUtente (string output){
        List <string> patologieInput = Patologia.fillListaPatologieStringhe (output);
        //returna una lista vuota di interi (id) se la lista data dall'utente è vuota
        if (patologieInput.Count == 0){
            return new List <int> ();
        }
        return convertiListaPatologieStringToListaIdPatologia (patologieInput);
    }

    private static List <int> convertiListaPatologieStringToListaIdPatologia (List <string> listaPatolgie){
        List <int> patologieConvertite = new List <int> ();
        int patologiaConvertitaTemp;
        foreach (string patologiaString in listaPatolgie){
            try{
                patologiaConvertitaTemp = Patologia.patologiaStringToIdPatologia (patologiaString);
                patologieConvertite.Add (patologiaConvertitaTemp);
            }
            catch (InvalidOperationException e){
                Console.WriteLine (e.Message);
            }
        }
        return patologieConvertite;
    }

    private static List <string> fillListaPatologieStringhe (string output){
        Console.WriteLine (output);
        
        string patologiaTemp = "";
        List <string> patologieInput = new List <string> ();
        
        while (true){
            patologiaTemp = Console.ReadLine ();
            if (patologiaTemp.Equals("no") || patologiaTemp.Equals ("fine"))
                break;
            patologieInput.Add (patologiaTemp);
        }

        return patologieInput;
    }

    public static int getNewIdDatabasePatologia (Patologia oggetto){
        List <Patologia> databaseOggetto = Database.getDatabaseOggetto (oggetto);
        return databaseOggetto [databaseOggetto.Count - 1].idPatologia + 1;
    }

    ~Patologia()
    {
        
    }

    public static Patologia idToPatologia (int id, List <Patologia> databasePatalogie = null){
        if (id == -1)
            return new Patologia();

        databasePatologie ??= Database.getDatabaseOggetto (new Patologia ());
        
        foreach (Patologia patologia in databasePatologie){
            if (id == patologia.idPatologia)
                return patologia;
        }

        throw new Exception ("Patologia non trovata idToPatologia");
    }

    private static List <Patologia> idListToPatologieList (List <int> idList){
        idList = idList.Distinct().ToList(); //rimuove eventuali duplicati
        List <Patologia> databasePatologie = Database.getDatabaseOggetto (new Patologia ());
        List <Patologia> listaPatologie = new List <Patologia> ();
        foreach (int id in idList){
            foreach (Patologia patologia in databasePatologie){
                if (id == patologia.idPatologia){
                    listaPatologie.Add (patologia);
                }
            }
        }
        return listaPatologie;
    }

    public static List <int> getListIdTutteLePatologie (){
        List <Patologia> databasePatologie = Database.getDatabaseOggetto (new Patologia ());
        List <int> output = new List <int> ();
        foreach (Patologia patologia in databasePatologie){
            output.Add (patologia.idPatologia);
        }
        return output;
    }
}