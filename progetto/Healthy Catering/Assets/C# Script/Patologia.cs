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
}