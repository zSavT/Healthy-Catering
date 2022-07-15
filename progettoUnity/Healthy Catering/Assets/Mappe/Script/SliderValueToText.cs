using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe utilizata aggiornare in automatico i valori degli slider.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Da aggiungere al testo del valore da aggiornare.
/// </para>
/// </summary>
public class SliderValueToText : MonoBehaviour
{
    [SerializeField] private Slider sliderUI;
    private TextMeshProUGUI textSliderValue;
    
    void Start()
    {
        textSliderValue = GetComponent<TextMeshProUGUI>();
    }


    /// <summary>
    /// Aggiorna il valore del testo in corrispondenza dello slider.
    /// </summary>
    public void aggiornaValore()
    {
        textSliderValue.text = sliderUI.value.ToString();
    }

    /// <summary>
    /// Aggiorna il valore in percetuale del testo in corrispondenza dello slider.
    /// </summary>
    public void aggiornaValorePercentuale()
    {
        float valoreCaricamento = sliderUI.value * 100f;
        textSliderValue.text = valoreCaricamento + "%";
    }

}