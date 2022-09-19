using System.Collections.Generic;

public class Costanti
{
    // DATABASE OGGETTI
    public static readonly List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
    
    public static readonly List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto());

    public static readonly List<Player> databasePlayer = Database.getDatabaseOggetto (new Player());

    public static readonly List <Patologia> databasePatologie = Database.getDatabaseOggetto(new Patologia());

    // INVENTARI LIVELLI
    public static readonly List<OggettoQuantita<int>> inventarioLivello0 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (12,10),
        new OggettoQuantita<int> (15,10),
        new OggettoQuantita<int> (0,10),
        new OggettoQuantita<int> (18,10),
        new OggettoQuantita<int> (16,10),
        new OggettoQuantita<int> (46,10)
    };

    public static readonly List<OggettoQuantita<int>> inventarioLivello05 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2)
    };

    public static readonly List<OggettoQuantita<int>> inventarioLivello1 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2),
        new OggettoQuantita<int> (12,1),
        new OggettoQuantita<int> (15,1),
        new OggettoQuantita<int> (0,1),
        new OggettoQuantita<int> (18,1),
        new OggettoQuantita<int> (16,1),
        new OggettoQuantita<int> (46,1)
    };

    public static readonly List<OggettoQuantita<int>> inventarioLivello2 = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2),
        new OggettoQuantita<int> (12,2),
        new OggettoQuantita<int> (15,1),
        new OggettoQuantita<int> (0,1),
        new OggettoQuantita<int> (18,1),
        new OggettoQuantita<int> (16,1),
        new OggettoQuantita<int> (46,1)
    };

    public static readonly List<OggettoQuantita<int>> inventarioTest = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (0,10),
        new OggettoQuantita<int> (34,10),
        new OggettoQuantita<int> (33,10)
    };

    // INDENTAZIONE COLORI E CSS GENERALE
    public static readonly string fineColore = "</color>";
    public static readonly string colorePiatti = "<color=#FFA64C>";
    public static readonly string coloreIngredienti = "<color=#ffcc66>";
    public static readonly string coloreDieta = "<color=#64568c>";
    public static readonly string colorePatologia = "<color=#009082>";
    public static readonly string coloreVerde = "<color=#B6D89C>";
    public static readonly string grassetto = "<b>";
    public static readonly string fineGrassetto = "</b>";
    
    public static readonly string coloreInizio = "<color=#";

    // TUTORIAL
    public static readonly List<string> scritteZio = new List<string>
    {
        "Eccoti qua! Questo edificio è il nostro <color=#B5D99C>ristorante</color>, o meglio, il tuo.",
        "Spero che questo lavoro ti piacerà! E ricordati, sii sempre garbatƏ con i <color=#B5D99C>clienti</color>. Sapranno ricompensarti."
    };

    // NEGOZIO
    public static readonly int numeroBottoniNellaPaginaNegozio = 9;
    public static readonly int numeroPannelliXElementiNellaPaginaNegozio = 3;

    //BOTTONI NELL'ARRAY BOTTONEFAKEINGREDIENTE.GetComponentsInChildren <Button> ():
    public static readonly int posizionebottoneGenerale = 0;
    public static readonly int posizioneBottoneAumentaQuantita = 1;
    public static readonly int posizioneBottoneDiminuisciQuantita = 2;
    public static readonly int posizioneBottoneAggiungiIngredienteAlCarrello = 3;

    //MENU AIUTO
    public static readonly List<string> titoliMessaggiDiAiuto = new List<string>
    {
        "Comandi Movimento del gioco:",
        "Interazione con i clienti:",
        "Scelta del piatto - Parte 1:",
        "Scelta del piatto - Parte 2:",
        "Gestione magazzino:",
        "Come vengono calcolati I bonus:",
        "Interazione con gli NPC:",
        "Rifornirsi di ingredienti dal negozio:",
        "Visionare il ricettario:",
        "Criteri di vittoria e game Over:",
    };

    public static readonly List<string> messaggiDiAiuto = new List<string>
    {
        "Utilizza i tasti \"" + Costanti.coloreVerde + "W" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "A" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "S" + Costanti.fineColore + "\", " + Costanti.coloreVerde + "D" + Costanti.fineColore + "\" per muoveti rispettivamente in \"" +Costanti.coloreVerde + "Avanti" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "Sinistra" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "Indietro" + Costanti.fineColore + "\", \"" + Costanti.coloreVerde + "Destra" + Costanti.fineColore + "\". In più premi il tasto \"" + Costanti.coloreVerde + "Spazio" + Costanti.fineColore + "\" per saltare e tieni premuto il tasto \"" + Costanti.coloreVerde + "Shift" + Costanti.fineColore + "\" per correre mentre ti muovi.",
        "Per avviare l'interazione con un " + Costanti.coloreVerde + "cliente" + Costanti.fineColore +" al bancone inquadralo e premi il tasto \"" + Costanti.coloreVerde + "E" + Costanti.fineColore + "\". Per servire i clienti utilizza il mouse e selezionare il " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " da servire al cliente quando richiesto attraverso la relativa schermata.",
        "Scegliere il piatto migliore fra quelli disponibili ti permette di aumentare il tuo denaro e il tuo punteggio, così da poter superare il livello. Nel caso dovessi servire ad un <color=#B5D99C>cliente</color> con una certa " + Costanti.colorePatologia + "patologia" + Costanti.fineColore + " e/o " + Costanti.coloreDieta + "dieta"  + Costanti.fineColore + " un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore + " dove è presente un " + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " non compatibile con essa, verrà mostrato un pop up in cui potrai visualizzare quali degli " + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " del " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " sono compatibili con la " + Costanti.colorePatologia + "patologia" + Costanti.fineColore + " e quali no.",
        "Stesso discorso per un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore + " non compatibile con la " + Costanti.coloreDieta + "dieta" + Costanti.fineColore + " del " + Costanti.coloreVerde + "cliente" + Costanti.fineColore + ". Servire un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore +" non idoneo comporta delle penalità al punteggio e non si riceveranno bonus in denaro. I bonus e i malus vengono calcolati in base alla compatibilità del "+ Costanti.colorePiatti+ "piatto" + Costanti.fineColore + " e ai suoi volori <color=#B5D99C>nutriScore</color> e <color=#B5D99C>costoEco</color>.",
        "Sarà possibile scegliere solo i " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " per i quali " + Costanti.coloreVerde + "sono disponibili tutti gli ingredienti nelle quantità necessarie" + Costanti.fineColore + "; quindi, si dovrà tenere conto degli ingredienti disponibili nel proprio magazzino e comprare gli ingredienti mancanti. É possibile visionoare lo stato del magazzino aprendo il programma \"" + Costanti.coloreVerde + "MyInventory" + Costanti.fineColore + "\", accessibile nel computer presente nell'ufficio del ristorante.",
        "I bonus della vendita del " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " è calcolato come segue: prezzo finale = prezzo base (somma dei costi di tutti gli ingredienti + 10%) + bonus affinità (+5% se il piatto è affine al cliente, -5% se non lo è) + extra bonus pari al 3%, 2% o 1% se il " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " servito è rispettivamente il primo, secondo o terzo migliore servibile. Il puntegggio invece parte da 100 se viene servito un piatto affine, -10 altrimenti. Vengono aggiunti bonus in base al <color=#B5D99C>nutriScore</color> e al <color=#B5D99C>costoEco</color> calcolati come segue:",
        "Interagire con gli <color=#B5D99C>NPC</color> in giro per la città permetterà di ottenere <color=#B5D99C>suggerimenti</color> utili per servire piatti migliori, sia dal punto di vista dell’affinità le patologie che dal punto di vista del " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore +  " e dell'" + Costanti.coloreVerde +  "costoEco" + Costanti.fineColore + ".",
        "Per acquistare nuovi " + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "per il ristorante, bisogna recarsi al " + Costanti.coloreVerde + "negozio" + Costanti.fineColore + ". Interagendo con il negoziante, sarà possibile scegliere gli ingredienti da acquistare con i soldi a disposizione.",
        "Sul " + Costanti.coloreVerde + "ricettario" + Costanti.fineColore + " è possibile visionare tutte le ricette per i " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " e gli " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " presenti nel gioco. In più è possibile visionare anche la scheda tecnica di un "  + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " e di un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore +" per visionare le sue caratteristiche (" + Costanti.coloreVerde + "costoEco" + Costanti.fineColore + "- " + Costanti.coloreVerde + "nutriScore " + Costanti.fineColore + "- breve descrizione).",
        "Ad inizio livello verrano visualizzati i criteri per terminare un livello, come per esempio servire un numero X di clienti e raggiungere un punteggio pari a Y. É possibile visionare il progresso degli obbiettivi nella sezione obbiettivi in alto a destra. Una volta superati entrambi gli obbiettivi, il livello sarà considerato terminato e si avvia la schermata di fine livello dopo è possibile avere un breve riepilogo e tornare al menu principale. Il game over, invece, avviene se dopo X clienti serviti, non si raggiungono gli obbiettivi prefissati, oppure non si ha a disposizione il denaro per acquisire nuovi ingredienti. In tal caso, si avvia la schermata di game Over, dove è possibile tornare al menu principale."
    };

    public static readonly List<string> nomiAnimazioni = new List<string>
    {
        "movimenti",
        "interazioneConIClientiPrimaParte",
        "interazioneConIClienti",
        "interazioneConIClientiSecondaParte",
        "visualizzareMagazzino",
        "bonus",
        "interazionePassanti",
        "negozio",
        "ricettario",
        "vuota",
    };


    // ALTRO
    public static readonly int numeroLivelli = 3;

    public static readonly int percentualeGuadagnoSulPiatto = 10;

}
