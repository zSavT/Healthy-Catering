using System.Collections;
using System.Collections.Generic;
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

    [Header("DropDown")]
    [SerializeField] private TMP_Dropdown ingredientiDisponibili;
    [SerializeField] private TMP_Dropdown ingredientiInseriti;

    [Header("Testi Controlli")]
    [SerializeField] private TextMeshProUGUI testoPiattoGiaPresente;
    [SerializeField] private TextMeshProUGUI testoDescrizioneNonValida;

    [Header("Testo e Altro")]
    [SerializeField] private int numeroCaratteriMinimiDescrizione = 20;


    //BOOLEANI DI CONTROLLO
    private bool nomeValido = false;
    private bool descrizioneValida = false;

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
        testoDescrizioneNonValida.text = "Inserire più di " + numeroCaratteriMinimiDescrizione + " caratteri";
    }

    /// <summary>
    /// Il metodo controlla se il nome dell'ingrediente inserito è già presente, in tal caso blocca il salvataggio ed attiva il messaggio di errore!
    /// </summary>
    public void controlloNomePiattoInserito()
    {
        if (nomePiattoInputField.text.Equals(string.Empty))
        {
            testoPiattoGiaPresente.gameObject.SetActive(true);
            testoPiattoGiaPresente.text = "Il campo non può essere vuoto";
            nomeValido = false;
        }
        else if (Piatto.checkPiattoOnonimoPresente(nomePiattoInputField.text))
        {
            testoPiattoGiaPresente.gameObject.SetActive(true);
            testoPiattoGiaPresente.text = "Piatto già presente!";
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
    /// <returns>booleano True: Tutte i vincoli rispettati, False: Uno o più vincoli non rispetati</returns>
    private bool tuttiValoriCorrettiInseriti()
    {
        return nomeValido && descrizioneValida;
    }

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
    /// Il metodo
    /// </summary>
    public void aggiuntaElementoDropDownIngredienti()
    {
        Utility.aggiungiElementoDropDown(ingredientiInseriti, new TMP_Dropdown.OptionData(ingredientiDisponibili.options[ingredientiDisponibili.value].text));
        Utility.rimuoviElementoDropDown(ingredientiDisponibili, ingredientiDisponibili.options[ingredientiDisponibili.value].text);
    }

    /// <summary>
    /// Il metodo
    /// </summary>
    public void RimuoviElementoDropDownIngredienti()
    {
        Utility.aggiungiElementoDropDown(ingredientiDisponibili, new TMP_Dropdown.OptionData(ingredientiInseriti.options[ingredientiInseriti.value].text));
        Utility.rimuoviElementoDropDown(ingredientiInseriti, ingredientiInseriti.options[ingredientiInseriti.value].text);
    }


    /// <summary>
    /// Il metodo permette di creare ed aggiungere la Piatto nel database e su file
    /// </summary>
    public void creaPiatto()
    {
        /*
        Piatto nuovo = new Piatto(nomePiattoInputField.text, descrizionePiattoInputField.text);
        Costanti.databasePiatti.Add(nuovo);
        Database.aggiornaDatabaseOggetto(Costanti.databasePiatti);
        */
    }
}
