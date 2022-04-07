using System.Reflection.Emit;
public class Player
{
    /*
    tutti i metodi e gli attributi e le variabili dichiarate nei metodi di questa classe con il nome 'Item' al loro interno sono in verità Ingredienti (o id di Ingredienti)
    reference: discussione relativa a questa cosa a partire dal commento che inizia con questa stringa:
    "
    @zSavT l'ultimo commit ha un problema bello grande ovvero che quando aggiungo un item al player, siccome può essere sia un Item generico che un Ingrediente non si possono distinguere gli id degli Item e gli id degli ingredienti, in quanto hanno 2 database diversi.
    " 
    nella Pull Request chiamata PR issue#18
    */
    public string nome = "";
    
    public float soldi = 0;

    public List <OggettoQuantita<int>> inventario = new List<OggettoQuantita<int>> ();

    public Player(string nome, int soldi, List<OggettoQuantita<int>> inventario)
    {
        this.nome = nome;
        this.soldi = soldi;
        this.inventario = inventario;
    }

    public Player (){
        this.nome = "";
        this.soldi = -1;
        this.inventario = new List<OggettoQuantita<int>> (); //int perchè sono gli id degli item e non gli item veri e propri
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Player))
        {
            return false;
        }
        return (this.nome.Equals (((Player)obj).nome))
            && (this.soldi == ((Player)obj).soldi)
            && OggettoQuantita<int>.listeIdQuantitaUguali (this.inventario, ((Player)obj).inventario);
    }

    public static List<OggettoQuantita<int>> popolaInventario (List <Item> itemGiaPresenti = null){
        itemGiaPresenti ??= new List<Item> ();

        itemGiaPresenti = aggiungiAltriItem (itemGiaPresenti);

        List <int> quantitaItemGiaPresenti = new List<int> ();

        quantitaItemGiaPresenti = chiediQuantitaItem (itemGiaPresenti);

        return creaInventarioFromListaItemEQuantita (itemGiaPresenti, quantitaItemGiaPresenti);
    }

    private static List <Item> aggiungiAltriItem (List <Item> itemGiaPresenti){
        while (true){
            Console.WriteLine("Inserisci la keyword 'inizia' o la keyword 'continua' per inserire un nuovo item e la parola 'fine' per concludere l'inserimento");
            string input = Console.ReadLine ();
            if (input.Equals ("fine")){
                break;
            }
            itemGiaPresenti.Add(Item.creaNuovoItem ());
        }
        
        return itemGiaPresenti;
    }

    private static List <int> chiediQuantitaItem (List <Item> itemGiaPresenti){
        List <int> quantita = new List<int> ();
        foreach (Item item in itemGiaPresenti){
            int numeroInput = -1;
            Console.WriteLine ("Quanti " + item.nome + " devono essere presenti nell'inventario?");
            while (true){
                string input = Console.ReadLine();
                try{
                    numeroInput = Int32.Parse (input);
                    if (numeroInput >= 0){
                        quantita.Add (numeroInput);
                        break;
                    } 
                }
                catch (Exception e){
                    Console.WriteLine ("Non hai inserito un numero");
                }
                Console.WriteLine ("Non hai inserito un numero valido"); 
            }
        }
        return quantita;
    }

    private static List<OggettoQuantita<int>> creaInventarioFromListaItemEQuantita(List <Item> itemGiaPresenti, List <int> quantitaItemGiaPresenti){
        if (itemGiaPresenti.Count == quantitaItemGiaPresenti.Count){
            List<OggettoQuantita<int>> output = new List<OggettoQuantita<int>> ();
            int i = 0;
            while (i < itemGiaPresenti.Count){
                output.Add (new OggettoQuantita <int> (itemGiaPresenti [i].idItem, quantitaItemGiaPresenti [i]));
                i++;
            }
            return output;
        }
        throw new Exception ("Le dimensioni della lista contente gli item e le quantita di essi non corrispondo");
    }

    ~Player()
    {
        
    }
    
}