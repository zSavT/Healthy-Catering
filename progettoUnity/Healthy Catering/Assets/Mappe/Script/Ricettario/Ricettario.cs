using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ricettario : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titoloSchermata;
    [SerializeField] TextMeshProUGUI testoSchermata;
    [SerializeField] Button avanti;
    [SerializeField] Button indietro;
    [SerializeField] Button switchPiatti;
    [SerializeField] Button switchIngredienti;

    private List<Ingrediente> databaseIngredienti = Database.getDatabaseOggetto(new Ingrediente());
    private List<Piatto> databasePiatti = Database.getDatabaseOggetto(new Piatto());

    void Start()
    {
        switchPiatti.interactable = false;
        indietro.interactable = false;
        titoloSchermata.text = databasePiatti[0].nome;
        testoSchermata.text = databasePiatti[0].getListaIngredientiQuantitaToString();
    }
}
