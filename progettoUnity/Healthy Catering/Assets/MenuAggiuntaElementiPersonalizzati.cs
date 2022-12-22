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
            bottoneSi.onClick.RemoveAllListeners();
            bottoneNo.onClick.RemoveAllListeners();
            ingrediente.attivaVisualeIngredienti();
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Ingrediente";
            patologia.disattivaVisualeIngredienti();
            bottoneSi.onClick.AddListener(() => 
            { 
                ingrediente.creaIngrediente(); 
                ingrediente.attivaVisualeIngredienti();
                bottoneSalva.gameObject.SetActive(true);
                bottoneIndietro.gameObject.SetActive(true);
                elementiUscita.SetActive(false);
            }
            );
            bottoneNo.onClick.AddListener(() =>
            {
                elementiUscita.SetActive(false);
                ingrediente.attivaVisualeIngredienti();
                bottoneSalva.gameObject.SetActive(true);
                bottoneIndietro.gameObject.SetActive(true);
            }
            );
       }
            
        else if (dropdownScelta.value == 1)
        {
            bottoneSi.onClick.RemoveAllListeners();
            bottoneNo.onClick.RemoveAllListeners();
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Patologia";
            ingrediente.disattivaVisualeIngredienti();
            patologia.attivaVisualeIngredienti();
            bottoneSi.onClick.AddListener(() =>
            {
                patologia.creaPatologia();
                patologia.attivaVisualeIngredienti();
                bottoneSalva.gameObject.SetActive(true);
                bottoneIndietro.gameObject.SetActive(true);
                elementiUscita.SetActive(false);
            }
            );
            bottoneNo.onClick.AddListener(() =>
            {
                patologia.attivaVisualeIngredienti();
                bottoneSalva.gameObject.SetActive(true);
                bottoneIndietro.gameObject.SetActive(true);
                elementiUscita.SetActive(false);
            }
            );
        } else if (dropdownScelta.value == 2)
        {
            bottoneSi.onClick.RemoveAllListeners();
            bottoneNo.onClick.RemoveAllListeners();
            bottoneSalva.GetComponentInChildren<TextMeshProUGUI>().text = "Salva Piatto";
            ingrediente.disattivaVisualeIngredienti();
        }
    }


}
