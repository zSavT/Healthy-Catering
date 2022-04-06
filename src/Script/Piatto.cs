using System.Collections.Generic;
public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    private float costo = 0;
    private float costoEco = 0;
    private int nutriScore = 0; //media fra i nutriScore degli ingredienti ma approssimata per difetto all'intero più vicino
    
    //                          int al posto di ingredienti perché sono gli id degli ingredienti
    public List <OggettoQuantita <int>> listaIdIngredientiQuantita = null;

    private int percentualeGuadagnoSulPiatto = 10;
    
    public Piatto(string nome, string descrizione, float costo, float costoEco, int nutriScore, List<OggettoQuantita<int>> listaIdIngredientiQuantita)
    {
        this.nome = nome;
        this.descrizione = descrizione;
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.listaIdIngredientiQuantita = listaIdIngredientiQuantita;
    }

    public Piatto (){
        this.nome = "";
        this.descrizione = "";
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.listaIdIngredientiQuantita = new List<OggettoQuantita<int>> ();
    }

    ~Piatto()
    {
        
    }

    public float calcolaCosto (){
        float costo = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita){
            costo = costo + (Ingrediente.IdToIngrediente (ingredienteQuantita.oggetto).costo * ingredienteQuantita.quantita);
        }
        return costo + ((costo * percentualeGuadagnoSulPiatto) / 100);
    }

    public float calcolaCostoEco (){
        float costoEco = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita){
            costoEco = costoEco + (Ingrediente.IdToIngrediente (ingredienteQuantita.oggetto).costoEco * ingredienteQuantita.quantita);
        }
        return costoEco;
    }

    public int calcolaNutriScore (){
        int sommanutriScore = 0;
        int numeroIngredienti = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita){
            sommanutriScore = sommanutriScore + (Ingrediente.IdToIngrediente (ingredienteQuantita.oggetto).nutriScore * ingredienteQuantita.quantita);
            numeroIngredienti = numeroIngredienti + ingredienteQuantita.quantita;
        }
        int nutriScore = (int) (sommanutriScore / numeroIngredienti);
        return nutriScore;
    }

    public List <Ingrediente> getIngredientiPiatto (){
        List <Ingrediente> databaseIngredienti = Database.getDatabaseOggetto (new Ingrediente ());
        List <Ingrediente> ingredientiPiatto = new List <Ingrediente> ();
        int i = 0;
        foreach (OggettoQuantita <int> ingredienteQuantita in this.listaIdIngredientiQuantita){
            foreach (Ingrediente ingrediente in databaseIngredienti){
                if (ingredienteQuantita.oggetto == ingrediente.idItem){
                    ingredientiPiatto.Add (ingrediente);
                } 
            }
            i++;
        }
        return ingredientiPiatto;
    }

    public List <int> getPatologieCompatibili (){
        List <Ingrediente> ingredientiPiatto = this.getIngredientiPiatto ();
        List <int> IdtutteLePatologie = Patologia.getListIdTutteLePatologie ();
        foreach (Ingrediente ingrediente in ingredientiPiatto){
            foreach (int id in IdtutteLePatologie){
                if (!(ingrediente.listaIdPatologieCompatibili.Contains (id))){
                    IdtutteLePatologie.Remove (id);
                }
            }
        }
        return IdtutteLePatologie;
    }

    public int getDietaMinimaCompatibile (){
        List <Ingrediente> ingredientiPiatto = this.getIngredientiPiatto ();
        int output = -1;
        foreach (Ingrediente ingrediente in ingredientiPiatto){
            if (output < ingrediente.dieta)
                output = ingrediente.dieta;
        }
        return output;
    }
}