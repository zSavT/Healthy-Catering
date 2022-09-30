using System;
using System.Collections.Generic;

public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    public float costo = 0;
    public float costoEco = 0;
    public int nutriScore = 0;

    public List<OggettoQuantita<int>> listaIdIngredientiQuantita = null;

    public Piatto(string nome, string descrizione, List<OggettoQuantita<int>> listaIdIngredientiQuantita)
    {
        this.nome = nome;
        this.descrizione = descrizione;
        this.listaIdIngredientiQuantita = listaIdIngredientiQuantita;
        
        this.costo = calcolaCostoBase();
        this.costoEco = calcolaCostoEco();
        this.nutriScore = calcolaNutriScore();
    }

    public Piatto()
    {
        this.nome = "";
        this.descrizione = "";
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.listaIdIngredientiQuantita = new List<OggettoQuantita<int>>();
    }

    public Piatto(string nomePiatto) : base()
    {
        this.nome = nomePiatto;
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Piatto))
        {
            return false;
        }
        return (this.nome.Equals(((Piatto)obj).nome))
            && (this.descrizione.Equals(((Piatto)obj).descrizione))
            && (this.costo == ((Piatto)obj).costo)
            && (this.costoEco == ((Piatto)obj).costoEco)
            && (this.nutriScore == ((Piatto)obj).nutriScore)
            && OggettoQuantita<int>.listeIdQuantitaUguali(this.listaIdIngredientiQuantita, ((Piatto)obj).listaIdIngredientiQuantita);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        // il nome non serve perche' questo metodo viene chiamato solo nel ricettario dove
        // il nome è mostrato da un'altra parte rispetto a dove va la stringa in output da
        // questo metodo
        string output = this.descrizione + "\n\n" + 
        "Costo: " + (this.costo != 0 ? this.costo : this.calcolaCostoBase()) + "\n" +
        "Costo eco: " + (this.costoEco != 0 ? this.costoEco : this.calcolaCostoEco()) + "\n" +
        "Nutriscore: " + (this.nutriScore != 0 ? this.nutriScore : this.calcolaNutriScore()) + "\n";

        return output;
    }

    public string getListaIngredientiQuantitaToString()
    {        
        string ingredientiQuantitaString = "";
        
        foreach (OggettoQuantita<int> ingredienteQuantita in listaIdIngredientiQuantita)
        {
            Ingrediente temp = Ingrediente.idToIngrediente(ingredienteQuantita.oggetto);
            if (temp.idIngrediente != -1)
                ingredientiQuantitaString = ingredientiQuantitaString + temp.nome + " x" + ingredienteQuantita.quantita.ToString() + "\n";
        }

        return ingredientiQuantitaString;
    }

    ~Piatto()
    {

    }

    public float calcolaCostoBase()
    {
        float costo = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
            costo = costo + (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto).costo * ingredienteQuantita.quantita);

        return costo + ((costo * Costanti.percentualeGuadagnoSulPiatto) / 100);
    }

    public float calcolaCostoConBonus (bool affine, float costoBase){
        if (affine){
            float output;
            int posizioneNellaListaMigliori;

            output = costoBase;
            output += Utility.calcolaCostoPercentuale(costoBase, 7);
            
            posizioneNellaListaMigliori = this.getListaAffinitaOrdinata().IndexOf(this);
            if (posizioneNellaListaMigliori == 0 || posizioneNellaListaMigliori == 1 || posizioneNellaListaMigliori == 2){
                output += Utility.calcolaCostoPercentuale(costoBase, posizioneNellaListaMigliori + 1);
            }
            return output;
        }

        return costoBase - Utility.calcolaCostoPercentuale(costoBase, 5);
    }

    private List <Piatto> getListaAffinitaOrdinata(){
        List <Piatto> databasePiattiTemp = Costanti.databasePiatti;
        
        databasePiattiTemp.Sort (new PiattiComparer());
        return databasePiattiTemp;
    }

    private class PiattiComparer : IComparer<Piatto>
    {
        //Compare returns -1 (less than), 0 (equal), or 1 (greater)
        //nutriScore costoEco costo
        public int Compare(Piatto first, Piatto second)
        {
            if (first.nutriScore > second.nutriScore){
                if (first.costoEco < second.costoEco){   
                    return 1;
                }
            }    
            else if ((first.nutriScore == second.nutriScore) && (first.costoEco == second.costoEco)){
                if (first.costo == second.costo){
                    return 0;
                }    
                else if (first.costo < second.costo){
                    return 1;
                }
                else{
                    return -1;
                }
            }
            
            return -1;
        }
    }

    public float calcolaCostoEco()
    {
        float costoEco = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
            costoEco += (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto).costoEco * ingredienteQuantita.quantita);

        return costoEco;
    }

    public int calcolaNutriScore()
    {
        int sommanutriScore = 0;
        int numeroIngredienti = 0;

        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
        {
            sommanutriScore += (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto).nutriScore * ingredienteQuantita.quantita);
            numeroIngredienti += ingredienteQuantita.quantita;
        }

        return (int)(sommanutriScore / numeroIngredienti);
    }

    private List<int> getPatologieCompatibili()
    {
        List<int> IdtutteLePatologie = Patologia.getListIdTutteLePatologie();

        foreach (Ingrediente ingrediente in this.getIngredientiPiatto())
            foreach (int id in IdtutteLePatologie)
                if (!(ingrediente.listaIdPatologieCompatibili.Contains(id)))
                    IdtutteLePatologie.Remove(id);

        return IdtutteLePatologie;
    }

    private int getDietaMinimaCompatibile()
    {
        List<Ingrediente> ingredientiPiatto = this.getIngredientiPiatto();
        int output = -1;

        foreach (Ingrediente ingrediente in ingredientiPiatto)
            if (output < ingrediente.dieta)
                output = ingrediente.dieta;

        return output;
    }

    public List<Ingrediente> getIngredientiPiatto()
    {
        List<Ingrediente> ingredientiPiatto = new List<Ingrediente>();

        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
        {
            ingredientiPiatto.Add(Ingrediente.idToIngrediente(ingredienteQuantita.oggetto));
        }

        return ingredientiPiatto;
    }

    public bool checkAffinitaConCliente (Cliente cliente)
    {
        bool patologieCompatibili = checkAffinitaPatologiePiatto(this.listaIdIngredientiQuantita, cliente.listaIdPatologie);
        bool dietaCompatibile = checkAffinitaDietaPiatto(this.listaIdIngredientiQuantita, cliente.dieta);

        return (patologieCompatibili && dietaCompatibile);
    }

    public bool checkAffinitaPatologiePiatto(List <OggettoQuantita <int>> listaIdIngredientiQuantita, List <int> listaIdPatologieCliente)
    {
        foreach (int idPatologiaCliente in listaIdPatologieCliente)
        {
            foreach (OggettoQuantita<int> idIngredienteEQuantita in listaIdIngredientiQuantita) 
            {
                Ingrediente ingrediente = Ingrediente.idToIngrediente(idIngredienteEQuantita.oggetto);
                if (!ingrediente.listaIdPatologieCompatibili.Contains (idPatologiaCliente)){
                    return false;
                }
            }
        }
        return true;
    }

    public bool checkAffinitaDietaPiatto(List <OggettoQuantita<int>> listaIdIngredientiQuantita, int dietaCliente)
    {
        foreach (OggettoQuantita<int> idIngredienteEQuantita in listaIdIngredientiQuantita)
        {
            Ingrediente ingrediente = Ingrediente.idToIngrediente(idIngredienteEQuantita.oggetto);

            if (ingrediente.dieta > dietaCliente)
                return false;
        }
        return true;
    }

    public static Piatto getPiattoFromNomeBottone(string nomePiattoBottone)
    {
        Piatto piattoSelezionato = new Piatto();
        foreach (Piatto piatto in Costanti.databasePiatti)
        {
            if (nomePiattoBottone.Contains(piatto.nome))//contains perché viene aggiunta la stringa ingredienti nel nome del bottone
            {
                piattoSelezionato = piatto;
                break;
            }
        }
        return piattoSelezionato;
    }

    public bool piattoInInventario(List <OggettoQuantita<int>> inventario)
    {
        bool ingredienteTrovato = false;
        foreach (OggettoQuantita<int> ingredientePiatto in listaIdIngredientiQuantita)
        {
            ingredienteTrovato = false;
            foreach (OggettoQuantita<int> ingredienteInventario in inventario)
            {
                if (ingredientePiatto.oggetto == ingredienteInventario.oggetto)
                {
                    ingredienteTrovato = true;
                    if (ingredientePiatto.quantita > ingredienteInventario.quantita)
                    {
                        return false;
                    }
                }
            }
            if (!ingredienteTrovato)
            {
                return false;
            }
            ingredienteTrovato = false;
        }

        return true;
    }

    public static Piatto nomeToPiatto (string nome)
    {
        foreach (Piatto temp in Costanti.databasePiatti)
        {
            if (nome.Contains (temp.nome))
                return temp;
        }

        return new Piatto();
    }
}