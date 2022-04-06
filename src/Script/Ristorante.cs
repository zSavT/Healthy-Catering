public class Ristorante
{
    public string nome = "";
    
    public int punteggio = -1;

    public List <OggettoQuantita<int>> magazzinoIngredienti = new List<OggettoQuantita<int>> ();

    public Ristorante(string nome, int punteggio, List<OggettoQuantita<int>> magazzinoIngredienti)
    {
        this.nome = nome;
        this.punteggio = punteggio;
        this.magazzinoIngredienti = magazzinoIngredienti;
    }

    public Ristorante (){
        this.nome = "";
        this.punteggio = -1;
        this.magazzinoIngredienti = new List<OggettoQuantita<int>> ();
    }

    public static int getNewPunteggioFromUtente (){
        Console.WriteLine ("Inserisci il punteggio del ristorante");
        
        int numeroInput;
        string input;

        input = Console.ReadLine();

        try{
            numeroInput = Int32.Parse(input);
            if (numeroInput >= 0) 
                return numeroInput;
        }
        catch (Exception e){}
        Console.WriteLine ("Il numero inserito non è valido");
        
        return -1;
    }

    public static List<OggettoQuantita<int>> fillMagazzinoIngredienti (){
        Console.WriteLine ("Il programma avvierà la procedura per avere gli ingredienti e le relative quantità per l'aggiunga di un piatto");
        Console.WriteLine ("Questo siccome la procedura è esattamente la stessa");
        Console.WriteLine ("Gli ingredienti e le relative quantità verranno comunque ovviamente aggiunte al database del ristorante");
        
        return Piatto.getListaIdIngredientiQuantitaPiattoFromUtente ("'ristorante'");
    }

    ~Ristorante()
    {
        
    }
}