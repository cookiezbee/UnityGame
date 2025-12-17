using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrol : MonoBehaviour
{
    [Header("Патруль")]
    public Transform[] points;
    public float waitTime = 1f;

    [Header("Detection/Chase")]
    [SerializeField] private ZombieDetector detector;
    [SerializeField][Range(1f, 5f)] private float chaseSpeedMultiplier = 1.5f;
    [SerializeField][Range(0.05f, 2f)] private float repathInterval = 0.2f;

    [Header("Attack")]
    [SerializeField][Range(0.5f, 3f)] private float attackDistance = 1.6f;
    [SerializeField][Range(0.2f, 5f)] private float attackCooldown = 2f;
    [SerializeField][Range(0f, 20f)] private float rotationSpeed = 10f;
    [SerializeField][Range(1, 100)] private int attackDamage = 10;

    private NavMeshAgent agent;
    private int currentIndex;
    private float waitTimer;

    private float baseSpeed;
    private float repathTimer;

    private float attackTimer;

    [Header("Анимация")]
    public Animator animator;
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private HPScript hpScript;

    public AudioSource audioSource;
    public AudioClip aggroSound;
    private bool wasChasing = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (detector == null)
            detector = GetComponent<ZombieDetector>();

        hpScript = GetComponent<HPScript>();

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (hpScript != null) hpScript.OnDeath.AddListener(OnDeathHandler);
    }

    private void OnDisable()
    {
        if (hpScript != null) hpScript.OnDeath.RemoveListener(OnDeathHandler);
    }

    private void OnDeathHandler()
    {
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        this.enabled = false;
    }

    private void Start()
    {
        if (agent != null)
            baseSpeed = agent.speed;

        if (points != null && points.Length > 0 && agent != null)
        {
            currentIndex = 0;
            agent.SetDestination(points[currentIndex].position);
        }

        attackTimer = attackCooldown;
    }

    private void Update()
    {
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
            return;

        attackTimer += Time.deltaTime;

        // анимация движения
        float speed = new Vector3(agent.velocity.x, 0f, agent.velocity.z).magnitude;
        if (animator != null)
            animator.SetFloat(SpeedHash, speed);

        bool isChasingNow = detector != null && detector.PlayerDetected && detector.PlayerTransform != null;

        if (isChasingNow && !wasChasing)
        {
            if (audioSource != null && aggroSound != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(aggroSound);
            }
        }
        wasChasing = isChasingNow;

        // погоня / атака
        if (isChasingNow)
        {
            Transform player = detector.PlayerTransform;

            float dist = Vector3.Distance(transform.position, player.position);

            // если подошли на дистанцию атаки - останавливаемся и атакуем
            if (dist <= attackDistance)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;

                // поворачиваемся к игроку (по Y)
                Vector3 lookDir = player.position - transform.position;
                lookDir.y = 0f;
                if (lookDir.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(lookDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                }

                if (attackTimer >= attackCooldown)
                {
                    if (animator != null) animator.SetTrigger(AttackHash);

                    // Сбрасываем таймер. Урон нанесется ЧЕРЕЗ Animation Event
                    attackTimer = 0f;
                }

                return;
            }

            // иначе продолжаем погоню
            agent.isStopped = false;
            agent.speed = baseSpeed * chaseSpeedMultiplier;

            repathTimer += Time.deltaTime;
            if (repathTimer >= repathInterval)
            {
                // чтобы не дергалось
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(detector.LastPlayerPosition, path) &&
                    path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(detector.LastPlayerPosition);
                }

                repathTimer = 0f;
            }

            return;
        }

        // возврат к патрулю
        agent.isStopped = false;
        agent.speed = baseSpeed;

        if (points == null || points.Length == 0)
            return;

        if (agent.pathPending)
            return;

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

    public void DealDamageFromAnimationEvent()
    {
        if (detector == null || detector.PlayerTransform == null) return;

        float dist = Vector3.Distance(transform.position, detector.PlayerTransform.position);

        if (dist <= attackDistance + 0.5f)
        {
            HPScript playerHp = detector.PlayerTransform.GetComponentInParent<HPScript>();
            if (playerHp != null) playerHp.TakeDamage(attackDamage);
        }
    }
}
