using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("гдеяэ мсфмн днаюбхрэ менаундхлсч кнцхйс, яеивюя онйю рюй");
    }
}
