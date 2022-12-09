using System;
using TMPro;
using UnityEngine;

public class OrarioScritta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DateTime now = DateTime.Now;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = now.Hour.ToString() + ":" + now.Minute.ToString(); 
    }

    private void OnEnable()
    {
        DateTime now = DateTime.Now;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = now.Hour.ToString() + ":" + now.Minute.ToString();
    }


}
