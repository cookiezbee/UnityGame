using UnityEngine;
using UnityEngine.SceneManagement;

public class BootMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("START CLICKED");
        SceneManager.LoadScene(1); // 01_Game
    }
}