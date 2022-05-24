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

    public void cambioColoreNonCliccatoGrigio()
    {
        testo.color = new Color32(125,125,125,125);
    }

    public void cambioColoreCliccatoNero()
    {
        testo.color = Color.black;
    }

    public void cambioColoreCliccatoBianco()
    {
        testo.color = new Color32(255,255,255,255);
    }
}