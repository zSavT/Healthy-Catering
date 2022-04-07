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

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Cliente))
        {
            return false;
        }
        return (this.nome.Equals(((Cliente)obj).nome))
            && (this.dieta == ((Cliente)obj).dieta)
            && (Enumerable.SequenceEqual(this.listaIdPatologie, ((Cliente)obj).listaIdPatologie));
    }

    //distruttore
    ~Cliente() 
    {
    
    }
}