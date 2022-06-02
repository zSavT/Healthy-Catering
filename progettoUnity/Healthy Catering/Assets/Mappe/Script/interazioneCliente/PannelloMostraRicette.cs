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
        Debug.Log("Ciao dentro\n" + ingrediente.ToString());
        Debug.Log("Ciao dentro\n" + databaseIngredienti);
        Debug.Log("Ciao dentro\n" + databasePiatti);
        List<Piatto> piattiRealizzabili = ingrediente.getListaPiattiRealizzabiliConIngrediente(databaseIngredienti, databasePiatti);
        string stringaPiattiRealizzabili = "";

        titoloPannelloMostraRicette.text = "Ricette realizzabili con l'ingrediente:\n" + ingrediente.nome;
        foreach (Piatto piattoRealizzabile in piattiRealizzabili)
        {
            stringaPiattiRealizzabili += piattoRealizzabile.nome + "\n";
        }
        listaRicettePannelloMostraRicette.text = stringaPiattiRealizzabili;

        pannelloMostraRicette.SetActive(true);
        pannelloMostraRicetteAperto = true;
    }

    public void ing(int i)
    {
        Debug.Log(i);
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
