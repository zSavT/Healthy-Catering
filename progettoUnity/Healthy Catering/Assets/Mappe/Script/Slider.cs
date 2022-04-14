using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    public Slider sliderUI;
    public Text textSliderValue;

    void Start()
    {
        textSliderValue = GetComponent<Text>();
       // ShowSliderValue();

    }

    public void ShowSliderValue()
    {
        string sliderMessage = "Slider value = " + sliderUI.textSliderValue;
        textSliderValue.text = sliderMessage;
    }
}