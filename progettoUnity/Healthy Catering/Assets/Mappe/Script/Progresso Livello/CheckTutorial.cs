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

    /// <summary>
    ///  Il metodo controlla se il giocatore ha raggiunto il cono del tutorial premendo i tasti per il movimento
    /// </summary>
    /// <param name="controllerInput">ControllerInput input per il controllo</param>
    /// <param name="posizionePlayer">Transform posizione del player</param>
    /// <param name="posizioneTarget">Transform posizione del cono da raggiungere</param>
    /// <returns>bool True: Cono raggiuto, False: Cono non raggiunto</returns>
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

    /// <summary>
    /// Il metodo controlla se il giocatore ha effettuato il salto
    /// </summary>
    /// <param name="controllerInput">ControllerInput input per il controllo</param>
    /// <returns>bool True: Effettuato salto, False: Non effettuato il salto</returns>
    public static bool checkSalto(ControllerInput controllerInput)
    {
        if (!MenuInGame.menuAperto)
            return controllerInput.Player.Salto.WasReleasedThisFrame();
        return false;
    }

    /// <summary>
    /// Il metodo controlla se il giocatore ha effettuato lo sprint
    /// </summary>
    /// <param name="controllerInput">ControllerInput input per il controllo</param>
    /// <returns>bool True: Effettuato sprint, False: Non effettuato lo sprint</returns>
    public static bool checkSprint(ControllerInput controllerInput)
    {
        if (!MenuInGame.menuAperto)
        {
            return controllerInput.Player.Corsa.WasReleasedThisFrame();
        }
            
        return false;
    }


    /// <summary>
    /// Il metodo controlla se il giocatore ha interagito con lo zio
    /// </summary>
    /// <returns>bool True: Parlato con lo zio, False: non ancora parlato con lo zio</returns>
    public static bool checkParlaConZio()
    {
        return InterazionePassanti.parlatoConZio;
    }

    /// <summary>
    /// Il metodo controlla se il giocatore ha raggiunto l'interno del ristorante
    /// </summary>
    /// <returns>bool True: Ristorante raggiunto, False: Ristorante non ancora raggiunto</returns>
    public static bool checkVaiRistorante()
    {
        return Interactor.nelRistorante;
    }

    /// <summary>
    /// Il metodo controlla se il giocatore ha servito in piatto capatibile al cliente
    /// </summary>
    /// <param name="giocatore">Player giocatore da controllare l'inventario</param>
    /// <returns>bool True: Servito piatto  campatibile, False: Non servito piatto compatibile</returns>
    public static bool checkServitoPiattoCompatibile(Player giocatore)
    {
        return !Costanti.piattoCompatibileTutorial.piattoInInventario(giocatore.inventario);
    }

    /// <summary>
    /// Il metodo controlla se il giocatore ha servito in piatto non capatibile al cliente
    /// </summary>
    /// <param name="giocatore">Player giocatore da controllare l'inventario</param>
    /// <returns>bool True: Servito piatto non campatibile, False: Non servito piatto non compatibile</returns>
    public static bool checkServitoPiattoNonCompatibile(Player giocatore)
    {
        return !Costanti.piattoNonCompatibileTutorial.piattoInInventario(giocatore.inventario);
    }

    /// <summary>
    /// Il metodo controlla se si è aperto il menu magazzino
    /// </summary>
    /// <returns>bool True: Pannello Magazzino aperto, False: Pannello Magazzino non aperto</returns>
    public static bool checkVistoMagazzino()
    {
        return PannelloMagazzino.pannelloMagazzinoApertoPerTutorial;
    }
    

    /// <summary>
    /// Il metodo controlla se l'inventario del giocatore è vuoto e quindi ha servito il piatto
    /// </summary>
    /// <param name="giocatore">Player giocatore da controllare</param>
    /// <returns>bool True: Giocatore ha servito il piatto, False, il giocatore non ha servito il piatto</returns>
    public static bool checkCompratiIngredienti(Player giocatore)
    {
        return !giocatore.inventarioVuoto();
    }

    /// <summary>
    /// Il metodo controlla se il giocatore ha parlato con un NPC
    /// </summary>
    /// <returns>bool True: Parlato con un NPC, False: Non ancora parlato con un NPC</returns>
    public static bool checkParlatoConNPC()
    {
        return InterazionePassanti.parlatoConNPC;
    }

    /// <summary>
    /// Il metodo controlla se il Menu Ricettario è stato aperto
    /// </summary>
    /// <returns>bool True: Menu Aperto una volta, False: Menu non aperto ancora una volta</returns>
    public static bool checkMostratoRicettario()
    {
        return Ricettario.apertoRicettario;
    }

    /// <summary>
    /// Il metodo controlla se il Menu aiuto è stato aperto
    /// </summary>
    /// <returns>bool True: Menu Aperto una volta, False: Menu non aperto ancora una volta</returns>
    public static bool checkMostratoMenuAiuto()
    {
        return MenuAiuto.apertoMenuAiuto;
    }
}
