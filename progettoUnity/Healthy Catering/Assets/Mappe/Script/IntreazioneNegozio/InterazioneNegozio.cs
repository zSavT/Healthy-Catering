using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class InterazioneNegozio : MonoBehaviour
{
    [SerializeField] private GameObject pannelloNegozio;
    [SerializeField] private Button bottoneAvanti;
    [SerializeField] private Button bottoneIndietro;

    private List<Ingrediente> tuttiGliIngredienti;

    //readonly == final in java
    private readonly int numeroBottoniNellaPagina = 9;
    private Button[] ingredientiBottoniFake;

    public void interazioneNegozio()
    {
        tuttiGliIngredienti = Database.getDatabaseOggetto(new Ingrediente());
        ingredientiBottoniFake = pannelloNegozio.GetComponentsInChildren<Button>();
        caricaPaginaIngredientiInPannelloNegozio(0);
    }

    private void caricaPaginaIngredientiInPannelloNegozio(int pagina)
    {
        int indiceIniziale = calcolaIndiceInizialeLista(pagina);

        while (indiceIniziale < (indiceIniziale + numeroBottoniNellaPagina))
        {
            modificaBottoneIngredienteNegozio(ingredientiBottoniFake[indiceIniziale]);
            indiceIniziale++;
        }
    }

    private int calcolaIndiceInizialeLista(int pagina)
    {
        return pagina * numeroBottoniNellaPagina;
    }

    private void modificaBottoneIngredienteNegozio (Button bottoneIngrediente)
    {
        print(bottoneIngrediente.transform.Find ("nome"));
    }
}