using System.Reflection.Emit;
public class Player
{
    public string nome = "";
    
    public float soldi = 0;

    public List <OggettoQuantita<Item>> inventario = null;

    public Player(string nome, int soldi, List<OggettoQuantita<Item>> inventario)
    {
        this.nome = nome;
        this.soldi = soldi;
        this.inventario = inventario;
    }

    public Player (){
        this.nome = "";
        this.soldi = -1;
        this.inventario = new List<OggettoQuantita<Item>> ();
    }

    ~Player()
    {
        
    }
    
}