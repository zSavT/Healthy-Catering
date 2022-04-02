public class Ingrediente : Item
{
    float costo = 0;
    int costoEco = 0;
    int nutriScore = 0;
    int dieta = 0;

    //base sarebbe, più o meno, il super di java
    public Ingrediente(
        int idItem, string nome, string descrizione, float costo, 
        int costoEco, int nutriScore) 
        : base (idItem, nome, descrizione)
    {
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
    }

    ~Ingrediente()
    {
        
    }
}