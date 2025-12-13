using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Progress")]
    [SerializeField] int npcTotal = 0;
    [SerializeField] int npcTalked = 0;
    [SerializeField] bool exitUnlocked = false;

    [Header("State")]
    [SerializeField] bool isGameOver = false;
    [SerializeField] bool isWin = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetNpcTotal(int total)
    {
        npcTotal = Mathf.Max(0, total);
        npcTalked = 0;
        exitUnlocked = false;
        isGameOver = false;
        isWin = false;
        // позже тут обновим UI
    }

    public void RegisterNpcTalk()
    {
        if (isGameOver) return;

        npcTalked = Mathf.Min(npcTalked + 1, npcTotal);

        if (npcTalked >= npcTotal && npcTotal > 0)
        {
            exitUnlocked = true;
            // позже: вызвать ExitController.Open()
        }

        // позже: обновить UI
    }

    public bool IsExitUnlocked() => exitUnlocked;

    public void Lose()
    {
        if (isGameOver) return;
        isGameOver = true;
        isWin = false;
        // позже: показать losePanel
    }

    public void Win()
    {
        if (isGameOver) return;
        isGameOver = true;
        isWin = true;
        SceneManager.LoadScene("02_End");
    }
}