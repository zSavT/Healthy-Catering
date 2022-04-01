public class OggettoQuantita <Oggetto>
{
    Oggetto oggetto = null;

    int quantita;

    public OggettoQuantita(Oggetto oggetto, int quantita)
    {
        this.oggetto = oggetto;
        this.quantita = quantita;
    }

    ~OggettoQuantita()
    {
        
    }
    
}