public class OggettoQuantita <Oggetto>
{
    public Oggetto oggetto;//TODO non si fa cosi: = null;

    public int quantita;

    public OggettoQuantita(Oggetto oggetto, int quantita)
    {
        this.oggetto = oggetto;
        this.quantita = quantita;
    }

    ~OggettoQuantita()
    {
        
    }
    
}