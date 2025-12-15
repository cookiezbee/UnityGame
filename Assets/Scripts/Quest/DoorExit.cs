using TMPro;
using UnityEngine;

public class DoorExit : MonoBehaviour
{
    public string nextSceneName = "Victory";
    private Canvas denyText;

    void Start()
    {
        GameObject canvasObj = new GameObject("DenyCanvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = Vector3.zero;

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        RectTransform rectTransform = canvasObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 100);
        rectTransform.localScale = Vector3.one * 0.01f;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(canvasObj.transform);
        textObj.transform.localPosition = Vector3.zero;

        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = "Выполни все квесты!";
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 36;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(300, 100);

        denyText = canvas;
        denyText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (QuestManager.Instance.AllQuestsCompleted())
            {
                Debug.Log("Все квесты выполнены! Выход открыт!");
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                denyText.enabled = true;
                Invoke("HideDenyText", 3f);
            }
        }
    }

    void HideDenyText()
    {
        denyText.enabled = false;
    }
}