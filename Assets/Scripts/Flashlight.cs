using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Light flashlightSource;
    [SerializeField] AudioSource switchSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) ToggleLight();
    }

    void ToggleLight()
    {
        bool newState = !flashlightSource.enabled;
        flashlightSource.enabled = newState;

        if (switchSound != null) switchSound.Play();
    }
}
