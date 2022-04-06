using System;

//TODO creare un metodo per chiedere gli interi e sostituire tutte le richieste per interi fatte fino ad ora, in tutti i file

public class Database {
    public Database (){}

    public static void Main(string[] args)
    {
        
    }

    public static void aggiungiRistorante (Ristorante ristorante){
        while (ristorante.nome.Equals("")){
            ristorante.nome = getNewStringaFromUtente ("Inserisci il nome del ristorante");
        }

        while (ristorante.punteggio < 0){
            ristorante.punteggio = Ristorante.getNewPunteggioFromUtente ();
        }

        while (ristorante.magazzinoIngredienti.Count == 0){
            ristorante.magazzinoIngredienti = Ristorante.fillMagazzinoIngredienti ();
        }

        salvaNuovoOggettoSuFile (ristorante);
    }

    public static void aggiungiPlayer (Player player){
        while (player.nome.Equals("")){
            player.nome = getNewStringaFromUtente ("Inserisci il nome del player");
        }

        while (player.soldi == -1){
            try{
                Console.WriteLine ("Inserisci i soldi del player " + player.nome);
                int soldi = Int32.Parse (Console.ReadLine ());
                if (soldi >= 0){
                    player.soldi = soldi;
                }
            }
            catch (Exception e){
                Console.WriteLine ("Non hai inserito un numero valido");
            }
        }
        
        while (player.inventario.Count == 0){
            player.inventario = Player.popolaInventario ();
        }

        salvaNuovoOggettoSuFile (player);
    }

    public static void aggiungiPiatto (Piatto piatto){
        while (piatto.nome.Equals("")){
            piatto.nome = getNewStringaFromUtente ("Inserisci il nome del piatto");
        }
        
        Piatto piattoGiaPresente = Piatto.checkPiattoOnonimoGiaPresente (piatto.nome);
        if (piattoGiaPresente == null){
            while (piatto.descrizione.Equals("")){
                piatto.descrizione = getNewStringaFromUtente ("Inserisci la descrizione del piatto");
            }

            while (piatto.listaIdIngredientiQuantita.Count == 0){
                piatto.listaIdIngredientiQuantita = Piatto.getListaIdIngredientiQuantitaPiattoFromUtente (piatto.nome);
            }

            piatto.calcolaCosto ();
            piatto.calcolaCostoEco ();
            piatto.calcolaNutriScore ();

            salvaNuovoOggettoSuFile (piatto);    
        }
        
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
        while (ingrediente.nome.Equals("")){
            ingrediente.nome = getNewStringaFromUtente ("Inserisci il nome dell'ingrediente");
        }

        Ingrediente ingredienteGiaPresente = Ingrediente.checkIngredienteOnonimoGiaPresente (ingrediente.nome);
        if (ingredienteGiaPresente == null){
            ingrediente.idItem = Item.getNewIdDatabaseItem (ingrediente);
            
            while (ingrediente.descrizione.Equals("")){
                ingrediente.descrizione = getNewStringaFromUtente ("Inserisci la descrizione dell'ingrediente " + ingrediente.nome);
            }

            while (ingrediente.costo <= 0.0){
                ingrediente.costo = ingrediente.getNewNumeroIngredienteFromUtente ("Inserisci il costo dell'ingrediente " + ingrediente.nome, "Non hai inserito un numero valido, inserisci un numero a cifra decimale (con .)");
            }

            while (ingrediente.costoEco <= 0.0){
                ingrediente.costoEco = (int) ingrediente.getNewNumeroIngredienteFromUtente ("Inserisci il costo eco dell'ingrediente " + ingrediente.nome, "Non hai inserito un numero valido, inserisci un numero intero");
            }

            while (ingrediente.nutriScore <= 0.0){
                ingrediente.nutriScore = (int) ingrediente.getNewNumeroIngredienteFromUtente ("Inserisci il nutriscore dell'ingrediente " + ingrediente.nome, "Non hai inserito un numero valido, inserisci un numero intero");
            }

            while ((ingrediente.dieta < 0) || (ingrediente.dieta > 2)){
                ingrediente.dieta = Dieta.getNewDietaFromUtente ("Inserisci la dieta minima con la quale è compatibile l'ingrediente " + ingrediente.nome);
            }

            if (ingrediente.listaIdPatologieCompatibili.Count == 0){
                ingrediente.listaIdPatologieCompatibili = Patologia.getNewListaIdPatologieFromUtente ("Inserisci le patologie compatibili con l'ingrediente " + ingrediente.nome + " e la keyword 'fine' quando hai finito l'inserimento");
            }

            salvaNuovoOggettoSuFile (ingrediente);
        }
    }

    public static void aggiungiDieta (Dieta dieta){
        while (dieta.nome.Equals("")){
            dieta.nome = getNewStringaFromUtente ("Inserisci il nome della dieta");
        }
        
        while (dieta.descrizione.Equals("")){
            dieta.descrizione = getNewStringaFromUtente ("Inserisci la descrizione della dieta " + dieta.nome);
        }

        salvaNuovoOggettoSuFile (dieta);
    }

    public static void aggiungiCliente (Cliente cliente){
        while (cliente.nome.Equals("")){
            cliente.nome = cliente.getNewNomeClienteFromUtente ("Inserisci il nome del cliente");
        }
        
        while ((cliente.dieta != 0) && (cliente.dieta != 1) && (cliente.dieta != 2)){
            cliente.dieta = Dieta.getNewDietaFromUtente ("Inserisci il nome della dieta del cliente " + cliente.nome);
        }
        
        while (cliente.listaIdPatologie.Count == 0){
            cliente.listaIdPatologie = Patologia.getNewListaIdPatologieFromUtente ("Inserisci le patologie del cliente " + cliente.nome + " e la keyword 'fine' quando hai finito l'inserimento");
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

    public static Oggetto getUltimoOggettoAggiuntoAlDatabase <Oggetto> (Oggetto oggetto){
        List <Oggetto> databaseOggetto = getDatabaseOggetto (oggetto);
        return databaseOggetto [databaseOggetto.Count - 1];
    }

    public static List <Oggetto> getDatabaseOggetto <Oggetto> (Oggetto oggetto){
        string pathJson = Serializza.getJsonPath (oggetto);
        return Serializza.leggiOggettiDaFile <Oggetto> (pathJson);
    }

    private static void creaDatabaseBase (){
        creaDatabaseBaseCliente ();
        creaDatabaseBaseDieta ();
        creaDatabaseBaseIngrediente ();
        creaDatabaseBaseItem ();
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

    private static void creaDatabaseBaseItem (){
        //Item
        List <Item> tempItem = new List<Item> ();
        tempItem.Add(new Item ());

        Serializza.salvaOggettiSuFile <Item> (tempItem);
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