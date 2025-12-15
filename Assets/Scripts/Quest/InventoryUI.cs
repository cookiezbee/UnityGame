using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI keyStatusText;

    void Update()
    {
        if (keyStatusText == null) return;

        bool showText = QuestManager.Instance.keyQuestStarted && !QuestManager.Instance.keyQuestCompleted;

        if (showText)
        {
            keyStatusText.gameObject.SetActive(true);
            keyStatusText.text = InventoryManager.Instance.hasKey ? "Ключ: есть" : "Ключ: нет";
        }
        else keyStatusText.gameObject.SetActive(false);
    }
}