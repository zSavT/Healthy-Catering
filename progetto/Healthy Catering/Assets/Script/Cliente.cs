public class Cliente
{
    string nome = "";
    
    int dieta = 0;
    List <int> listaIdPatologie = null;

    //costruttore
    public Cliente(string nome, int dieta, List<int> listaIdPatologie)
    {
        this.nome = nome;
        this.dieta = dieta;
        this.listaIdPatologie = listaIdPatologie;
    }

    //distruttore
    ~Cliente() 
    {
    
    }
}