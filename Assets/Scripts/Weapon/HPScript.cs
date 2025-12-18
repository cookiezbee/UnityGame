using UnityEngine;
using UnityEngine.Events;

public class HPScript : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public UnityEvent<int> OnHPChanged;
    public UnityEvent OnDeath;

    private bool isDead = false;

    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip deathSound;

    void Start()
    {
        currentHP = maxHP;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        OnHPChanged?.Invoke(currentHP);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;

        if (currentHP < 0) currentHP = 0;

        OnHPChanged?.Invoke(currentHP);

        if (currentHP > 0 && audioSource != null && damageSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(damageSound, 0.6f);
        }

        if (currentHP <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;

        if (deathSound != null) AudioSource.PlayClipAtPoint(deathSound, transform.position, 0.8f);

        isDead = true;
        OnDeath?.Invoke();

        PlayerAttack attackScript = GetComponent<PlayerAttack>();
        Flashlight flashlight = GetComponent<Flashlight>();
        if (attackScript != null) attackScript.enabled = false;
        if (flashlight != null) flashlight.enabled = false;
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        OnHPChanged?.Invoke(currentHP);
    }
}
