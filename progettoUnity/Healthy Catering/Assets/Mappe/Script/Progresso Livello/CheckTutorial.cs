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
        new OggettoQuantita<int> (0,1),
        new OggettoQuantita<int> (1,1),
        new OggettoQuantita<int> (2,1),
        new OggettoQuantita<int> (3,1),
        new OggettoQuantita<int> (4,1),
        new OggettoQuantita<int> (5,1),
        new OggettoQuantita<int> (6,1)
    };
    private static List<OggettoQuantita<int>> ingredientiPiattoNonCompatibile = new List<OggettoQuantita<int>>
    {
        new OggettoQuantita<int> (7,1),
        new OggettoQuantita<int> (8,1),
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
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        return false;
    }

    public static bool checkSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            return true;
        return false; 
    }

    public static bool checkParlaConZio()
    {
        return true; //TODO
    }

    public static bool checkVaiRistorante()
    {
        float posizionePlayer = GameObject.FindGameObjectWithTag("Player").transform.position.y;

        return posizionePlayer < -500; // siccome e' nel ristorante la y e' minore di -500
    }

    /*
    public static bool checkIsAllaCassa()
    {
        return false; //TODO
    }
    */

    public static bool checkServitoPiattoCompatibileENon(Player giocatore)
    {
        return
            !piattoCompatibile.piattoInInventario(giocatore.inventario) 
            && 
            !piattoNonCompatibile.piattoInInventario(giocatore.inventario);
    }

    public static bool checkVistoMagazzino()
    {
        return false; //TODO
    }

    /*
    public static bool checkIsNelNegozio()
    {
        return false; //TODO
    }
    */

    public static bool checkCompratiIngredienti()
    {
        return false; //TODO
    }

    public static bool checkParlatoConNPC()
    {
        return false; //TODO
    }
}
