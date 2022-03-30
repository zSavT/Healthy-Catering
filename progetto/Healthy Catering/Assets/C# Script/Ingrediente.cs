public class Ingrediente : Item
{
    float costo = 0;
    int costoEco = 0;
    int nutriScore = 0;

    public Ingrediente(float costo, int costoEco, int nutriScore)
    {
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
    }

    ~Ingrediente()
    {
        
    }
}