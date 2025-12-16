using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public bool keyQuestStarted = false;
    public bool keyQuestCompleted = false;

    public bool zombieQuestStarted = false;
    public int zombiesKilled = 0;
    public bool zombieQuestCompleted = false;

    public bool talkQuestCompleted = false;

    public bool zombieRewardGiven = false;

    public Vector3 npc1Position;
    public Vector3 npc2Position;
    public Vector3 npc3Position;

    public Vector3 keyPosition;

    private TextMeshProUGUI finalMessageText;
    private bool gameFinished = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        FindFinalText();
    }

    public void CheckGameCompletion()
    {
        if (gameFinished) return;

        if (keyQuestCompleted && zombieQuestCompleted && talkQuestCompleted) StartCoroutine(ShowFinalTextRoutine());
    }

    IEnumerator ShowFinalTextRoutine()
    {
        gameFinished = true;

        if (finalMessageText != null)
        {
            finalMessageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            finalMessageText.gameObject.SetActive(false);
        }

        GateAnimationScript gate = FindObjectOfType<GateAnimationScript>();
        if (gate != null) gate.OpenGate();
    }

    public void AddZombieKill()
    {
        if (zombieQuestStarted && !zombieQuestCompleted)
        {
            zombiesKilled++;
            if (zombiesKilled >= 2) zombieQuestCompleted = true;
        }
    }

    public bool AllQuestsCompleted()
    {
        return keyQuestCompleted && zombieQuestCompleted && talkQuestCompleted;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        keyQuestStarted = false;
        keyQuestCompleted = false;
        zombieQuestStarted = false;
        zombieQuestCompleted = false;
        talkQuestCompleted = false;
        zombieRewardGiven = false;
        zombiesKilled = 0;

        gameFinished = false;

        FindFinalText();
    }

    void FindFinalText()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            Transform[] allChildren = canvas.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.name == "FinalMessageText")
                {
                    finalMessageText = child.GetComponent<TextMeshProUGUI>();
                    break;
                }
            }

            if (finalMessageText != null) finalMessageText.gameObject.SetActive(false);
        }
    }
}