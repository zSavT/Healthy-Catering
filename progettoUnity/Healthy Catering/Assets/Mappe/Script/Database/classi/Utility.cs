using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Utility{

    public static float calcolaCostoPercentuale (float costoBase, float percentuale){
        return ((costoBase * percentuale) / 100);
    }

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

    public static bool gamePadConnesso()
    {
        if (Gamepad.current == null)
            return false;
        else
            return true;
    }

    public static bool qualsiasiTastoPremuto(ControllerInput controllerInput)
    {
        return Input.anyKey || controllerInput.Player.Movimento.WasPerformedThisFrame() || controllerInput.Player.MovimentoCamera.WasPerformedThisFrame();
    }
}