using UnityEngine;
using UnityEngine.Events;

public class TargetHitScript : MonoBehaviour
{
    public UnityEvent<EventParameters> OnTargetHit;

    [SerializeField] HPScript hpScript;

    public void targetHit(EventParameters parameters)
    {
        if (hpScript != null) hpScript.TakeDamage(parameters.damage);

        OnTargetHit?.Invoke(parameters);
    }
}