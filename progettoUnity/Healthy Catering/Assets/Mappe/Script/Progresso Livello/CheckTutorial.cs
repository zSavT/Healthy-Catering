using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTutorial : MonoBehaviour
{
    //Player

    //check WASD e mouse
    private static bool premutoW = false;
    private static bool premutoA = false;
    private static bool premutoS = false;
    private static bool premutoD = false;

    //check servito piatto compatibile e non
    private static List<OggettoQuantita<int>> ingredientiPiattoCompatibile = new List<OggettoQuantita<int>>{
        new OggettoQuantita<int> (12,10),
        new OggettoQuantita<int> (15,10),
        new OggettoQuantita<int> (0,10),
        new OggettoQuantita<int> (18,10),
        new OggettoQuantita<int> (16,10),
        new OggettoQuantita<int> (46,10)
    };
    private static List<OggettoQuantita<int>> ingredientiPiattoNonCompatibile = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (30,1),
        new OggettoQuantita<int> (35,1),
        new OggettoQuantita<int> (33,2)
    };
    private static Piatto piattoCompatibile = new Piatto("", "", ingredientiPiattoCompatibile);
    private static Piatto piattoNonCompatibile = new Piatto("", "", ingredientiPiattoNonCompatibile);

    public static bool checkWASDeMouse()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            premutoW = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            premutoA = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            premutoS = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            premutoD = true;
        }
        return premutoW && premutoA && premutoS && premutoD;
    }

    public static bool checkSalto()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            return true;
        return false;
    }

    public static bool checkSprint()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
            return true;
        return false; 
    }

    public static bool checkParlaConZio()
    {
        return InterazionePassanti.parlatoConZio;
    }


    public static bool checkVaiRistorante()
    {
        float posizionePlayer = GameObject.FindGameObjectWithTag("Player").transform.position.y;

        return posizionePlayer < -500; // siccome e' nel ristorante la y e' minore di -500
    }

    public static bool checkIsAllaCassa()
    {
        return true; //TODO
    }

    public static bool checkServitoPiattoCompatibile(Player giocatore)
    {
        return !piattoCompatibile.piattoInInventario(giocatore.inventario);
    }
    public static bool checkServitoPiattoNonCompatibile(Player giocatore)
    {
        return !piattoNonCompatibile.piattoInInventario(giocatore.inventario);
    }

    public static bool checkVistoMagazzino()
    {
        return PannelloMagazzino.pannelloMagazzinoApertoPerTutorial;
    }
    
    public static bool checkIsNelNegozio()
    {
        return true; //TODO
    }


    public static bool checkCompratiIngredienti(Player giocatore)
    {
        return !giocatore.inventarioVuoto();
    }

    public static bool checkParlatoConNPC()
    {
        return InterazionePassanti.parlatoConNPC;
    }
}
