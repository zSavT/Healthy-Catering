public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    public float costo = 0;
    public float costoEco = 0;
    public int nutriScore = 0; //media fra i nutriscore degli ingredienti ma approssimata per difetto all'intero più vicino

    List <OggettoQuantita <Ingrediente>> ingredienti = null;

    public Piatto(string nome, string descrizione, float costo, float costoEco, int nutriScore, List<OggettoQuantita<Ingrediente>> ingredienti)
    {
        this.nome = nome;
        this.descrizione = descrizione;
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.ingredienti = ingredienti;
    }

    ~Piatto()
    {
        
    }

    //TODO metodo che trova patologie compatibili in base agli ingredienti
    //TODO metodo che trova diete compatibili in base agli ingredienti


}