using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrol : MonoBehaviour
{
    [Header("Патруль")]
    public Transform[] points;   // точки патруля
    public float waitTime = 1f;  // пауза на точке

    private NavMeshAgent agent;
    private int currentIndex;
    private float waitTimer;

    [Header("Анимация")]
    public Animator animator;    // аниматор зомби
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // если не назначили animator в инспекторе — попробуем найти на детях
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
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

        // --- ОБНОВЛЕНИЕ АНИМАЦИИ ХОДЬБЫ ---
        // берём скорость агента по плоскости XZ
        float speed = new Vector3(agent.velocity.x, 0f, agent.velocity.z).magnitude;
        if (animator != null)
            animator.SetFloat(SpeedHash, speed);

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