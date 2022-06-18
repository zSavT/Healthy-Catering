using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PannelloMagazzino : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMagazzino;
    private bool pannelloMagazzinoAperto;
    [SerializeField] private Image sfondoImmaginePC;
    [SerializeField] private Button tastoX;

    //mi serve per settare il parent dell'oggetto sotto a questo oggetto, poi se la vede unity a sistemarli all'interno della schermata
    [SerializeField] private GameObject pannelloMostraInventario;
    private bool pannelloMostraInventarioAperto;

    [SerializeField] private GameObject pannelloXElementi;
    [SerializeField] private GameObject pannelloInventarioCanvas;
    private int bottoniMassimiPerPannelloXElementi = 4;
    private int numeroPannelliXElementiPresenti = 1;
    private Button bottoneIngredienteTemplate;

    [SerializeField] private PannelloMostraRicette pannelloMostraRicette;

    private bool schermataMagazzinoPopolata;

    [SerializeField] private TextMeshProUGUI testoInventarioVuoto;
    private string testoInventarioVuotoString = "Non hai alcun ingrediente nel tuo inventario, vai al negozio per acquistarne altri";

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

    public void apriPannelloMagazzino()
    {
        CambioCursore.cambioCursoreNormale();
        pannelloMagazzino.SetActive(true);
        pannelloMagazzinoAperto = true;
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        cambiaSfondoDesktop();
        if (!schermataMagazzinoPopolata)
            popolaSchermata();
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
        List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
        List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto()); //mi serve per vedere le ricette
        List<OggettoQuantita<int>> inventario = Database.getPlayerDaNome(PlayerSettings.caricaNomePlayerGiocante()).inventario;

        int bottoniAggiuntiFinoAdOra = 0;

        foreach (OggettoQuantita<int> oggettoDellInventario in inventario)
        {
            Button bottoneDaAggiungereTemp = creaBottoneConValoriIngrediente(oggettoDellInventario, bottoneIngredienteTemplate, databaseIngredienti, databasePiatti);
            bottoneDaAggiungereTemp.transform.SetParent(pannelloXElementi.transform, false);
            bottoniAggiuntiFinoAdOra++;

            if (bottoniAggiuntiFinoAdOra > bottoniMassimiPerPannelloXElementi)
            {
                if (oggettoDellInventario != inventario[inventario.Count - 1]) // se e' diverso dall'ultimo elemento, previene che venga creato un pannello vuoto
                {
                    aggiungiPannelloXElementi();
                    bottoniAggiuntiFinoAdOra = 0;
                }
            }
        }

        schermataMagazzinoPopolata = true;
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
        GameObject nuovoPannelloXElementi = Instantiate(pannelloXElementi);
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
