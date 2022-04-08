using System;
public class Ingrediente : Item
{
    public float costo = 0;
    public int costoEco = 0;
    public int nutriScore = 0;
    public int dieta = -1;

    public List<int> listaIdPatologieCompatibili = null;

    //base sarebbe, più o meno, il super di java
    public Ingrediente(
        int idItem, string nome, string descrizione,
        float costo, int costoEco, int nutriScore, int dieta, List<int> listaIdPatologieCompatibili)
        : base(idItem, nome, descrizione)
    {
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.dieta = dieta;
        this.listaIdPatologieCompatibili = listaIdPatologieCompatibili;
    }

    public Ingrediente() : base()
    {
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.dieta = -1;
        this.listaIdPatologieCompatibili = new List<int>();
    }

    public Ingrediente(string nomeIngrediente) : this()
    { //this () = chiamata al costruttore vuoto
        this.nome = nomeIngrediente;
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Ingrediente))
        {
            return false;
        }
        return (this.nome.Equals(((Ingrediente)obj).nome))
            && (this.descrizione.Equals(((Ingrediente)obj).descrizione))
            && (this.costo == ((Ingrediente)obj).costo)
            && (this.costoEco == ((Ingrediente)obj).costoEco)
            && (this.nutriScore == ((Ingrediente)obj).nutriScore)
            && (this.dieta == ((Ingrediente)obj).dieta)
            && (Enumerable.SequenceEqual(this.listaIdPatologieCompatibili, ((Ingrediente)obj).listaIdPatologieCompatibili));
    }

    public override string ToString()
    {
        string output = "Ingrediente:" + "\n\t" + this.nome + "\n" +
        "Descrizione:" + "\n\t" + this.descrizione + "\n" +
        "Costo:" + "\n\t" + this.costo + "\n" +
        "Costo eco:" + "\n\t" + this.costoEco + "\n" +
        "Nutriscore:" + "\n\t" + this.nutriScore + "\n" +
        "Dieta compatibile:" + "\n\t" + Dieta.IdDietaToDietaString(this.dieta) + "\n" +
        "Patologie compatibili:" + Patologia.listIdToListPatologie(this.listaIdPatologieCompatibili);
    }

    ~Ingrediente()
    {

    }

    private idNutriScoreToString(int id)
    {
        if (id == 0)
            return "A";
        if (id == 1)
            return "B";
        if (id == 2)
            return "C";
        if (id == 3)
            return "D";
        if (id == 4)
            return "E";
        else
            throw new InvalidOperationException("Id nutriscore inserito non valido");
    }

    public static Ingrediente checkIngredienteOnonimoGiaPresente(string nomeIngrediente)
    {
        List<Ingrediente> ingredientiConNomeSimileInDatabase = getIngredientiConNomeSimileInDatabase(nomeIngrediente);
        if (ingredientiConNomeSimileInDatabase.Count > 0)
            return scegliIngredienteConNomeSimile(nomeIngrediente, ingredientiConNomeSimileInDatabase);
        else return null;
    }

    public static List<Ingrediente> getIngredientiConNomeSimileInDatabase(string nomeIngrediente, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente());

        List<Ingrediente> output = new List<Ingrediente>();
        foreach (Ingrediente ingredienteTemp in databaseIngredienti)
        {
            if ((ingredienteTemp.nome.ToLower().Contains(nomeIngrediente.ToLower())))
            {
                output.Add(ingredienteTemp);
            }
        }

        return output;
    }

    public static Ingrediente scegliIngredienteConNomeSimile(string nomeIngrediente, List<Ingrediente> ingredientiConNomeSimile)
    {
        int numero = -1;
        while ((numero < 0) || (numero >= ingredientiConNomeSimile.Count))
        {
            numero = Database.getNewIntFromUtente(getStampaIngredientiSimiliPerSceltaUtente(nomeIngrediente, ingredientiConNomeSimile));
            if (numero == 0)
                return null;
        }
        return ingredientiConNomeSimile[numero - 1];
    }

    private static void stampaIngredientiSimiliPerSceltaUtente(string nomeIngrediente, List<Ingrediente> ingredientiConNomeSimile)
    {
        string output = "Il nome dell'ingrediente che hai inserito (" + nomeIngrediente + ") non è stato trovato ma sono stati trovati ingredienti con nomi simili, intendi uno di questi? Inserisci '0' per uscire da questo menu";

        int i = 1;
        foreach (Ingrediente ingredienteSimile in ingredientiConNomeSimile)
            output = "\n" + i.ToString() + ") " + ingredienteSimile.nome;

        return output;
    }

    public static Ingrediente idToIngrediente(int id, List<Ingrediente> databaseIngredienti)
    {
        if (id == -1)
            return new Ingrediente();

        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente());

        foreach (Ingrediente ingrediente in databaseIngredienti)
        {
            if (id == ingrediente.idItem)
            {
                return ingrediente;
            }
        }

        throw new Exception("Ingrediente non trovato idToIngrediente");
    }
}