using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    /// <summary>
    /// Il metodo permette di rimuovere l'elemento nel dropdown passato in input
    /// </summary>
    /// <param name="dropdown">TMP_Dropdown dropdown da rimuovere l'elemento</param>
    /// <param name="nomeElementoDaRimuovere">string nome elemento da eliminare</param>
    public static void rimuoviElementoDropDown(TMP_Dropdown dropdown, string nomeElementoDaRimuovere)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
            if (dropdown.options[i].text.Equals(nomeElementoDaRimuovere))
            {
                dropdown.options.RemoveAt(i);
                break;
            }
        dropdown.RefreshShownValue();
    }

    /// <summary>
    /// Il metodo permette di aggiungere l'elemento nel dropdown passato in input
    /// </summary>
    /// <param name="dropdown">TMP_Dropdown dropdown da aggiungere l'elemento</param>
    /// <param name="elementoDaAggiungere">string nome elemento da aggiungere</param>
    public static void aggiungiElementoDropDown(TMP_Dropdown dropdown, TMP_Dropdown.OptionData elementoDaAggiungere)
    {
        dropdown.options.Add(elementoDaAggiungere);
        dropdown.RefreshShownValue();
    }


    public static List<T> differenzaListe<T>(List<T> lista1, List<T> lista2)
    {
        List<T> temp = lista1;
        foreach(T temp1 in lista2)
            if(lista1.Contains(temp1))
                    temp.Remove(temp1);
        return temp;
    }

}