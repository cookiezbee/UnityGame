using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathAnimationPlayer : MonoBehaviour
{
    [SerializeField] Animator animator;

    CanvasGroup blackScreenGroup;
    GameObject gameOverButtons;

    private GameObject crosshair;
    private GameObject ammoText;

    private InventoryUI inventoryUIScript;

    [SerializeField] float fadeDuration = 2.0f;
    
    private bool isDeathPlayed = false;
    
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
        
        // Crosshair
        crosshair = GameObject.Find("Crosshair");
        if (crosshair != null)
            crosshair.SetActive(true);
        
        ammoText = GameObject.Find("AmmoText");
        if (ammoText != null)
            ammoText.SetActive(true);

        inventoryUIScript = FindObjectOfType<InventoryUI>();
    }

    public void PlayDeath()
    {
        if (isDeathPlayed) return;
        isDeathPlayed = true;
        
        if (animator != null) animator.SetTrigger("Death");

        UnityEngine.InputSystem.PlayerInput input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (input != null) input.DeactivateInput();

        // убрать прицел
        if (crosshair != null)
            crosshair.SetActive(false);
        
        if (ammoText != null)
            ammoText.SetActive(false);

        if (inventoryUIScript != null)
        {
            inventoryUIScript.enabled = false;

            if (inventoryUIScript.keyStatusText != null) inventoryUIScript.keyStatusText.gameObject.SetActive(false);
        }
        else
        {
            GameObject keyTextObj = GameObject.Find("KeyStatusText");
            if (keyTextObj != null) keyTextObj.SetActive(false);
        }

        // заморозить игру (все в сцене остановится)
        Time.timeScale = 0f;
        
        if (blackScreenGroup != null) StartCoroutine(FadeToBlackSequence());
    }

    IEnumerator FadeToBlackSequence()
    {
        // важно: использовать RealTime, потому что timeScale=0
        yield return new WaitForSecondsRealtime(0.2f);

        float timer = 0f;

        blackScreenGroup.gameObject.SetActive(true);
        blackScreenGroup.alpha = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            blackScreenGroup.alpha = timer / fadeDuration;
            yield return null;
        }

        blackScreenGroup.alpha = 1f;

        if (gameOverButtons != null) gameOverButtons.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void TryAgain()
    {
        // вернуть время, иначе следующая сцена "замрет"
        Time.timeScale = 1f;

        // первая сцена (где Start)
        SceneManager.LoadScene(0);
    }
}    

