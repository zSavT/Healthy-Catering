using UnityEngine;
using UnityEngine.AI;

public class AINPC : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        updateDestinazione();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 1)
        {
            iterazioneIndex();
            updateDestinazione();
        }
    }

    void updateDestinazione()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    void iterazioneIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}
