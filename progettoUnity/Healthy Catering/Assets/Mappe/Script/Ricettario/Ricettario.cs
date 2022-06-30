using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ricettario : MonoBehaviour
{
    [SerializeField] GameObject pannelloPrincipale;
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

    private string coloreIngrediente = "<color=#ffcc66>";
    private string colorePiatto = "<color=#ffa64c>";
    private string fineColore = "</color>";

    void Start()
    {
        pannelloPrincipale.SetActive(true);

        switchToPiattiView();
    }

    private void Update()
    {
        print("avanti: " + avanti.IsInteractable().ToString());
        print("indietro: " + indietro.IsInteractable().ToString());
    }

    private void attivaDisattivaAvantiIndietro()
    {
        if (isVistaPiatto)
        {
            if (numeroPaginaPiatti == 0)
                setBottoniAvantiIndietroInteractable(true, false);
            else if (numeroPaginaPiatti == databasePiatti.Count - 1)
                setBottoniAvantiIndietroInteractable(false, true);
            else
                setBottoniAvantiIndietroInteractable(true, true);
        }
        else
        {
            if (numeroPaginaIngredienti == 0)
                setBottoniAvantiIndietroInteractable(true, false);
            else if (numeroPaginaIngredienti == databaseIngredienti.Count - 1)
                setBottoniAvantiIndietroInteractable(false, true);
            else
                setBottoniAvantiIndietroInteractable(true, true);
        }
    }

    private void setBottoniAvantiIndietroInteractable(bool avantiAttivo, bool indietroAttivo)
    {
        avanti.interactable = avantiAttivo;
        indietro.interactable = indietroAttivo;
    }

    public void switchToIngredientiView()
    {
        isVistaPiatto = false;
        resetIndiciPagina();

        setTestoSchermata();
        setSwitchPiattiIngredienti();
    }

    public void switchToPiattiView()
    {
        isVistaPiatto = true;
        resetIndiciPagina();

        setTestoSchermata();
        setSwitchPiattiIngredienti();
    }

    private void setSwitchPiattiIngredienti()
    {
        if (isVistaPiatto)
        {
            switchIngredienti.interactable = true;
            switchPiatti.interactable = false;
        }
        else
        {
            switchIngredienti.interactable = false;
            switchPiatti.interactable = true;
        }
    }

    private void resetIndiciPagina()
    {
        numeroPaginaIngredienti = 0;
        numeroPaginaPiatti = 0;
    }

    private void setTestoSchermata(string titolo, string testo)
    {
        titoloSchermata.text = titolo + fineColore;
        testoSchermata.text = testo + fineColore;
    }

    private void setTestoSchermata()
    {
        if (isVistaPiatto)
        {
            setTestoSchermata(
                colorePiatto + databasePiatti[numeroPaginaPiatti].nome,
                coloreIngrediente + databasePiatti[numeroPaginaPiatti].getListaIngredientiQuantitaToString()
            );
        }
        else
        {
            setTestoSchermata(
               coloreIngrediente + databaseIngredienti[numeroPaginaIngredienti].nome,
               colorePiatto + databaseIngredienti[numeroPaginaIngredienti].getListaPiattiRealizzabiliConIngredienteToSingolaString(databaseIngredienti, databasePiatti)
            );
        }
        attivaDisattivaAvantiIndietro();
    }

    public void avantiPagina()
    {
        if (isVistaPiatto)
        {
            if (numeroPaginaPiatti != databasePiatti.Count - 1)
                numeroPaginaPiatti++;
            setTestoSchermata();
        }
        else
        {
            if (numeroPaginaIngredienti != databaseIngredienti.Count - 1)
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
}
