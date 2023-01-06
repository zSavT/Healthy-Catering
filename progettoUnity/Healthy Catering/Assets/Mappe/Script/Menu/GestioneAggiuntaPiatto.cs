using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe per gestire l'aggiunta di Piatto<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject vuoto Piatto
/// </para>
/// </summary>
public class GestioneAggiuntaPiatto : MonoBehaviour
{
    [Header("Pannelli")]
    private GameObject contenitorePiatto;

    [Header("Input Field")]
    [SerializeField] private TMP_InputField nomePiattoInputField;
    [SerializeField] private TMP_InputField descrizionePiattoInputField;

    [Header("Bottoni")]
    [SerializeField] private Button bottoneSalvaPiatto;
    [SerializeField] private Button bottoneAggiungi;
    [SerializeField] private Button bottoneRimuovi;

    [Header("Quantit‡")]
    [SerializeField] private Button bottonePi˘;
    [SerializeField] private Button bottoneMeno;
    [SerializeField] private TextMeshProUGUI quantit‡Valore;
    [SerializeField] private TextMeshProUGUI testoQuantit‡;

    [Header("DropDown")]
    [SerializeField] private TMP_Dropdown ingredientiDisponibili;
    [SerializeField] private TMP_Dropdown ingredientiInseriti;

    [Header("Testi Controlli")]
    [SerializeField] private TextMeshProUGUI testoPiattoGiaPresente;
    [SerializeField] private TextMeshProUGUI testoDescrizioneNonValida;

    [Header("Testo e Altro")]
    [SerializeField] private int numeroCaratteriMinimiDescrizione = 20;
    private List<OggettoQuantita<int>> listaIngredientiQuantit‡ = new List<OggettoQuantita<int>>();


    //BOOLEANI DI CONTROLLO
    private bool nomeValido = false;
    private bool descrizioneValida = false;
    private bool almenoUnIngrediente = false;

    // Start is called before the first frame update
    void Start()
    {
        inizializzaElementiPiatto();
    }

    // Update is called once per frame
    void Update()
    {
        if (contenitorePiatto.gameObject.activeInHierarchy)
            if (tuttiValoriCorrettiInseriti())
                bottoneSalvaPiatto.interactable = true;
            else
                bottoneSalvaPiatto.interactable = false;
        if(Input.GetKeyUp(KeyCode.F1))
        {
            foreach (OggettoQuantita<int> oggettoQuantita in listaIngredientiQuantit‡)
            {
                Debug.Log(Ingrediente.idToIngrediente(oggettoQuantita.oggetto).nome);
            }
        }    

    }

    /// <summary>
    /// Il metodo permette di impostare i placeHolder iniziali
    /// </summary>
    private void impostaPlaceHolder()
    {
        nomePiattoInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Imposta nome Piatto...";
        descrizionePiattoInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Imposta Descrizione Piatto...";

    }

    /// <summary>
    /// Il metodo resetta tutti i valori inseriti
    /// </summary>
    public void resetValoriInseriti()
    {
        nomePiattoInputField.text = string.Empty;
        descrizionePiattoInputField.text = string.Empty;
        aggiuntaElementiDropDownIngredienti(true);
        ingredientiDisponibili.RefreshShownValue();
        ingredientiInseriti.ClearOptions();
        ingredientiInseriti.RefreshShownValue();
        listaIngredientiQuantit‡.Clear();
        quantit‡Valore.text = "1";

    }

    /// <summary>
    /// Il metodo attiva tutti gli elementi della Piatto
    /// </summary>
    public void attivaVisualePiatto()
    {
        contenitorePiatto.SetActive(true);
    }

    /// <summary>
    /// Il metodo disattiva tutti gli elementi della Piatto
    /// </summary>
    public void disattivaVisualePiatto()
    {
        contenitorePiatto.SetActive(false);
    }

    /// <summary>
    /// Il metodo permette di inizializzare tutti i valori
    /// </summary>
    private void inizializzaElementiPiatto()
    {
        contenitorePiatto = this.gameObject;
        aggiuntaElementiDropDownIngredienti(true);
        testoDescrizioneNonValida.text = "Inserire pi˘ di " + numeroCaratteriMinimiDescrizione + " caratteri";
        aumentaQuantit‡Ingrediente();
        diminuisciQuantit‡Ingrediente();
        impostaPlaceHolder();
    }

    /// <summary>
    /// Il metodo controlla se il nome dell'ingrediente inserito Ë gi‡ presente, in tal caso blocca il salvataggio ed attiva il messaggio di errore!
    /// </summary>
    public void controlloNomePiattoInserito()
    {
        if (nomePiattoInputField.text.Equals(string.Empty))
        {
            testoPiattoGiaPresente.gameObject.SetActive(true);
            testoPiattoGiaPresente.text = "Il campo non puÚ essere vuoto";
            nomeValido = false;
        }
        else if (Piatto.checkPiattoOnonimoPresente(nomePiattoInputField.text))
        {
            testoPiattoGiaPresente.gameObject.SetActive(true);
            testoPiattoGiaPresente.text = "Piatto gi‡ presente!";
            nomeValido = false;
        }
        else
        {
            testoPiattoGiaPresente.gameObject.SetActive(false);
            nomeValido = true;
        }
    }

    /// <summary>
    /// Il metodo controlla se la descrizione inserita rispetta i criteri di dimensione <see cref="numeroCaratteriMinimiDescrizione"/>
    /// </summary>
    public void controlloDescrizionePiattoValida()
    {
        if (descrizionePiattoInputField.text.Length > numeroCaratteriMinimiDescrizione)
        {
            testoDescrizioneNonValida.gameObject.SetActive(false);
            descrizioneValida = true;
        }
        else
        {
            descrizioneValida = false;
            testoDescrizioneNonValida.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Il metodo controlla se tutti i criteri per la creazione della Piatto sono validi
    /// </summary>
    /// <returns>booleano True: Tutte i vincoli rispettati, False: Uno o pi˘ vincoli non rispetati</returns>
    private bool tuttiValoriCorrettiInseriti()
    {
        return nomeValido && descrizioneValida && almenoUnIngrediente;
    }

    /// <summary>
    /// Il metodo permette di aggiungere un elemento al dropdown degli ingredienti inseriti
    /// </summary>
    /// <param name="pulisciPrima">booleano True: Elimina i valori gi‡ presenti prima, False: Non rimuove prima i valori gi‡ presenti</param>
    private void aggiuntaElementiDropDownIngredienti(bool pulisciPrima)
    {
        if(pulisciPrima)
            ingredientiDisponibili.ClearOptions();
        foreach (Ingrediente temp in Costanti.databaseIngredienti)
        {
            Utility.aggiungiElementoDropDown(ingredientiDisponibili, new TMP_Dropdown.OptionData(temp.nome));
        }
    }

    /// <summary>
    /// Il metodo gestisce l'inserimento di un nuovo ingrediente al piatto nuovo
    /// </summary>
    public void aggiuntaElementoDropDownIngredienti()
    {
        Utility.aggiungiElementoDropDown(ingredientiInseriti, new TMP_Dropdown.OptionData(ingredientiDisponibili.options[ingredientiDisponibili.value].text));
        Utility.rimuoviElementoDropDown(ingredientiDisponibili, ingredientiDisponibili.options[ingredientiDisponibili.value].text);
        refreshValoriDropdownIngredientiInseriti(true);
    }

    /// <summary>
    /// Il metodo aggiorna i valori presenti nel dropdown
    /// </summary>
    /// <param name="inserimento">bool, True: Aggiunge il nuovo ingrediente al piatto, False: Non inserire l'ingrediente</param>
    private void refreshValoriDropdownIngredientiInseriti(bool inserimento)
    {

        ingredientiInseriti.value = ingredientiInseriti.options.Count()-1;
        ingredientiInseriti.RefreshShownValue();
        if (inserimento)
        {
            listaIngredientiQuantit‡.Add(new OggettoQuantita<int>(Ingrediente.getIngredienteDaNome(ingredientiInseriti.options[ingredientiInseriti.value].text).idIngrediente, 1));
            almenoUnIngrediente = true;
        }
        aggiornaTestoQuantit‡Ingrediente();
    }



    /// <summary>
    /// Il metodo permette di rimuovere l'ingrediente selezionato nella lista di ingredienti aggiunti al nuovo piatto.
    /// </summary>
    public void RimuoviElementoDropDownIngredienti()
    {
        rimuoviDaListaIngredientiIngrediente(ingredientiInseriti.options[ingredientiInseriti.value].text);
        Utility.aggiungiElementoDropDown(ingredientiDisponibili, new TMP_Dropdown.OptionData(ingredientiInseriti.options[ingredientiInseriti.value].text));
        Utility.rimuoviElementoDropDown(ingredientiInseriti, ingredientiInseriti.options[ingredientiInseriti.value].text);
        refreshValoriDropdownIngredientiInseriti(false);
    }

    /// <summary>
    /// Il metodo aggiorna il testo della label della quantit‡ ingredienti in base all'ingrediente selezionato.
    /// <para>Se la lista degli ingredienti inseriti Ë vuota, il valore inserito non comprende il nome di nessun ingrediente</para>
    /// </summary>
    public void aggiornaTestoQuantit‡Ingrediente()
    {
        if (ingredientiInseriti.options.Count() > 0)
        {
            testoQuantit‡.text = "Quantit‡ ingrediente \"" + ingredientiInseriti.options[ingredientiInseriti.value].text + "\":";
            aggiornaValoreQuantit‡IngredienteSelezionato(ingredientiInseriti.value);
        }
        else
            testoQuantit‡.text = "Quantit‡ ingrediente:";
    }

    /// <summary>
    /// Il metodo permette di aggiornare il valore della quantit‡ in base all'ingrediente selezionato nella lista di OggettiQuantit‡
    /// </summary>
    /// <param name="quantit‡"></param>
    /// <param name="nomeIngredienteDaAggiornare"></param>
    private void aggiornaQuantit‡IngredienteInserito(int quantit‡, string nomeIngredienteDaAggiornare)
    {
       foreach (OggettoQuantita<int> oggettoQuantita in listaIngredientiQuantit‡)
        {
            if (oggettoQuantita.oggetto.Equals(Ingrediente.getIngredienteDaNome(nomeIngredienteDaAggiornare).idIngrediente))
            {
                oggettoQuantita.quantita = quantit‡;
                break;
            }
        }
    }

    /// <summary>
    /// Il metodo permette di rimuovere l'ingrediente dalla lista degli ingredienti inseriti. 
    /// <para>Se la lista degli ingredienti inseriti Ë vuota, la variabile almeno <strong>UnIngrediente</strong> viene settato su false</para>
    /// </summary>
    /// <param name="nomeIngredienteDaRimuovere">string nome dell'ingrediente da eliminare</param>
    private void rimuoviDaListaIngredientiIngrediente(string nomeIngredienteDaRimuovere)
    {
        foreach (OggettoQuantita<int> oggettoQuantita in listaIngredientiQuantit‡)
        {
            if (oggettoQuantita.oggetto.Equals(Ingrediente.getIngredienteDaNome(nomeIngredienteDaRimuovere).idIngrediente))
            {
                listaIngredientiQuantit‡.Remove(oggettoQuantita);
                if (listaIngredientiQuantit‡.Count() == 0)
                    almenoUnIngrediente = false;
                break;
            }
        }
    }

    /// <summary>
    /// Il metodo permette di aumentare di una unit‡ numerica il valore della quantit‡ numerica
    /// </summary>
    private void aumentaQuantit‡Ingrediente()
    {
        bottonePi˘.onClick.AddListener(() =>
        {
            quantit‡Valore.text = (int.Parse(quantit‡Valore.text) + 1).ToString();
            aggiornaQuantit‡IngredienteInserito(int.Parse(quantit‡Valore.text), ingredientiInseriti.options[ingredientiInseriti.value].text);
        }
        );
    }

    /// <summary>
    /// Il metodo permette di diminuire di una unit‡ numerica il valore della quantit‡ numerica
    /// </summary>
    private void diminuisciQuantit‡Ingrediente()
    {
        bottoneMeno.onClick.AddListener(() =>
        {
            quantit‡Valore.text = (int.Parse(quantit‡Valore.text) - 1).ToString();
            if ((int.Parse(quantit‡Valore.text) < 1))
                quantit‡Valore.text = "1";
            aggiornaQuantit‡IngredienteInserito(int.Parse(quantit‡Valore.text), ingredientiInseriti.options[ingredientiInseriti.value].text);
        }
        );
    }

    /// <summary>
    /// Il metodo permette di aggiornare il valore del testo della quantit‡ dell'ingrediente selezionato per il piatto
    /// </summary>
    /// <param name="index"></param>
    public void aggiornaValoreQuantit‡IngredienteSelezionato(int index)
    {
        foreach (OggettoQuantita<int> oggettoQuantita in listaIngredientiQuantit‡)
        {
            if (oggettoQuantita.oggetto.Equals(Ingrediente.getIngredienteDaNome(ingredientiInseriti.options[index].text).idIngrediente))
            {
                quantit‡Valore.text = oggettoQuantita.quantita.ToString();
                break;
            }
        }
    }

    /// <summary>
    /// Il metodo permette di creare ed aggiungere la Piatto nel database e su file
    /// </summary>
    public void creaPiatto()
    {
        Piatto nuovo = new Piatto(nomePiattoInputField.text, descrizionePiattoInputField.text, listaIngredientiQuantit‡);
        Costanti.databasePiatti.Add(nuovo);
        Database.aggiornaDatabaseOggetto(Costanti.databasePiatti);
        Costanti.databasePiatti = Database.getDatabaseOggetto(new Piatto());
        resetValoriInseriti();
    }
}
