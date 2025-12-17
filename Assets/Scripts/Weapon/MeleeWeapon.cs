using UnityEngine;
using System.Collections;

public class MeleeWeapon : Weapon
{
    public float attackRange = 1.5f;
    public float hitDelay = 0.2f;
    public Transform attackPoint;

    public AudioSource meleeAudioSource;
    public AudioClip swingSound;

    private void Start()
    {
        if (meleeAudioSource == null) meleeAudioSource = GetComponent<AudioSource>();
    }

    protected override void PerformAttack()
    {
        if (meleeAudioSource != null && swingSound != null)
        {
            meleeAudioSource.pitch = Random.Range(0.9f, 1.1f);
            meleeAudioSource.PlayOneShot(swingSound);
        }

        StartCoroutine(MeleeStrikeRoutine());
    }

    IEnumerator MeleeStrikeRoutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(hitDelay);

        Vector3 center = attackPoint != null ? attackPoint.position : transform.position + transform.forward;

        Collider[] hitColliders = Physics.OverlapSphere(center, attackRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player")) continue;

            TargetHitScript targetHit = hitCollider.GetComponentInParent<TargetHitScript>();

            if (targetHit != null)
            {
                RaycastHit fakeHit = new RaycastHit();
                fakeHit.point = hitCollider.transform.position;
                fakeHit.normal = (transform.position - hitCollider.transform.position).normalized;

                targetHit.targetHit(new EventParameters(transform.position, fakeHit, 0, damage));

                EnemyAnimations zombieAnim = hitCollider.GetComponentInParent<EnemyAnimations>();
                if (zombieAnim != null) zombieAnim.PlayHit();

                break;
            }
        }

        yield return new WaitForSeconds(0.6f);

        isAttacking = false;
    }
}