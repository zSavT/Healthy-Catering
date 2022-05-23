using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CambioColoreScritta : MonoBehaviour
{
    private TextMeshProUGUI testo;

    // Start is called before the first frame update
    void Start()
    {
        testo = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void cambioColoreNonCliccato()
    {
        testo.color = new Color32(255,255,255,255);
    }

    public void cambioColoreCliccato()
    {
        testo.color = Color.black;
    }
}