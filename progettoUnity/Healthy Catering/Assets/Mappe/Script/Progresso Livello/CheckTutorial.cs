using UnityEngine;

/// <summary>
/// Classe per la gestione delle azioni del Tutorial in game
/// <strong>Da aggiungere a:</strong><br></br>
/// Nessun GameObject
/// </para>
/// </summary>
public class CheckTutorial : MonoBehaviour
{
    public static bool tastiMovimentoPremuti = false;
    public static bool checkWASDeMouse(ControllerInput controllerInput, Transform posizionePlayer, Transform posizioneTarget)
    {
        if(!MenuInGame.menuAperto)
        {
            if (!tastiMovimentoPremuti)
                tastiMovimentoPremuti = controllerInput.Player.Movimento.WasPressedThisFrame();
            else
                return Vector3.Distance(posizionePlayer.position, posizioneTarget.position) < 1.2f;
            /*
            if (Vector3.Distance(posizionePlayer.position, posizioneTarget.position) < 1.2f)
            {
                Debug.Log("Raggiunto");
                return controllerInput.Player.Movimento.WasPressedThisFrame();
            }
               */

        }
            
        return false;
    }

    public static bool checkSalto(ControllerInput controllerInput)
    {
        if (!MenuInGame.menuAperto)
            return controllerInput.Player.Salto.WasReleasedThisFrame();
        return false;
    }

    public static bool checkSprint(ControllerInput controllerInput)
    {
        if (!MenuInGame.menuAperto)
        {
            return controllerInput.Player.Corsa.WasReleasedThisFrame();
        }
            
        return false;
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
