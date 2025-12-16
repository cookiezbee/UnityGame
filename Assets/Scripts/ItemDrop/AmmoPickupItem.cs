using UnityEngine;

public class AmmoPickupItem : PickupItem
{
    protected override string GetPickupText()
    {
        return "Взять патроны [E]";
    }

    protected override string GetWarningText()
    {
        return "Оружие не найдено!";
    }

    protected override bool OnPickup(GameObject player)
    {
        Gun gun = player.GetComponentInChildren<Gun>(true);

        if (gun != null)
        {
            gun.AddAmmo(Random.Range(5, 16));
            return true;
        }

        return false;
    }
}
