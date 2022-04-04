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
        return convertiListaPatologieStringToIdPatologia (patologieInput);
    }

    private static List <int> convertiListaPatologieStringToIdPatologia (List <string> listaPatolgie){
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

    /* TODO commentato perché non finito
    public static List <Patologia> idToPatologia(int idPatologia) 
    {
        int max; //max che dovrà diventare della dimensione delle patologie
        List<Patologia> temp = new List<Patologia>();
        List<Patologia> pato = new List<Patologia>();   //variabile contenente tutte le patologie
        for(int i = 0; i<max; i++) 
        {
            if (idPatologia = pato.idPatologia) {
                temp.Add(pato.Get(i));
            }
            pato.Next();
        }
        return temp;
    }
    */
}