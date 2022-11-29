using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe per la gestione delle immagini in base ai controller inseriti<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Immagine da modificare
/// </para>
/// </summary>
public class GestoreTastoUI : MonoBehaviour
{

    /// <summary>
    /// Il metodo aggiorna l'immagine presente in base alla tipologia di controller inserito
    /// </summary>
    /// <param name="comando">string tipologia di comando da inserire<br><strong>Comandi possibili:<br>L1, L2, R1, R2, X</br></strong></br></param>
    public void impostaImmagineInBaseInput(string comando)
    {
        string controller = PlayerSettings.tipologiaControllerInserito();
        if (controller.Equals("Generico"))
            controller = "Xbox";
        Image immagine = GetComponent<Image>();
        if (Utility.gamePadConnesso())
            immagine.sprite = Resources.Load<Sprite>("immaginiComandi/click" + controller + comando);
        else
            immagine.sprite = Resources.Load<Sprite>("immaginiComandi/click");
        Debug.Log("immaginiComandi/click" + controller + comando);
    }


    /// <summary>
    /// Il metodo aggiorna l'immagine presente in base alla tipologia di controller inserito
    /// </summary>
    /// <param name="comando">string tipologia di comando da inserire<br><strong>Comandi possibili:<br>L1, L2, R1, R2, X</br></strong></br></param>
    /// <param name="immagineDaModificare">Image immagine da aggiornare</param>
    public static void impostaImmagineInBaseInput(string comando, Image immagineDaModificare)
    {
        string controller = PlayerSettings.tipologiaControllerInserito();
        if (controller.Equals("Generico"))
            controller = "Xbox";
        if (Utility.gamePadConnesso())
            immagineDaModificare.sprite = Resources.Load<Sprite>("immaginiComandi/click" + controller + comando);
        else
            immagineDaModificare.sprite = Resources.Load<Sprite>("immaginiComandi/click");
    }
}
