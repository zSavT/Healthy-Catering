using System;
public class Ingrediente : Item
{
    public float costo = 0;
    public int costoEco = 0;
    public int nutriScore = 0;
    public int dieta = -1;

    public List <int> listaIdPatologieCompatibili = null;

    //base sarebbe, più o meno, il super di java
    public Ingrediente(
        int idItem, string nome, string descrizione,
        float costo, int costoEco, int nutriScore, int dieta, List <int> listaIdPatologieCompatibili) 
        : base (idItem, nome, descrizione)
    {
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.dieta = dieta;
        this.listaIdPatologieCompatibili = listaIdPatologieCompatibili;
    }

    public Ingrediente ():base (){
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.dieta = -1;
        this.listaIdPatologieCompatibili = new List <int> ();    
    }

    //                                          chiamata al costruttore vuoto
    public Ingrediente (string nomeIngrediente):this (){
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

    public static Ingrediente checkIngredienteOnonimoGiaPresente (string nomeIngrediente){
        List <Ingrediente> ingredientiConNomeSimileInDatabase = getIngredientiConNomeSimileInDatabase (nomeIngrediente);
        if (ingredientiConNomeSimileInDatabase.Count > 0)
            return scegliIngredienteConNomeSimile (nomeIngrediente, ingredientiConNomeSimileInDatabase);
        else return null;
    }

    public static List <Ingrediente> getIngredientiConNomeSimileInDatabase (string nomeIngrediente, List<Ingrediente> databaseIngredienti = null){
        databaseIngredienti ??= Database.getDatabaseOggetto (new Ingrediente ());
        
        List <Ingrediente> output = new List<Ingrediente> ();
        string nomeIngredientePerConfronto = nomeIngrediente.ToLower ();
        foreach (Ingrediente ingredienteTemp in databaseIngredienti){
            if ((ingredienteTemp.nome.ToLower ().Contains (nomeIngredientePerConfronto))){
                output.Add (ingredienteTemp);
            }
        }

        return output;
    }

    public static Ingrediente scegliIngredienteConNomeSimile (string nomeIngrediente, List <Ingrediente> ingredientiConNomeSimile){
        stampaIngredientiSimiliPerSceltaUtente (nomeIngrediente, ingredientiConNomeSimile);

        string input = Console.ReadLine ();
        int numeroInput;
        try{ 
            numeroInput = Int32.Parse (input);
            return ingredientiConNomeSimile [numeroInput - 1];
        } 
        catch (Exception ex){
            //se non viene inserito un numero (quindi anche se viene inserito 'no')
            return null;
        }
    }

    private static void stampaIngredientiSimiliPerSceltaUtente (string nomeIngrediente, List <Ingrediente> ingredientiConNomeSimile){
        Console.WriteLine ("Il nome dell'ingrediente che hai inserito (" + nomeIngrediente + ") non è stato trovato ma sono stati trovati ingredienti con nomi simili, intendi uno di questi? Inserisci 'no' per uscire da questo menu");
                    
        int i = 1;
        foreach (Ingrediente ingredienteSimile in ingredientiConNomeSimile){
            Console.WriteLine (i.ToString () + ") " + ingredienteSimile.nome);
        }
    }

    public float getNewNumeroIngredienteFromUtente (string output, string outputError){
        bool numeroValido = false;
        float temp = -1;
        while ((!(numeroValido)) && (temp == -1)){
            Console.WriteLine (output);
            try{
                temp = float.Parse (Console.ReadLine ());
                numeroValido = true;
            }
            catch (Exception e){
                Console.WriteLine(e.Message + "\n" + outputError);
            }
        }
        return temp;
    }

    public static Ingrediente idToIngrediente (int id, List <Ingrediente> databaseIngredienti){
        if (id == -1)
            return new Ingrediente ();
        
        databaseIngredienti ??= Database.getDatabaseOggetto (new Ingrediente ());
        
        foreach (Ingrediente ingrediente in databaseIngredienti){
            if (id == ingrediente.idItem){
                return ingrediente;
            }
        }
        
        throw new Exception ("Ingrediente non trovato idToIngrediente");
    }

    ~Ingrediente()
    {
        
    }
}