using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PannelloMostraRicette : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMostraRicette;
    [SerializeField] private GameObject tornaIndietroTasto;
    private bool pannelloMostraRicetteAperto = false;
    [SerializeField] private TextMeshProUGUI titoloPannelloMostraRicette;
    [SerializeField] private TextMeshProUGUI listaRicettePannelloMostraRicette;
        
    public void apriPannelloMostraRicette(Ingrediente ingrediente)
    {
        List<Piatto> piattiRealizzabili = ingrediente.getListaPiattiRealizzabiliConIngrediente();

        titoloPannelloMostraRicette.text = "Ricette realizzabili con l'ingrediente:\n" + Costanti.coloreIngredienti + ingrediente.nome + Costanti.fineColore;
        
        listaRicettePannelloMostraRicette.text = creaStringaRicetteRealizzabili(piattiRealizzabili);

        pannelloMostraRicette.SetActive(true);
        pannelloMostraRicetteAperto = true;
        EventSystem.current.SetSelectedGameObject(tornaIndietroTasto);
    }

    private string creaStringaRicetteRealizzabili (List <Piatto> piattiRealizzabili)
    {
        string stringaPiattiRealizzabili = "";

        foreach (Piatto piattoRealizzabile in piattiRealizzabili)
        {
            stringaPiattiRealizzabili += Costanti.colorePiatti + piattoRealizzabile.nome + "\n" + Costanti.fineColore;
        }

        return stringaPiattiRealizzabili;
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
