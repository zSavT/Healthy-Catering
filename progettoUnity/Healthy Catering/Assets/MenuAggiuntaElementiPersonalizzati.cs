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

    [Header("Dropdown Scelta")]
    [SerializeField] private TMP_Dropdown dropdownScelta;

    [Header("Salva Elementi")]
    [SerializeField] private Button bottoneSi;
    [SerializeField] private Button bottoneNo;
    [SerializeField] private Button bottoneSalva;
    [SerializeField] private Button bottoneIndietro;


    // Start is called before the first frame update
    void Start()
    {
        bottoneSi.onClick.RemoveAllListeners();
        bottoneNo.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        attivaElementoScelto();
    }

    /// <summary>
    /// Il metodo attiva gli elementi in base alla scelta fatta nel dropdown
    /// </summary>
    private void attivaElementoScelto()
    {
        if (dropdownScelta.value == 0)
        {
            ingrediente.attivaVisualeIngredienti();
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Ingrediente";
            bottoneSi.onClick.AddListener(() => 
            { 
                ingrediente.creaIngrediente(); 
                ingrediente.attivaVisualeIngredienti();
                bottoneSalva.gameObject.SetActive(true);
                bottoneIndietro.gameObject.SetActive(true);
            }
            );
            bottoneNo.onClick.AddListener(() =>
            {
                ingrediente.attivaVisualeIngredienti();
                bottoneSalva.gameObject.SetActive(true);
                bottoneIndietro.gameObject.SetActive(true);
            }
            );
       }
            
        else if (dropdownScelta.value == 1)
        {
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Patologia";
            ingrediente.disattivaVisualeIngredienti();
        } else if (dropdownScelta.value == 2)
        {
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Piatto";
            ingrediente.disattivaVisualeIngredienti();
        }
    }


}
