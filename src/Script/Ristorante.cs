public class Ristorante
{
    public string nome = "";

    public int punteggio = -1;

    public List<OggettoQuantita<int>> magazzinoIngredienti = new List<OggettoQuantita<int>>();

    public Ristorante(string nome, int punteggio, List<OggettoQuantita<int>> magazzinoIngredienti)
    {
        this.nome = nome;
        this.punteggio = punteggio;
        this.magazzinoIngredienti = magazzinoIngredienti;
    }

    public Ristorante()
    {
        this.nome = "";
        this.punteggio = -1;
        this.magazzinoIngredienti = new List<OggettoQuantita<int>>();
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Ristorante))
        {
            return false;
        }
        return (this.nome.Equals(((Ristorante)obj).nome))
            && (this.punteggio == ((Ristorante)obj).punteggio)
            && OggettoQuantita<int>.listeIdQuantitaUguali(this.magazzinoIngredienti, ((Ristorante)obj).magazzinoIngredienti);
    }

    public override string ToString()
    {
        string listaIdItemString = "";

        if (this.magazzinoIngredienti.Count > 0)
        {
            //se non lo prendo prima viene ricreato ogni volta che viene chiamato il metodo idToIngrediente
            List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
            foreach (int id in magazzinoIngredienti)
            {
                Ingrediente temp = Ingrediente.idToIngrediente(id, databasePatalogie).nome;
                if (temp.idIngrediente != -1)
                    magazzinoIngredientiString = magazzinoIngredientiString + "\n\t" + Ingrediente.idToIngrediente(id).nome + "\n";
            }
        }

        string output = "Ristorante:" + "\n\t" + this.nome + "\n" + "Punteggio:" + "\n\t" + this.punteggio + "\n";

        if (!(magazzinoIngredientiString.Equals("")))
            output = output + "Magazzino ingredienti:" + magazzinoIngredientiString + "\n";

        return output + "Fine Ristorante " + this.nome;
    }

    ~Ristorante()
    {

    }

    public static List<OggettoQuantita<int>> fillMagazzinoIngredienti()
    {
        Console.WriteLine("Il programma avvierà la procedura per avere gli ingredienti e le relative quantità per l'aggiunga di un piatto");
        Console.WriteLine("Questo siccome la procedura è esattamente la stessa");
        Console.WriteLine("Gli ingredienti e le relative quantità verranno comunque ovviamente aggiunte al database del ristorante");

        return Piatto.getmagazzinoIngredietiQuantitaPiattoFromUtente("\"ristorante\"");
    }
}