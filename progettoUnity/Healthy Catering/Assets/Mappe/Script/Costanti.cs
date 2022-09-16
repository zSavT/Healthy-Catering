using System.Collections.Generic;

public class Costanti
{
    // DATABASE OGGETTI
    public static readonly List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
    
    public static readonly List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto());

    public static readonly List<Player> databasePlayer = Database.getDatabaseOggetto (new Player());

    public static readonly List <Patologia> databasePatologie = Database.getDatabaseOggetto(new Patologia());

    // INVENTARI LIVELLI
    public static readonly List<OggettoQuantita<int>> inventarioLivello0 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (12,10),
        new OggettoQuantita<int> (15,10),
        new OggettoQuantita<int> (0,10),
        new OggettoQuantita<int> (18,10),
        new OggettoQuantita<int> (16,10),
        new OggettoQuantita<int> (46,10)
    };

    public static readonly List<OggettoQuantita<int>> inventarioLivello05 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2)
    };

    public static readonly List<OggettoQuantita<int>> inventarioLivello1 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2),
        new OggettoQuantita<int> (12,1),
        new OggettoQuantita<int> (15,1),
        new OggettoQuantita<int> (0,1),
        new OggettoQuantita<int> (18,1),
        new OggettoQuantita<int> (16,1),
        new OggettoQuantita<int> (46,1)
    };

    public static readonly List<OggettoQuantita<int>> inventarioLivello2 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2),
        new OggettoQuantita<int> (12,2),
        new OggettoQuantita<int> (15,1),
        new OggettoQuantita<int> (0,1),
        new OggettoQuantita<int> (18,1),
        new OggettoQuantita<int> (16,1),
        new OggettoQuantita<int> (46,1)
    };

    public static readonly List<OggettoQuantita<int>> inventarioTest = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (0,10),
        new OggettoQuantita<int> (34,10),
        new OggettoQuantita<int> (33,10)
    };

    // INDENTAZIONE COLORI E CSS GENERALE
    public static readonly string fineColore = "</color>";
    public static readonly string colorePiatti = "<color=#FFA64C>";
    public static readonly string coloreIngredienti = "<color=#ffcc66>";
    public static readonly string coloreDieta = "<color=#64568c>";
    public static readonly string colorePatologia = "<color=#009082>";
    public static readonly string coloreVerde = "<color=#B6D89C>";
    public static readonly string grassetto = "<b>";
    public static readonly string fineGrassetto = "</b>";
    
    public static readonly string coloreInizio = "<color=#";

    // TUTORIAL
    public static readonly List<string> scritteZio = new List<string>
    {
        "Eccoti qua! Questo edificio è il nostro <color=#B5D99C>ristorante</color>, o meglio, il tuo.",
        "Spero che questo lavoro ti piacerà! E ricordati, sii sempre garbatƏ con i <color=#B5D99C>clienti</color>. Sapranno ricompensarti."
    };

    // ALTRO
    public static readonly int numeroLivelli = 3;

    public static readonly int percentualeGuadagnoSulPiatto = 10;

}
