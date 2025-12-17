using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform visor;

    [SerializeField]
    [Range(0.01f, 10f)] float xSens = 0.1f;
    [SerializeField]
    [Range(0.01f, 10f)] float ySens = 0.1f;

    Quaternion center;

    private void Start() => center = visor.localRotation;

    private void OnLook(InputValue lookValue)
    {
        if (DialogueController.IsDialogueActive) return;

        Vector2 rotation = lookValue.Get<Vector2>();
        float mouseY = rotation.y * ySens;
        Quaternion yRotation = visor.localRotation * Quaternion.AngleAxis(-mouseY, Vector3.right);

        if (Quaternion.Angle(center, yRotation) < 90) visor.localRotation = yRotation;

        float mouseX = rotation.x * xSens;
        Quaternion xRotation = player.localRotation * Quaternion.AngleAxis(mouseX, Vector3.up);
        player.localRotation = xRotation;
    }
}

