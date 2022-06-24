using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTutorial :MonoBehaviour
{
    //check WASD e mouse
    private static bool premutoW = false;
    private static bool premutoA = false;
    private static bool premutoS = false;
    private static bool premutoD = false;
    private static bool mossoMouse = false;

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
