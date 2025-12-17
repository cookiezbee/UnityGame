using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("START CLICKED");
        SceneManager.LoadScene(0); // 01_Game
    }
}