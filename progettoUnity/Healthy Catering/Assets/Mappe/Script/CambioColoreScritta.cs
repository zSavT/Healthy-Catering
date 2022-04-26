using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CambioColoreScritta : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI testo;

    // Start is called before the first frame update
    void Start()
    {
        testo = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void cambioColoreNonCliccato()
    {
        testo.color = Color.gray;
    }

    public void cambioColoreCliccato()
    {
        testo.color = Color.white;
    }
}
