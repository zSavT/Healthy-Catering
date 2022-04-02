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

    ~Item()
    {
        
    }
}