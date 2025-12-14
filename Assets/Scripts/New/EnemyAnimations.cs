using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimations : MonoBehaviour
{
    private Animator animator;
    private Collider col;
    private NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void PlayHit()
    {
        if (animator != null) animator.SetTrigger("Hit");

    }

    public void PlayDeath()
    {
        animator.SetBool("isDead", true);

        if (col != null) col.enabled = false;

        if (agent != null) agent.enabled = false;

        Destroy(gameObject, 5f);
    }
}
