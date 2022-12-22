using System.Collections.Generic;

public class Piatto
{
    public string nome = "";
    public string descrizione = "";

    private float costo = 0;
    private float costoEco = 0;
    private int nutriScore = 0; //media fra i nutriScore degli ingredienti ma approssimata per difetto all'intero più vicino

    //                          int al posto di ingredienti perché sono gli id degli ingredienti
    public List<OggettoQuantita<int>> listaIdIngredientiQuantita = null;

    private int percentualeGuadagnoSulPiatto = 10;

    public Piatto(string nome, string descrizione, List<OggettoQuantita<int>> listaIdIngredientiQuantita)
    {
        this.nome = nome;
        this.descrizione = descrizione;
        this.listaIdIngredientiQuantita = listaIdIngredientiQuantita;
        
        List<Ingrediente> databaseIngredienti = Costanti.databaseIngredienti;
        this.costo = calcolaCostoBase(databaseIngredienti);
        this.costoEco = calcolaCostoEco(databaseIngredienti);
        this.nutriScore = calcolaNutriScore(databaseIngredienti);
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
        string output = this.descrizione + "\n\n" +
        "Costo: " + this.calcolaCostoBase() + "\n" +
        "Costo eco: " + this.calcolaCostoEco() + "\n" +
        "Nutriscore: " + this.calcolaNutriScore() + "\n";
        /*
        if (listaIdIngredientiQuantita.Count > 0)
        { 
            output = output + this.getListaIngredientiQuantitaToString();
        }*/

        return output;/* = output + "Fine piatto " + this.nome;*/
    }

    /// <summary>
    /// Il metodo restituisce la stringa quantità ingredienti presenti nel database con il relativo nome
    /// </summary>
    /// <returns>string nome e quantità di tutti gli ingredienti</returns>
    public string getListaIngredientiQuantitaToString()
    {
        string ingredientiQuantitaString = "";
        //se non lo prendo prima viene ricreato ogni volta che viene chiamato il metodo idToIngrediente
        List<Ingrediente> databaseIngredienti = Costanti.databaseIngredienti;

        foreach (OggettoQuantita<int> ingredienteQuantita in listaIdIngredientiQuantita)
        {
            Ingrediente temp = Ingrediente.idToIngrediente(ingredienteQuantita.oggetto, databaseIngredienti);
            if (temp.idIngrediente != -1)
                ingredientiQuantitaString = ingredientiQuantitaString + temp.nome + " x" + ingredienteQuantita.quantita.ToString() + "\n";
        }

        return ingredientiQuantitaString;
    }

    ~Piatto()
    {

    }

    /// <summary>
    /// Il metodo permette di calcolare il consto base dell'ingrediente
    /// </summary>
    /// <param name="databaseIngredienti">database ingredienti per la ricerca dell'ingredienti</param>
    /// <returns>float costo base ingrediente</returns>
    public float calcolaCostoBase(List <Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Costanti.databaseIngredienti;

        float costo = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
            costo = costo + (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto, databaseIngredienti).costo * ingredienteQuantita.quantita);

        return costo + ((costo * percentualeGuadagnoSulPiatto) / 100);
    }

    /// <summary>
    /// Il metodo restituisce il costo con i bonus applicati
    /// </summary>
    /// <param name="affine">bool True: Piatto Affine, False: Piatto non affine</param>
    /// <param name="costoBase">float costo base del piatto</param>
    /// <returns>float costo finale del piatto</returns>
    public float calcolaCostoConBonus (bool affine, float costoBase){
        if (affine){
            float output;
            int posizioneNellaListaMigliori;

            output = costoBase;
            output += Utility.calcolaCostoPercentuale(costoBase, 7);
            
            posizioneNellaListaMigliori = this.getListaAffinitaOrdinata ().IndexOf(this);
            if (posizioneNellaListaMigliori == 0 ||posizioneNellaListaMigliori == 1 || posizioneNellaListaMigliori == 2){
                output += Utility.calcolaCostoPercentuale(costoBase, posizioneNellaListaMigliori + 1);
            }
            return output;
        }

        return costoBase - Utility.calcolaCostoPercentuale(costoBase, 5);
    }

    /// <summary>
    /// Il metodo permette di restituire una lista sortata di piatti secondo il criterio di <see cref="PiattiComparer"/>
    /// </summary>
    /// <param name="databasePiatti">database piatti</param>
    /// <returns>List piatti sortata</returns>
    private List <Piatto> getListaAffinitaOrdinata(List <Piatto> databasePiatti = null){
        databasePiatti ??= Database.getDatabaseOggetto (this);
        
        databasePiatti.Sort (new PiattiComparer());
        return databasePiatti;
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

    /// <summary>
    /// Il metodo permette di calcolare il CostoEco del piatto
    /// </summary>
    /// <param name="databaseIngredienti">database ingredienti</param>
    /// <returns>int valore CostoEco</returns>
    public float calcolaCostoEco(List <Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Costanti.databaseIngredienti;
        
        float costoEco = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
            costoEco = costoEco + (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto, databaseIngredienti).costoEco * ingredienteQuantita.quantita);

        return costoEco;
    }

    /// <summary>
    /// Il metodo permette di calcolare il NutriScore del piatto
    /// </summary>
    /// <param name="databaseIngredienti">database ingredienti</param>
    /// <returns>int valore nutriScore</returns>
    public int calcolaNutriScore(List <Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Costanti.databaseIngredienti;
        
        int sommanutriScore = 0;
        int numeroIngredienti = 0;

        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
        {
            sommanutriScore = sommanutriScore + (Ingrediente.idToIngrediente(ingredienteQuantita.oggetto, databaseIngredienti).nutriScore * ingredienteQuantita.quantita);
            numeroIngredienti = numeroIngredienti + ingredienteQuantita.quantita;
        }

        return (int)(sommanutriScore / numeroIngredienti);
    }

    /// <summary>
    /// Il metodo permette di convertire la lista di ID ingredienti del piatto in una lista di ingredienti (sempre del piatto)
    /// </summary>
    /// <param name="databaseIngredienti">Database ingredienti da controllare</param>
    /// <returns>List ingredienti presenti nel piatto</returns>
    public List<Ingrediente> getIngredientiPiatto(List<Ingrediente> databaseIngredienti = null)
    {
        databaseIngredienti ??= Costanti.databaseIngredienti;

        List<Ingrediente> ingredientiPiatto = new List<Ingrediente>();

        int i = 0;
        foreach (OggettoQuantita<int> ingredienteQuantita in this.listaIdIngredientiQuantita)
        {
            foreach (Ingrediente ingrediente in databaseIngredienti)
                if (ingredienteQuantita.oggetto == ingrediente.idIngrediente)
                    ingredientiPiatto.Add(ingrediente);
            i++;
        }

        return ingredientiPiatto;
    }

    /// <summary>
    /// Il metodo controlla se il cliente è compatibile con il piatto
    /// </summary>
    /// <param name="cliente">Cliente cliente da controllare</param>
    /// <returns>booleano True: Compatibile con cliente, False: Non compatibile con il cliente</returns>
    public bool checkAffinitaConCliente (Cliente cliente)
    {
        bool patologieCompatibili = checkAffinitaPatologiePiatto(this.listaIdIngredientiQuantita, cliente.listaIdPatologie);
        bool dietaCompatibile = checkAffinitaDietaPiatto(this.listaIdIngredientiQuantita, cliente.dieta);

        return (patologieCompatibili && dietaCompatibile);
    }

    /// <summary>
    /// Il metodo restituisce un booleano se la lista di ingredienti passati in input è compatibile o meno con la Patolgia passata in input
    /// </summary>
    /// <param name="listaIdIngredientiQuantita">(List <OggettoQuantita<int>> lista ID Ingredienti Quantita</param>
    /// <param name="listaIdPatologieCliente">ID patologia</param>
    /// <returns>booleano True: Patolgia compatibile con il piatto, False: Patolgia non compatibile con il piatto</returns>
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

    /// <summary>
    /// Il metodo restituisce un booleano se la lista di ingredienti passati in input è compatibile o meno con la dieta passata in input
    /// </summary>
    /// <param name="listaIdIngredientiQuantita">(List <OggettoQuantita<int>> lista ID Ingredienti Quantita</param>
    /// <param name="dietaCliente">ID dieta</param>
    /// <returns>booleano True: Dieta compatibile con il piatto, False: Dieta non compatibile con il piatto</returns>
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

    /// <summary>
    /// Il metodo controlla se nell'inventario passato in input è presente tutto il necessario per avere il piatto
    /// </summary>
    /// <param name="inventario">List <OggettoQuantita<int>> inventario da controllare</param>
    /// <returns>bool, True: Ingredienti e Quantità presenti per servire il piatto, False:  Ingredienti e Quantità presenti per servire il piatto</returns>
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

    /// <summary>
    /// Il metodo controlla se esiste un piatto con lo stesso nome nel database
    /// </summary>
    /// <param name="nomePiatto">string nome del piatto</param>
    /// <param name="databasePiatti">database piatti</param>
    /// <returns>booleano True: Piatto con lo stesso nome presente, False: Piatto con lo stesso nome non presente</returns>
    public static bool checkPiattoOnonimoPresente(string nomePiatto, List<Piatto> databasePiatti = null)
    {
        databasePiatti ??= Costanti.databasePiatti;

        foreach (Piatto temp in databasePiatti)
            if (temp.nome.Equals(nomePiatto))
            {
                return true;
            }
        return false;       
    }

    //FUNZIONI PER DATABASE

    /// <summary>
    /// Il metodo permette di restituire il Piatto corrispondente alla stringa passata in input
    /// </summary>
    /// <param name="nome">string nome del piatto</param>
    /// <returns>Piatto ricercato</returns>
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