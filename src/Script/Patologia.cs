public class Patologia
{
    int idPatologia = -1;

    string nome = "";
    string descrizione = "";

    public Patologia(int idPatologia, string nome, string descrizione)
    {
        this.idPatologia = idPatologia;
        this.nome = nome;
        this.descrizione = descrizione;
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