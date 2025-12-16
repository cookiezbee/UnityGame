using UnityEngine;
using UnityEngine.SceneManagement;

public class BootMenu : MonoBehaviour
{
    [Header("UI Roots")]
    [SerializeField] private GameObject mainCard;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject hintsPanel;

    private void Awake()
    {
        ShowMain();
    }

    public void StartGame()
    {
        Debug.Log("START CLICKED");
        SceneManager.LoadScene(1); // 01_Game
    }

    public void OpenControls()
    {
        HideAll();
        if (controlsPanel != null) controlsPanel.SetActive(true);
    }

    public void OpenHints()
    {
        HideAll();
        if (hintsPanel != null) hintsPanel.SetActive(true);
    }

    public void BackToMain()
    {
        ShowMain();
    }

    private void ShowMain()
    {
        HideAll();
        if (mainCard != null) mainCard.SetActive(true);
    }

    private void HideAll()
    {
        if (mainCard != null) mainCard.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (hintsPanel != null) hintsPanel.SetActive(false);
    }
}