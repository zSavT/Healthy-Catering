using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Classe per la stampa dell'orario su un text object
/// </summary>
public class OrarioScritta : MonoBehaviour
{


    private void OnEnable()
    {
        DateTime now = DateTime.Now;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = now.Hour.ToString() + ":" + now.Minute.ToString();
    }


}
