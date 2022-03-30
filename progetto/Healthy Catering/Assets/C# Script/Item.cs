class Item
{
    protected int idItem = -1;

    protected string nome = "";
    protected string descrizione = "";

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