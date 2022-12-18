using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Utility{

    /// <summary>
    /// Il metodo permette di calcolare una percentuale
    /// </summary>
    /// <param name="costoBase">float base calcolo</param>
    /// <param name="percentuale">float percentuale da calcolare</param>
    /// <returns>float percetuale corrispondente</returns>
    public static float calcolaCostoPercentuale (float costoBase, float percentuale){
        return ((costoBase * percentuale) / 100);
    }

    /// <summary>
    /// Il metodo controlla se il numero passatto in input è compreso tra i due estremi passati sempre in input
    /// </summary>
    /// <param name="numero">float numero da controllare</param>
    /// <param name="estremoInferiore">float estremoInferiore</param>
    /// <param name="estremoSuperiore">float estremoSuperiore</param>
    /// <returns>booleano True: Compreso tra i due estremi, False: Non compreso fra i due estremi</returns>
    public static bool compresoFra (float numero, float estremoInferiore, float estremoSuperiore)
    {
        return ((estremoInferiore <= numero) && (numero <= estremoSuperiore));
    }


    public static string getStringaConCapitalLetterIniziale (string stringa)
    {
        if (char.IsLower(stringa[0]))
        {
            stringa = char.ToUpper(stringa[0]) + stringa.Remove (0,1);//c# è fatto proprio male, le stringhe non le puoi cambiare di carattere in carattere kek
        }
        return stringa;
    }

    /// <summary>
    /// Il metodo controlla se un controller è connesso al PC
    /// </summary>
    /// <returns>booleano True: Controller connesso, False: Nessun controller connesso</returns>
    public static bool gamePadConnesso()
    {
        if (Gamepad.current == null)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Il metodo controlla se il giocatore ha premuto un qualsiasi tasto della tastiera/mouse/controller
    /// </summary>
    /// <param name="controllerInput">ControllerInput controller degli input</param>
    /// <returns>booleano True: Il giocatore a premuto un tasto, False: Il giocatore non ha premuto alcun tasto</returns>
    public static bool qualsiasiTastoPremuto(ControllerInput controllerInput)
    {
        return Input.anyKey || controllerInput.Player.Movimento.WasPerformedThisFrame() || controllerInput.Player.MovimentoCamera.WasPerformedThisFrame();
    }
}