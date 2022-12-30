using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Classe per gestire l'aggiunta di elementi<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject vuoto menu
/// </para>
/// </summary>
public class MenuAggiuntaElementiPersonalizzati : MonoBehaviour
{
    [Header("Gestori singoli")]
    [SerializeField] private GestioneAggiuntaIngrediente ingrediente;
    [SerializeField] private GestioneAggiuntaPatologia patologia;
    [SerializeField] private GestioneAggiuntaPiatto piatto;

    [Header("Dropdown Scelta")]
    [SerializeField] private TMP_Dropdown dropdownScelta;

    [Header("Salva Elementi")]
    [SerializeField] private Button bottoneSi;
    [SerializeField] private Button bottoneNo;
    [SerializeField] private Button bottoneSalva;
    [SerializeField] private Button bottoneIndietro;
    [SerializeField] private GameObject elementiUscita;


    // Start is called before the first frame update
    void Start()
    {
        bottoneSi.onClick.RemoveAllListeners();
        bottoneNo.onClick.RemoveAllListeners();
        attivaElementoScelto();
    }


    /// <summary>
    /// Il metodo attiva gli elementi in base alla scelta fatta nel dropdown
    /// </summary>
    public void attivaElementoScelto()
    {
        if (dropdownScelta.value == 0)
        {
            bottoneSi.onClick.RemoveAllListeners();
            bottoneNo.onClick.RemoveAllListeners();
            ingrediente.attivaVisualeIngredienti();
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Ingrediente";
            patologia.disattivaVisualePatologia();
            patologia.resetValoriInseriti();
            piatto.disattivaVisualePiatto();
            piatto.resetValoriInseriti();
            bottoneSi.onClick.AddListener(() =>
            {
                ingrediente.creaIngrediente();
                ingrediente.attivaVisualeIngredienti();
                attivaElementiDopoClick();
            }
            );
            bottoneNo.onClick.AddListener(() =>
            {
                ingrediente.attivaVisualeIngredienti();
                attivaElementiDopoClick();
            }
            );
       }
            
        else if (dropdownScelta.value == 1)
        {
            bottoneSi.onClick.RemoveAllListeners();
            bottoneNo.onClick.RemoveAllListeners();
            patologia.attivaVisualePatologia();
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Patologia";
            ingrediente.disattivaVisualeIngredienti();
            ingrediente.resetValoriInseriti();
            piatto.disattivaVisualePiatto();
            piatto.resetValoriInseriti();
            bottoneSi.onClick.AddListener(() =>
            {
                patologia.creaPatologia();
                patologia.attivaVisualePatologia();
                attivaElementiDopoClick();
            }
            );
            bottoneNo.onClick.AddListener(() =>
            {
                patologia.attivaVisualePatologia();
                attivaElementiDopoClick();
            }
            );


        } else if (dropdownScelta.value == 2)
        {
            bottoneSi.onClick.RemoveAllListeners();
            bottoneNo.onClick.RemoveAllListeners();
            piatto.attivaVisualePiatto();
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Piatto";
            ingrediente.disattivaVisualeIngredienti();
            ingrediente.resetValoriInseriti();
            patologia.disattivaVisualePatologia();
            patologia.resetValoriInseriti();
            bottoneSi.onClick.AddListener(() =>
            {
                piatto.creaPiatto();
                piatto.attivaVisualePiatto();
                attivaElementiDopoClick();
            }
);
            bottoneNo.onClick.AddListener(() =>
            {
                patologia.attivaVisualePatologia();
                attivaElementiDopoClick();
            }
            );
        }
    }

    /// <summary>
    /// Il metodo attiva gli elementi dopo i click
    /// </summary>
    private void attivaElementiDopoClick()
    {
        bottoneSalva.gameObject.SetActive(true);
        bottoneIndietro.gameObject.SetActive(true);
        elementiUscita.SetActive(false);
    }
    
    /// <summary>
    /// Il metodo permette di caricare la scena del menu principale
    /// </summary>
    public void menuPrincipale()
    {
        SelezioneLivelli.caricaMenuPrincipale();
    }
}
