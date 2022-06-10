using System;
using System.Collections.Generic;

public class Player
{
    /*
    tutti i metodi e gli attributi e le variabili dichiarate nei metodi di questa classe con il nome 'Item' al loro interno sono in verità Ingredienti (o id di Ingredienti)
    reference: discussione relativa a questa cosa a partire dal commento che inizia con questa stringa:
    "
    @zSavT l'ultimo commit ha un problema bello grande ovvero che quando aggiungo un item al player, siccome può essere sia un Item generico che un Ingrediente non si possono 
    distinguere gli id degli Item e gli id degli ingredienti, in quanto hanno 2 database diversi.
    " 
    nella Pull Request chiamata PR issue#18
    */
    public string nome = "";

    public float soldi = 0;

    public List<OggettoQuantita<int>> inventario = new List<OggettoQuantita<int>>();

    public int punteggio = 0;

    public Player(string nome, int soldi, List<OggettoQuantita<int>> inventario)
    {
        this.nome = nome;
        this.soldi = soldi;
        this.inventario = inventario;
    }

    public Player(string nome)
    {
        this.nome = nome;
        this.soldi = 0;
        this.inventario = new List<OggettoQuantita<int>>();
    }

    public Player()
    {
        this.nome = "";
        this.soldi = 0;
        this.inventario = new List<OggettoQuantita<int>>(); //int perchè sono gli id degli item e non gli item veri e propri
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
        return (this.nome.Equals(((Player)obj).nome));
    }

    public override string ToString()
    {
        string inventarioString = "";

        if (inventario.Count > 0)
        {
            //se non lo prendo prima viene ricreato ogni volta che viene chiamato il metodo idToIngrediente
            List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
            foreach (OggettoQuantita<int> temp in inventario)
            {
                int id = temp.oggetto;
                Ingrediente ingredienteTemp = Ingrediente.idToIngrediente(id, databaseIngredienti);
                if (ingredienteTemp.idItem != -1)
                    inventarioString = inventarioString + "\n\t" + ingredienteTemp.nome + "\n";
            }
        }

        string output = "Player:" + "\n\t" + this.nome + "\n" + "Soldi:" + "\n\t" + this.soldi + "\n";

        if (!(inventarioString.Equals("")))
            output = output + "Inventario:" + inventarioString + "\n";

        return output + "Fine player " + this.nome;
    }

    ~Player()
    {

    }

    public void guadagna(float guadagno)
    {
        this.soldi += guadagno;
    }

    public void aggiungiDiminuisciPunteggio(bool affine, int nutriScore, float costoEco)
    {
        float punteggioDaAggiungere;
        if (affine)
            punteggioDaAggiungere = 100;
        else
            punteggioDaAggiungere = -10;

        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(Utility.valoreAssoluto(punteggioDaAggiungere), trovaPercentualeNutriScore(nutriScore));
        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(Utility.valoreAssoluto(punteggioDaAggiungere), trovaPercentualeEcoScore(costoEco));

        this.punteggio += (int)punteggioDaAggiungere;
    }

    public float trovaPercentualeNutriScore(int nutriScore)
    {
        if (nutriScore == 1)
            return 10;
        else if (nutriScore == 2)
            return 5;
        else if (nutriScore == 3)
            return 0;
        else if (nutriScore == 4)
            return -5;
        else //nutriscore == 5
            return -10;
    }

    public float trovaPercentualeEcoScore(float costoEco)
    {
        if (Utility.compresoFra(costoEco, 0, 10))
            return 10;
        else if (Utility.compresoFra(costoEco, 11, 20))
            return 5;
        else if (Utility.compresoFra(costoEco, 21, 30))
            return 0;
        else if (Utility.compresoFra(costoEco, 31, 40))
            return -5;
        else //Utility.compresoFra (costoEco, 41, infinito)
            return -10;
    }

    public void aggiornaInventario(Ingrediente ingrediente, int quantitaDaComprare)
    {
        //TODO
    }

    //DATABASE
    public static List<OggettoQuantita<int>> popolaInventario()
    {
        List<Item> itemNuovi = getNewItem();

        List<int> quantitaItemNuovi = new List<int>();

        quantitaItemNuovi = chiediQuantitaItem(itemNuovi);

        return creaInventarioFromListaItemEQuantita(itemNuovi, quantitaItemNuovi);
    }

    private static List<Item> getNewItem()
    {
        List<Item> itemGiaPresenti = new List<Item>();

        while (true)
        {
            Console.WriteLine("Inserisci la keyword 'inizia' o la keyword 'continua' per inserire un nuovo item e la parola 'fine' per concludere l'inserimento");
            string input = Console.ReadLine();
            if (input.ToLower().Equals("fine"))
                break;
            else if ((input.ToLower().Equals("inizia")) || (input.Equals("continua")))
                itemGiaPresenti.Add(Item.creaNuovoItem());
            else
                Console.WriteLine("Input sbagliato");
        }

        return itemGiaPresenti;
    }

    private static List<int> chiediQuantitaItem(List<Item> itemGiaPresenti)
    {
        List<int> quantita = new List<int>();
        foreach (Item item in itemGiaPresenti)
        {
            int numero = -1;
            while (numero < 0)
                numero = Database.getNewIntFromUtente("Quanti " + item.ToString() + "\n" + " devono essere presenti nell'inventario?");
            quantita.Add(numero);
        }
        return quantita;
    }

    private static List<OggettoQuantita<int>> creaInventarioFromListaItemEQuantita(List<Item> itemGiaPresenti, List<int> quantitaItemNuovi)
    {
        if (itemGiaPresenti.Count == quantitaItemNuovi.Count)
        {
            List<OggettoQuantita<int>> output = new List<OggettoQuantita<int>>();
            for (int i = 0; i < itemGiaPresenti.Count; i++)
                output.Add(new OggettoQuantita<int>(itemGiaPresenti[i].idItem, quantitaItemNuovi[i]));
            return output;
        }
        throw new Exception("Le dimensioni della lista contente gli item e le quantita di essi non corrispondo");
    }
}