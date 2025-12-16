using UnityEngine;

public class LootFactory : MonoBehaviour
{
    public static LootFactory Instance;

    public GameObject ammoPrefab;
    public GameObject healthPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void CreateLoot(LootType type, Vector3 position)
    {
        GameObject prefabToSpawn = null;
        float yOffset = 0.23f;

        switch (type)
        {
            case LootType.Ammo:
                prefabToSpawn = ammoPrefab;
                break;
            case LootType.Health:
                prefabToSpawn = healthPrefab;
                break;
        }

        if (prefabToSpawn != null) Instantiate(prefabToSpawn, position + Vector3.up * yOffset, Quaternion.identity);
    }
}

public enum LootType
{
    Ammo,
    Health
}