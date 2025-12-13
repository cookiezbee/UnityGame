using UnityEngine;
using UnityEngine.AI;       // для NavMeshAgent (если нужно)
using Unity.AI.Navigation;  // для NavMeshSurface

public class Spawner : MonoBehaviour
{
    [Header("Базовые ссылки")]
    public Camera cam;               // ссылка на камеру
    public GameObject mazeHandler;   // объект-контейнер для визуальных ячеек (MazeRoot)

    [Header("Префабы")]
    public Cell CellPrefab;          // префаб ячейки
    public GameObject startPrefab;   // префаб игрока (раньше здесь был шар)
    public GameObject floorPrefab;   // ПРЕФАБ БОЛЬШОГО ПОЛА (Plane 10x10)

    [Header("Параметры лабиринта")]
    public Vector2 CellSize = new Vector2(1, 1); // размер ячейки в мире
    public int Width = 10;           // ширина лабиринта в ячейках
    public int Height = 10;          // высота лабиринта в ячейках

    [Header("Навигация и зомби")]
    public NavMeshSurface surface;       // NavMeshSurface на MazeRoot
    public ZombieSpawner zombieSpawner;  // скрипт, который спавнит зомби

    private void Start()
    {
        GenerateMaze();
    }

    /// <summary>
    /// Создаёт один большой пол под всем лабиринтом.
    /// На нём и будет строиться единый NavMesh.
    /// </summary>
    private void CreateBigFloor()
    {
        if (floorPrefab == null || mazeHandler == null)
            return;

        // ширина/высота лабиринта в мировых координатах
        float worldW = Width * CellSize.x;
        float worldH = Height * CellSize.y;

        // создаём пол как ребёнка MazeRoot
        GameObject floor = Instantiate(floorPrefab, mazeHandler.transform);

        // стандартный Plane в Unity — 10x10 по XZ
        floor.transform.localScale = new Vector3(worldW / 10f, 1f, worldH / 10f);

        // центр под лабиринтом (ячейки от 0…Width-1)
        floor.transform.position = new Vector3(
            (worldW - CellSize.x) / 2f,
            0f,
            (worldH - CellSize.y) / 2f
        );

        // пол нужен только для навмеша — коллайдер можно выключить
        Collider col = floor.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    // Генерация лабиринта (алгоритм recursive backtracker)
    public void GenerateMaze()
    {
        if (mazeHandler == null)
        {
            Debug.LogError("Spawner: mazeHandler не назначен");
            return;
        }

        // 1. Удаляем старый лабиринт (и старый пол, если был)
        foreach (Transform child in mazeHandler.transform)
        {
            Destroy(child.gameObject);
        }

        // 2. Создаём один большой пол под будущим лабиринтом
        CreateBigFloor();

        // 3. Генерируем структуру лабиринта
        Generator generator = new Generator();
        Maze maze = generator.GenerateMaze(Width, Height);

        // 4. Создаём визуальные ячейки
        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int z = 0; z < maze.cells.GetLength(1); z++)
            {
                Vector3 pos = new Vector3(x * CellSize.x, 0f, z * CellSize.y);

                // создаём ячейку
                Cell c = Instantiate(CellPrefab, pos, Quaternion.identity);

                // удаляем стены, если между ячейками есть проход
                if (!maze.cells[x, z].Left)
                    Destroy(c.Left);
                if (!maze.cells[x, z].Right)
                    Destroy(c.Right);
                if (!maze.cells[x, z].Up)
                    Destroy(c.Up);
                if (!maze.cells[x, z].Bottom)
                    Destroy(c.Bottom);

                // кладём ячейку под MazeRoot
                c.transform.parent = mazeHandler.transform;

                // стартовая ячейка – туда ставим игрока
                if (x == maze.startX && z == maze.startY && startPrefab != null)
                {
                    Vector3 playerPos = pos + new Vector3(0f, 1f, 0f);
                    GameObject player = Instantiate(startPrefab, playerPos, Quaternion.identity);
                    player.transform.parent = mazeHandler.transform; // можно поменять на SpawnedActors
                }
            }
        }

        // 5. Камера сверху на центр лабиринта
        if (cam != null)
        {
            cam.transform.position = new Vector3(
                (Width * CellSize.x) / 2f,
                Mathf.Max(Width, Height) * 8f,
                (Height * CellSize.y) / 2f
            );
            cam.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
        }

        // 6. Строим NavMesh по уже готовому лабиринту + большому полу
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("Spawner: surface (NavMeshSurface) не назначен");
        }

        // 7. Спавним зомби после того, как NavMesh построен
        if (zombieSpawner != null)
        {
            zombieSpawner.SpawnZombies();
        }
        else
        {
            Debug.LogWarning("Spawner: ZombieSpawner не назначен");
        }
    }
}
