using UnityEngine;
using Unity.AI.Navigation;  // для NavMeshSurface

public class Spawner : MonoBehaviour
{
    [Header("Базовые ссылки")]
    public GameObject mazeHandler;   // MazeRoot (родитель всех ячеек)

    [Header("Префабы")]
    public Cell CellPrefab;          // префаб ячейки
    public GameObject startPrefab;   // префаб игрока

    [Header("Gate")]
    public GameObject gatePrefab;    // NEW: префаб ворот (Gate.prefab)

    [Header("Параметры лабиринта")] 
    public Vector2 CellSize = new Vector2(1, 1);
    public int Width = 20;
    public int Height = 20;

    [Header("Навигация и зомби")]
    public NavMeshSurface surface;       
    public ZombieSpawner zombieSpawner;  // спавнер зомби

    private void Start() 
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        // чистим старый лабиринт, но не трогаем большой пол
        foreach (Transform child in mazeHandler.transform)
        {
            // у большого пола тег "BigFloor"
            if (child.CompareTag("BigFloor"))
                continue;

            Destroy(child.gameObject);
        }

        // генерируем структуру лабиринта
        Generator generator = new Generator();
        Maze maze = generator.GenerateMaze(Width, Height);

        Cell exitCellInstance = null;

        // создаем визуальные ячейки + стены
        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int z = 0; z < maze.cells.GetLength(1); z++)
            {
                Vector3 pos = new Vector3(x * CellSize.x, 0f, z * CellSize.y);

                Cell c = Instantiate(CellPrefab, pos, Quaternion.identity, mazeHandler.transform);

                // вместо Destroy - SetActive
                // предполагаем, что maze.cells[x,z].Left == true означает "стена есть"
                c.Left.SetActive(maze.cells[x, z].Left);
                c.Right.SetActive(maze.cells[x, z].Right);
                c.Up.SetActive(maze.cells[x, z].Up);
                c.Bottom.SetActive(maze.cells[x, z].Bottom);

                // запоминаем инстанс клетки выхода
                if (x == maze.exitX && z == maze.exitY)
                    exitCellInstance = c;

                // стартовая ячейка: спавним игрока
                if (x == maze.startX && z == maze.startY && startPrefab != null)
                {
                    Vector3 playerPos = pos + new Vector3(0f, 1f, 0f);
                    GameObject player = Instantiate(startPrefab, playerPos, Quaternion.identity);
                    player.transform.parent = mazeHandler.transform;
                }
            }
        }

        // NEW: ставим ворота в месте выхода наружу
        if (gatePrefab != null && exitCellInstance != null)
        {
            Transform anchor = null;

            if (maze.exitSide == 0) anchor = exitCellInstance.Left.transform;
            else if (maze.exitSide == 1) anchor = exitCellInstance.Right.transform;
            else if (maze.exitSide == 2) anchor = exitCellInstance.Bottom.transform;
            else if (maze.exitSide == 3) anchor = exitCellInstance.Up.transform;

            if (anchor != null)
            {
                GameObject gate = Instantiate(gatePrefab, anchor.position, anchor.rotation, mazeHandler.transform);
                gate.name = "Gate";
            }
        }
        else
        {
            if (gatePrefab == null)
                Debug.LogWarning("Spawner: gatePrefab не назначен");
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

        // строим NavMesh по большому полу + активным стенам
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("Spawner: surface (NavMeshSurface) не назначен");
        }

        // спавним зомби после построения навмеша
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
