using System.Collections;
using System.Collections.Generic;
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

    [Header("Altro")]
    private List<Patologia> listaPatologieIngredienteNuovo;

    // Start is called before the first frame update
    void Start()
    {
        inizializzaElementiIniziali();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Il metodo permette di inizializzare i valori iniziali
    /// </summary>
    private void inizializzaElementiIniziali()
    {
        contenitoreIngredienti = this.gameObject;
        listaPatologieIngredienteNuovo = new List<Patologia>();
        popolaElementiDropDownPatologieEsistenti();
        popolaElementiDropDownPatologieIngredienti();
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

    public void popolaElementiDropDownPatologieEsistenti()
    {
        patologieEsistentiDropDown.ClearOptions();
        patologieEsistentiDropDown.AddOptions(Patologia.getListStringNomePatologie());
    }

    private List<string> getPatologie()
    {
        List<string> opzioni = new List<string>();

        foreach (Patologia temp in Costanti.databasePatologie)
            opzioni.Add(temp.nome);
        return opzioni;
    }

    public void popolaElementiDropDownPatologieIngredienti()
    {
        patologieIngredienteDropDown.ClearOptions();
        patologieIngredienteDropDown.AddOptions(Patologia.getListStringNomePatologie(listaPatologieIngredienteNuovo));
    }

    public void aggiungiPatologieIngredienteNuovo()
    {

        listaPatologieIngredienteNuovo.Add(Patologia.getPatologiaDaID(patologieEsistentiDropDown.value));
        popolaElementiDropDownPatologieIngredienti();

    }

    private void aggiornaDropDownPatologieDisponibili()
    {
        /*
        patologieEsistentiDropDown.ClearOptions();
        foreach (Patologia temp in (Patologia.getListStringNomePatologie()))
        {
            if (temp.nome.Equals(listaPatologieIngredienteNuovo))

        }
        patologieEsistentiDropDown.AddOptions(Patologia.getListStringNomePatologie().Remove());
        */
    }

}
