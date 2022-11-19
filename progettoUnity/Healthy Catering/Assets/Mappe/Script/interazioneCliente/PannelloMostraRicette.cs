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


    public void apriPannelloMostraRicette(Ingrediente ingrediente)
    {
        List<Piatto> piattiRealizzabili = ingrediente.getListaPiattiRealizzabiliConIngrediente();
        string stringaPiattiRealizzabili = "";

        titoloPannelloMostraRicette.text = "Ricette realizzabili con l'ingrediente:\n" + Costanti.coloreIngredienti + ingrediente.nome + Costanti.fineColore;
        foreach (Piatto piattoRealizzabile in piattiRealizzabili)
        {
            stringaPiattiRealizzabili += Costanti.colorePiatti + piattoRealizzabile.nome + "\n" + Costanti.fineColore;
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
