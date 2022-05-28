using UnityEngine;
using UnityEngine.EventSystems;

public class CambioCursore : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursoreNormale;
    public Texture2D cursoreSuElemento;


    private void Start()
    {
        cursoreNormale = Resources.Load("Puntatore") as Texture2D;
        cursoreSuElemento = Resources.Load("PuntatoreSelezionato") as Texture2D;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(cursoreNormale, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursoreSuElemento, Vector2.zero, CursorMode.Auto);
    }


    public static void cambioCursoreSelezionato()
    {
        Cursor.SetCursor(Resources.Load("Puntatore") as Texture2D, Vector2.zero, CursorMode.Auto);
    }

    public static void cambioCursoreNormale()
    {
        Cursor.SetCursor(Resources.Load("Puntatore") as Texture2D, Vector2.zero, CursorMode.Auto);
    }

}
