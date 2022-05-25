using UnityEngine;
using TMPro;
using System.Collections;

public class HudInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soldiTesto;
    [SerializeField] private TextMeshProUGUI punteggioTesto;
    [SerializeField] private ParticleSystem animazioneSoldi;
    [SerializeField] private ParticleSystem animazionePunteggio;
    public int CountFPS = 30;
    public float durata = 1f;
    public string formatoNumero = "N0";
    private int valorePrecedentePunteggio;
    private float valorePrecedenteSoldi;

    private Coroutine CountingCoroutine;

    //AGGIORNAMENETO DINAMICO VALORE INT
    private void UpdateText(int newValue, TextMeshProUGUI testo)
    {
        if (CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }

        CountingCoroutine = StartCoroutine(CountText(newValue, testo));
    }

    private IEnumerator CountText(int newValue, TextMeshProUGUI testo)
    {
        WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
        int previousValue = valorePrecedentePunteggio;
        int stepAmount;

        if (newValue - previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((newValue - previousValue) / (CountFPS * durata)); // newValue = -20, previousValue = 0. CountFPS = 30, and durata = 1; (-20- 0) / (30*1) // -0.66667 (ceiltoint)-> 0
        }
        else
        {
            stepAmount = Mathf.CeilToInt((newValue - previousValue) / (CountFPS * durata)); // newValue = 20, previousValue = 0. CountFPS = 30, and durata = 1; (20- 0) / (30*1) // 0.66667 (floortoint)-> 0
        }

        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                }

                testo.text = previousValue.ToString(formatoNumero);

                yield return Wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }

                testo.text = previousValue.ToString(formatoNumero);

                yield return Wait;
            }
        }
    }


    void Start()
    {
        animazioneSoldi.Stop();
        animazionePunteggio.Stop();
    }

    public void aggiornaValoreSoldi(float soldi)
    {
        soldiTesto.text = soldi.ToString("0.00");
        valorePrecedenteSoldi = soldi;
        animazioneSoldi.Play();

    }

    public void aggiornaValorePunteggio(int punteggio)
    {

        UpdateText(punteggio, punteggioTesto);
        valorePrecedentePunteggio = punteggio;
        animazionePunteggio.Play();
    }

    

}
