public class OggettoQuantita <Oggetto>
{
    Oggetto oggetto;//TODO non si fa cosi: = null;

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