using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ricettario : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titoloSchermata;
    [SerializeField] TextMeshProUGUI testoSchermata;
    [SerializeField] Button avanti;
    [SerializeField] Button indietro;
    [SerializeField] Button switchPiatti;
    [SerializeField] Button switchIngredienti;

    private List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
    private List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto());

    private int numeroPaginaIngredienti = 0;
    private int numeroPaginaPiatti = 0;

    private bool isVistaPiatto = true;// all'inizio viene mostrata la pagina dei piatti

    void Start()
    {
        switchToPiattiView();
    }

    private void attivaDisattivaAvantiIndietro()
    {
        if (numeroPaginaIngredienti == 0 || numeroPaginaPiatti == 0)
            setBottoniAvantiIndietroInteractable(true, false);
        //resetto i valori dei 2 indici ogni volta che cambio dal pannello degli ingredienti a quello dei piatti
        else if (numeroPaginaIngredienti == databaseIngredienti.Count || numeroPaginaPiatti == databasePiatti.Count)
            setBottoniAvantiIndietroInteractable(false, true);
        else
            setBottoniAvantiIndietroInteractable(true, true);
    }

    private void setBottoniAvantiIndietroInteractable(bool avantiAttivo, bool indietroAttivo)
    {
        avanti.interactable = avantiAttivo;
        indietro.interactable = indietroAttivo;
    }

    private void switchToIngredientiView()
    {
        isVistaPiatto = false;
        numeroPaginaIngredienti = 0;
       
        setTestoSchermata();
        switchPiatti.interactable = false;
    }

    private void switchToPiattiView()
    {
        isVistaPiatto = true;
        numeroPaginaPiatti = 0;

        setTestoSchermata();
        switchPiatti.interactable = false;
    }

    private void setTestoSchermata(string titolo, string testo)
    {
        titoloSchermata.text = titolo;
        testoSchermata.text = testo;
    }

    private void setTestoSchermata()
    {
        if (isVistaPiatto)
        {
            setTestoSchermata(
                databasePiatti[numeroPaginaPiatti].nome,
                databasePiatti[numeroPaginaPiatti].getListaIngredientiQuantitaToString()
            );
        }
        else
        {
            setTestoSchermata(
               databaseIngredienti[numeroPaginaIngredienti].nome,
               databaseIngredienti[numeroPaginaIngredienti].getListaPiattiRealizzabiliConIngredienteToSingolaString(databaseIngredienti, databasePiatti)
            );
        }
        attivaDisattivaAvantiIndietro();
    }

    public void avantiPagina()
    {
        if (isVistaPiatto)
        {
            if (numeroPaginaPiatti != databasePiatti.Count)
                numeroPaginaPiatti++;
            setTestoSchermata();
        }
        else
        {
            if (numeroPaginaIngredienti != databaseIngredienti.Count)
                numeroPaginaIngredienti++;
            setTestoSchermata();
        }
    }

    public void indietroPagina()
    {
        if (isVistaPiatto)
        {
            if (numeroPaginaPiatti != 0)
                numeroPaginaPiatti--;
            setTestoSchermata();
        }
        else
        {
            if (numeroPaginaIngredienti != 0)
                numeroPaginaIngredienti--;
            setTestoSchermata();
        }
    }

    private void Update()
    {
        
    }
}
