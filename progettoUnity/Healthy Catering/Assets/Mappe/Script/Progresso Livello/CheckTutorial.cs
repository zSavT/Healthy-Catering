using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTutorial :MonoBehaviour
{
    public static bool checkWASDeMouse()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return true; 
        }
        return false;
    }
    public static bool checkSalto()
    {
        return false; //TODO
    }
    public static bool checkSprint()
    {
        return false; //TODO
    }

    public static bool checkParlaConZio()
    {
        return false; //TODO
    }

    public static bool checkVaiRistorante()
    {
        return false; //TODO
    }

    /*
    public static bool checkIsAllaCassa()
    {
        return false; //TODO
    }
    */

    public static bool checkServitoPiattoCompatibileENon()
    {
        return false; //TODO
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
