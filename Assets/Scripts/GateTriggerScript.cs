using UnityEngine.Events;
using UnityEngine;

public class GateTriggerScript : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;  // событие "игрок вошёл в триггер"
    public UnityEvent OnPlayerExit;   // событие "игрок вышел из триггера"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnPlayerEnter.Invoke();   // вызвать событие при входе
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            OnPlayerExit.Invoke();    // вызвать событие при выходе
    }
}
