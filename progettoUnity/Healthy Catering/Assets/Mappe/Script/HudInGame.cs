using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soldiTesto;
    [SerializeField] private TextMeshProUGUI punteggioTesto;


    private void aggiornaValoreSoldi(string soldi)
    {
        soldiTesto.text = soldi;
    }

    private void aggiornaValorePunteggio(string punteggio)
    {
        punteggioTesto.text = punteggio;
    }


}
