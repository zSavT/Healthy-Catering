using Internal;
using System;
public class Ingrediente : Item
{
    public float costo = 0;
    public int costoEco = 0;
    public int nutriScore = 0;
    public int dieta = -1;

    public List <int> listaIdPatologieCompatibili = null;

    //base sarebbe, più o meno, il super di java
    public Ingrediente(
        int idItem, string nome, string descrizione,
        float costo, int costoEco, int nutriScore, int dieta, List <int> listaIdPatologieCompatibili) 
        : base (idItem, nome, descrizione)
    {
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.dieta = dieta;
        this.listaIdPatologieCompatibili = listaIdPatologieCompatibili;
    }

    public static Tipo getNewNumeroIngredienteFromUtente <Tipo> (string output, string outputError){
        bool numeroValido = false
        while (!(numeroValido)){
            Console.WriteLine (output);
            try{
                Tipo temp = Tipo.Parse (Console.ReadLine ());
                return temp;
            }
            catch (Exception e){
                Console.WriteLine(e.Message + "\n" + outputError);
            }
        }
    }

    ~Ingrediente()
    {
        
    }
}