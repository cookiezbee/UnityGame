using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject fpsCamera;
    [SerializeField] GameObject tpsCamera;

    [SerializeField] GameObject crosshair;

    void Start()
    {
        if (crosshair == null)
        {
            GameObject foundCrosshair = GameObject.Find("Crosshair");
            if (foundCrosshair != null) crosshair = foundCrosshair;
        }

        SetFPS();
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SetFPS();

        if (Keyboard.current.digit2Key.wasPressedThisFrame) SetTPS();
    }

    void SetFPS()
    {
        fpsCamera.SetActive(true);
        tpsCamera.SetActive(false);

        if (crosshair != null) crosshair.SetActive(true);
    }

    void SetTPS()
    {
        fpsCamera.SetActive(false);
        tpsCamera.SetActive(true);

        if (crosshair != null) crosshair.SetActive(false);
    }
}
