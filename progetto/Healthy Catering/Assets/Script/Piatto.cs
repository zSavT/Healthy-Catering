public class Piatto
{
    string nome = "";
    string descrizione = "";

    float costo = 0;
    float costoEco = 0;
    int nutriScore = 0; //media fra i nutriscore degli ingredienti ma approssimata per difetto all'intero più vicino

    List <OggettoQuantita <Ingrediente>> ingredienti = null;

    //TODO metodo che trova patologie compatibili in base agli ingredienti
    //TODO metodo che trova diete compatibili in base agli ingredienti
}