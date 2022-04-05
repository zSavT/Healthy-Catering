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

    public Ingrediente ():base (){
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.dieta = -1;
        this.listaIdPatologieCompatibili = new List <int> ();    
    }

    public float getNewNumeroIngredienteFromUtente (string output, string outputError){
        bool numeroValido = false;
        float temp = -1;
        while ((!(numeroValido)) && (temp == -1)){
            Console.WriteLine (output);
            try{
                temp = float.Parse (Console.ReadLine ());
                numeroValido = true;
            }
            catch (Exception e){
                Console.WriteLine(e.Message + "\n" + outputError);
            }
        }
        return temp;
    }

    ~Ingrediente()
    {
        
    }
}