using UnityEngine;
using UnityEngine.Events;

public class TargetHitScript : MonoBehaviour
{
    public UnityEvent<EventParameters> OnTargetHit;

    public void targetHit(EventParameters parameters)
    {
        OnTargetHit?.Invoke(parameters);
    }
}