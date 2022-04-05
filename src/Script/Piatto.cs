public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    private float costo = 0;
    private float costoEco = 0;
    private int nutriScore = 0; //media fra i nutriscore degli ingredienti ma approssimata per difetto all'intero più vicino
    //                          int al posto di ingredienti perché sono gli id degli ingredienti
    public List <OggettoQuantita <int>> ingredienti = null;

    public Piatto(string nome, string descrizione, float costo, float costoEco, int nutriScore, List<OggettoQuantita<int>> ingredienti)
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