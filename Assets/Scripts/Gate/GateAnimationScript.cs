using UnityEngine;

public class GateAnimationScript : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //public void OpenGate()
    //{
    //    isOpen = true;
    //    animator.SetBool("isOpen", isOpen);
    //}

    //public void CloseGate()
    //{
    //    isOpen = false;
    //    animator.SetBool("isOpen", isOpen);
    //}

    public void OpenGate()
    {
        if (isOpen) return;
        isOpen = true;

        if (animator != null)
            animator.SetBool("isOpen", true);
    }
}