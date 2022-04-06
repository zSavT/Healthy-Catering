public class OggettoQuantita <Oggetto>
{
    public Oggetto oggetto;

    public int quantita;

    public OggettoQuantita(Oggetto oggetto, int quantita)
    {
        this.oggetto = oggetto;
        this.quantita = quantita;
    }

    public OggettoQuantita (){
        /*
        Can we have a generic constructor? No, generic constructors are not allowed. 
        Which means that you cannot define the parameter T on the constructor itself.
        https://www.codingame.com/playgrounds/2290/demystifying-c-generics/generics-classes
        */
    }

    ~OggettoQuantita()
    {
        
    }
    
}