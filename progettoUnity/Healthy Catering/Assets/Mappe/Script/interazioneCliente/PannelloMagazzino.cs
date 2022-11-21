using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PannelloMagazzino : MonoBehaviour
{
    [Header ("Gestione pannello PC")]
    [SerializeField] private GameObject pannelloMagazzino;
    private bool pannelloMagazzinoAperto;
    [SerializeField] private Image sfondoImmaginePC;
    [SerializeField] private Button tastoX;

    public static bool pannelloMagazzinoApertoPerTutorial = false;

    [Header("Suoni PC")]
    [SerializeField] private AudioSource suonoAperturaPC;
    [SerializeField] private AudioSource suonoChiusuraPC;

    [Header ("Pannello mostra inventario")]
    //mi serve per settare il parent dell'oggetto sotto a questo oggetto, poi se la vede unity a sistemarli all'interno della schermata
    [SerializeField] private GameObject pannelloMostraInventario;
    
    [SerializeField] private GameObject pannelloXElementi;
    private GameObject copiaPannelloXElementi;
    private int numeroPannelliXElementiPresenti = 1;
    
    [SerializeField] private GameObject pannelloInventarioCanvas;

    [SerializeField] private TextMeshProUGUI testoInventarioVuoto;

    [Header ("Bottone ingrediente")]
    private Button bottoneIngredienteTemplate;

    [Header ("altro")]
    [SerializeField] private PannelloMostraRicette pannelloMostraRicette;

    private Player giocatore;

    private List<GameObject> pannelliXElementiAttivi = new List<GameObject>();

    private void Start()
    {
        cambiaSfondoDesktop();
        pannelloMagazzino.SetActive(false);

        //creo una copia del bottone template
        bottoneIngredienteTemplate = Instantiate(pannelloXElementi.GetComponentInChildren<Button>());
        //poi lo elimino dal pannello cosi che non ci sia piu' (non posso eliminare l'instanza successivamente siccome ci sono piu' pannelli 
        //al posto di uno solo come in menu (interazione cliente))
        pannelloXElementi = rimuoviTuttiFigliDaPannello(pannelloXElementi);

        copiaPannelloXElementi = Instantiate(pannelloXElementi);
    }

    public void cambiaSfondoDesktop()
    {
        if (AspectRatio(Screen.height, Screen.width) == "4:3" || AspectRatio(Screen.height, Screen.width) == "3:4")
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SfondoBasePC4_3");
        else
            sfondoImmaginePC.sprite = Resources.Load<Sprite>("SfondoBasePC");
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

    private GameObject rimuoviTuttiFigliDaPannello(GameObject pannello)
    {
        foreach (Transform child in pannello.transform)
        {
            Destroy(child.gameObject);
        }

        return pannello;//non sono sicuro sia necessario il return del pannello, se non serve poi lo togliamo
    }

    public void apriPannelloMagazzino(Player player)
    {
        giocatore = player;

        pannelloInventarioCanvas.SetActive(false);
    
        popolaSchermata();

        setSchermataInizialePC();
    }

    private void setSchermataInizialePC()
    {
        CambioCursore.cambioCursoreNormale();
        pannelloMagazzino.SetActive(true);
        pannelloMagazzinoAperto = true;
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        cambiaSfondoDesktop();

        suonoAperturaPC.Play();
    }

    public void chiudiPannelloMagazzino()
    {
        pannelloMostraRicette.chiudiPannelloMostraRicette();
        pannelloMagazzino.SetActive(false);
        pannelloMagazzinoAperto = false;
        suonoChiusuraPC.Play();

        resetPannelloMagazzino();
    }

    private void resetPannelloMagazzino()
    {
        foreach (GameObject pannelloXElementiTemp in pannelliXElementiAttivi)
        {
            Destroy(pannelloXElementiTemp);
        }

        pannelliXElementiAttivi = new List<GameObject>();
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

    private void popolaSchermata()
    {   
        if (!giocatore.inventarioVuoto())
        {
            List<OggettoQuantita<int>> inventario = giocatore.inventario;
            
            pannelloXElementi.SetActive(true);

            int numeroBottoniAggiuntiFinoAdOraInPannelloXElementi = 0;

            foreach (OggettoQuantita<int> oggettoDellInventario in inventario)
            {
                if(oggettoDellInventario.quantita != 0)
                {
                    Button bottoneDaAggiungereTemp = creaBottoneConValoriIngrediente(oggettoDellInventario, bottoneIngredienteTemplate);

                    bottoneDaAggiungereTemp.transform.SetParent(pannelloXElementi.transform, false);
                    numeroBottoniAggiuntiFinoAdOraInPannelloXElementi++;

                    if ((numeroBottoniAggiuntiFinoAdOraInPannelloXElementi > Costanti.bottoniMassimiPerPannelloXElementi)
                        &&
                        (oggettoDellInventario != inventario[inventario.Count - 1])) // se e' diverso dall'ultimo elemento, previene che venga creato un pannello vuoto
                    {
                        pannelliXElementiAttivi.Add(pannelloXElementi);
                        aggiungiPannelloXElementi();
                        numeroBottoniAggiuntiFinoAdOraInPannelloXElementi = 0;
                    }
                }
            }

            if (!testoInventarioVuoto.text.Equals(""))
                testoInventarioVuoto.text = "";
        }
        else
        {
            testoInventarioVuoto.text = Costanti.testoInventarioVuotoString;
            pannelloXElementi.SetActive(false);
        }
    }

    private Button creaBottoneConValoriIngrediente(OggettoQuantita<int> oggettoDellInventario, Button bottoneIngredienteTemplate)
    {
        Ingrediente ingrediente = Ingrediente.idToIngrediente(oggettoDellInventario.oggetto);

        Button output = Instantiate(bottoneIngredienteTemplate);
        output.name = ingrediente.nome;

        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Ingrediente: " + Costanti.coloreIngredienti + ingrediente.nome + Costanti.fineColore;
        output.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Quantità presente: " + oggettoDellInventario.quantita.ToString();

        output.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            pannelloMostraRicette.apriPannelloMostraRicette(ingrediente);
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
}
