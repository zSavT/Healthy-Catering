using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe per gestire l'aggiunta di Patologia<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject vuoto patologia
/// </para>
/// </summary>
public class GestioneAggiuntaPatologia : MonoBehaviour
{
    [Header("Pannelli")]
    private GameObject contenitorePatologia;

    [Header("Input Field")]
    [SerializeField] private TMP_InputField nomePatologiaInputField;
    [SerializeField] private TMP_InputField descrizionePatologiaInputField;

    [Header("Bottoni")]
    [SerializeField] private Button bottoneSalvaPatologia;

    [Header("Testi Controlli")]
    [SerializeField] private TextMeshProUGUI testoPatologiaGiaPresente;
    [SerializeField] private TextMeshProUGUI testoDescrizioneNonValida;

    [Header("Testo e Altro")]
    [SerializeField] private int numeroCaratteriMinimiDescrizione = 20;


    //BOOLEANI DI CONTROLLO
    private bool nomeValido = false;
    private bool descrizioneValida = false;

    // Start is called before the first frame update
    void Start()
    {
        inizializzaElementiPatologia();
    }

    // Update is called once per frame
    void Update()
    {
        if (contenitorePatologia.gameObject.activeInHierarchy)
            if (tuttiValoriCorrettiInseriti())
                bottoneSalvaPatologia.interactable = true;
            else
                bottoneSalvaPatologia.interactable = false;
    }

    /// <summary>
    /// Il metodo inizializza i placeHolder degli input field
    /// </summary>
    private void inizializzaPlaceHolderInputField()
    {
        nomePatologiaInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Inserisci nome Patologia...";
        descrizionePatologiaInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Descrizione Patologia...";
    }

    /// <summary>
    /// Il metodo permette di resettare tutti i valori inseriti nei form
    /// </summary>
    public void resetValoriInseriti()
    {
        nomePatologiaInputField.text = string.Empty;
        descrizionePatologiaInputField.text = string.Empty;
    }

    /// <summary>
    /// Il metodo attiva tutti gli elementi della patologia
    /// </summary>
    public void attivaVisualePatologia()
    {
        contenitorePatologia.SetActive(true);
    }

    /// <summary>
    /// Il metodo disattiva tutti gli elementi della patologia
    /// </summary>
    public void disattivaVisualePatologia()
    {
        contenitorePatologia.SetActive(false);
    }

    /// <summary>
    /// Il metodo permette di inizializzare tutti i valori
    /// </summary>
    private void inizializzaElementiPatologia()
    {
        contenitorePatologia = this.gameObject;
        testoDescrizioneNonValida.text = "Inserire più di " + numeroCaratteriMinimiDescrizione + " caratteri";
        inizializzaPlaceHolderInputField();
    }

    /// <summary>
    /// Il metodo controlla se il nome dell'Patologia inserito è già presente, in tal caso blocca il salvataggio ed attiva il messaggio di errore!
    /// </summary>
    public void controlloNomePatologiaInserito()
    {
        if (nomePatologiaInputField.text.Equals(string.Empty))
        {
            testoPatologiaGiaPresente.gameObject.SetActive(true);
            testoPatologiaGiaPresente.text = "Il campo non può essere vuoto";
            nomeValido = false;
        }
        else if (Patologia.patologiaEsistente(Patologia.getPatologiaDaNome(nomePatologiaInputField.text)))
        {
            testoPatologiaGiaPresente.gameObject.SetActive(true);
            testoPatologiaGiaPresente.text = "Patologia già presente!";
            nomeValido = false;
        }
        else
        {
            testoPatologiaGiaPresente.gameObject.SetActive(false);
            nomeValido = true;
        }
    }

    /// <summary>
    /// Il metodo controlla se la descrizione inserita rispetta i criteri di dimensione <see cref="numeroCaratteriMinimiDescrizione"/>
    /// </summary>
    public void controlloDescrizionePatologiaValida()
    {
        if (descrizionePatologiaInputField.text.Length > numeroCaratteriMinimiDescrizione)
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
    /// Il metodo controlla se tutti i criteri per la creazione della patologia sono validi
    /// </summary>
    /// <returns>booleano True: Tutte i vincoli rispettati, False: Uno o più vincoli non rispetati</returns>
    private bool tuttiValoriCorrettiInseriti()
    {
        return nomeValido && descrizioneValida;
    }

    /// <summary>
    /// Il metodo permette di creare ed aggiungere la patologia nel database e su file
    /// </summary>
    public void creaPatologia()
    {
        Patologia nuovo = new Patologia(Costanti.databasePatologie.Count, nomePatologiaInputField.text, descrizionePatologiaInputField.text);
        Costanti.databasePatologie.Add(nuovo);
        Database.aggiornaDatabaseOggetto(Costanti.databasePatologie);
    }

}
