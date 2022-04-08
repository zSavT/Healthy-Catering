using System.Collections.Generic;
public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    private float costo = 0;
    private float costoEco = 0;
    private int nutriScore = 0; //media fra i nutriScore degli ingredienti ma approssimata per difetto all'intero più vicino

    //                          int al posto di ingredienti perché sono gli id degli ingredienti
    public List<OggettoQuantita<int>> listaIdIngredientiQuantita = null;

    private int percentualeGuadagnoSulPiatto = 10;

    public Piatto(string nome, string descrizione, List<OggettoQuantita<int>> listaIdIngredientiQuantita)
    {
        this.nome = nome;
        this.descrizione = descrizione;
        this.listaIdIngredientiQuantita = listaIdIngredientiQuantita;
        this.costo = calcolaCosto();
        this.costoEco = calcolaCostoEco();
        this.nutriScore = calcolaNutriScore();
    }

    public Piatto()
    {
        this.nome = "";
        this.descrizione = "";
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.listaIdIngredientiQuantita = new List<OggettoQuantita<int>>();
    }

    public Piatto(string nomePiatto) : base()
    {
        this.nome = nomePiatto;
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Piatto))
        {
            return false;
        }
        return (this.nome.Equals(((Piatto)obj).nome))
            && (this.descrizione.Equals(((Piatto)obj).descrizione))
            && (this.costo == ((Piatto)obj).costo)
            && (this.costoEco == ((Piatto)obj).costoEco)
            && (this.nutriScore == ((Piatto)obj).nutriScore)
            && OggettoQuantita<int>.listeIdQuantitaUguali(this.listaIdIngredientiQuantita, ((Piatto)obj).listaIdIngredientiQuantita);
    }

    public override string ToString()
    {
        string output = "Piatto:" + "\n\t" + this.nome + "\n" +
        "Descrizione: " + "\n\t" + this.descrizione + "\n" +
        "Costo: " + "\n\t" + this.costo + "\n" +
        "Costo eco: " + "\n\t" + this.costoEco + "\n" +
        "Nutriscore: " + "\n\t" + this.nutriScore + "\n";

        if (listaIdIngredientiQuantita.Count > 0)
        {
            string ingredientiQuantitaString = "";
            //se non lo prendo prima viene ricreato ogni volta che viene chiamato il metodo idToIngrediente
            List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());

            foreach (OggettoQuantita<int> ingredienteQuantita in listaIdIngredientiQuantita)
            {
                Ingrediente temp = Ingrediente.idToIngrediente(ingredienteQuantita.oggetto, databaseIngredienti);
                if (temp.idItem != -1)
                    ingredientiQuantitaString = ingredientiQuantitaString + "\n\t" + temp.nome + " in quantita: " + ingredienteQuantita.quantita.ToString() + "\n";
            }

            output = output + ingredientiQuantitaString;
        }

        return output = output + "Fine piatto " + this.nome;
    }

    ~Piatto()
    {

    }

    public float calcolaCosto()
    {
        float costo = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
            costo = costo + (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto).costo * ingredienteQuantita.quantita);

        return costo + ((costo * percentualeGuadagnoSulPiatto) / 100);
    }

    public float calcolaCostoEco()
    {
        float costoEco = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
            costoEco = costoEco + (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto).costoEco * ingredienteQuantita.quantita);

        return costoEco;
    }

    public int calcolaNutriScore()
    {
        int sommanutriScore = 0;
        int numeroIngredienti = 0;

        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
        {
            sommanutriScore = sommanutriScore + (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto).nutriScore * ingredienteQuantita.quantita);
            numeroIngredienti = numeroIngredienti + ingredienteQuantita.quantita;
        }

        return (int)(sommanutriScore / numeroIngredienti);
    }

    public static Piatto checkPiattoOnonimoGiaPresente(string nomePiatto, List<Piatto> piattiConNomeSimileInDatabase = null)
    {
        piattiConNomeSimileInDatabase ??= getPiattiConNomeSimileInDatabase(nomePiatto);

        if (piattiConNomeSimileInDatabase.Count > 0)
        {
            int scelta = Database.getNewIntFromUtente(getStringaStampaPiattiConNomeSimilePerSceltaUtente(nomePiatto, piattiConNomeSimileInDatabase));
            if (scelta != -1)
                return piattiConNomeSimileInDatabase[scelta - 1];
        }

        return null;
    }

    private static List<Piatto> getPiattiConNomeSimileInDatabase(string nomePiatto, List<Piatto> databasePiatti = null)
    {
        databasePiatti ??= Database.getDatabaseOggetto(new Piatto());

        List<Piatto> output = new List<Piatto>();
        string nomePiattoPerConfronto = nomePiatto.ToLower();

        foreach (Piatto piattoTemp in databasePiatti)
            if ((piattoTemp.nome.ToLower().Contains(nomePiattoPerConfronto)) || (nomePiattoPerConfronto.Contains(piattoTemp.nome.ToLower())))
                output.Add(piattoTemp);

        return output;
    }

    private static string getStringaStampaPiattiConNomeSimilePerSceltaUtente(string nomePiatto, List<Piatto> piattiConNomeSimileInDatabase)
    {
        string output = "Il nome del piatto che hai inserito (" + nomePiatto + ") non è stato trovato ma sono stati trovati piatti con nomi simili, intendi uno di questi? Inserisci 'no' per uscire da questo menu\n";

        int i = 1;
        foreach (Piatto piatto in piattiConNomeSimileInDatabase)
            output = i.ToString() + ") " + piatto.nome + "\n";

        return output;
    }

    public static List<OggettoQuantita<int>> getListaIdIngredientiQuantitaPiattoFromUtente(string nomePiatto, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente());

        List<OggettoQuantita<int>> listaIdIngredientiQuantitaPiatto = new List<OggettoQuantita<int>>();

        List<string> inputUtente = getNomeIngredientiFromUtente(nomePiatto);

        foreach (string nomeIngrediente in inputUtente)
        {
            Ingrediente ingredienteTemp;

            if (nomeIngredientePresenteNelDatabase(nomeIngrediente, databaseIngredienti))
            {
                ingredienteTemp = getIngredienteByNome(nomeIngrediente, databaseIngredienti);
            }
            else
            {
                List<Ingrediente> ingredientiConNomeSimile = Ingrediente.getIngredientiConNomeSimileInDatabase(nomeIngrediente, databaseIngredienti);

                if (ingredientiConNomeSimile.Count > 0)
                {
                    Ingrediente ingredienteScelto = Ingrediente.scegliIngredienteConNomeSimile(nomeIngrediente, ingredientiConNomeSimile);
                    if (ingredienteScelto == null)
                    {
                        Database.aggiungiIngrediente(new Ingrediente(nomeIngrediente));
                        ingredienteTemp = Database.getUltimoOggettoAggiuntoAlDatabase(new Ingrediente());
                    }
                    else
                    {
                        ingredienteTemp = ingredienteScelto;
                    }
                }
                else
                {
                    Database.aggiungiIngrediente(new Ingrediente(nomeIngrediente));
                    ingredienteTemp = Database.getUltimoOggettoAggiuntoAlDatabase(new Ingrediente());
                }
            }

            int quantita = getQuantitaIngredienteNelPiattoFromUtente(ingredienteTemp.nome, nomePiatto);
            listaIdIngredientiQuantitaPiatto.Add(new OggettoQuantita<int>(ingredienteTemp.idItem, quantita));
        }

        return listaIdIngredientiQuantitaPiatto;
    }

    private static List<string> getNomeIngredientiFromUtente(string nomePiatto)
    {
        Console.WriteLine("Inserisci il nome degli ingredienti del piatto " + nomePiatto + " e la keyword 'fine' quando vuoi finire l'inserimento");

        List<string> nomiIngredienti = new List<string>();
        string input = "";

        while (true)
        {
            input = Console.ReadLine();
            if (input.Equals("fine"))
                break;
            nomiIngredienti.Add(input);
        }

        return nomiIngredienti;
    }

    private static bool nomeIngredientePresenteNelDatabase(string nomeIngrediente, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente()); //check se il valore del database è nullo, nel caso la crea

        foreach (Ingrediente ingrediente in databaseIngredienti)
            if (nomeIngrediente.ToLower().Equals(ingrediente.nome.ToLower()))
                return true;

        return false;
    }

    private static Ingrediente getIngredienteByNome(string nomeIngrediente, List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente());

        foreach (Ingrediente ingrediente in databaseIngredienti)
            if (nomeIngrediente.ToLower().Equals(ingrediente.nome.ToLower()))
                return ingrediente;

        throw new Exception("Ingrediente non trovato getIngredienteByNome");
    }

    private static int getQuantitaIngredienteNelPiattoFromUtente(string nomeIngrediente, string nomePiatto)
    {
        while (true)
        {
            int numero = Database.getNewIntFromUtente("Qual'è la quantita di " + nomeIngrediente + " nel piatto " + nomePiatto);

            if (numero > 0)
                return numero;
        }
    }

    private List<int> getPatologieCompatibili()
    {
        List<Ingrediente> ingredientiPiatto = this.getIngredientiPiatto();
        List<int> IdtutteLePatologie = Patologia.getListIdTutteLePatologie();

        foreach (Ingrediente ingrediente in ingredientiPiatto)
            foreach (int id in IdtutteLePatologie)
                if (!(ingrediente.listaIdPatologieCompatibili.Contains(id)))
                    IdtutteLePatologie.Remove(id);

        return IdtutteLePatologie;
    }

    private int getDietaMinimaCompatibile()
    {
        List<Ingrediente> ingredientiPiatto = this.getIngredientiPiatto();
        int output = -1;

        foreach (Ingrediente ingrediente in ingredientiPiatto)
            if (output < ingrediente.dieta)
                output = ingrediente.dieta;

        return output;
    }

    private List<Ingrediente> getIngredientiPiatto(List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Database.getDatabaseOggetto(new Ingrediente());

        List<Ingrediente> ingredientiPiatto = new List<Ingrediente>();

        int i = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
        {
            foreach (Ingrediente ingrediente in databaseIngredienti)
                if (ingredienteQuantita.oggetto == ingrediente.idItem)
                    ingredientiPiatto.Add(ingrediente);
            i++;
        }

        return ingredientiPiatto;
    }
}