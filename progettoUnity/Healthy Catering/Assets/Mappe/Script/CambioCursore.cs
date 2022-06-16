using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Classe per il controllo del cambiamento del cursore quando interagisce con un elemento cliccabili<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Sull'elemento interagibile, per triggerare il cambio cursore.
/// </para>
/// </summary>
public class CambioCursore : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Texture2D cursoreNormale;
    private Texture2D cursoreSuElemento;


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


    /// <summary>
    /// Carica il cursore con la mano della selezione.
    /// </summary>
    public static void cambioCursoreSelezionato()
    {
        Cursor.SetCursor(Resources.Load("PuntatoreSelezionato") as Texture2D, Vector2.zero, CursorMode.Auto);
    }


    /// <summary>
    /// Carica il cursore normale (La freccia)
    /// </summary>
    public static void cambioCursoreNormale()
    {
        Cursor.SetCursor(Resources.Load("Puntatore") as Texture2D, Vector2.zero, CursorMode.Auto);
    }


}
