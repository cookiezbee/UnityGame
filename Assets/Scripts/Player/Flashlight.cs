using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private ZombieDetector[] zombieDetectors;

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

        if (zombieDetectors == null || zombieDetectors.Length == 0)
            zombieDetectors = FindObjectsByType<ZombieDetector>(FindObjectsSortMode.None);

        foreach (var detector in zombieDetectors)
            detector.SetFlashlightState(newState);

        if (switchSound != null) switchSound.Play();
    }
}