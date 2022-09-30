using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Player
{
    public string nome = "";

    public float soldi = 0;

    [JsonIgnore] public List<OggettoQuantita<int>> inventario = new List<OggettoQuantita<int>>();
    
    public int[] punteggio = new int [Costanti.numeroLivelli];

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
            foreach (OggettoQuantita<int> temp in inventario)
            {
                int id = temp.oggetto;
                Ingrediente ingredienteTemp = Ingrediente.idToIngrediente(id);
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

    //controlla se c'è almeno un piatto realizzabile con l'inventario
    public bool piattiRealizzabiliConInventario()
    {
        foreach(Piatto temp in Costanti.databasePiatti)
        {
            if(temp.piattoInInventario(this.inventario))
            {
                return true;
            }
        }

        return false;
    }

    public void guadagna(float guadagno)
    {
        this.soldi += guadagno;
    }

    public void paga (float costo)
    {
        this.soldi -= costo;
    }

    public void aggiungiDiminuisciPunteggio(bool affine, int nutriScore, float costoEco, int livello)
    {
        float punteggioDaAggiungere;
        if (affine)
            punteggioDaAggiungere = 100;
        else
            punteggioDaAggiungere = -10;

        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(
            Math.Abs(punteggioDaAggiungere), 
            trovaPercentualeNutriScore(nutriScore)
        );
        punteggioDaAggiungere += Utility.calcolaCostoPercentuale(
            Math.Abs(punteggioDaAggiungere), 
            trovaPercentualeEcoScore(costoEco)
        );

        this.punteggio [livello] += (int)punteggioDaAggiungere;
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

    //@overloading (trovo il tag per l'override ma non per l'overloading :| , pero è quello)
    public void aggiornaInventario(List<OggettoQuantita<int>> ingredienti, bool compra)
    {
        //compra == true se compra, false se vendi
        foreach (OggettoQuantita<int> ingrediente in ingredienti)
        {
            compraVendiSingoloIngrediente(ingrediente, compra);
        }
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
        if (compra) // se l'ingrediente comprato non è già nell'inventario lo inserisco come nuovo
            this.inventario.Add(oggettoDaComprare);
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
                this.inventario = Costanti.inventarioLivello0;
                break;
            case 0.5:
                this.inventario = Costanti.inventarioLivello05;
                break;
            case 1:
                aggiornaInventario (Costanti.inventarioLivello1, true);
                break;
            case 2:
                aggiornaInventario(Costanti.inventarioLivello2, true);
                break;
            case 3:
                aggiornaInventario(Costanti.inventarioTest, true);
                break;
        }
    }

    public static List <Player> getListaSortata(){
        List <Player> databasePlayerTemp = Costanti.databasePlayer;
        
        databasePlayerTemp.Sort (new PlayerComparer());
        return databasePlayerTemp;
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