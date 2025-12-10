using UnityEngine;
using UnityEngine.InputSystem; // Подключаем Input System

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject fpsCamera;
    [SerializeField] GameObject tpsCamera;

    void Start() => SetFPS();

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SetFPS();

        if (Keyboard.current.digit2Key.wasPressedThisFrame) SetTPS();
    }

    void SetFPS()
    {
        fpsCamera.SetActive(true);
        tpsCamera.SetActive(false);
    }

    void SetTPS()
    {
        fpsCamera.SetActive(false);
        tpsCamera.SetActive(true);
    }
}
