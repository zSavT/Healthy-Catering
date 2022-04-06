using System.Collections.Generic;
public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    private float costo = 0;
    private float costoEco = 0;
    private int nutriScore = 0; //media fra i nutriscore degli ingredienti ma approssimata per difetto all'intero più vicino
    //                          int al posto di ingredienti perché sono gli id degli ingredienti
    public List <OggettoQuantita <int>> idIngredienti = null;

    public Piatto(string nome, string descrizione, float costo, float costoEco, int nutriScore, List<OggettoQuantita<int>> idIngredienti)
    {
        this.nome = nome;
        this.descrizione = descrizione;
        this.costo = costo;
        this.costoEco = costoEco;
        this.nutriScore = nutriScore;
        this.idIngredienti = idIngredienti;
    }

    public Piatto (){
        this.nome = "";
        this.descrizione = "";
        this.costo = -1;
        this.costoEco = -1;
        this.nutriScore = -1;
        this.idIngredienti = new List<OggettoQuantita<int>> ();
    }

    ~Piatto()
    {
        
    }

    public List <Ingrediente> getIngredientiPiatto (){
        List <Ingrediente> databaseIngredienti = Database.getDatabaseOggetto (new Ingrediente ());
        List <Ingrediente> ingredientiPiatto = new List <Ingrediente> ();
        int i = 0;
        foreach (int id in this.idIngredienti [i]){
            foreach (Ingrediente ingrediente in databaseIngredienti){
                if (id == ingrediente.idItem){
                    ingredientiPiatto.Add (ingrediente);
                } 
            }
            i++;
        }
    }

    public List <int> getPatologieCompatibili (){
        List <Ingrediente> ingredientiPiatto = this.getIngredientiPiatto ();
        List <int> IdtutteLePatologie = Patologia.getListIdTutteLePatologie ();
        foreach (Ingrediente ingrediente in ingredientiPiatto){
            foreach (int id in IdtutteLePatologie){
                if (!(ingrediente.listaIdPatologie.Contains (id))){
                    IdtutteLePatologie.remove (id);
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