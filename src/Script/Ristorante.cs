public class Ristorante
{
    string nome = "";
    
    int punteggio = 0;

    List <OggettoQuantita<Ingrediente>> magazzinoIngredienti = null;

    public Ristorante(string nome, int punteggio, List<OggettoQuantita<Ingrediente>> magazzinoIngredienti)
    {
        this.nome = nome;
        this.punteggio = punteggio;
        this.magazzinoIngredienti = magazzinoIngredienti;
    }

    ~Ristorante()
    {
        
    }
}