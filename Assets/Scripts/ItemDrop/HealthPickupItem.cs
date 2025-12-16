using UnityEngine;

public class HealthPickupItem : PickupItem
{
    public int healAmount = 10;

    protected override string GetPickupText()
    {
        return "Аптечка (+10 HP) [E]";
    }

    protected override string GetWarningText()
    {
        return "Максимум здоровья!";
    }

    protected override bool OnPickup(GameObject player)
    {
        HPScript hpScript = player.GetComponent<HPScript>();

        if (hpScript != null)
        {
            if (hpScript.currentHP < hpScript.maxHP)
            {
                hpScript.Heal(healAmount);
                return true;
            }

            else return false;
        }

        return false;
    }
}
