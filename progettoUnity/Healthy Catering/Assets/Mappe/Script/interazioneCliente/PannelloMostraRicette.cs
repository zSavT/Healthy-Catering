using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PannelloMostraRicette : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMostraRicette;
    private bool pannelloMostraRicetteAperto = false;
    [SerializeField] private TextMeshProUGUI titoloPannelloMostraRicette;
    [SerializeField] private TextMeshProUGUI listaRicettePannelloMostraRicette;


    public void apriPannelloMostraRicette(Ingrediente ingrediente, List<Ingrediente> databaseIngredienti, List<Piatto> databasePiatti)
    {
        List<Piatto> piattiRealizzabili = ingrediente.getListaPiattiRealizzabiliConIngrediente(databaseIngredienti, databasePiatti);
        string stringaPiattiRealizzabili = "";

        titoloPannelloMostraRicette.text = "Ricette realizzabili con l'ingrediente:\n" + Utility.coloreIngredienti + ingrediente.nome + Utility.fineColore;
        foreach (Piatto piattoRealizzabile in piattiRealizzabili)
        {
            stringaPiattiRealizzabili += Utility.colorePiatti + piattoRealizzabile.nome + "\n" + Utility.fineColore;
        }
        listaRicettePannelloMostraRicette.text = stringaPiattiRealizzabili;

        pannelloMostraRicette.SetActive(true);
        pannelloMostraRicetteAperto = true;
    }

    public void chiudiPannelloMostraRicette()
    {
        if (pannelloMostraRicette != null)
        {
            pannelloMostraRicette.SetActive(false);
            pannelloMostraRicetteAperto = false;
        }
    }

    public bool getPannelloMostraRicetteAperto()
    {
        return pannelloMostraRicetteAperto;
    }
}
