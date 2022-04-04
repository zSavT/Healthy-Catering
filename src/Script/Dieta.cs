public class Dieta
{
    public string nome = "";
    public string descrizione = "";

    public Dieta(string nome, string descrizione)
    {
        this.nome = nome;
        this.descrizione = descrizione;
    }
    
    public static int getNewDietaFromUtente (string output){
        string dietaTemp;
        int dietaTempInt = -1;
        Console.WriteLine (output);
        dietaTemp = Console.ReadLine();
        try{
            dietaTempInt = Dieta.dietaStringToIdDieta(dietaTemp);
        }
        catch (InvalidOperationException e){
            Console.WriteLine (e.Message);
        }
        return dietaTempInt;
    }

    public static int dietaStringToIdDieta (string dieta){
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