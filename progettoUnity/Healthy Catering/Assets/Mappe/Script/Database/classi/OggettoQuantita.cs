using System.Collections.Generic;

public class OggettoQuantita<Oggetto>
{
    public Oggetto oggetto;

    public int quantita;

    public OggettoQuantita(Oggetto oggetto, int quantita)
    {
        this.oggetto = oggetto;
        this.quantita = quantita;
    }

    public override bool Equals(object obj) //funziona solo con gli int
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is OggettoQuantita<Oggetto>))
        {
            return false;
        }
        return (this.oggetto.Equals(((OggettoQuantita<int>)obj).oggetto))
            && (this.quantita == ((OggettoQuantita<int>)obj).quantita);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "OggettoQuantita" + "\n\t" + "Valore oggetto: " + this.oggetto.ToString() + "\n\t" + "Quantità oggetto: " + this.quantita + "\n" + "Fine oggetto quantità";
    }

    ~OggettoQuantita()
    {

    }

    /// <summary>
    /// Il metodo permette di controllare se le due liste passate in input hanno lo stesso numero di elementi
    /// </summary>
    /// <param name="lista1">List<OggettoQuantita<int>> lista1</param>
    /// <param name="lista2">List<OggettoQuantita<int>> lista2</param>
    /// <returns>bool True: Le due liste Uguali, False: Le due liste diverse</returns>
    public static bool listeIdQuantitaUguali(List<OggettoQuantita<int>> lista1, List<OggettoQuantita<int>> lista2)
    {
        if (lista1.Count != lista2.Count)
            return false;

        for (int i = 0; i < lista1.Count; i++)
            for (int j = i; j < lista2.Count; j++)
                if (!(lista1[i].Equals(lista2[j])))
                    return false;

        return true;
    }
}