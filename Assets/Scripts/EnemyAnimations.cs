using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimations : MonoBehaviour
{
    public GameObject ammoPrefab;
    [Range(0, 1)] public float dropChance = 0.3f;

    private Animator animator;
    private Collider col;
    private NavMeshAgent agent;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void PlayHit()
    {
        if (isDead) return;
        if (animator != null) animator.SetTrigger("Hit");
    }

    public void PlayDeath()
    {
        if (isDead) return;
        isDead = true;

        animator.SetBool("isDead", true);

        if (col != null) col.enabled = false;

        if (agent != null) agent.enabled = false;

        if (QuestManager.Instance != null) QuestManager.Instance.AddZombieKill();

        TryDropLoot();

        Destroy(gameObject, 5f);
    }

    void TryDropLoot()
    {
        if (ammoPrefab != null && Random.value <= dropChance)
            Instantiate(ammoPrefab, transform.position + Vector3.up * 0.25f, Quaternion.identity);
    }
}
