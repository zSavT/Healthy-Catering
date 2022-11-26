using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    private TextMeshProUGUI testo;
    public static ToolTipManager instance;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        testo = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void impostaAttivaToolTip(string messaggio)
    {
        this.gameObject.SetActive(true);
        testo.text = messaggio;
    }

    public void nascondiResettaToolTip()
    { 
        this.gameObject.SetActive(false);
        testo.text = string.Empty;
    }
}
