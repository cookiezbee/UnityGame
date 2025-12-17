using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MusicController.Instance != null) MusicController.Instance.StopMusic();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene(2);
        }
    }
}