using System.Collections;
using UnityEngine;

public class ZombieDetector : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField][Range(0.1f, 50f)] private float detectionRadius = 6f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Line of Sight (optional)")]
    [SerializeField] private bool useLineOfSight = true;
    [SerializeField][Range(0f, 3f)] private float eyeHeight = 1.2f;

    [Header("Update")]
    [SerializeField][Range(0.05f, 1f)] private float scanInterval = 0.1f;

	[Header("Flashlight")]
    [SerializeField][Range(1f, 5f)] private float flashlightRadiusMultiplier = 2f;
    
    private float baseDetectionRadius;
    
    private Vector3 lastPlayerPosition;
    private bool playerDetected;
    private Transform playerTransform;
    public bool PlayerDetected => playerDetected;
    public Vector3 LastPlayerPosition => lastPlayerPosition;
    public Transform PlayerTransform => playerTransform;

    private void Start()
    {
        baseDetectionRadius = detectionRadius;
        StartCoroutine(DetectLoop());
    }

    private IEnumerator DetectLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(scanInterval);

            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

            if (hits != null && hits.Length > 0)
            {
                // берем первого найденного игрока
                Transform candidate = hits[0].transform;

                if (!useLineOfSight || HasLineOfSight(candidate))
                {
                    playerDetected = true;
                    playerTransform = candidate;
                    lastPlayerPosition = candidate.position;
                    continue;
                }
            }

            playerDetected = false;
            playerTransform = null;
        }
    }

    private bool HasLineOfSight(Transform target)
    {
        Vector3 from = transform.position + Vector3.up * eyeHeight;

        // целимся примерно в центр цели (если есть коллайдер - берем его bounds)
        Vector3 to = target.position;
        Collider c = target.GetComponentInChildren<Collider>();
        if (c != null) to = c.bounds.center;

        Vector3 dir = to - from;
        float dist = dir.magnitude;
        if (dist <= 0.001f) return true;

        dir /= dist;

        // если между нами и игроком есть препятствие - LOS нет
        if (Physics.Raycast(from, dir, dist, obstacleLayer))
            return false;

        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
    
    public void SetFlashlightState(bool enabled)
    {
        detectionRadius = enabled
            ? baseDetectionRadius * flashlightRadiusMultiplier
            : baseDetectionRadius;
        
        // Debug.Log($"{name}: flashlight={enabled}, detectionRadius={detectionRadius}");
    }
}
