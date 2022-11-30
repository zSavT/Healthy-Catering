using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Costanti : MonoBehaviour
{
    // DATABASE OGGETTI
    public static readonly List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
    
    public static readonly List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto());

    public static readonly List<Patologia> databasePatologie = Database.getDatabaseOggetto(new Patologia());

    public static readonly List<Cliente> databaseClienti = Database.getDatabaseOggetto(new Cliente());

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

    public static readonly List<OggettoQuantita<int>> inventarioVuoto = new List<OggettoQuantita<int>>
    {
    };

    //TASTI

    public static TMP_SpriteAsset spriteTastiera;
    public static TMP_SpriteAsset spriteXbox;
    public static TMP_SpriteAsset spritePlaystation;

    //TASTI TASTIERA
    public static string tastoW = "<sprite=0>";
    public static string tastoS = "<sprite=1>";
    public static string tastoA = "<sprite=2>";
    public static string tastoD = "<sprite=3>";
    public static string tastoE = "<sprite=4>";
    public static string tastoH = "<sprite=5>";
    public static string tastoR = "<sprite=6>";
    public static string tastoShift = "<sprite=7>";
    public static string tastoSpazio = "<sprite=8>";
    public static string tastoClickSinistro = "<sprite=9>";
    public static string tastoEsc = "<sprite=10>";
    public static string tastoWASD = "<sprite=11>";

    //TASTI CONTROLLER XBOX


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
    public static readonly List<string> titoliMenuAiuto = new List<string>
    {
        "Comandi Movimento del gioco:",
        "Interazione con i clienti:",
        "Scelta del piatto - Parte 1:",
        "Scelta del piatto - Parte 2:",
        "Gestione magazzino:",
        "Calcolo dei Bonus:",
        "Interazione con i Passanti:",
        "Rifornirsi di ingredienti dal negozio:",
        "Visionare il ricettario:",
        "Criteri di vittoria e Game Over:",
    };
    public static readonly List<string> testiMenuAiuto = new List<string>
    {
        "Utilizza i tasti " + Costanti.coloreVerde + Costanti.tastoW + Costanti.fineColore + ", " + Costanti.coloreVerde + Costanti.tastoA + Costanti.fineColore + ", " + Costanti.coloreVerde + Costanti.tastoS + Costanti.fineColore + ", " + Costanti.coloreVerde + Costanti.tastoD + Costanti.fineColore + " per muoveti rispettivamente in " + Costanti.coloreVerde + "Avanti" + Costanti.fineColore + ", " + Costanti.coloreVerde + "Sinistra" + Costanti.fineColore + ", " + Costanti.coloreVerde + "Indietro" + Costanti.fineColore + ", " + Costanti.coloreVerde + "Destra" + Costanti.fineColore + ". In più premi il tasto " + Costanti.coloreVerde + Costanti.tastoSpazio + Costanti.fineColore + " per saltare e tieni premuto il tasto " + Costanti.coloreVerde + Costanti.tastoShift + Costanti.fineColore + " per correre mentre ti muovi.",
        "Per servire un cliente al bancone inquadralo e premi " + Costanti.coloreVerde + Costanti.tastoE + Costanti.fineColore + ". Utilizza il mouse e seleziona il " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " da servire attraverso la relativa schermata sulla sinistra.",
        "Scegliere il piatto migliore fra quelli disponibili ti permette di aumentare denaro e punteggio, così da poter superare il livello. Se servi un " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " dove è presente un " + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " non compatibile con la " + Costanti.colorePatologia + "patologia" + Costanti.fineColore + " e/o " + Costanti.coloreDieta + "dieta" + Costanti.fineColore + " del cliente, verrà mostrato un pop-up che riporta quali degli " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " sono compatibili e quali no.",
        "Servire un "  + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "non idoneo comporta penalità al punteggio e non si riceveranno bonus in denaro. Bonus e malus vengono calcolati in base alla compatibilità del piatto e ai suoi volori " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore +  " e " + Costanti.coloreVerde +  "costoEco" + Costanti.fineColore + ".",
        "Puoi scegliere solo " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " di cui sono disponibili tutti gli " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " nelle quantità necessarie. Devi tenere conto degli " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " disponibili nel magazzino e comprare quelli mancanti. Puoi visionoare lo stato del magazzino aprendo il programma " + Costanti.coloreVerde + "MyInventory" + Costanti.fineColore + ", accessibile nel computer nell'ufficio del ristorante.",
        "I bonus per la vendita del " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " è calcolato come segue:\n" + Costanti.coloreVerde + "Prezzo finale" + Costanti.fineColore + " = prezzo base (costo di tutti gli ingredienti + 10%)\n+ bonus affinità (+5% se il piatto è affine al cliente, -5% se non lo è)\n+ extra bonus (3%, 2% o 1% se il " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + " servito è rispettivamente il primo, secondo o terzo migliore servibile).\n" + Costanti.coloreVerde + "Puntegggio" + Costanti.fineColore + " = Valore base (100 se viene servito un piatto affine, -10 altrimenti). Sono poi aggiunti bonus in base al " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore + " e al " + Costanti.coloreVerde + "costoEco" + Costanti.fineColore + " calcolati come segue:",
        "Interagire con i " + Costanti.coloreVerde + "passanti" + Costanti.fineColore + " in giro per la città permetterà di ottenere " + Costanti.coloreVerde + "suggerimenti" + Costanti.fineColore + " utili per servire " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " migliori, sia dal punto di vista dell’affinità con le " + Costanti.colorePatologia + "patologie" + Costanti.fineColore + " che dal punto di vista del " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore +  " e dell'" + Costanti.coloreVerde +  "costoEco" + Costanti.fineColore + ". Per interagire con un passante, inquadralo e premi " + Costanti.tastoE + ".",
        "Per acquistare nuovi " + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "per il ristorante, bisogna recarsi al " + Costanti.coloreVerde + "negozio" + Costanti.fineColore + ". Interagendo con il negoziante, sarà possibile scegliere gli ingredienti da acquistare con i soldi a disposizione. Nel " + Costanti.coloreVerde + "negozio" + Costanti.fineColore + " seleziona la quantità dell'" + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " che ti interessa, aggiungila al carrello e poi prosegui con l'acquisto finale.",
        "Sul " + Costanti.coloreVerde + "ricettario" + Costanti.fineColore + " è possibile visionare tutte le ricette per i " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " e gli " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " presenti nel gioco. In più è possibile visionare anche la scheda tecnica di un "  + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " e di un " + Costanti.colorePiatti+ "piatto" + Costanti.fineColore +" per visionare le sue caratteristiche (" + Costanti.coloreVerde + "costoEco" + Costanti.fineColore + "- " + Costanti.coloreVerde + "nutriScore " + Costanti.fineColore + "- breve descrizione).",
        "All'inizio del livello sono mostrati i criteri di " + Costanti.coloreVerde + "vittoria" + Costanti.fineColore + " e " + Costanti.coloreVerde + "Game Over" + Costanti.fineColore + ", come ad esempio servire un certo numero di " + Costanti.coloreVerde + "clienti" + Costanti.fineColore + " e ottenere un certo " + Costanti.coloreVerde + "punteggio" + Costanti.fineColore + ".\nPuoi osservare il progresso degli obbiettivi in alto a destra. Una volta raggiunti tutti gli obbiettivi, il livello è superato e appare una schermata in cui è visibile un breve riepilogo con l'opzione per tornare al menu principale. Il Game Over avviene invece se, dopo aver servito il numero massimo di " + Costanti.coloreVerde + "clienti" + Costanti.fineColore + ", non si raggiungono i " + Costanti.coloreVerde + "restanti obbiettivi" + Costanti.fineColore + " oppure non si dispone di denaro sufficiente ad acquistare " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + ". In tal caso, si avvia la schermata di Game Over, dove è possibile tornare al menu principale."
    };
    public static readonly List<string> nomiAnimazioniMenuAiuto = new List<string>
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

    //PANNELLO MAGAZZINO
    public static readonly int bottoniMassimiPerPannelloXElementi = 4;
    public static readonly string testoInventarioVuotoString = "Inventario magazzino vuoto";

    //PANNELLO MENU
    public static readonly Color32 coloreTestoIngredientiGiusti = new Color32(104, 176, 60, 255);
    public static readonly Color32 coloreTestoIngredientiSbagliatiDieta = new Color32(255, 8, 10, 255);
    public static readonly Color32 coloreTestoIngredientiSbagliatiPatologia = new Color32(255, 8, 10, 255);     


    //TUTORIAL
    //check servito piatto compatibile e non
    public static readonly Piatto piattoCompatibileTutorial = new Piatto(
        "", 
        "", 
        new List<OggettoQuantita<int>>
        {
            new OggettoQuantita<int> (12,10),
            new OggettoQuantita<int> (15,10),
            new OggettoQuantita<int> (0,10),
            new OggettoQuantita<int> (18,10),
            new OggettoQuantita<int> (16,10),
            new OggettoQuantita<int> (46,10)
        }
    );
    public static readonly Piatto piattoNonCompatibileTutorial = new Piatto(
        "", 
        "", 
        new List<OggettoQuantita<int>>
        {
            new OggettoQuantita<int> (30,1),
            new OggettoQuantita<int> (35,1),
            new OggettoQuantita<int> (33,2)
        }
    );

    //OK BOX VIDEO
    public static readonly int WASD = 0;
    public static readonly int salto = 1;
    public static readonly int sprint = 2;
    public static readonly int parlaZio = 3;
    public static readonly int vaiAlRistorante = 4;
    public static readonly int meccanicheServireCompatibile = 5;
    public static readonly int meccanicheServireNonCompatibile = 6;
    public static readonly int finitiIngredienti = 7;
    public static readonly int doveEIlNegozio = 8;
    public static readonly int interazioneNPC = 9;
    public static readonly int apriRicettario = 10;
    public static readonly int apriMenuAiuto = 11;

    public static readonly List<string> titoliOkBoxVideo = new List<string>
    {
        "Comandi base per il movimento",
        "Comando Salto",
        "Comando Sprint",
        "Parla con tuo zio",
        "Entra nel ristorante",
        "Servire un piatto idoneo ad un cliente",
        "Servire un piatto non idoneo ad un cliente",
        "Come controllare il magazzino del ristorante",
        "Rifornire il Magazzino visitando il Negozio",
        "Acquisire informazioni interagendo con i passanti",
        "Aprire il ricettario",
        "Aprire il menu aiuto"
    };
    public static readonly List<string> testiOkBoxVideo = new List<string>
    {
        "Premi " + Costanti.coloreVerde + Costanti.tastoW + Costanti.fineColore + " per andare avanti, " + Costanti.coloreVerde + Costanti.tastoS + Costanti.fineColore + " per andare indietro, " + Costanti.coloreVerde + Costanti.tastoA + Costanti.fineColore + " per andare a sinistra, " + Costanti.coloreVerde + Costanti.tastoD + Costanti.fineColore + " per andare a destra. Il giocatore si muove verso la direzione inquadrata con il " + Costanti.coloreVerde + "mouse" + Costanti.fineColore + ". Ora prova a muoverti.",
        "Per saltare premi il tasto " + Costanti.coloreVerde + Costanti.tastoSpazio + Costanti.fineColore + ". Ora prova a saltare per proseguire.",
        "Per correre premi il tasto " + Costanti.coloreVerde + Costanti.tastoShift + Costanti.fineColore + ". Ora prova a correre.",
        "Per interagire con una persona premi " + Costanti.coloreVerde + Costanti.tastoE + Costanti.fineColore + ". Raggiungi tuo zio e interagisci con lui.",
        "Per entrare nel " + Costanti.coloreVerde + "ristorante" + Costanti.fineColore + " premi " + Costanti.coloreVerde + Costanti.tastoE + Costanti.fineColore + " una volta inquadrata la porta con il mouse. Fai lo stesso per uscire. Ora entra nel ristorante.",
        "Per servire un cliente al bancone inquadralo e premi " + Costanti.coloreVerde + "E" + Costanti.fineColore + ". Per scegliere un " + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "selezionalo dal menu sulla sinistra. È importante servire un " + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "idoneo al cliente, ovvero la " + Costanti.coloreDieta + "dieta " + Costanti.fineColore + "e le " + Costanti.colorePatologia + "patologie" + Costanti.fineColore + ". Servire piatti idonei permette di ricevere dei bonus. Ora prova a servire un piatto idoneo al cliente al bancone.",
        "Servire un "  + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "non idoneo comporta penalità al punteggio e non si riceveranno bonus in denaro. Bonus e malus vengono calcolati in base alla compatibilità del piatto e ai suoi volori " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore + " e " + Costanti.coloreVerde + "costoEco" + Costanti.fineColore + ". Consulta il " + Costanti.coloreVerde + "menu aiuto " + Costanti.fineColore + "per ulteriori informazioni. Ora prova a servire un piatto non idoneo al cliente al bancone.",
        "Controlla il " + Costanti.coloreVerde + "magazzino" + Costanti.fineColore + " per tener d'occhio quali " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " sono disponibili per la realizzazione dei " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + ". Servendo un " + Costanti.colorePiatti + "piatto" + Costanti.fineColore + ", diminuiscono nel magazzino le quantità di " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " che in esso figurano. Puoi verificare lo stato del magazzino dal " + Costanti.coloreVerde + "PC" + Costanti.fineColore + " presente in " + Costanti.coloreVerde + "ufficio" + Costanti.fineColore + " attraverso il programma " + Costanti.coloreVerde + "MyInventory" + Costanti.fineColore + ". Ora raggiungi l'ufficio e controlla lo stato del magazzino.",
        "Per fare rifornimenti di " + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "visita il " + Costanti.coloreVerde + "negozio " + Costanti.fineColore + "dove acquistare con i soldi guadagnati, quelli necessari a realizzare altri " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + ". Nel " + Costanti.coloreVerde + "negozio" + Costanti.fineColore + " seleziona la quantità dell'" + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + " che ti interessa, aggiungila al carrello e poi prosegui con l'acquisto finale. Ora esci dal " + Costanti.coloreVerde + "ristorante" + Costanti.fineColore + ", raggiungi il " + Costanti.coloreVerde + "negozio" + Costanti.fineColore + " e compra un " + Costanti.coloreIngredienti + "ingrediente" + Costanti.fineColore + ".",
        "Interagire con i " + Costanti.coloreVerde + "passanti" + Costanti.fineColore + " in giro per la città ti permette di ottenere " + Costanti.coloreVerde + "suggerimenti" + Costanti.fineColore + " utili a servire " + Costanti.colorePiatti + "piatti " + Costanti.fineColore + "migliori, sia per affinità con le " + Costanti.colorePatologia + "patologie" + Costanti.fineColore + " che per " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore + " e " + Costanti.coloreVerde + "costoEco" + Costanti.fineColore + ". Ora prova a parlare con una persona.",
        "Puoi utilizzare il " + Costanti.coloreVerde + "ricettario" + Costanti.fineColore + " quando e dove vuoi. Premi il tasto " + Costanti.coloreVerde + Costanti.tastoR + Costanti.fineColore + " per visualizzarlo e controllare le quantità di " + Costanti.coloreIngredienti + "ingredienti " + Costanti.fineColore + "di un " + Costanti.colorePiatti + "piatto " + Costanti.fineColore + "e i valori " + Costanti.coloreVerde + "nutriScore" + Costanti.fineColore + " e " + Costanti.coloreVerde + "costoEco" + Costanti.fineColore + " di " + Costanti.colorePiatti + "piatti" + Costanti.fineColore + " e " + Costanti.coloreIngredienti + "ingredienti" + Costanti.fineColore + " nelle loro schede tecniche.",
        "Puoi consultare il " + Costanti.coloreVerde + "menu aiuto" + Costanti.fineColore + " quando e dove vuoi premendo il tasto " + Costanti.coloreVerde + Costanti.tastoH + Costanti.fineColore + ". Al suo interno trovi tutte le informazioni e le meccaniche del gioco mostrate in precedenza.",
    };
    public static readonly List<string> nomiAnimazioniOkBoxVideo = new List<string>
    {
        "wasd",
        "salto",
        "shift",
        "interazioneZio",
        "entrareRistorante",
        "clienteCompatibile",
        "clienteNonCompatibile",
        "visualizzareMagazzino",
        "negozio",
        "interazionePassanti",
        "ricettario",
        "menuAiuto",
    };

    // ALTRO
    public static readonly int numeroLivelli = 3;

    public static readonly int percentualeGuadagnoSulPiatto = 10;

}
