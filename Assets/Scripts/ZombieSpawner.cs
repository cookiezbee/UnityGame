using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Ссылки")]
    public GameObject zombiePrefab;
    public GameObject mazeRoot;
    public Transform zombiesRoot;

    [Header("Режим спавна")]
    public bool spawnByCycles = true;
    public string cyclesContainerName = "CyclesRoot"; // родитель циклов под MazeRoot

    [Header("Параметры спавна (fallback)")]
    public int zombieCount = 5;

    [Header("Патруль (fallback)")]
    public float patrolRadius = 4f;
    public int patrolPointsPerZombie = 5;

    public void SpawnZombies()
    {
        if (zombiePrefab == null || mazeRoot == null)
        {
            Debug.LogWarning("ZombieSpawner: не назначены zombiePrefab или mazeRoot");
            return;
        }

        if (spawnByCycles && TrySpawnByCycles())
            return;
        
        SpawnRandomPatrol();
    }

    private bool TrySpawnByCycles()
    {
        Transform cyclesRoot = mazeRoot.transform.Find(cyclesContainerName);
        if (cyclesRoot == null)
        {
            Debug.LogWarning("ZombieSpawner: cyclesRoot не найден, использую fallback");
            return false;
        }

        int cyclesCount = 0;

        foreach (Transform cycle in cyclesRoot)
        {
            List<Transform> points = new List<Transform>();

            foreach (Transform p in cycle)
                points.Add(p);

            if (points.Count < 2)
                continue;

            cyclesCount++;

            GameObject zombie = Instantiate(
                zombiePrefab,
                points[0].position + Vector3.up * 0.1f,
                Quaternion.identity
            );

            if (zombiesRoot != null)
                zombie.transform.parent = zombiesRoot;

            ZombiePatrol patrol = zombie.GetComponent<ZombiePatrol>();
            if (patrol != null)
                patrol.points = points.ToArray();
        }

        Debug.Log($"ZombieSpawner: заспавнено {cyclesCount} зомби по циклам");

        return cyclesCount > 0;
    }

    private void SpawnRandomPatrol()
    {
        Cell[] cells = mazeRoot.GetComponentsInChildren<Cell>();
        if (cells.Length == 0)
        {
            Debug.LogWarning("ZombieSpawner: не найдено ни одной клетки");
            return;
        }

        for (int i = 0; i < zombieCount; i++)
        {
            Cell baseCell = cells[Random.Range(0, cells.Length)];
            Vector3 basePos = baseCell.transform.position;

            GameObject zombie = Instantiate(zombiePrefab, basePos + Vector3.up * 0.1f, Quaternion.identity);

            if (zombiesRoot != null)
                zombie.transform.parent = zombiesRoot;

            Transform[] patrolPoints = CreatePatrolPoints(basePos);

            ZombiePatrol patrol = zombie.GetComponent<ZombiePatrol>();
            if (patrol != null)
                patrol.points = patrolPoints;
        }
    }

    private Transform[] CreatePatrolPoints(Vector3 basePos)
    {
        Transform[] result = new Transform[patrolPointsPerZombie];

        for (int i = 0; i < patrolPointsPerZombie; i++)
        {
            GameObject pointObj = new GameObject("ZombiePoint");

            Vector2 offset2D = Random.insideUnitCircle * patrolRadius;
            Vector3 targetPos = basePos + new Vector3(offset2D.x, 0f, offset2D.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos, out hit, patrolRadius * 2f, NavMesh.AllAreas))
                pointObj.transform.position = hit.position;
            else
                pointObj.transform.position = basePos;

            pointObj.transform.parent = zombiesRoot != null ? zombiesRoot : mazeRoot.transform;
            result[i] = pointObj.transform;
        }

        return result;
    }
}
