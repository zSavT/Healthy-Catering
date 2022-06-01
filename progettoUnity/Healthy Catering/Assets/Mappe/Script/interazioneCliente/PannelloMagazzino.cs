/*
    
 
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PannelloMagazzino : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMagazzino;
    [SerializeField] private Image sfondoImmaginePC;

    //mi serve per settare il parent dell'oggetto sotto a questo oggetto, poi se la vede unity a sistemarli all'interno della schermata
    [SerializeField] private GameObject PannelloMostraInventario;
    [SerializeField] private GameObject sottoPannelloMostraInventarioXElementi;
    private int bottoniMassimiPerPannelloMostraInventarioXElementi = 5;
    private int numeroPannelliMostraInventarioXElementiPresenti = 1;
    private Button mostraSingoloIngredienteTemplate;

    private void Start()
    {
        pannelloMagazzino.SetActive(false);
        mostraSingoloIngredienteTemplate = sottoPannelloMostraInventarioXElementi.GetComponentInChildren<Button>();
    }

    public void attivaPannello()
    {
        pannelloMagazzino.SetActive(true);
        popolaSchermata();
    }

    private void popolaSchermata()
    {
        List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
        Player giocatore = Database.getPlayerDaNome (PlayerSettings.caricaNomePlayerGiocante ());
        

        int bottoniAggiuntiFinoAdOra = 0;

        foreach (OggettoQuantita <int> oggettoDellInventario in giocatore.inventario)
        {
            Button bottoneDaAggiungereTemp = creaBottoneConValoriSingoloIngrediente(oggettoDellInventario, mostraSingoloIngredienteTemplate, databaseIngredienti);
            
            bottoneDaAggiungereTemp.transform.SetParent(sottoPannelloMostraInventarioXElementi.transform, false);
            bottoniAggiuntiFinoAdOra++;

            if (bottoniAggiuntiFinoAdOra > bottoniMassimiPerPannelloMostraInventarioXElementi)
            {
                aggiungiPannelloMostraInventarioXElementi();
                bottoniAggiuntiFinoAdOra = 0;
            }
        }

        Destroy(mostraSingoloIngredienteTemplate);
    }

    private Button creaBottoneConValoriSingoloIngrediente(OggettoQuantita<int> oggettoDellInventario, Button mostraSingoloIngredienteTemplate, List <Ingrediente> databaseIngredienti)
    {
        Ingrediente temp = Ingrediente.idToIngrediente(oggettoDellInventario.oggetto, databaseIngredienti);
     
        Button output = Instantiate(mostraSingoloIngredienteTemplate);

        output.name = temp.nome;

        output.GetComponentsInChildren<TextMeshProUGUI>()[0].text = temp.nome;
        output.GetComponentsInChildren<TextMeshProUGUI>()[1].text = oggettoDellInventario.quantita.ToString ();

        return output;
    }
    
    private void aggiungiPannelloMostraInventarioXElementi()
    {
        GameObject nuovoSottoPannelloMostraInventarioXElementi = Instantiate (sottoPannelloMostraInventarioXElementi);
        numeroPannelliMostraInventarioXElementiPresenti++;
        nuovoSottoPannelloMostraInventarioXElementi.name = "SottoPannelloMostraInventarioXElementi" + numeroPannelliMostraInventarioXElementiPresenti.ToString ();

        nuovoSottoPannelloMostraInventarioXElementi.transform.SetParent(PannelloMostraInventario.transform, false);
        //ora la variabile di prima viene usata per il nuovo pannello
        sottoPannelloMostraInventarioXElementi = nuovoSottoPannelloMostraInventarioXElementi;
    }

    public void cambiaSfondo()
    {
        sfondoImmaginePC.sprite = Resources.Load<Sprite>("SchermataMagazzino");
    }

}
