using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PannelloMagazzino : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMagazzino;
    private bool pannelloMagazzinoAperto;
    public static bool pannelloMagazzinoApertoPerTutorial = false;
    [SerializeField] private Image sfondoImmaginePC;
    [SerializeField] private Button tastoX;

    //mi serve per settare il parent dell'oggetto sotto a questo oggetto, poi se la vede unity a sistemarli all'interno della schermata
    [SerializeField] private GameObject pannelloMostraInventario;
    private bool pannelloMostraInventarioAperto;

    [SerializeField] private GameObject pannelloXElementi;
    private GameObject copiaPannelloXElementi;
    [SerializeField] private GameObject pannelloInventarioCanvas;
    private int bottoniMassimiPerPannelloXElementi = 4;
    private int numeroPannelliXElementiPresenti = 1;
    private Button bottoneIngredienteTemplate;
    GameObject copiaPannelloMostraInventario;

    [SerializeField] private PannelloMostraRicette pannelloMostraRicette;

    private bool schermataMagazzinoPopolata;

    [SerializeField] private TextMeshProUGUI testoInventarioVuoto;
    private string testoInventarioVuotoString = "Inventario magazzino vuoto";

    private Player giocatore;

    private void Start()
    {
        cambiaSfondoDesktop();
        pannelloMagazzino.SetActive(false);
        //creo una copia del bottone template
        bottoneIngredienteTemplate = Instantiate(pannelloXElementi.GetComponentInChildren<Button>());
        //poi lo elimino dal pannello cosi che non ci sia piu' (non posso eliminare l'instanza successivamente siccome ci sono piu' pannelli 
        //al posto di uno solo come in menu (interazione cliente))
        pannelloXElementi = rimuoviTuttiFigliDaPannello(pannelloXElementi);
        schermataMagazzinoPopolata = false;
    }

    public void apriPannelloMagazzino(Player player)
    {
        giocatore = player;

        CambioCursore.cambioCursoreNormale();
        pannelloMagazzino.SetActive(true);
        pannelloMagazzinoAperto = true;
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        cambiaSfondoDesktop();

        pannelloInventarioCanvas.SetActive(false);

        copiaPannelloMostraInventario = Instantiate(pannelloMostraInventario);
        copiaPannelloXElementi = Instantiate(pannelloXElementi);

        if (!schermataMagazzinoPopolata)
            popolaSchermata();
        else
        {
            aggiornaSchermataMagazzino();
        }
    }

    public void chiudiPannelloMagazzino()
    {
        pannelloMagazzino.SetActive(false);
        pannelloMagazzinoAperto = false;
        pannelloMostraRicette.chiudiPannelloMostraRicette();

    }

    public bool getPannelloMagazzinoAperto()
    {
        return pannelloMagazzinoAperto;
    }

    public void cambiaSfondoMagazzino()
    {
        pannelloMagazzinoApertoPerTutorial = true;
        if (AspectRatio(Screen.height, Screen.width) == "4:3" || AspectRatio(Screen.height, Screen.width) == "3:4")
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SchermataMagazzino4_3");
        else
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SchermataMagazzino");
    }

    public void cambiaSfondoDesktop()
    {
        if (AspectRatio(Screen.height, Screen.width) == "4:3" || AspectRatio(Screen.height, Screen.width) == "3:4")
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SfondoBasePC4_3");
        else
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SfondoBasePC");
    }

    private void popolaSchermata()
    {   
        if (!giocatore.inventarioVuoto())
        {
            pannelloXElementi.SetActive(true);
            
            List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
            List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto()); //mi serve per vedere le ricette
            List<OggettoQuantita<int>> inventario = giocatore.inventario;

            int numeroBottoniAggiuntiFinoAdOraInPannelloXElementi = 0;

            foreach (OggettoQuantita<int> oggettoDellInventario in inventario)
            {
                if(oggettoDellInventario.quantita != 0)
                {
                    Button bottoneDaAggiungereTemp = creaBottoneConValoriIngrediente(oggettoDellInventario, bottoneIngredienteTemplate, databaseIngredienti, databasePiatti);

                    bottoneDaAggiungereTemp.transform.SetParent(pannelloXElementi.transform, false);
                    numeroBottoniAggiuntiFinoAdOraInPannelloXElementi++;

                    if (numeroBottoniAggiuntiFinoAdOraInPannelloXElementi > bottoniMassimiPerPannelloXElementi)
                    {
                        if (oggettoDellInventario != inventario[inventario.Count - 1]) // se e' diverso dall'ultimo elemento, previene che venga creato un pannello vuoto
                        {
                            aggiungiPannelloXElementi();
                            numeroBottoniAggiuntiFinoAdOraInPannelloXElementi = 0;
                        }
                    }
                }
            }

            schermataMagazzinoPopolata = true;

            if (!testoInventarioVuoto.text.Equals(""))
                testoInventarioVuoto.text = "";
        }
        else
        {
            testoInventarioVuoto.text = testoInventarioVuotoString;
            pannelloXElementi.SetActive(false);
        }
    }

    private Button creaBottoneConValoriIngrediente(OggettoQuantita<int> oggettoDellInventario, Button bottoneIngredienteTemplate, List<Ingrediente> databaseIngredienti, List<Piatto> databasePiatti)
    {
        Ingrediente ingrediente = Ingrediente.idToIngrediente(oggettoDellInventario.oggetto, databaseIngredienti);

        Button output = Instantiate(bottoneIngredienteTemplate);
        output.name = ingrediente.nome;

        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingrediente: " + Utility.coloreIngredienti + ingrediente.nome + Utility.fineColore;
        output.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Quantità presente: " + oggettoDellInventario.quantita.ToString();

        output.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            pannelloMostraRicette.apriPannelloMostraRicette(ingrediente, databaseIngredienti, databasePiatti);
            pannelloInventarioCanvas.SetActive(false);
        });

        return output;
    }

    private void aggiungiPannelloXElementi()
    {
        GameObject nuovoPannelloXElementi = Instantiate(copiaPannelloXElementi);
        nuovoPannelloXElementi = rimuoviTuttiFigliDaPannello(nuovoPannelloXElementi);

        nuovoPannelloXElementi.name = "SottoPannelloMostraInventarioXElementi" + numeroPannelliXElementiPresenti.ToString();
        nuovoPannelloXElementi.transform.SetParent(pannelloMostraInventario.transform, false);
        numeroPannelliXElementiPresenti++;

        //ora la variabile di prima viene usata per il nuovo pannello
        pannelloXElementi = nuovoPannelloXElementi;
    }

    private GameObject rimuoviTuttiFigliDaPannello(GameObject pannello)
    {
        foreach (Transform child in pannello.transform)
        {
            Destroy(child.gameObject);
        }

        return pannello;//non sono sicuro sia necessario il return del pannello, se non serve poi lo togliamo
    }

    private void aggiornaSchermataMagazzino()
    {
        /*
        si potrebbe aggiungere una gestione dinamica della cosa,
        senza distruggere e ricreare tutti i bottoni ogni volta ma significherebbe
        gestire la disposizione degli elementi nei pannelliXElementi e aggiornarli
        ogni volta che uno degli elementi non e' più presente nell'inventario:
        es:
        ho un pannelloXElementi con:
            ingrediente1
            ingrediente2
            ingrediente3
            ingrediente4
        mettiamo caso che ingrediente 2 finisca, cosa succede? 
        diventa grigio o viene rimosso completamente?
        se diventa grigio, la prossima volta che viene aperto il gioco quell'ingrediente 
        non è più nella lista
        se devo eliminarlo dalla lista, sopratutto se e' nel primo dei pannelliXElementi
        devo far scalare di posto tutti gli elementi dei pannelli successivi, il che 
        e' sia scomodo che, molto probabilmente, piu' pesante di ricreare i bottoni da capo
        quindi, almeno secondo e' meglio fare cosi
        */
        copiaPannelloMostraInventario.transform.SetParent(pannelloMostraInventario.transform.parent, false);

        foreach (Transform child in pannelloMostraInventario.GetComponentsInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }

        pannelloMostraInventario = copiaPannelloMostraInventario;
        pannelloMostraInventario = rimuoviTuttiFigliDaPannello(pannelloMostraInventario);


        numeroPannelliXElementiPresenti = 0;
        aggiungiPannelloXElementi();//nuovo primo pannello

        popolaSchermata();
    }

    /// <summary>
    /// Metodo che permette di calcolare l'aspect ratio del gioco.
    /// </summary>
    /// <param name="a">Altezza risoluzione</param>
    /// <param name="b">Larghezza risoluzione</param>
    /// <returns>Rapporto Altezza/risoluzione</returns>
    private string AspectRatio(int a, int b)
    {
        int r;
        int oa = a;
        int ob = b;
        while (b != 0)
        {
            r = a % b;
            a = b;
            b = r;
        }
        return (oa / a).ToString() + ":" + (ob / a).ToString();
    }

}
