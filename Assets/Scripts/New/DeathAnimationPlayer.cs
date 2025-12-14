using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathAnimationPlayer : MonoBehaviour
{
    [SerializeField] Animator animator;

    CanvasGroup blackScreenGroup;
    GameObject gameOverButtons;

    [SerializeField] float fadeDuration = 2.0f;

    void Start()
    {
        GameObject panelObj = GameObject.Find("DeathPanel");
        if (panelObj != null)
        {
            blackScreenGroup = panelObj.GetComponent<CanvasGroup>();
            panelObj.SetActive(false);

            if (blackScreenGroup != null)
            {
                blackScreenGroup.alpha = 0f;
                blackScreenGroup.blocksRaycasts = false;
            }
        }

        gameOverButtons = GameObject.Find("GameOverButtons");
        if (gameOverButtons != null) gameOverButtons.SetActive(false);
    }

    public void PlayDeath()
    {
        if (animator != null) animator.SetTrigger("Death");

        UnityEngine.InputSystem.PlayerInput input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (input != null) input.DeactivateInput();

        if (blackScreenGroup != null) StartCoroutine(FadeToBlackSequence());
    }

    IEnumerator FadeToBlackSequence()
    {
        yield return new WaitForSeconds(0.2f);

        float timer = 0f;

        blackScreenGroup.gameObject.SetActive(true);
        blackScreenGroup.alpha = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            blackScreenGroup.alpha = timer / fadeDuration;
            yield return null;
        }

        blackScreenGroup.alpha = 1f;

        if (gameOverButtons != null) gameOverButtons.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
