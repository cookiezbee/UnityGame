using UnityEngine;
using UnityEngine.Events;

public class HPScript : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public UnityEvent<int> OnHPChanged;
    public UnityEvent OnDeath;

    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        OnHPChanged?.Invoke(currentHP);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;

        if (currentHP < 0) currentHP = 0;

        OnHPChanged?.Invoke(currentHP);

        if (currentHP <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        OnDeath?.Invoke();
    }
}
