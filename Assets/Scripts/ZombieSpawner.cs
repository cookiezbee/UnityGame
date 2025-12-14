using UnityEngine;
using UnityEngine.AI;   // ОБЯЗАТЕЛЬНО
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Ссылки")]
    public GameObject zombiePrefab;   // префаб зомби
    public GameObject mazeRoot;       // MazeRoot – родитель всех Cell
    public Transform zombiesRoot;     // контейнер для зомби (SpawnedActors)

    [Header("Параметры спавна")]
    public int zombieCount = 5;       // сколько зомби создать

    [Header("Патруль")]
    public float patrolRadius = 4f;       // радиус патруля вокруг базовой клетки
    public int patrolPointsPerZombie = 5; // сколько точек в маршруте

    // вызывается после генерации лабиринта (из Spawner)
    public void SpawnZombies()
    {
        if (zombiePrefab == null || mazeRoot == null)
        {
            Debug.LogWarning("ZombieSpawner: не назначены zombiePrefab или mazeRoot");
            return;
        }

        // находим все клетки
        Cell[] cells = mazeRoot.GetComponentsInChildren<Cell>();
        if (cells.Length == 0)
        {
            Debug.LogWarning("ZombieSpawner: не найдено ни одной клетки");
            return;
        }

        Debug.Log($"ZombieSpawner: найдено {cells.Length} клеток");

        for (int i = 0; i < zombieCount; i++)
        {
            // берём случайную клетку как базу патруля
            Cell baseCell = cells[Random.Range(0, cells.Length)];
            Vector3 basePos = baseCell.transform.position;

            // создаём зомби
            GameObject zombie = Instantiate(
                zombiePrefab,
                basePos + Vector3.up * 0.1f,
                Quaternion.identity
            );

            if (zombiesRoot != null)
                zombie.transform.parent = zombiesRoot;

            // создаём точки патруля вокруг этой клетки
            Transform[] patrolPoints = CreatePatrolPoints(basePos);

            // передаём точки в скрипт патруля
            ZombiePatrol patrol = zombie.GetComponent<ZombiePatrol>();
            if (patrol != null)
            {
                patrol.points = patrolPoints;
            }
        }
    }

    private Transform[] CreatePatrolPoints(Vector3 basePos)
    {
        Transform[] result = new Transform[patrolPointsPerZombie];

        for (int i = 0; i < patrolPointsPerZombie; i++)
        {
            GameObject pointObj = new GameObject("ZombiePoint");

            // случайное смещение вокруг базовой клетки
            Vector2 offset2D = Random.insideUnitCircle * patrolRadius;
            Vector3 targetPos = basePos + new Vector3(offset2D.x, 0f, offset2D.y);

            // ПРИЛИПАЕМ к ближайшей точке на NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos, out hit, patrolRadius * 2f, NavMesh.AllAreas))
            {
                pointObj.transform.position = hit.position;
            }
            else
            {
                // на всякий случай — центр клетки
                pointObj.transform.position = basePos;
            }

            pointObj.transform.parent = zombiesRoot != null
                ? zombiesRoot
                : mazeRoot.transform;

            result[i] = pointObj.transform;
        }

        return result;
    }
}