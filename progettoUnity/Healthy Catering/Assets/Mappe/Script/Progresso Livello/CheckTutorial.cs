using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTutorial : MonoBehaviour
{
    //check WASD e mouse
    private static bool premutoW = false;
    private static bool premutoA = false;
    private static bool premutoS = false;
    private static bool premutoD = false;

    public static bool checkWASDeMouse()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            premutoW = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            premutoA = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            premutoS = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            premutoD = true;
        }
        return premutoW && premutoA && premutoS && premutoD;
    }

    public static bool checkSalto()
    {
        return Input.GetKeyUp(KeyCode.Space);
    }

    public static bool checkSprint()
    {
        return Input.GetKeyUp(KeyCode.LeftShift);
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
    
    public static bool checkIsNelNegozio()
    {
        return true; //TODO
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
