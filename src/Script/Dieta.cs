public class Dieta
{
    public string nome = "";
    public string descrizione = "";

    public Dieta(string nome, string descrizione)
    {
        this.nome = nome;
        this.descrizione = descrizione;
    }

    public void getNewNomeDietaFromUtente (){
        Console.WriteLine ("Inserisci il nome della dieta");
        this.nome = Console.ReadLine();
    }
    
    public void getNewDescrizioneDietaFromUtente (){
        Console.WriteLine ("Inserisci la descrizione della dieta");
        this.descrizione = Console.ReadLine();
    }
    
    ~Dieta()
    {
        
    }
}