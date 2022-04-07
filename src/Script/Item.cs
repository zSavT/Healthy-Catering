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

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Item))
        {
            return false;
        }
        return (this.nome.Equals(((Ingrediente)obj).nome))
            && (this.descrizione.Equals(((Ingrediente)obj).descrizione));
    }

    public static int getNewIdDatabaseItem (Item oggetto){
        List <Item> databaseOggetto = Database.getDatabaseOggetto (oggetto);
        return databaseOggetto [databaseOggetto.Count - 1].idItem + 1;
    }

    public static Item creaNuovoItem (string nome = null /*@Deprecated ,string tipoItem = null*/){
        //tipoItem ??= getTipoItemFromUtente (); // @Deprecated 
        nome ??= Database.getNewStringaFromUtente ("Inserisci il nome dell'item che vuoi aggiungere");

        //if (tipoItem.Equals ("ingrediente")){ //@Deprecated 
            Database.aggiungiIngrediente (new Ingrediente (nome));
            return Database.getUltimoOggettoAggiuntoAlDatabase (new Ingrediente ());
        /*//@Deprecated 
        }
        else{
            Item nuovoItem = getNewItemGenerico (nome);
            Database.salvaNuovoOggettoSuFile (nuovoItem);
            return nuovoItem;
        }
        */
    }

    //@Deprecated 
    /*
    private static Item getNewItemGenerico (string nome){
        Item output = new Item (nome);
        while (output.descrizione.Equals("")){
            output.descrizione = Database.getNewStringaFromUtente ("Inserisci la descrizione dell'item " + nome);
        }

        return output;
    }
    */

    //@Deprecated 
    /*
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
    */

    ~Item()
    {
        
    }
}