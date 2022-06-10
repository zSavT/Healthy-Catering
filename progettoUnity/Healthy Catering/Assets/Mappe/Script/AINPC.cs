using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Classe per la gestione del patter dei movimenti degli NPC passivi.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore dove è presente il modello del NPC (non all'oggetto effettivo del modello del NPC).
/// </para>
/// </summary>
public class AINPC : MonoBehaviour
{
    //Controller della mappa percorribile degli NPC
    NavMeshAgent agent;
    //Waypoint percorso degli NPC
    public Transform[] waypoints;
    //Indice per la gestione dei waypoint raggiunti
    int waypointIndex;
    //Vettore per calcolare la distanza tra il waypoint ed NPC
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        //Inizializza il controller
        agent = GetComponent<NavMeshAgent>();
        //refresh delle impostazioni
        updateDestinazione();
    }

    // Update is called once per frame
    void Update()
    {
        //Controllo della distanza minima per considerare il waypoint raggiunto, in caso positivo si
        if (Vector3.Distance(transform.position, target) < 1)
        {
            iterazioneIndex();
            updateDestinazione();
        }
    }

    /// <summary>
    /// Imposta la destinazione del WayPoint Successiva
    /// </summary>
    private void updateDestinazione()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    /// <summary>
    /// Reset dell'indice dei waypoint da raggiungere.<br></br>
    /// Permette di far camminare all'infinito gli NPC una volta che hanno raggiunto tutti i waypoint facendoli ri-percorre tutti i waypoint
    /// </summary>
    private void iterazioneIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}
