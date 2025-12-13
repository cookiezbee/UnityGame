using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrol : MonoBehaviour
{
    public Transform[] points;   // точки патруля
    public float waitTime = 1f;  // пауза на точке

    private NavMeshAgent agent;
    private int currentIndex;
    private float waitTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (points != null && points.Length > 0 && agent != null)
        {
            currentIndex = 0;
            agent.SetDestination(points[currentIndex].position);
        }
    }

    private void Update()
    {
        if (agent == null || points == null || points.Length == 0)
            return;

        if (agent.pathPending)
            return;

        // пришли к точке?
        if (agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                currentIndex = (currentIndex + 1) % points.Length;
                agent.SetDestination(points[currentIndex].position);
                waitTimer = 0f;
            }
        }
    }
}