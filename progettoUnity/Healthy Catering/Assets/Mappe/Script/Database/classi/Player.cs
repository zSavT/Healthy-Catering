using System;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

public class Player
{
    public string nome = "";

    public float soldi = 0;

    [JsonIgnore] public List<OggettoQuantita<int>> inventario = new List<OggettoQuantita<int>>();

    public static readonly int numeroLivelli = 3;
    public int[] punteggio = new int [numeroLivelli];

    //INVENTARI LIVELLI
     List<OggettoQuantita<int>> inventarioLivello0 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (12,10),
        new OggettoQuantita<int> (15,10),
        new OggettoQuantita<int> (0,10),
        new OggettoQuantita<int> (18,10),
        new OggettoQuantita<int> (16,10),
        new OggettoQuantita<int> (46,10)
    };
    List<OggettoQuantita<int>> inventarioLivello05 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2)
    };

    List<OggettoQuantita<int>> inventarioLivello1 = new List<OggettoQuantita<int>>
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
    
    List<OggettoQuantita<int>> inventarioLivello2 = new List<OggettoQuantita<int>>
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

    //FINE INVENTARI LIVELLI

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
        this.inventario = new List<OggettoQuantita<int>>(); //int perchè sono gli id degli Ingrediente e non gli Ingrediente veri e propri
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
                if (ingredienteTemp.idIngrediente != -1)
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

    public bool piattiRealizzabiliConInventario()
    {
        bool realizzabilePiatti = false;
        List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto());

        foreach(Piatto temp in databasePiatti)
        {
            if(temp.piattoInInventario(this.inventario))
            {
                realizzabilePiatti = true;
                break;
            }
        }
        return realizzabilePiatti;
    }

    public void guadagna(float guadagno)
    {
        this.soldi += guadagno;
    }

    public void aggiungiDiminuisciPunteggio(bool affine, int nutriScore, float costoEco, int livello)
    {
        Debug.Log(affine);
        float punteggioDaAggiungere;
        if (affine)
            punteggioDaAggiungere = 100;
        else
            punteggioDaAggiungere = -10;

        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(Utility.valoreAssoluto(punteggioDaAggiungere), trovaPercentualeNutriScore(nutriScore));
        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(Utility.valoreAssoluto(punteggioDaAggiungere), trovaPercentualeEcoScore(costoEco));
        Debug.Log(punteggioDaAggiungere);
        this.punteggio [livello] += (int)punteggioDaAggiungere;
        Debug.Log(this.punteggio[livello]);
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

    public void aggiornaInventario(OggettoQuantita<int> ingrediente, bool compra)
    {
        //compra == true se compra, false se vendi
        compraVendiSingoloIngrediente(ingrediente, compra);
    }

    private void compraVendiSingoloIngrediente(OggettoQuantita<int> oggettoDaComprare, bool compra)
    {
        //compra == true se compra, false se vendi
        int i = 0;
        
        foreach (OggettoQuantita <int> oggettoInventario in this.inventario)
        {
            if (oggettoDaComprare.oggetto == oggettoInventario.oggetto)
            {
                if (compra)
                    this.inventario[i].quantita = this.inventario[i].quantita + oggettoDaComprare.quantita;
                else
                {
                    if (this.inventario[i].quantita - oggettoDaComprare.quantita >= 0)
                    {
                        this.inventario[i].quantita = this.inventario[i].quantita - oggettoDaComprare.quantita;
                    }
                    else
                    {
                        this.inventario[i].quantita = 0; //cosi non creo problemi se qualche cosa è andata storta sopra
                        this.inventario.Remove(this.inventario[i]);
                    }
                       
                }
                return;
            }
            i++;
        }
        if (compra)
            this.inventario.Add(oggettoDaComprare);
        //else
        //////stessa cosa di prima, non creo problemi se qualche cosa è andata storta sopra, 
        //////ci dovrebbero essere dei controlli prima comunque
    }

    //@overloading (trovo il tag per l'override ma non per l'overloading :| , pero è quello)
    public void aggiornaInventario(List<OggettoQuantita<int>> ingredienti, bool compra)
    {   
        //compra == true se compra, false se vendi
        foreach (OggettoQuantita<int> ingrediente in ingredienti)
        {
            compraVendiSingoloIngrediente(ingrediente, compra);
        }
    }

    public bool inventarioVuoto()
    {
        foreach (OggettoQuantita<int> ingrediente in inventario)
        {
            if (ingrediente.quantita != 0)
            {
                return false;
            }
        }
        return true;
    }

    public string stampaInventario()
    {
        string output = "";
        foreach (OggettoQuantita<int> oggetto in inventario)
        {
            Ingrediente temp = Ingrediente.idToIngrediente(oggetto.oggetto);
            output += temp.ToString() + "\n" + oggetto.quantita + "\n\n";
        }
        return output;
    }

    public void setInventarioLivello (double livello)
    {
        switch (livello)
        {
            case 0:
                this.inventario = inventarioLivello0;
                break;
            case 0.5:
                this.inventario = inventarioLivello05;
                break;
            case 1:
                this.inventario = inventarioLivello1;
                break;
            case 2:
                this.inventario = inventarioLivello2;
                break;
        }
    }

    //DATABASE
    public static List<OggettoQuantita<int>> popolaInventario()
    {
        List<Ingrediente> ingredientiNuovi = getNewIngredienti();

        List<int> quantitaIngredientiNuovi = new List<int>();

        quantitaIngredientiNuovi = chiediQuantitaIngredienti(ingredientiNuovi);

        return creaInventarioFromListaIngredientiEQuantita(ingredientiNuovi, quantitaIngredientiNuovi);
    }

    private static List<Ingrediente> getNewIngredienti()
    {
        List<Ingrediente> ingredientiGiaPresenti = new List<Ingrediente>();

        while (true)
        {
            Console.WriteLine("Inserisci la keyword 'inizia' o la keyword 'continua' per inserire un nuovo ingrediente e la parola 'fine' per concludere l'inserimento");
            string input = Console.ReadLine();
            if (input.ToLower().Equals("fine"))
                break;
            else if ((input.ToLower().Equals("inizia")) || (input.Equals("continua")))
                ingredientiGiaPresenti.Add(Ingrediente.creaNuovoIngrediente());
            else
                Console.WriteLine("Input sbagliato");
        }

        return ingredientiGiaPresenti;
    }

    private static List<int> chiediQuantitaIngredienti(List<Ingrediente> ingredientiGiaPresenti)
    {
        List<int> quantita = new List<int>();
        foreach (Ingrediente ingrediente in ingredientiGiaPresenti)
        {
            int numero = -1;
            while (numero < 0)
                numero = Database.getNewIntFromUtente("Quanti " + ingrediente.ToString() + "\n" + " devono essere presenti nell'inventario?");
            quantita.Add(numero);
        }
        return quantita;
    }

    private static List<OggettoQuantita<int>> creaInventarioFromListaIngredientiEQuantita(List<Ingrediente> ingredientiGiaPresenti, List<int> quantitaIngredientiNuovi)
    {
        if (ingredientiGiaPresenti.Count == quantitaIngredientiNuovi.Count)
        {
            List<OggettoQuantita<int>> output = new List<OggettoQuantita<int>>();
            for (int i = 0; i < ingredientiGiaPresenti.Count; i++)
                output.Add(new OggettoQuantita<int>(ingredientiGiaPresenti[i].idIngrediente, quantitaIngredientiNuovi[i]));
            return output;
        }
        throw new Exception("Le dimensioni della lista contente gli ingredienti e le quantita di essi non corrispondo");
    }

    public static List <Player> getListaSortata(List <Player> databasePlayer = null){
        databasePlayer ??= Database.getDatabaseOggetto (new Player());
        
        databasePlayer.Sort (new PlayerComparer());
        return databasePlayer;
    }

    private class PlayerComparer : IComparer<Player>
    {
        //Compare returns -1 (less than), 0 (equal), or 1 (greater)
        //nutriScore costoEco costo
        public int Compare(Player first, Player second)
        {
            int punteggioPrimo = first.punteggio[PlayerSettings.livelloSelezionato];
            int punteggioSecondo = second.punteggio[PlayerSettings.livelloSelezionato];
            if (punteggioPrimo != punteggioSecondo)
            {
                if (first.punteggio[PlayerSettings.livelloSelezionato] > second.punteggio[PlayerSettings.livelloSelezionato])
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}