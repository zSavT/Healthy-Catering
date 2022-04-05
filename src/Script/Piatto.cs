public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    private float costo = 0;
    private float costoEco = 0;
    private int nutriScore = 0; //media fra i nutriscore degli ingredienti ma approssimata per difetto all'intero più vicino
    //                          int al posto di ingredienti perché sono gli id degli ingredienti
    public List <OggettoQuantita <int>> idIngredienti = null;

    public Piatto(string nome, string descrizione, float costo, float costoEco, int nutriScore, List<OggettoQuantita<int>> idIngredienti)
    {
        this.nome = nome;
        this.descrizione = descrizione;
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.idIngredienti = idIngredienti;
    }

    public Piatto (){
        this.nome = "";
        this.descrizione = "";
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.idIngredienti = new List<OggettoQuantita<int>> ();
    }

    ~Piatto()
    {
        
    }

    //TODO metodo che trova patologie compatibili in base agli ingredienti
    //TODO metodo che trova diete compatibili in base agli ingredienti


}