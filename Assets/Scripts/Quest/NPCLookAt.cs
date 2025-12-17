using UnityEngine;

public class NPCLookAt : MonoBehaviour
{
    public float lookRange = 5f;
    public float turnSpeed = 5f;

    private Transform player;

    public AudioSource audioSource;
    public AudioClip greetingSound;
    private bool hasGreeted = false;

    public LayerMask obstacleLayer;
    [Range(0f, 3f)] public float eyeHeight = 1.6f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance <= lookRange)
        {
            if (HasLineOfSight())
            {
                if (!hasGreeted)
                {
                    if (audioSource != null && greetingSound != null)
                        audioSource.PlayOneShot(greetingSound);

                    hasGreeted = true;
                }

                LookAtPlayer();
            }
        }
        else
        {
            if (distance > lookRange * 1.2f) hasGreeted = false;
        }
    }

    bool HasLineOfSight()
    {
        Vector3 origin = transform.position + Vector3.up * eyeHeight;

        Vector3 target = player.position + Vector3.up * eyeHeight;

        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (Physics.Raycast(origin, direction.normalized, distance, obstacleLayer)) return false;

        return true;
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }
}
