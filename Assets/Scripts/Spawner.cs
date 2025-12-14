using UnityEngine;
using Unity.AI.Navigation;  // для NavMeshSurface

public class Spawner : MonoBehaviour
{
    [Header("Базовые ссылки")]
    public GameObject mazeHandler;   // MazeRoot (родитель всех ячеек и пола)

    [Header("Префабы")]
    public Cell CellPrefab;          // префаб ячейки
    public GameObject startPrefab;   // префаб игрока

    [Header("Параметры лабиринта")]
    public Vector2 CellSize = new Vector2(1, 1);
    public int Width = 20;
    public int Height = 20;

    [Header("Навигация и зомби")]
    public NavMeshSurface surface;       // NavMeshSurface на MazeRoot
    public ZombieSpawner zombieSpawner;  // спавнер зомби

    private void Start()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        if (mazeHandler == null)
        {
            Debug.LogError("Spawner: mazeHandler не назначен");
            return;
        }

        // 1. Чистим старый лабиринт, но НЕ трогаем большой пол
        foreach (Transform child in mazeHandler.transform)
        {
            // Предположим, у большого пола тег "BigFloor"
            if (child.CompareTag("BigFloor"))
                continue;

            Destroy(child.gameObject);
        }

        // 2. Генерируем структуру лабиринта
        Generator generator = new Generator();
        Maze maze = generator.GenerateMaze(Width, Height);

        // 3. Создаём визуальные ячейки + стены
        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int z = 0; z < maze.cells.GetLength(1); z++)
            {
                Vector3 pos = new Vector3(x * CellSize.x, 0f, z * CellSize.y);

                Cell c = Instantiate(CellPrefab, pos, Quaternion.identity, mazeHandler.transform);

                // ВАЖНО: вместо Destroy — SetActive
                // Предполагаем, что maze.cells[x,z].Left == true означает "стена есть"
                c.Left.SetActive(maze.cells[x, z].Left);
                c.Right.SetActive(maze.cells[x, z].Right);
                c.Up.SetActive(maze.cells[x, z].Up);
                c.Bottom.SetActive(maze.cells[x, z].Bottom);

                // Стартовая ячейка: спавним игрока
                if (x == maze.startX && z == maze.startY && startPrefab != null)
                {
                    Vector3 playerPos = pos + new Vector3(0f, 1f, 0f);
                    GameObject player = Instantiate(startPrefab, playerPos, Quaternion.identity);
                    player.transform.parent = mazeHandler.transform;
                }
            }
        }

        // 5. Строим NavMesh по большому полу + активным стенам
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("Spawner: surface (NavMeshSurface) не назначен");
        }

        // 6. Спавним зомби после построения навмеша
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