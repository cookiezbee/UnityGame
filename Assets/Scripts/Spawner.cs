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
    public NavMeshSurface surface;       
    public ZombieSpawner zombieSpawner;  // спавнер зомби

    [Header("Выход")]
    public GameObject exitDoorPrefab;
    public Transform exitDoorParent; // можно оставить пустым

    private void Start() 
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        // Чистим старый лабиринт, но не трогаем большой пол
        foreach (Transform child in mazeHandler.transform)
        {
            // У большого пола тег "BigFloor"
            if (child.CompareTag("BigFloor"))
                continue;

            Destroy(child.gameObject);
        }

        // Генерируем структуру лабиринта
        Generator generator = new Generator();
        Maze maze = generator.GenerateMaze(Width, Height);

        // Создаем визуальные ячейки + стены
        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int z = 0; z < maze.cells.GetLength(1); z++)
            {
                Vector3 pos = new Vector3(x * CellSize.x, 0f, z * CellSize.y);

                Cell c = Instantiate(CellPrefab, pos, Quaternion.identity, mazeHandler.transform);

                // Вместо Destroy - SetActive
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

        // создаем точки циклов в сцене: CyclesRoot / Cycle_0 / Point_0...
        if (maze.cycles != null && maze.cycles.Count > 0)
        {
            GameObject cyclesRoot = new GameObject("CyclesRoot");
            cyclesRoot.transform.parent = mazeHandler.transform;

            for (int i = 0; i < maze.cycles.Count; i++)
            {
                var cycle = maze.cycles[i];
                if (cycle == null || cycle.Count < 2)
                    continue;

                GameObject cycleObj = new GameObject("Cycle_" + i);
                cycleObj.transform.parent = cyclesRoot.transform;

                for (int j = 0; j < cycle.Count; j++)
                {
                    MazeCell mc = cycle[j];

                    Vector3 p = new Vector3(mc.X * CellSize.x, 0f, mc.Y * CellSize.y);

                    GameObject pointObj = new GameObject("Point_" + j);
                    pointObj.transform.position = p;
                    pointObj.transform.parent = cycleObj.transform;
                }
            }
        }
        
        if (exitDoorPrefab != null)
        {
            TrySpawnExitDoor(maze);
        }

        // Строим NavMesh по большому полу + активным стенам
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("Spawner: surface (NavMeshSurface) не назначен");
        }

        // Спавним зомби после построения навмеша
        if (zombieSpawner != null)
        {
            zombieSpawner.SpawnZombies();
        }
        else
        {
            Debug.LogWarning("Spawner: ZombieSpawner не назначен");
        }
    }
    
    private void TrySpawnExitDoor(Maze maze)
    {
        int width = maze.cells.GetLength(0);
        int height = maze.cells.GetLength(1);

        MazeCell exitCell = null;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isBorder = x == 0 || x == width - 1 || y == 0 || y == height - 1;
                if (!isBorder) continue;

                MazeCell c = maze.cells[x, y];

                bool openToOutside =
                    (x == 0 && c.Left == false) ||
                    (x == width - 1 && c.Right == false) ||
                    (y == 0 && c.Bottom == false) ||
                    (y == height - 1 && c.Up == false);

                if (openToOutside)
                {
                    exitCell = c;
                    break;
                }
            }

            if (exitCell != null)
                break;
        }

        if (exitCell == null)
        {
            Debug.LogWarning("Spawner: не найдена клетка выхода для двери");
            return;
        }

        // Центр клетки
        Vector3 cellCenter = new Vector3(
            exitCell.X * CellSize.x + CellSize.x * 0.5f,
            0f,
            exitCell.Y * CellSize.y + CellSize.y * 0.5f
        );

        // Поворот двери наружу
        Quaternion rot = Quaternion.identity;

        if (exitCell.X == 0) rot = Quaternion.Euler(0f, 180f, 0f);
        else if (exitCell.X == width - 1) rot = Quaternion.Euler(0f, 0f, 0f);
        else if (exitCell.Y == 0) rot = Quaternion.Euler(0f, 90f, 0f);
        else if (exitCell.Y == height - 1) rot = Quaternion.Euler(0f, 270f, 0f);

        Transform parent = exitDoorParent != null ? exitDoorParent : mazeHandler.transform;
        Instantiate(exitDoorPrefab, cellCenter, rot, parent);
    }
}