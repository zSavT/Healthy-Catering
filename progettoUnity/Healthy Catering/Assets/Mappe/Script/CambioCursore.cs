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

    /// <summary>
    /// Metodo che setta il cursore normale quando il puntatore del mouse non è più sopra l'elemento con questo script collegato
    /// </summary>
    /// <param name="eventData">Puntatore esce dalla zona dell'elmento</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(cursoreNormale, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Metodo che setta il curosore over quando il puntatore del mouse è sopra ad un elemento con questo script collegato.
    /// </summary>
    /// <param name="eventData">Puntatore del mouse sopra l'elemento</param>
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
