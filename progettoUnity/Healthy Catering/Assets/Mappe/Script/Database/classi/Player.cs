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


    List<OggettoQuantita<int>> inventarioTest = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (0,10),
        new OggettoQuantita<int> (34,10),
        new OggettoQuantita<int> (33,10)
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

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        string inventarioString = "";

        if (inventario.Count > 0)
        {
            //se non lo prendo prima viene ricreato ogni volta che viene chiamato il metodo idToIngrediente
            List<Ingrediente> databaseIngredienti = Costanti.databaseIngredienti;
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

    /// <summary>
    /// Il metodo controlla se esistono piatti realizzabili con l'inventario del Player
    /// </summary>
    /// <returns>booleano True: Almeno un piatto realizzabile con l'inventario del piatto, False: Nessun piatto realizzabile con l'inventario del Player</returns>
    public bool piattiRealizzabiliConInventario()
    {
        bool realizzabilePiatti = false;
        List<Piatto> databasePiatti = Costanti.databasePiatti;

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

    /// <summary>
    /// Il metodo permette di aumentare il denaro del player
    /// </summary>
    /// <param name="guadagno">float denaro da aggiungere</param>
    public void guadagna(float guadagno)
    {
        this.soldi += guadagno;
    }

    /// <summary>
    /// Il metodo permette di diminuire il denaro del player
    /// </summary>
    /// <param name="guadagno">float denaro da sottrarre</param>
    public void paga(float guadagno)
    {
        this.soldi -= guadagno;
    }

    /// <summary>
    /// Il metodo permette di aggiornare il punteggio del giocatore
    /// </summary>
    /// <param name="affine">booleano, True: Piatto affine al cliente, False: Piatto non affine al cliente</param>
    /// <param name="nutriScore">int valore nutriScore</param>
    /// <param name="costoEco">float costoEco</param>
    /// <param name="livello">int indice livello</param>
    public void aggiungiDiminuisciPunteggio(bool affine, int nutriScore, float costoEco, int livello)
    {
        float punteggioDaAggiungere;
        if (affine)
            punteggioDaAggiungere = 100;
        else
            punteggioDaAggiungere = -10;

        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(Math.Abs(punteggioDaAggiungere), trovaPercentualeNutriScore(nutriScore));
        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(Math.Abs(punteggioDaAggiungere), trovaPercentualeEcoScore(costoEco));
        this.punteggio [livello] += (int)punteggioDaAggiungere;
        Debug.Log("Punteggio: " + this.punteggio[livello]);
    }

    /// <summary>
    /// Il metodo permette di calcolare la percentuale del bonus del nutriScore
    /// </summary>
    /// <param name="nutriScore">float costoEco</param>
    /// <returns>float percetuale</returns>
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

    /// <summary>
    /// Il metodo permette di calcolare la percentuale del bonus del costoEco
    /// </summary>
    /// <param name="costoEco">float costoEco</param>
    /// <returns>float percetuale</returns>
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

    /// <summary>
    /// Il metodo permette di aggiornare l'inventario del player
    /// </summary>
    /// <param name="ingrediente">Lista di ingredienti da aggiungere o rimuovere dall'inventario del cliente</param>
    /// <param name="compra">booleano True: Gli ingredienti devono essere rimossi dall'inventario, False: Gli ingredienti devono essere aggiunti all'inventario</param>
    public void aggiornaInventario(OggettoQuantita<int> ingrediente, bool compra)
    {
        //compra == true se compra, false se vendi
        compraVendiSingoloIngrediente(ingrediente, compra);
    }

    /// <summary>
    /// Il metodo permette di aggiornare l'inventario del player
    /// </summary>
    /// <param name="oggettoDaComprare">Lista di oggetti da aggiungere o rimuovere dall'inventario del cliente</param>
    /// <param name="compra">booleano True: Gli oggettoDaComprare devono essere rimossi dall'inventario, False: Gli oggettoDaComprare devono essere aggiunti all'inventario</param>
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

    /// <summary>
    /// Il metodo permette di aggiornare l'inventario del player
    /// </summary>
    /// <param name="ingredienti">Lista di ingredienti da aggiungere o rimuovere dall'inventario del cliente</param>
    /// <param name="compra">booleano True: Gli ingredienti devono essere rimossi dall'inventario, False: Gli ingredienti devono essere aggiunti all'inventario</param>
    public void aggiornaInventario(List<OggettoQuantita<int>> ingredienti, bool compra)
    {   
        //compra == true se compra, false se vendi
        foreach (OggettoQuantita<int> ingrediente in ingredienti)
        {
            compraVendiSingoloIngrediente(ingrediente, compra);
        }
    }

    /// <summary>
    /// Il metodo controlla se l'inventario del player è vuoto
    /// </summary>
    /// <returns>booleano True: Inventario vuoto, Falso: Inventario non vuoto</returns>
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

    /// <summary>
    /// Il metodo restituisce la stringa dell'intero inventario del player
    /// </summary>
    /// <returns>string inventario player</returns>
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

    /// <summary>
    /// Il metodo permette di impostare un inventario specifico in base al livello del gioco
    /// </summary>
    /// <param name="livello">double livello</param>
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
                aggiornaInventario (inventarioLivello1, true);
                break;
            case 2:
                aggiornaInventario(inventarioLivello2, true);
                break;
            case 3:
                aggiornaInventario(inventarioTest, true);
                break;
        }
    }

    //DATABASE

    /// <summary>
    /// Il metodo restituisce la lista sortata secondo <see cref="PlayerComparer"/> dei giocatori presenti nel database
    /// </summary>
    /// <param name="databasePlayer">database player presenti</param>
    /// <returns>lista player sortata</returns>
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