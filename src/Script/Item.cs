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

    public static int getNewIdDatabaseItem (Item oggetto){
        List <Item> databaseOggetto = Database.getDatabaseOggetto (oggetto);
        return databaseOggetto [databaseOggetto.Count - 1].idItem + 1;
    }

    ~Item()
    {
        
    }
}