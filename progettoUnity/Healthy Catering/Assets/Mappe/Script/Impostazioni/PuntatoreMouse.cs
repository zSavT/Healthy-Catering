using UnityEngine;

public class PuntatoreMouse : MonoBehaviour
{
    // Start is called before the first frame update
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
