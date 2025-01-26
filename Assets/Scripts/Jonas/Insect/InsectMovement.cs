using UnityEngine;
using UnityEngine.AI;

public class InsectMovement : MonoBehaviour
{
    public NavMeshAgent agent;          // Reference to the NavMeshAgent
    public float roamRadius = 10f;      // Maximum radius for random movement
    public float roamInterval = 3f;     // Time interval to pick a new random destination

    private bool isTrapped = false;     // Flag to track if the insect is trapped

    private void Start()
    {
        // Start the random movement
        InvokeRepeating("SetRandomDestination", 0f, roamInterval);
    }

    private void Update()
    {
        // Check if the insect is trapped and prevent movement if true
        if (isTrapped)
        {
            // Disable movement if the insect is trapped
            if (agent != null)
            {
                agent.isStopped = true; // Stop the agent's movement
            }
        }
        else
        {
            // Resume movement if not trapped
            if (agent != null && agent.isStopped)
            {
                agent.isStopped = false; // Resume movement
            }
        }
    }

    void SetRandomDestination()
    {
        // Only move if the insect is not trapped
        if (!isTrapped)
        {
            // Get a random point within the specified radius
            Vector3 randomPoint = new Vector3(
                transform.position.x + Random.Range(-roamRadius, roamRadius),
                transform.position.y,
                transform.position.z + Random.Range(-roamRadius, roamRadius)
            );

            // Check if the random point is on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, roamRadius, NavMesh.AllAreas))
            {
                // Set the agent's destination to the random point
                agent.SetDestination(hit.position);
            }
        }
    }

    public void TrapInBubble()
    {
        // Called when the insect is trapped
        isTrapped = true;
        Debug.Log($"{gameObject.name} is trapped and will stop moving.");
    }

    public void UntrapFromBubble()
    {
        // Called when the insect is untrapped
        isTrapped = false;
        Debug.Log($"{gameObject.name} is untrapped and can resume movement.");
    }
}