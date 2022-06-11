using UnityEngine;

/// <summary>
/// Classe per la gestione del puntatore del mouse.<para>
/// </summary>
public class PuntatoreMouse : MonoBehaviour
{

    /// <summary>
    /// Attiva il cursore del mouse.
    /// </summary>
    public static void abilitaCursore()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    /// <summary>
    /// Disattiva il cursore del mouse.
    /// </summary>
    public static void disabilitaCursore()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
}
