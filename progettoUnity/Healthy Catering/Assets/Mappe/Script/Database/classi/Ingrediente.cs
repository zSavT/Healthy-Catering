using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem.Editor;

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

    /// <summary>
    /// Il metodo restituisce la lista piatti realizzabili con l'ingrediente
    /// </summary>
    /// <returns>list piatto realizzabili</returns>
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

    /// <summary>
    /// Il metodo restituisce la stringa piatti realizzabili con l'ingrediente
    /// </summary>
    /// <returns>stringa piatti realizzabili</returns>
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

    /// <summary>
    /// Il metodo permette di convertire ID passato in input passato in Ingrediente corrispondente
    /// </summary>
    /// <param name="id">int ID da convertire</param>
    /// <param name="databaseIngredienti">database ingredienti da controllare</param>
    /// <returns>Ingrediente corrispondente all'ID</returns>
    /// <exception cref="Exception">ID ingrediente passato inesistente</exception>
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

    /// <summary>
    /// Il metodo permette di ottenere la stringa corrispondente della lista di ingredienti passata in input formatta
    /// </summary>
    /// <param name="ingredienti">lista ingredienti</param>
    /// <returns>string corrispondente alla lista passata in input</returns>
    public static string listIngredientiToStringa (List <Ingrediente> ingredienti)
    {
        string output = "";
        foreach (Ingrediente ingrediente in ingredienti)
        {
            output += ingrediente.nome + "\n";
        }
        return output;
    }

    /// <summary>
    /// Il metodo controlla se nel database sono presenti ingredienti con lo stesso nome passato in Input
    /// </summary>
    /// <param name="nomeIngrediente">string nome ingrediente da controllare</param>
    /// <returns>booleano True: É presente l'ingrediente, False: Non è presente l'ingrediente</returns>
    public static bool checkIngredienteOnonimoGiaPresente(string nomeIngrediente)
    {
        List<Ingrediente> ingredientiConNomeSimileInDatabase = getIngredientiConNomeUgualeInDatabase(nomeIngrediente);
        if (ingredientiConNomeSimileInDatabase.Count > 0)
            return true;
        else return false;
    }

    /// <summary>
    /// Il metodo restituisce una lista di tutti gli ingredienti con il nome uguale a quello passato in input
    /// </summary>
    /// <param name="nomeIngrediente">string nome ingrediente da controllare</param>
    /// <param name="databaseIngredienti">database ingredienti da controllare</param>
    /// <returns>List<Ingrediente> ingredienti con lo stesso nome</returns>
    public static List<Ingrediente> getIngredientiConNomeUgualeInDatabase(string nomeIngrediente, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Costanti.databaseIngredienti;

        List<Ingrediente> output = new List<Ingrediente>();
        foreach (Ingrediente ingredienteTemp in databaseIngredienti)
        {
            if ((ingredienteTemp.nome.ToLower().Equals(nomeIngrediente.ToLower())))
            {
                output.Add(ingredienteTemp);
            }
        }

        return output;
    }

    /// <summary>
    /// Il metodo permette di ricercare nel database un ingrediente tramite il suo nome ed restituirlo in output
    /// </summary>
    /// <param name="nomeIngredienteDaCercare">string nome dell'ingrediente da ricercare</param>
    /// <param name="databaseIngredienti">database ingrediente dove ricercare l'ingrediente</param>
    /// <returns>Ingrediente ricercato. Se non è stato ritrovato nessun ingrediente, il valore di ritorno è null</returns>
    public static Ingrediente getIngredienteDaNome(string nomeIngredienteDaCercare, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Costanti.databaseIngredienti;
        foreach (Ingrediente ingredienteTemp in databaseIngredienti)
        {
            if ((ingredienteTemp.nome.ToLower().Equals(nomeIngredienteDaCercare.ToLower())))
            {
                return ingredienteTemp;
            }
        }
        return new Ingrediente(100, "B", "L", 1, 1 ,1 ,1, new List<int>());
    }

    //METODO NON UTILIZZATI MA EVENTUALMENTE UTILI

    /// <summary>
    /// Il metodo converte ID dell'ingrediente passato del nustri score passato in input in char
    /// </summary>
    /// <param name="id">int ID ingrediente</param>
    /// <returns>char corrispondente al nutri score</returns>
    /// <exception cref="InvalidOperationException"></exception>
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

}