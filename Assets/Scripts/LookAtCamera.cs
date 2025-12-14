using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera mainCam;

    void Start() => mainCam = Camera.main;

    void LateUpdate()
    {
        if (mainCam != null) transform.LookAt(transform.position + mainCam.transform.forward);
    }
}
