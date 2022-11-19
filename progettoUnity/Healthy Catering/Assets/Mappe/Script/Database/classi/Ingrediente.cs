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

    public override int GetHashCode()
    {
        return base.GetHashCode();
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
            throw new InvalidOperationException("Nutriscore inserito non valido");
    }

    public string getListaPiattiRealizzabiliConIngredienteToSingolaString()
    {
        List<Piatto> ricettePossibiliConIngrediente = getListaPiattiRealizzabiliConIngrediente();

        string output = "";
        foreach (Piatto piatto in ricettePossibiliConIngrediente)
        {
            output += piatto.nome + "\n";
        }
        return output;
    }


    public List<Piatto> getListaPiattiRealizzabiliConIngrediente()
    {
        List<Piatto> ricettePossibiliConIngrediente = new List<Piatto>();

        foreach (Piatto piatto in Costanti.databasePiatti)
        {
            List<Ingrediente> ingredientiPiatto = piatto.getIngredientiPiatto();
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

    public static Ingrediente idToIngrediente(int id)
    {
        if (id == -1)
            return new Ingrediente();

        foreach (Ingrediente ingrediente in Costanti.databaseIngredienti)
        {
            if (id == ingrediente.idIngrediente)
            {
                return ingrediente;
            }
        }

        throw new Exception("Ingrediente non trovato idToIngrediente " + id.ToString());
    }

    public static string listIngredientiToStringa(List<Ingrediente> ingredienti)
    {
        string output = "";
        foreach (Ingrediente ingrediente in ingredienti)
        {
            output += ingrediente.nome + "\n";
        }
        return output;
    }
}