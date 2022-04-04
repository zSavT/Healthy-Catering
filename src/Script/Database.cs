using System;

public class Database {
    
    public Database (){}

    public static void Main(string[] args)
    {
    }

    public static void aggiungiCliente (Cliente cliente){
        while (cliente.nome.Equals("")){
            cliente.getNewNomeClienteFromUtente ();
        }
        while (cliente.dieta != 0 && cliente.dieta != 1 && cliente.dieta != 2){
            cliente.getNewDietaClienteFromUtente ();
        }
        while (cliente.listaIdPatologie.Count == 0){
            cliente.getNewListaIdPatologieFromUtente ();
        }
        Database.salvaNuovoOggettoSuFile (cliente);
    }

    public static void salvaNuovoOggettoSuFile <Oggetto> (Oggetto oggetto){   
        string pathJson = Serializza.getJsonPath (oggetto);
        List <Oggetto> oggettiVecchi = Serializza.leggiOggettiDaFile <Oggetto> (pathJson);
        if (!(oggettiVecchi.Contains (oggetto))){
            oggettiVecchi.Add (oggetto);
        }
        Serializza.salvaOggettiSuFile (oggettiVecchi);
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