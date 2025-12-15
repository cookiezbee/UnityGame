using UnityEngine;
using UnityEngine.AI;

public class KeySpawner : MonoBehaviour
{
    public static KeySpawner Instance;

    public GameObject keyPrefab;

    public GameObject mazeRoot;
    public Transform itemContainer;

    private bool keySpawned = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnKey()
    {
        if (keySpawned || keyPrefab == null || mazeRoot == null)
            return;

        Cell[] cells = mazeRoot.GetComponentsInChildren<Cell>();
        if (cells.Length == 0) return;

        Cell randomCell = cells[Random.Range(0, cells.Length)];
        Vector3 spawnPos = randomCell.transform.position + Vector3.up * 2f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPos, out hit, 2f, NavMesh.AllAreas))
        {
            spawnPos = new Vector3(hit.position.x, 0.5f, hit.position.z);
        }

        GameObject key = Instantiate(keyPrefab, spawnPos, Quaternion.identity);
        key.name = "Key";
        if (itemContainer) key.transform.parent = itemContainer;

        QuestManager.Instance.keyPosition = spawnPos;
        keySpawned = true;
    }
}