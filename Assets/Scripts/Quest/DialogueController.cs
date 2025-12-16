using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance;

    public GameObject dialoguePanel;
    public GameObject hudPanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;

    private string currentNPC;
    private int dialogueStep = 0;
    private bool canAdvance = false;

    public static bool IsDialogueActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string npcID, string npcName)
    {
        IsDialogueActive = true;
        currentNPC = npcID;
        dialogueStep = 0;

        if (hudPanel != null) hudPanel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (npcNameText != null) npcNameText.text = npcName;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ShowDialogue();
    }

    public void OnNextButton()
    {
        if (!canAdvance) return;
        dialogueStep++;
        ShowDialogue();
    }

    void ShowDialogue()
    {
        canAdvance = false;

        if (currentNPC == "KeyGiver")
        {
            if (npcNameText) npcNameText.text = "Охотник ключей";
            ShowKeyGiverDialogue();
        }
        else if (currentNPC == "ZombieGiver")
        {
            if (npcNameText) npcNameText.text = "Охотник за зомби";
            ShowZombieGiverDialogue();
        }
        else if (currentNPC == "Chatter")
        {
            if (npcNameText) npcNameText.text = "Путник";
            ShowChatterDialogue();
        }
    }

    void ShowKeyGiverDialogue()
    {
        if (QuestManager.Instance.keyQuestCompleted)
        {
            dialogueText.text = "Спасибо еще раз! Ты спас мне жизнь.";
            if (dialogueStep > 0) ExitDialogue();
            canAdvance = true;
        }

        else if (QuestManager.Instance.keyQuestStarted && InventoryManager.Instance.hasKey)
        {
            dialogueText.text = "О боги! Ты нашел его! Спасибо тебе огромное!";

            InventoryManager.Instance.RemoveKey();
            QuestManager.Instance.keyQuestCompleted = true;

            QuestManager.Instance.CheckGameCompletion();

            if (dialogueStep > 0) ExitDialogue();
            canAdvance = true;
        }

        else if (QuestManager.Instance.keyQuestStarted && !InventoryManager.Instance.hasKey)
        {
            dialogueText.text = "Ты еще не нашел ключ? Он где-то в этом лабиринте...";
            if (dialogueStep > 0) ExitDialogue();
            canAdvance = true;
        }

        else
        {
            if (dialogueStep == 0)
            {
                dialogueText.text = "Помоги! Я потерял ключ в этом проклятом лабиринте!";
                canAdvance = true;
            }
            else if (dialogueStep == 1)
            {
                dialogueText.text = "Пожалуйста, он лежит где-то здесь! Ты можешь его найти?";
                canAdvance = true;
            }
            else if (dialogueStep == 2)
            {
                dialogueText.text = "Если найдешь - я тебе очень буду благодарен!";

                QuestManager.Instance.keyQuestStarted = true;
                if (KeySpawner.Instance != null) KeySpawner.Instance.SpawnKey();

                canAdvance = true;
            }
            else ExitDialogue();
        }
    }

    void ShowZombieGiverDialogue()
    {
        if (QuestManager.Instance.zombieQuestCompleted)
        {
            dialogueText.text = "Воздух стал чище...";

            if (!QuestManager.Instance.zombieRewardGiven)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Gun weapon = player.GetComponentInChildren<Gun>();
                    if (weapon != null)
                    {
                        dialogueText.text = "Спасибо! Как и обещал, держи патроны!";
                        weapon.AddAmmo(25);
                        QuestManager.Instance.zombieRewardGiven = true;
                    }
                }
                QuestManager.Instance.CheckGameCompletion();
            }
            QuestManager.Instance.CheckGameCompletion();

            if (dialogueStep > 0) ExitDialogue();
            canAdvance = true;
        }
        else if (QuestManager.Instance.zombieQuestStarted)
        {
            dialogueText.text = $"Убей еще {2 - QuestManager.Instance.zombiesKilled} зомби...";
            if (dialogueStep > 0) ExitDialogue();
            canAdvance = true;
        }
        else
        {
            if (dialogueStep == 0)
            {
                dialogueText.text = "Эти проклятые зомби не дают мне покоя!";
                canAdvance = true;
            }
            else if (dialogueStep == 1)
            {
                dialogueText.text = "Убей двоих, и ты станешь героем!";
                canAdvance = true;
            }
            else if (dialogueStep == 2)
            {
                dialogueText.text = "В награду я дам тебе патроны. Просто убей их!";
                QuestManager.Instance.zombieQuestStarted = true;
                canAdvance = true;
            }
            else ExitDialogue();
        }
    }

    void ShowChatterDialogue()
    {
        if (QuestManager.Instance.talkQuestCompleted)
        {
            dialogueText.text = "Приятно было поговорить с тобой еще раз!";
            if (dialogueStep > 0) ExitDialogue();
            canAdvance = true;
        }
        else
        {
            if (dialogueStep == 0)
            {
                dialogueText.text = "Привет, странник! Как дела?";
                canAdvance = true;
            }
            else if (dialogueStep == 1)
            {
                dialogueText.text = "Мне нравится в этом лабиринте... несмотря на зомби, конечно.";
                canAdvance = true;
            }
            else if (dialogueStep == 2)
            {
                dialogueText.text = "Спасибо, что остановился поговорить. Удачи в пути!";
                QuestManager.Instance.talkQuestCompleted = true;
                QuestManager.Instance.CheckGameCompletion();
                canAdvance = true;
            }
            else ExitDialogue();
        }
    }

    void ExitDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(true);

        Time.timeScale = 1f;

        StartCoroutine(EnableInputDelay());
    }

    IEnumerator EnableInputDelay()
    {
        yield return new WaitForSeconds(0.2f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        IsDialogueActive = false;
    }
}