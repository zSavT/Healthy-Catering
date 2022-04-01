using System.Reflection.Emit;
public class Player
{
    string nome = "";
    
    int soldi = 0;

    List <OggettoQuantita<Item>> inventario = null;

    public Player(string nome, int soldi, List<OggettoQuantita<Item>> inventario)
    {
        this.nome = nome;
        this.soldi = soldi;
        this.inventario = inventario;
    }

    ~Player()
    {
        
    }
    
}