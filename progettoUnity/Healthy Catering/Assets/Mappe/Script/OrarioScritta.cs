using System;
using TMPro;
using UnityEngine;

public class OrarioScritta : MonoBehaviour
{


    private void OnEnable()
    {
        DateTime now = DateTime.Now;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = now.Hour.ToString() + ":" + now.Minute.ToString();
    }


}
