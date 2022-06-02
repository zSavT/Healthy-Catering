using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PannelloMostraRicette : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMostraRicette;
    [SerializeField] private TextMeshProUGUI titoloPannelloMostraRicette;
    [SerializeField] private TextMeshProUGUI listaRicettePannelloMostraRicette;

    public void chiudiPannelloMostraRicette()
    {
        if (pannelloMostraRicette != null)
        {
            pannelloMostraRicette.SetActive(false);
        }
    }

    public void apriPannelloMostraRicette (Ingrediente ingrediente, List<Ingrediente> databaseIngredienti, List<Piatto> databasePiatti)
    {
        List<Piatto> piattiRealizzabili = ingrediente.getListaPiattiRealizzabiliConIngrediente(databaseIngredienti, databasePiatti);
        string stringaPiattiRealizzabili = "";

        titoloPannelloMostraRicette.text += ingrediente.nome;
        foreach (Piatto piattoRealizzabile in piattiRealizzabili)
        {
            stringaPiattiRealizzabili += piattoRealizzabile.nome + "\n";
        }
        listaRicettePannelloMostraRicette.text = stringaPiattiRealizzabili;

        pannelloMostraRicette.SetActive(true);
    }
}
