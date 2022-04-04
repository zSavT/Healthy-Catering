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