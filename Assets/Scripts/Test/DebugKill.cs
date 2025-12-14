using UnityEngine;

public class DebugKill : MonoBehaviour
{
    public void DamagePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            HPScript hp = player.GetComponent<HPScript>();

            if (hp != null)
            {
                hp.TakeDamage(10);
            }
        }
    }
}