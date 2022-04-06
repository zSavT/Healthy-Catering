using System;

public class Database {
    public Database (){}

    public static void Main(string[] args)
    {
        //salvaNuovoOggettoSuFile (new Piatto ());
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

        salvaNuovoOggettoSuFile (ingrediente);
    }

    public static void aggiungiDieta (Dieta dieta){
        while (dieta.nome.Equals("")){
            dieta.nome = getNewStringaFromUtente ("Inserisci il nome della dieta");
        }
        
        while (dieta.descrizione.Equals("")){
            dieta.descrizione = getNewStringaFromUtente ("Inserisci la descrizione della dieta");
        }

        salvaNuovoOggettoSuFile (dieta);
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
        
        salvaNuovoOggettoSuFile (cliente);
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
        creaDatabaseBaseCliente ();
        creaDatabaseBaseDieta ();
        creaDatabaseBaseIngrediente ();
        creaDatabaseBasePatologia ();
        creaDatabaseBasePiatto ();
        creaDatabaseBasePlayer ();
        creaDatabaseBaseRistorante ();
    }

    private static void creaDatabaseBaseCliente (){
        //Cliente
        List <Cliente> tempCliente = new List<Cliente> ();
        tempCliente.Add(new Cliente ());
    
        Serializza.salvaOggettiSuFile <Cliente> (tempCliente);
    }

    private static void creaDatabaseBaseDieta (){
        //Dieta
        List <Dieta> tempDieta = new List<Dieta> ();
        tempDieta.Add(new Dieta ());

        Serializza.salvaOggettiSuFile <Dieta> (tempDieta);
    }

    private static void creaDatabaseBaseIngrediente (){
        //Ingrediente
        List <Ingrediente> tempIngrediente = new List<Ingrediente> ();
        tempIngrediente.Add(new Ingrediente ());

        Serializza.salvaOggettiSuFile <Ingrediente> (tempIngrediente);
    }

    private static void creaDatabaseBasePatologia (){
        //Patologia
        List <Patologia> tempPatologia = new List<Patologia> ();
        tempPatologia.Add(new Patologia ());

        Serializza.salvaOggettiSuFile <Patologia> (tempPatologia);
    }

    private static void creaDatabaseBasePiatto (){
        //Piatto
        List <Piatto> tempPiatto = new List<Piatto> ();
        tempPiatto.Add(new Piatto ());

        Serializza.salvaOggettiSuFile <Piatto> (tempPiatto);
    }

    private static void creaDatabaseBasePlayer (){
        //Player
        List <Player> tempPlayer = new List<Player> ();
        tempPlayer.Add(new Player ());

        Serializza.salvaOggettiSuFile <Player> (tempPlayer);
    }

    private static void creaDatabaseBaseRistorante (){
    //Ristorante
        List <Ristorante> tempRistorante = new List<Ristorante> ();
        tempRistorante.Add(new Ristorante ());
        
        Serializza.salvaOggettiSuFile <Ristorante> (tempRistorante);
    }
}