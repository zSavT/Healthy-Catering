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

    public void aggiornaValore()
    {
        textSliderValue.text = sliderUI.value.ToString();
    }

    public void aggiornaValorePercentuale()
    {
        float valoreCaricamento = sliderUI.value * 100f;
        textSliderValue.text = valoreCaricamento + "%";
    }

}