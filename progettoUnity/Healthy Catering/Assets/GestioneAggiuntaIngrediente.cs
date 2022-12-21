using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe per gestire l'aggiunta di ingredienti<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject vuoto ingredienti
/// </para>
/// </summary>
public class GestioneAggiuntaIngrediente : MonoBehaviour
{

    [Header("Pannelli")]
    private GameObject contenitoreIngredienti;

    [Header("Input Field")]
    [SerializeField] private TMP_InputField nomeIngredienteInputField;
    [SerializeField] private TMP_InputField descrizioneIngredienteInputField;
    [SerializeField] private TMP_InputField costoIngredienteInputField;
    [SerializeField] private TMP_InputField costoEcoIngredienteInputField;

    [Header("Dropdown")]
    [SerializeField] private TMP_Dropdown nutriScoreIngredienteDropDown;
    [SerializeField] private TMP_Dropdown dietaIngredienteDropDown;
    [SerializeField] private TMP_Dropdown patologieEsistentiDropDown;
    [SerializeField] private TMP_Dropdown patologieIngredienteDropDown;

    [Header("Bottoni")]
    [SerializeField] private Button bottoneAggiungiIngredieti;
    [SerializeField] private Button bottoneRimuoviIngredienti;

    [Header("Testi Controlli")]
    [SerializeField] private TextMeshProUGUI testoIngredienteGiaPresente;
    [SerializeField] private TextMeshProUGUI testoDescrizioneNonValida;
    [SerializeField] private TextMeshProUGUI testoCostoNonValida;

    [Header("Testo e Altro")]
    [SerializeField] private int numeroCaratteriMinimiDescrizione = 20;

    //BOOLEANI DI CONTROLLO
    private bool nomeValido = false;
    private bool descrizioneValida = false;

    // Start is called before the first frame update
    void Start()
    {
        inizializzaElementiIniziali();
    }

    /// <summary>
    /// Il metodo permette di inizializzare i valori iniziali
    /// </summary>
    private void inizializzaElementiIniziali()
    {
        contenitoreIngredienti = this.gameObject;
        popolaElementiDropDownPatologieEsistenti();
        patologieIngredienteDropDown.ClearOptions();
        controlloNomeIngredienteInserito();
        nomeValido = false;
        descrizioneValida = false;
        testoDescrizioneNonValida.text = "Inserire pi� di " + numeroCaratteriMinimiDescrizione + " caratteri";
    }

    /// <summary>
    /// Il metodo attiva tutti gli elementi degli ingredienti
    /// </summary>
    public void attivaVisualeIngredienti()
    {
        contenitoreIngredienti.SetActive(true);
    }

    /// <summary>
    /// Il metodo disattiva tutti gli elementi degli ingredienti
    /// </summary>
    public void disattivaVisualeIngredienti()
    {
        contenitoreIngredienti.SetActive(false);
    }

    /// <summary>
    /// Il metodo permette di rimuovere l'elemento nel dropdown passato in input
    /// </summary>
    /// <param name="dropdown">TMP_Dropdown dropdown da rimuovere l'elemento</param>
    /// <param name="nomeElementoDaRimuovere">string nome elemento da eliminare</param>
    private void rimuoviElementoDropDown(TMP_Dropdown dropdown, string nomeElementoDaRimuovere)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
            if (dropdown.options[i].text.Equals(nomeElementoDaRimuovere))
            {
                dropdown.options.RemoveAt(i);
                break;
            }
        dropdown.RefreshShownValue();
    }

    /// <summary>
    /// Il metodo permette di aggiungere l'elemento nel dropdown passato in input
    /// </summary>
    /// <param name="dropdown">TMP_Dropdown dropdown da aggiungere l'elemento</param>
    /// <param name="elementoDaAggiungere">string nome elemento da aggiungere</param>
    private void aggiungiElementoDropDown(TMP_Dropdown dropdown, TMP_Dropdown.OptionData elementoDaAggiungere)
    {
        dropdown.options.Add(elementoDaAggiungere);
        dropdown.RefreshShownValue();
    }

    //PATOLOGIE

    /// <summary>
    /// Il metodo permette di inserire tutte le patologie esistenti nel dropdown delle patologie esistenti
    /// </summary>
    public void popolaElementiDropDownPatologieEsistenti()
    {
        patologieEsistentiDropDown.ClearOptions();
        patologieEsistentiDropDown.AddOptions(Patologia.getListStringNomePatologie());
    }

    /*
    private List<string> getPatologie()
    {
        List<string> opzioni = new List<string>();

        foreach (Patologia temp in Costanti.databasePatologie)
            opzioni.Add(temp.nome);
        return opzioni;
    }
    */

    /// <summary>
    /// Il metodo permette di spostare la patologia selezionata nel dropdown patologieEsistentiDropDown nel dropdown patologieIngredienteDropDown
    /// </summary>
    public void aggiungiPatologieIngredienteNuovo()
    {
        if(patologieEsistentiDropDown.options.Count != 0)
        {
            aggiungiElementoDropDown(patologieIngredienteDropDown, new TMP_Dropdown.OptionData(patologieEsistentiDropDown.options[patologieEsistentiDropDown.value].text));
            rimuoviElementoDropDown(patologieEsistentiDropDown, patologieEsistentiDropDown.options[patologieEsistentiDropDown.value].text);
            bottoneAggiungiIngredieti.interactable = true;
            if (patologieEsistentiDropDown.options.Count == 0)
                bottoneAggiungiIngredieti.interactable = false;
        }

    }

    /// <summary>
    /// Il metodo permette di spostare la patologia selezionata nel dropdown patologieIngredienteDropDown nel dropdown patologieEsistentiDropDown
    /// </summary>
    public void rimuoviPatologiaIngredienteNuovo()
    {
        if (patologieIngredienteDropDown.options.Count != 0)
        {
            bottoneRimuoviIngredienti.interactable = true;
            aggiungiElementoDropDown(patologieEsistentiDropDown, new TMP_Dropdown.OptionData(patologieIngredienteDropDown.options[patologieIngredienteDropDown.value].text));
            rimuoviElementoDropDown(patologieIngredienteDropDown, patologieIngredienteDropDown.options[patologieIngredienteDropDown.value].text);
            if (patologieIngredienteDropDown.options.Count == 0)
                bottoneRimuoviIngredienti.interactable = false;
        }
    }

    /// <summary>
    /// Il metodo controlla se il nome dell'ingrediente inserito � gi� presente, in tal caso blocca il salvataggio ed attiva il messaggio di errore!
    /// </summary>
    public void controlloNomeIngredienteInserito()
    {
        if (nomeIngredienteInputField.text.Equals(string.Empty))
        {
            testoIngredienteGiaPresente.gameObject.SetActive(true);
            testoIngredienteGiaPresente.text = "Il campo non pu� essere vuoto";
            nomeValido = false;
        }
        else if (Ingrediente.checkIngredienteOnonimoGiaPresente(nomeIngredienteInputField.text))
        {
            testoIngredienteGiaPresente.gameObject.SetActive(true);
            testoIngredienteGiaPresente.text = "Ingrediente gi� presente!";
            nomeValido = false;
        }
        else
        {
            testoIngredienteGiaPresente.gameObject.SetActive(false);
            nomeValido = true;
        }  
    }

    public void controlloDescrizioneIngredienteValida()
    {
        if(descrizioneIngredienteInputField.text.Count() > numeroCaratteriMinimiDescrizione)
        {
            testoDescrizioneNonValida.gameObject.SetActive(false);
        }
        else
        {
            testoDescrizioneNonValida.gameObject.SetActive(true);
        }
    }

    public void controlloCostoIngredienteValido()
    {

    }


}

