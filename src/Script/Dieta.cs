public class Dieta
{
    public string nome = "";
    public string descrizione = "";

    public Dieta(string nome, string descrizione)
    {
        this.nome = nome;
        this.descrizione = descrizione;
    }

    public string getNewNomeDietaFromUtente (string output){
        Console.WriteLine (output);
        return Console.ReadLine();
    }
    
    public string getNewDescrizioneDietaFromUtente (string output){
        Console.WriteLine ();
        return Console.ReadLine();
    }
    
    public static int getNewDietaFromUtente (string output){
        string dietaTemp;
        int dietaTempInt;
        Console.WriteLine ();
        dietaTemp = Console.ReadLine();
        try{
            dietaTempInt = this.dietaStringToIdDieta(dietaTemp);
            return dietaTempInt;
        }
        catch (InvalidOperationException e){
            Console.WriteLine (e.Message);
        }
    }

     public int dietaStringToIdDieta (string dieta){
        if (dieta.ToLower () == "vegana")
            return 0;
        else if (dieta.ToLower () == "vegetariana")
            return 1;
        else if (dieta.ToLower () == "onnivora")
            return 2;
        else
            throw new InvalidOperationException ("Dieta inserita non valida");
    } 

    ~Dieta()
    {
        
    }
}