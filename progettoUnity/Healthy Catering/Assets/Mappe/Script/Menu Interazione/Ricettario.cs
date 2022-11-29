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
    [SerializeField] TextMeshProUGUI tastoUscita;
    [SerializeField] Image[] immaginiControlli;
    private ControllerInput controllerInput;

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
        controllerInput= new ControllerInput();
        controllerInput.Enable();
        chiudiRicettario();
    }

    private void Update()
    {
        if (apertoRicettario)
            if (controllerInput.UI.Avanti.WasPressedThisFrame())
                avantiPagina();
            else if (controllerInput.UI.Indietro.WasPressedThisFrame())
                indietroPagina();
            else if (controllerInput.UI.Submit.WasPressedThisFrame())
                visualizzaAltriDati();
            else if (controllerInput.UI.Ingredienti.WasPressedThisFrame())
                switchToIngredientiView();
            else if (controllerInput.UI.Piatti.WasPressedThisFrame())
                switchToPiattiView();
    }

    /// <summary>
    /// Il metodo controlla se i tasti avanti e deitro devono essere interagibili o meno
    /// </summary>
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

    /// <summary>
    /// Imposta il bottene per andare avanti ed indietro nel ricettario attivi o meno
    /// </summary>
    /// <param name="avantiAttivo">Booleano per il tasto avanti interagibile</param>
    /// <param name="indietroAttivo">Booleano per il tasto indietro interagibile</param>
    private void setBottoniAvantiIndietroInteractable(bool avantiAttivo, bool indietroAttivo)
    {
        avanti.interactable = avantiAttivo;
        indietro.interactable = indietroAttivo;
    }

    /// <summary>
    /// Il metodo permette di cambiare la visualizzazione del ricettario in modalità ingredienti
    /// </summary>
    public void switchToIngredientiView()
    {
        isVistaPiatto = false;
        resetIndiciPagina();

        setTestoSchermata();
        setSwitchPiattiIngredienti();
        visualizzazioneNormale = false;
    }

    /// <summary>
    /// Il metodo permette di cambiare la visualizzazione del ricettario in modalità piatti
    /// </summary>
    public void switchToPiattiView()
    {
        isVistaPiatto = true;
        resetIndiciPagina();

        setTestoSchermata();
        setSwitchPiattiIngredienti();
        visualizzazioneNormale = false;
    }

    /// <summary>
    /// Imposta i valori booleani della modalità attualmente visualizzata nel ricettario (Se in modalità piatti o ingredienti)
    /// </summary>
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

    /// <summary>
    /// IL metodo resetta l'indice del ricettario per gli ingredienti e per i piatti
    /// </summary>
    private void resetIndiciPagina()
    {
        numeroPaginaIngredienti = 0;
        numeroPaginaPiatti = 0;
    }

    /// <summary>
    /// Il metodo imposta il testo nella schermata
    /// </summary>
    /// <param name="titolo">Titolo del piatto o dell'ingrediente</param>
    /// <param name="testo">Testo del piatto o dell'ingrediente</param>
    /// <param name="testoBottoneSwitchVista">Testo del bottone per la scheda tecnica o lista ingredienti\Piatti realizzabili</param>
    private void setTestoSchermata(string titolo, string testo, string testoBottoneSwitchVista)
    {
        titoloSchermata.text = titolo + Costanti.fineColore;
        testoSchermata.text = testo + Costanti.fineColore;
        testoBottoneAltriDati.text = testoBottoneSwitchVista;
    }

    /// <summary>
    /// Il metodo popola la schermata con il testo corretto da visualizzare del ingrediente o del piatto
    /// </summary>
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

    /// <summary>
    /// Il metodo permette di visualizzare la pagina successiva del ricettario
    /// </summary>
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
        visualizzazioneNormale = false;
    }

    /// <summary>
    /// Il metodo aggiorna l'indice del ricettario ed aggiorna la schermata in base all'indice
    /// </summary>
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
        visualizzazioneNormale = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void visualizzaAltriDati()
    {
        visualizzazioneNormale = !visualizzazioneNormale;

        if (!visualizzazioneNormale)
        {
            setTestoSchermata();
        }
        else
        {
            switchAVisualizzazioneAltriDati();
        }
    }

    /// <summary>
    /// Il metodo aggiorna la schermata in base alla tipologia di elemento selezionato tra ingrediente o piatto
    /// </summary>
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

        PlayerSettings.addattamentoSpriteComandi(testoSchermata);
        PlayerSettings.addattamentoSpriteComandi(tastoUscita);
        immaginiControlli[0].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("L1");
        immaginiControlli[1].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("R1");
        immaginiControlli[2].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("X");
        immaginiControlli[3].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("L2");
        immaginiControlli[4].GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("R2");
    }

    /// <summary>
    /// Il metodo chiude il pannello ricettario resettando l'indice e settando "ricettarioAperto" su false
    /// </summary>
    public void chiudiRicettario()
    {
        pannelloPrincipale.SetActive(false);

        resetIndiciPagina();

        ricettarioAperto = false;
        visualizzazioneNormale = false;
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