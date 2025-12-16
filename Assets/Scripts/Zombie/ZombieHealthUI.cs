using UnityEngine;
using TMPro;

public class ZombieHealthUI : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;

    private int maxHP;

    void Start()
    {
        HPScript hpScript = GetComponent<HPScript>();
        if (hpScript != null)
        {
            maxHP = hpScript.maxHP;
            UpdateHealthBar(hpScript.maxHP);
        }
    }

    public void UpdateHealthBar(int currentHP)
    {
        if (hpText != null) hpText.text = currentHP.ToString() + " / " + maxHP.ToString();
    }

    public void HideHP()
    {
        if (hpText != null) hpText.gameObject.SetActive(false);
    }
}
