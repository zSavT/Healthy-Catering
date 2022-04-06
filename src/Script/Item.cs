public class Item
{
    public  int idItem = -1;

    public  string nome = "";
    public  string descrizione = "";

    public Item(int idItem, string nome, string descrizione)
    {
        this.idItem = idItem;
        this.nome = nome;
        this.descrizione = descrizione;
    }

    public Item (){
        this.idItem = -1;
        this.nome = "";
        this.descrizione = "";
    }

    public Item (string nome):base (){
        this.idItem = getNewIdDatabaseItem (this); 
        this.nome = nome;
    }

    public static int getNewIdDatabaseItem (Item oggetto){
        List <Item> databaseOggetto = Database.getDatabaseOggetto (oggetto);
        return databaseOggetto [databaseOggetto.Count - 1].idItem + 1;
    }

    public static Item creaNuovoItem (string nome = null, string tipoItem = null){
        tipoItem ??= getTipoItemFromUtente ();
        nome ??= Database.getNewStringaFromUtente ("Inserisci il nome dell'item che vuoi aggiungere");

        if (tipoItem.Equals ("ingrediente")){
            Database.aggiungiIngrediente (new Ingrediente (nome));
            return Database.getUltimoOggettoAggiuntoAlDatabase (new Ingrediente ());
        }
        else{
            Item nuovoItem = getNewItemGenerico (nome);
            Database.salvaNuovoOggettoSuFile (nuovoItem);
            return nuovoItem;
        }
    }

    private static Item getNewItemGenerico (string nome){
        Item output = new Item (nome);
        while (output.descrizione.Equals("")){
            output.descrizione = Database.getNewStringaFromUtente ("Inserisci la descrizione dell'item " + nome);
        }

        return output;
    }

    private static string getTipoItemFromUtente (){
        Console.WriteLine ("Che tipo di Item vuoi aggiungere ('ingrediente', 'item generico')");
        string input = "";
        while (true){
            input = Console.ReadLine ();
            if ((input.ToLower ().Equals ("ingrediente")) || (input.ToLower ().Equals ("item generico")))
                return input;
            Console.WriteLine ("Non hai inserito un input valido");
        }
    }

    ~Item()
    {
        
    }
}