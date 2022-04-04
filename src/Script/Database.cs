using System;

public class Database {
    
    public Database (){}

    public static void Main(string[] args)
    {
        aggiungiPatologia (new Patologia (-1, "", ""));
    }

    public static void aggiungiPatologia (Patologia patologia){
        patologia.idPatologia = Patologia.getNewIdDatabasePatologia(patologia);

        while (patologia.nome.Equals("")){
            patologia.nome = getNewStringaFromUtente ("Inserisci il nome della patologia");
        }
        
        while (patologia.descrizione.Equals("")){
            patologia.descrizione = getNewStringaFromUtente ("Inserisci la descrizione della patologia");
        }

        salvaNuovoOggettoSuFile (patologia);
    }

    public static void aggiungiIngrediente (Ingrediente ingrediente){
        ingrediente.idItem = Item.getNewIdDatabaseItem (ingrediente);

        while (ingrediente.nome.Equals("")){
            ingrediente.nome = getNewStringaFromUtente ("Inserisci il nome dell'ingrediente");
        }
        
        while (ingrediente.descrizione.Equals("")){
            ingrediente.descrizione = getNewStringaFromUtente ("Inserisci la descrizione dell'ingrediente");
        }

        while (ingrediente.costo <= 0.0){
            ingrediente.costo = ingrediente.getNewNumeroIngredienteFromUtente ("Inserisci il costo dell'ingrediente", "Non hai inserito un numero valido, inserisci un numero a cifra decimale (con .)");
        }

        while (ingrediente.costoEco <= 0.0){
            ingrediente.costoEco = (int) ingrediente.getNewNumeroIngredienteFromUtente ("Inserisci il costo eco dell'ingrediente", "Non hai inserito un numero valido, inserisci un numero intero");
        }

        while (ingrediente.nutriScore <= 0.0){
            ingrediente.nutriScore = (int) ingrediente.getNewNumeroIngredienteFromUtente ("Inserisci il nutriscore dell'ingrediente", "Non hai inserito un numero valido, inserisci un numero intero");
        }

        while ((ingrediente.dieta < 0) || (ingrediente.dieta > 2)){
            ingrediente.dieta = Dieta.getNewDietaFromUtente ("Inserisci la dieta minima con la quale è compatibile l'ingrediente");
        }

        if (ingrediente.listaIdPatologieCompatibili.Count == 0){
            ingrediente.listaIdPatologieCompatibili = Patologia.getNewListaIdPatologieFromUtente ("Inserisci le patologie compatibili con l'ingrediente e la keyword 'fine' quando hai finito l'inserimento");
        }

        Database.salvaNuovoOggettoSuFile (ingrediente);
    }

    public static void aggiungiDieta (Dieta dieta){
        while (dieta.nome.Equals("")){
            dieta.nome = getNewStringaFromUtente ("Inserisci il nome della dieta");
        }
        
        while (dieta.descrizione.Equals("")){
            dieta.descrizione = getNewStringaFromUtente ("Inserisci la descrizione della dieta");
        }

        Database.salvaNuovoOggettoSuFile (dieta);
    }

    public static void aggiungiCliente (Cliente cliente){
        while (cliente.nome.Equals("")){
            cliente.nome = cliente.getNewNomeClienteFromUtente ("Inserisci il nome del cliente");
        }
        
        while ((cliente.dieta != 0) && (cliente.dieta != 1) && (cliente.dieta != 2)){
            cliente.dieta = Dieta.getNewDietaFromUtente ("Inserisci il nome della dieta del cliente");
        }
        
        while (cliente.listaIdPatologie.Count == 0){
            cliente.listaIdPatologie = Patologia.getNewListaIdPatologieFromUtente ("Inserisci le patologie del cliente e la keyword 'fine' quando hai finito l'inserimento");
        }
        
        Database.salvaNuovoOggettoSuFile (cliente);
    }

    public static void salvaNuovoOggettoSuFile <Oggetto> (Oggetto oggetto){   
        List <Oggetto> oggettiVecchi = getDatabaseOggetto (oggetto);
        if (!(oggettiVecchi.Contains (oggetto))){
            oggettiVecchi.Add (oggetto);
        }
        Serializza.salvaOggettiSuFile (oggettiVecchi);
    }

    public static string getNewStringaFromUtente (string output){
        Console.WriteLine (output);
        return Console.ReadLine();
    }

    public static List <Oggetto> getDatabaseOggetto <Oggetto> (Oggetto oggetto){
        string pathJson = Serializza.getJsonPath (oggetto);
        return Serializza.leggiOggettiDaFile <Oggetto> (pathJson);
    }

    private static void creaDatabaseBase (){
        Database.creaDatabaseBaseCliente ();
        Database.creaDatabaseBaseDieta ();
        Database.creaDatabaseBaseIngrediente ();
        Database.creaDatabaseBasePatologia ();
        Database.creaDatabaseBasePiatto ();
        Database.creaDatabaseBasePlayer ();
        Database.creaDatabaseBaseRistorante ();
    }

    private static void creaDatabaseBaseCliente (){
        //Cliente
        List <Cliente> tempCliente = new List<Cliente> ();
        
        List <int> tempClienteListaIdPatologie = new List<int> ();
        tempClienteListaIdPatologie.Add(0);
        tempClienteListaIdPatologie.Add(1);
        
        tempCliente.Add(new Cliente ("", 0, tempClienteListaIdPatologie));
        tempCliente.Add(new Cliente ("", 0, tempClienteListaIdPatologie));

        Serializza.salvaOggettiSuFile <Cliente> (tempCliente);
    }

    private static void creaDatabaseBaseDieta (){
        //Dieta
        List <Dieta> tempDieta = new List<Dieta> ();
        
        tempDieta.Add(new Dieta ("", ""));
        tempDieta.Add(new Dieta ("", ""));

        Serializza.salvaOggettiSuFile <Dieta> (tempDieta);
    }

    private static void creaDatabaseBaseIngrediente (){
        //Ingrediente
        List <Ingrediente> tempIngrediente = new List<Ingrediente> ();

        List <int> listaIdPatologieCompatibili = new List <int> ();
        listaIdPatologieCompatibili.Add(-1);
        listaIdPatologieCompatibili.Add(-1);        

        tempIngrediente.Add(new Ingrediente (-1, "", "", 0, 0, 0, 0, listaIdPatologieCompatibili));
        tempIngrediente.Add(new Ingrediente (-1, "", "", 0, 0, 0, 0, listaIdPatologieCompatibili));

        Serializza.salvaOggettiSuFile <Ingrediente> (tempIngrediente);
    }

    private static void creaDatabaseBasePatologia (){
        //Patologia
        List <Patologia> tempPatologia = new List<Patologia> ();
        
        tempPatologia.Add(new Patologia (-1, "", ""));
        tempPatologia.Add(new Patologia (-1, "", ""));

        Serializza.salvaOggettiSuFile <Patologia> (tempPatologia);
    }

    private static void creaDatabaseBasePiatto (){
        //Piatto
        List <Piatto> tempPiatto = new List<Piatto> ();
        
        List <OggettoQuantita <int>> tempPiattoListaIngredienti = new List<OggettoQuantita<int>> ();
        tempPiattoListaIngredienti.Add (new OggettoQuantita<int> (-1, 0));
        tempPiattoListaIngredienti.Add (new OggettoQuantita<int> (-1, 0));
        
        tempPiatto.Add(new Piatto ("", "", 0, 0, 0, tempPiattoListaIngredienti));
        tempPiatto.Add(new Piatto ("", "", 0, 0, 0, tempPiattoListaIngredienti));

        Serializza.salvaOggettiSuFile <Piatto> (tempPiatto);
    }

    private static void creaDatabaseBasePlayer (){
        //Player
        List <Player> tempPlayer = new List<Player> ();
        
        List <OggettoQuantita<Item>> tempPlayerInventario = new List <OggettoQuantita<Item>> ();
        tempPlayerInventario.Add(new OggettoQuantita<Item> (new Item (-1, "", ""), 0));
        tempPlayerInventario.Add(new OggettoQuantita<Item> (new Item (-1, "", ""), 0));
        
        tempPlayer.Add(new Player ("", 0, tempPlayerInventario));
        tempPlayer.Add(new Player ("", 0, tempPlayerInventario));

        Serializza.salvaOggettiSuFile <Player> (tempPlayer);
    }

    private static void creaDatabaseBaseRistorante (){
    //Ristorante
        List <Ristorante> tempRistorante = new List<Ristorante> ();
        
        List <int> listaIdPatologieCompatibili = new List <int> ();
        listaIdPatologieCompatibili.Add(-1);
        listaIdPatologieCompatibili.Add(-1);   

        List <OggettoQuantita <Ingrediente>> tempRistorateMagazzinoIngredienti = new List<OggettoQuantita<Ingrediente>> ();
        tempRistorateMagazzinoIngredienti.Add (new OggettoQuantita<Ingrediente> (new Ingrediente (-1, "", "", 0, 0, 0, 0, listaIdPatologieCompatibili), 0));
        tempRistorateMagazzinoIngredienti.Add (new OggettoQuantita<Ingrediente> (new Ingrediente (-1, "", "", 0, 0, 0, 0, listaIdPatologieCompatibili), 0));
        
        tempRistorante.Add(new Ristorante ("", 0, tempRistorateMagazzinoIngredienti));
        tempRistorante.Add(new Ristorante ("", 0, tempRistorateMagazzinoIngredienti));

        Serializza.salvaOggettiSuFile <Ristorante> (tempRistorante);
    }
}