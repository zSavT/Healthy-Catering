using System;
using System.Linq;
using System.Collections.Generic;

public class Ingrediente
{
    //ex item
    public int idIngrediente = -1;
    public string nome = "";
    public string descrizione = "";
    //ingrediente
    public float costo = 0;
    public int costoEco = 0;
    public int nutriScore = 0;
    public int dieta = -1;

    public List<int> listaIdPatologieCompatibili = null;

    //base sarebbe, più o meno, il super di java
    public Ingrediente(
        int idIngrediente, string nome, string descrizione,
        float costo, int costoEco, int nutriScore, int dieta, List<int> listaIdPatologieCompatibili)
    {
        this.idIngrediente = idIngrediente;
        this.nome = nome;
        this.descrizione = descrizione;

        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.dieta = dieta;
        this.listaIdPatologieCompatibili = listaIdPatologieCompatibili;
    }

    public Ingrediente()
    {
        this.idIngrediente = -1;
        this.nome = "";
        this.descrizione = "";

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
        return this.descrizione + "\n\n" +
        "Costo: "  + this.costo + "\n" +
        "Costo eco: " + this.costoEco + "\n" +
        "Nutriscore: " + this.nutriScore + "\n";/* +
        "Dieta compatibile:" + "\n\t" + Dieta.IdDietaToDietaString(this.dieta) + "\n" +
        "Patologie compatibili:" + Patologia.listIdToListPatologie(this.listaIdPatologieCompatibili);*/
    }

    ~Ingrediente()
    {

    }

    public static bool ingredienteCompatibilePatologia (int idIngrediente, int idPatologia)
    {
        Ingrediente ingrediente = idToIngrediente(idIngrediente);

        foreach (int id in ingrediente.listaIdPatologieCompatibili)
            if (id == idPatologia)
                return true;

        return false;
    }

    private static char idNutriScoreToString(int id)
    {
        if (id == 0)
            return 'A';
        if (id == 1)
            return 'B';
        if (id == 2)
            return 'C';
        if (id == 3)
            return 'D';
        if (id == 4)
            return 'E';
        else
            throw new InvalidOperationException("Id nutriscore inserito non valido");
    }

    public List<Piatto> getListaPiattiRealizzabiliConIngrediente()
    {
        List<Piatto> ricettePossibiliConIngrediente = new List<Piatto>();

        foreach (Piatto piatto in Costanti.databasePiatti)
        {
            List<Ingrediente> ingredientiPiatto = piatto.getIngredientiPiatto(Costanti.databaseIngredienti);
            foreach (Ingrediente ingredientePiatto in ingredientiPiatto)
            {
                if (this.Equals(ingredientePiatto))
                {
                    ricettePossibiliConIngrediente.Add(piatto);
                }
            }
        }

        return ricettePossibiliConIngrediente;
    }
    public string getListaPiattiRealizzabiliConIngredienteToSingolaString()
    {
        List<Piatto> ricettePossibiliConIngrediente = new List<Piatto>();

        foreach (Piatto piatto in Costanti.databasePiatti)
        {
            List<Ingrediente> ingredientiPiatto = piatto.getIngredientiPiatto(Costanti.databaseIngredienti);
            foreach (Ingrediente ingredientePiatto in ingredientiPiatto)
            {
                if (this.Equals(ingredientePiatto))
                {
                    ricettePossibiliConIngrediente.Add(piatto);
                }
            }
        }

        string output = "";
        foreach (Piatto piatto in ricettePossibiliConIngrediente)
        {
            output += piatto.nome + "\n";
        }
        return output;
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
        databaseIngredienti ??= Costanti.databaseIngredienti;

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

    private static string getStampaIngredientiSimiliPerSceltaUtente(string nomeIngrediente, List<Ingrediente> ingredientiConNomeSimile)
    {
        string output = "Il nome dell'ingrediente che hai inserito (" + nomeIngrediente + ") non è stato trovato ma sono stati trovati ingredienti con nomi simili, intendi uno di questi? Inserisci '0' per uscire da questo menu";

        int i = 1;
        foreach (Ingrediente ingredienteSimile in ingredientiConNomeSimile)
            output = "\n" + i.ToString() + ") " + ingredienteSimile.nome;

        return output;
    }

    public static Ingrediente idToIngrediente(int id, List<Ingrediente> databaseIngredienti = null)
    {
        if (id == -1)
            return new Ingrediente();

        databaseIngredienti ??= Costanti.databaseIngredienti;

        foreach (Ingrediente ingrediente in databaseIngredienti)
        {
            if (id == ingrediente.idIngrediente)
            {
                return ingrediente;
            }
        }

        throw new Exception("Ingrediente non trovato idToIngrediente " + id.ToString());
    }

    public static string listIngredientiToStringa (List <Ingrediente> ingredienti)
    {
        string output = "";
        foreach (Ingrediente ingrediente in ingredienti)
        {
            output += ingrediente.nome + "\n";
        }
        return output;
    }

}