using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    private TMP_Text hpText;

    void Start()
    {
        GameObject textObj = GameObject.Find("PlayerHPText");

        if (textObj != null) hpText = textObj.GetComponent<TMP_Text>();
    }

    public void UpdateHealthBar(int currentHP)
    {
        if (hpText != null) hpText.text = "HP: " + currentHP.ToString();
    }

    public void HideHP()
    {
        if (hpText != null) hpText.gameObject.SetActive(false);
    }
}
