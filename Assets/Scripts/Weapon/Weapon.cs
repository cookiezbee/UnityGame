using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage = 10;
    public float attackRate = 0.5f;

    protected bool isAttacking = false;
    protected float nextAttackTime = 0f;

    public virtual void TryAttack()
    {
        if (DialogueController.IsDialogueActive) return;
        if (Time.time < nextAttackTime) return;
        if (isAttacking) return;

        PerformAttack();
        nextAttackTime = Time.time + attackRate;
    }

    protected abstract void PerformAttack();
}
