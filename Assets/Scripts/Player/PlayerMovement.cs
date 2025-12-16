using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1, 100)] float walkSpeed = 3f;
    [SerializeField][Range(1, 100)] float runSpeed = 6f;
    [SerializeField] CharacterController controller;

    [SerializeField] float gravity = -9.81f;
    Vector3 direction = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    bool grounded = true;

    [SerializeField] Animator animator;

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        direction = new Vector3(movementVector.x, 0, movementVector.y).normalized;
    }

    private void LateUpdate()
    {
        if (!controller.enabled) return;

        grounded = controller.isGrounded;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        velocity = transform.TransformDirection(direction) * currentSpeed * Time.deltaTime;

        if (grounded && velocity.y < 0) velocity.y = 0;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity);

        if (animator != null)
        {
            float targetAnimSpeed = 0f;

            if (direction.magnitude > 0.1f)
            {
                targetAnimSpeed = isRunning ? 1f : 0.5f;
            }

            animator.SetFloat("Speed", targetAnimSpeed, 0.1f, Time.deltaTime);
        }
    }
}
