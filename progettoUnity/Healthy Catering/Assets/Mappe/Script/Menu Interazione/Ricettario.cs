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

    private List<Ingrediente> databaseIngredienti = Costanti.databaseIngredienti;
    private List<Piatto> databasePiatti = Costanti.databasePiatti;

    private int numeroPaginaIngredienti = 0;
    private int numeroPaginaPiatti = 0;

    private bool isVistaPiatto = true;// all'inizio viene mostrata la pagina dei piatti

    private bool ricettarioAperto = false;

    private bool visualizzazioneNormale = true;

    public static bool apertoRicettario = false;//TUTORIAL

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
        titoloSchermata.text = titolo + Costanti.fineColore;
        testoSchermata.text = testo + Costanti.fineColore;
        testoBottoneAltriDati.text = testoBottoneSwitchVista;
    }

    private void setTestoSchermata()
    {
        if (isVistaPiatto)
        {
            setTestoSchermata(
                Costanti.colorePiatti + databasePiatti[numeroPaginaPiatti].nome,
                Costanti.coloreIngredienti + databasePiatti[numeroPaginaPiatti].getListaIngredientiQuantitaToString(),
                Costanti.colorePiatti + "Scheda tecnica del piatto"
            );
        }
        else
        {
            setTestoSchermata(
               Costanti.coloreIngredienti + databaseIngredienti[numeroPaginaIngredienti].nome,
               Costanti.colorePiatti + databaseIngredienti[numeroPaginaIngredienti].getListaPiattiRealizzabiliConIngredienteToSingolaString(),
               Costanti.coloreIngredienti + "Scheda tecnica dell'ingrediente"
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
            setTestoSchermata();
        }
        else
        {
            switchAVisualizzazioneAltriDati();
        }
    }

    private void switchAVisualizzazioneAltriDati()
    {
        if (isVistaPiatto)
        {
            testoSchermata.text = databasePiatti[numeroPaginaPiatti].ToString();
            testoBottoneAltriDati.text = "Lista ingredienti";
        }
        else
        {
            testoSchermata.text = databaseIngredienti[numeroPaginaIngredienti].ToString();
            testoBottoneAltriDati.text = "Lista ricette";
        }
    }


    /// <summary>
    /// Il metodo permette di aprire il pannello del ricettario ed impostando "ricettarioAperto" e "apertoRicettario" su true
    /// </summary>
    public void apriRicettario()
    {
        pannelloPrincipale.SetActive(true);

        switchToPiattiView();

        ricettarioAperto = true;

        apertoRicettario = true;//TUTORIAL
    }

    /// <summary>
    /// Il metodo chiude il pannello ricettario resettando l'indice e settando "ricettarioAperto" su false
    /// </summary>
    public void chiudiRicettario()
    {
        pannelloPrincipale.SetActive(false);

        resetIndiciPagina();

        ricettarioAperto = false;
    }

    /// <summary>
    /// Il metodo permette di ricevere in output la variabile booleana "ricettarioAperto" che permette di sapere se il ricettario è aperto o meno
    /// </summary>
    /// <returns>Booleano ricettario aperto</returns>
    public bool getRicettarioAperto()
    {
        return ricettarioAperto;
    }
}