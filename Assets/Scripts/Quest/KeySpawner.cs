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

        //Cell randomCell = cells[Random.Range(0, cells.Length)];
        //Vector3 spawnPos = randomCell.transform.position + Vector3.up * 2f;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        NPCInteractable[] allNpcs = FindObjectsByType<NPCInteractable>(FindObjectsSortMode.None);
        ZombiePatrol[] allZombies = FindObjectsByType<ZombiePatrol>(FindObjectsSortMode.None);

        Cell targetCell = null;

        for (int i = 0; i < 50; i++)
        {
            Cell randomCell = cells[Random.Range(0, cells.Length)];
            Vector3 pos = randomCell.transform.position;
            bool isSafe = true;

            if (player != null && Vector3.Distance(pos, player.position) < 5f) isSafe = false;

            if (isSafe)
            {
                foreach (var npc in allNpcs)
                {
                    if (npc == null) continue;
                    if (Vector3.Distance(pos, npc.transform.position) < 3f)
                    {
                        isSafe = false;
                        break;
                    }
                }
            }

            if (isSafe)
            {
                foreach (var zombie in allZombies)
                {
                    if (zombie == null) continue;
                    if (Vector3.Distance(pos, zombie.transform.position) < 2f)
                    {
                        isSafe = false;
                        break;
                    }
                }
            }

            if (isSafe)
            {
                targetCell = randomCell;
                break;
            }
        }

        if (targetCell == null) targetCell = cells[Random.Range(0, cells.Length)];

        Vector3 spawnPos = targetCell.transform.position;

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