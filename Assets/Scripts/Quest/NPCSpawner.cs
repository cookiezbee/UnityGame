using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;

    public GameObject mazeRoot;
    public Transform npcContainer;

    public void SpawnNPCs()
    {
        if (npcPrefab == null || mazeRoot == null) return;

        Cell[] cells = mazeRoot.GetComponentsInChildren<Cell>();
        if (cells.Length < 3) return;

        Cell[] randomCells = GetRandomUniqueCells(cells, 3);

        Vector3 spawnOffset = new Vector3(0f, 0.1668701f, 0f);

        GameObject npc1 = Instantiate(npcPrefab, randomCells[0].transform.position + spawnOffset, Quaternion.identity);
        npc1.name = "NPC_KeyGiver";
        npc1.GetComponent<NPCInteractable>().SetNPC("KeyGiver", "NPC: Охотник ключей");
        QuestManager.Instance.npc1Position = npc1.transform.position;
        if (npcContainer) npc1.transform.parent = npcContainer;

        GameObject npc2 = Instantiate(npcPrefab, randomCells[1].transform.position + spawnOffset, Quaternion.identity);
        npc2.name = "NPC_ZombieGiver";
        npc2.GetComponent<NPCInteractable>().SetNPC("ZombieGiver", "NPC: Охотник за зомби");
        QuestManager.Instance.npc2Position = npc2.transform.position;
        if (npcContainer) npc2.transform.parent = npcContainer;

        GameObject npc3 = Instantiate(npcPrefab, randomCells[2].transform.position + spawnOffset, Quaternion.identity);
        npc3.name = "NPC_Chatter";
        npc3.GetComponent<NPCInteractable>().SetNPC("Chatter", "NPC: Путник");
        QuestManager.Instance.npc3Position = npc3.transform.position;
        if (npcContainer) npc3.transform.parent = npcContainer;
    }

    private Cell[] GetRandomUniqueCells(Cell[] allCells, int count)
    {
        Cell[] result = new Cell[count];
        for (int i = 0; i < count; i++)
        {
            bool found = false;
            while (!found)
            {
                Cell candidate = allCells[Random.Range(0, allCells.Length)];
                bool isUnique = true;
                for (int j = 0; j < i; j++)
                {
                    if (result[j] == candidate) isUnique = false;
                }
                if (isUnique)
                {
                    result[i] = candidate;
                    found = true;
                }
            }
        }
        return result;
    }
}