using UnityEngine;
using UnityEngine.UI;
using TMPro;

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