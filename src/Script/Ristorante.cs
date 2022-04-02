public class Ristorante
{
    public string nome = "";
    
    public int punteggio = 0;

    public List <OggettoQuantita<Ingrediente>> magazzinoIngredienti = null;

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