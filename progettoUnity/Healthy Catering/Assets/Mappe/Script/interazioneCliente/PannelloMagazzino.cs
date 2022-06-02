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

    //mi serve per settare il parent dell'oggetto sotto a questo oggetto, poi se la vede unity a sistemarli all'interno della schermata
    [SerializeField] private GameObject pannelloMostraInventario;
    private bool pannelloMostraInventarioAperto;

    [SerializeField] private GameObject pannelloXElementi;
    private int bottoniMassimiPerPannelloXElementi = 4;
    private int numeroPannelliXElementiPresenti = 1;
    private Button bottoneIngredienteTemplate;

    [SerializeField] private PannelloMostraRicette pannelloMostraRicette;

    private bool schermataMagazzinoPopolata;

    private void Start()
    {
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
        pannelloMagazzino.SetActive(true);
        pannelloMagazzinoAperto = true;
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        
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
        sfondoImmaginePC.sprite = Resources.Load<Sprite>("SchermataMagazzino");
    }

    public void cambiaSfondoDesktop()
    {
        sfondoImmaginePC.sprite = Resources.Load<Sprite>("SfondoBasePC");
    }

    private void popolaSchermata()
    {
        List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
        List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto()); //mi serve per vedere le ricette
        List <OggettoQuantita <int>> inventario = Database.getPlayerDaNome (PlayerSettings.caricaNomePlayerGiocante ()).inventario;
        
        int bottoniAggiuntiFinoAdOra = 0;

        foreach (OggettoQuantita <int> oggettoDellInventario in inventario)
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

    private Button creaBottoneConValoriIngrediente(OggettoQuantita<int> oggettoDellInventario, Button bottoneIngredienteTemplate, List <Ingrediente> databaseIngredienti, List<Piatto> databasePiatti)
    {
        Ingrediente ingrediente = Ingrediente.idToIngrediente(oggettoDellInventario.oggetto, databaseIngredienti);
     
        Button output = Instantiate(bottoneIngredienteTemplate);
        output.name = ingrediente.nome;

        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingrediente: " + ingrediente.nome;
        output.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Quantità presente: " + oggettoDellInventario.quantita.ToString ();

        output.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            pannelloMostraRicette.apriPannelloMostraRicette(ingrediente, databaseIngredienti, databasePiatti);
        }); 

        return output;
    }
    
    private void aggiungiPannelloXElementi()
    {
        GameObject nuovoPannelloXElementi = Instantiate (pannelloXElementi);
        nuovoPannelloXElementi = rimuoviTuttiFigliDaPannello(nuovoPannelloXElementi);

        nuovoPannelloXElementi.name = "SottoPannelloMostraInventarioXElementi" + numeroPannelliXElementiPresenti.ToString ();
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
}
