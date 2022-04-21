using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miscellaneous : MonoBehaviour
{
    public static void abilitaCursore()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public static void disabilitaCursore()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
}
