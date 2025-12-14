using UnityEngine;

public class GateAnimationScript : MonoBehaviour
{
    Animator animator;
    bool isOpen = false;

    private void Start()
    {
        animator = GetComponent<Animator>(); // найти Animator
    }

    public void openGate()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);  // включить анимацию открытия
    }

    public void closeGate()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen);  // включить анимацию закрытия
    }
}
