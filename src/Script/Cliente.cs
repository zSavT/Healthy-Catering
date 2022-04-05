public class Cliente
{
    public string nome = "";
    
    public int dieta = 0;
    public List <int> listaIdPatologie = null;

    //costruttore
    public Cliente(string nome, int dieta, List<int> listaIdPatologie)
    {
        this.nome = nome;
        this.dieta = dieta;
        this.listaIdPatologie = listaIdPatologie;
    }

    public Cliente (){
        this.nome = "";
        this.dieta = -1;
        this.listaIdPatologie = new List <int> ();
    }

    public string getNewNomeClienteFromUtente (string output){
        Console.WriteLine (output);
        return Console.ReadLine();
    }

    //distruttore
    ~Cliente() 
    {
    
    }
}