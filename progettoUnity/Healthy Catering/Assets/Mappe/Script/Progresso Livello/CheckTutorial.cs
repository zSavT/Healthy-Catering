using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTutorial : MonoBehaviour
{

    public static bool checkWASDeMouse(ControllerInput controllerInput)
    {
        return controllerInput.Player.Movimento.WasPressedThisFrame();
    }

    public static bool checkSalto(ControllerInput controllerInput)
    {
        return controllerInput.Player.Salto.WasPressedThisFrame();
    }

    public static bool checkSprint(ControllerInput controllerInput)
    {
        return controllerInput.Player.Corsa.WasPressedThisFrame();
    }

    public static bool checkParlaConZio()
    {
        return InterazionePassanti.parlatoConZio;
    }

    public static bool checkVaiRistorante()
    {
        float posizionePlayer = GameObject.FindGameObjectWithTag("Player").transform.position.y;

        return posizionePlayer < -500; // siccome e' nel ristorante la y e' minore di -500
    }

    public static bool checkIsAllaCassa()
    {
        return true; //TODO
    }

    public static bool checkServitoPiattoCompatibile(Player giocatore)
    {
        return !Costanti.piattoCompatibileTutorial.piattoInInventario(giocatore.inventario);
    }
    
    public static bool checkServitoPiattoNonCompatibile(Player giocatore)
    {
        return !Costanti.piattoNonCompatibileTutorial.piattoInInventario(giocatore.inventario);
    }

    public static bool checkVistoMagazzino()
    {
        return PannelloMagazzino.pannelloMagazzinoApertoPerTutorial;
    }
    

    public static bool checkCompratiIngredienti(Player giocatore)
    {
        return !giocatore.inventarioVuoto();
    }

    public static bool checkParlatoConNPC()
    {
        return InterazionePassanti.parlatoConNPC;
    }

    public static bool checkMostratoRicettario()
    {
        return Ricettario.apertoRicettario;
    }

    public static bool checkMostratoMenuAiuto()
    {
        return MenuAiuto.apertoMenuAiuto;
    }
}
