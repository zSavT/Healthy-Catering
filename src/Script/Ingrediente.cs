public class Ingrediente : Item
{
    public float costo = 0;
    public int costoEco = 0;
    public int nutriScore = 0;
    public int dieta = 0;

    //base sarebbe, più o meno, il super di java
    public Ingrediente(
        int idItem, string nome, string descrizione,
        float costo, int costoEco, int nutriScore, int dieta) 
        : base (idItem, nome, descrizione)
    {
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.dieta = dieta;
    }

    ~Ingrediente()
    {
        
    }
}