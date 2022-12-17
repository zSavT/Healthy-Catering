using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Classe per gestire Pannello Mostra Ricette<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// GameObject del pannello Mostra ricette
/// </para>
/// </summary>
public class PannelloMostraRicette : MonoBehaviour
{
    [SerializeField] private GameObject pannelloMostraRicette;
    [SerializeField] private GameObject tornaIndietroTasto;
    private bool pannelloMostraRicetteAperto = false;
    [SerializeField] private TextMeshProUGUI titoloPannelloMostraRicette;
    [SerializeField] private TextMeshProUGUI listaRicettePannelloMostraRicette;
    
    /// <summary>
    /// Il metodo apre il Pannello Mostra ricette
    /// </summary>
    /// <param name="ingrediente">Ingrediente ingrediente da visualizzare le ricette realizzabile</param>
    public void apriPannelloMostraRicette(Ingrediente ingrediente)
    {
        List<Piatto> piattiRealizzabili = ingrediente.getListaPiattiRealizzabiliConIngrediente();

        titoloPannelloMostraRicette.text = "Ricette realizzabili con l'ingrediente:\n" + Costanti.coloreIngredienti + ingrediente.nome + Costanti.fineColore;
        
        listaRicettePannelloMostraRicette.text = creaStringaRicetteRealizzabili(piattiRealizzabili);

        pannelloMostraRicette.SetActive(true);
        pannelloMostraRicetteAperto = true;
        EventSystem.current.SetSelectedGameObject(tornaIndietroTasto);
    }

    /// <summary>
    /// Il metodo inizializza gli elementi per visualizzare la lista dei piatti realizzabili con gli ingredienti
    /// </summary>
    /// <param name="piattiRealizzabili">lista piatti dei piatti realizzabili</param>
    /// <returns>string contenente i piatti realizzabili</returns>
    private string creaStringaRicetteRealizzabili (List <Piatto> piattiRealizzabili)
    {
        string stringaPiattiRealizzabili = "";

        foreach (Piatto piattoRealizzabile in piattiRealizzabili)
        {
            stringaPiattiRealizzabili += Costanti.colorePiatti + piattoRealizzabile.nome + "\n" + Costanti.fineColore;
        }

        return stringaPiattiRealizzabili;
    }

    /// <summary>
    /// Il metodo chiude il panello Mostra Ricette
    /// </summary>
    public void chiudiPannelloMostraRicette()
    {
        if (pannelloMostraRicette != null)
        {
            pannelloMostraRicette.SetActive(false);
            pannelloMostraRicetteAperto = false;
        }
    }

    /// <summary>
    /// Il metodo restiuisce la variabile booleana pannelloMostraRicetteAperto
    /// </summary>
    /// <returns>bool pannelloMostraRicetteAperto True: Pannello Mostra Ricette Aperto, False: Pannello Mostra Ricette Chiuso</returns>
    public bool getPannelloMostraRicetteAperto()
    {
        return pannelloMostraRicetteAperto;
    }
}
