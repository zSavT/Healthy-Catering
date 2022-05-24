using UnityEngine;
using TMPro;

public class HudInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soldiTesto;
    [SerializeField] private TextMeshProUGUI punteggioTesto;
    [SerializeField] private ParticleSystem animazioneSoldi;
    [SerializeField] private ParticleSystem animazionePunteggio;


    void Start()
    {
        animazioneSoldi.Stop();
        animazionePunteggio.Stop();
    }

    public void aggiornaValoreSoldi(string soldi)
    {
        soldiTesto.text = soldi;
        animazioneSoldi.Play();

    }

    public void aggiornaValorePunteggio(string punteggio)
    {
        punteggioTesto.text = punteggio;
        animazionePunteggio.Play();
    }


}
