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
    [SerializeField] Button bottoneAltriDati;
    [SerializeField] TextMeshProUGUI testoBottoneAltriDati;

    private List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
    private List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto());

    private int numeroPaginaIngredienti = 0;
    private int numeroPaginaPiatti = 0;

    private bool isVistaPiatto = true;// all'inizio viene mostrata la pagina dei piatti

    private bool ricettarioAperto = false;

    private bool visualizzazioneNormale = true;

    private void Start()
    {
        chiudiRicettario();
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

    private void setTestoSchermata(string titolo, string testo, string testoBottoneSwitchVista)
    {
        titoloSchermata.text = titolo + Utility.fineColore;
        testoSchermata.text = testo + Utility.fineColore;
        testoBottoneAltriDati.text = testoBottoneSwitchVista;
    }

    private void setTestoSchermata()
    {
        if (isVistaPiatto)
        {
            setTestoSchermata(
                Utility.colorePiatti + databasePiatti[numeroPaginaPiatti].nome,
                Utility.coloreIngredienti + databasePiatti[numeroPaginaPiatti].getListaIngredientiQuantitaToString(),
                "Scheda tecnica del piatto"
            );
        }
        else
        {
            setTestoSchermata(
               Utility.coloreIngredienti + databaseIngredienti[numeroPaginaIngredienti].nome,
               Utility.colorePiatti + databaseIngredienti[numeroPaginaIngredienti].getListaPiattiRealizzabiliConIngredienteToSingolaString(databaseIngredienti, databasePiatti),
                "Scheda tecnica dell'ingrediente"
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

    public void visualizzaAltriDati()
    {
        visualizzazioneNormale = !visualizzazioneNormale;

        if (visualizzazioneNormale)
        {
            switchAVisualizzazioneAltriDati();
        }
        else
        {
            setTestoSchermata();
        }
    }

    private void switchAVisualizzazioneAltriDati()
    {
        if (isVistaPiatto)
        {
            testoSchermata.text = databasePiatti[numeroPaginaPiatti].ToString();
            testoBottoneAltriDati.text = "Visualizza ingredienti del piatto: " + databasePiatti[numeroPaginaPiatti].nome;
        }
        else
        {
            testoSchermata.text = databaseIngredienti[numeroPaginaIngredienti].ToString();
            testoBottoneAltriDati.text = "Visualizza ricette realizzabili con l'ingrediente: " + databaseIngredienti[numeroPaginaIngredienti].nome;
        }
    }


    public void apriRicettario()
    {
        pannelloPrincipale.SetActive(true);

        switchToPiattiView();

        ricettarioAperto = true;
    }

    public void chiudiRicettario()
    {
        pannelloPrincipale.SetActive(false);

        resetIndiciPagina();

        ricettarioAperto = false;
    }

    public bool getRicettarioAperto()
    {
        return ricettarioAperto;
    }
}
