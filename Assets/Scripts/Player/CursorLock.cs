using UnityEngine;

public class CursorLock : MonoBehaviour
{
    [SerializeField] bool cursorLock = true;

    private void Start() => changeCursorState();

    void OnEscape()
    {
        cursorLock = !cursorLock;
        changeCursorState();
    }

    void changeCursorState()
    {
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

