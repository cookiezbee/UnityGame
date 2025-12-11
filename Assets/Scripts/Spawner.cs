using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Camera cam;               //ссылка на камеру
    public GameObject mazeHandler;   //ссылка на объект хранени€ визуальных €чеек

    public Cell CellPrefab;          //шаблон €чейки
    public Vector2 CellSize = new Vector2(1, 1); //размер €чейки

    public int Width = 10;           //размеры лабиринта
    public int Height = 10;

    public GameObject startPrefab;   //префаб синего шара (отметка старта)

    // 1) ѕ≈–¬џ… —ѕќ—ќЅ: recursive backtracker
    public void GenerateMaze()       //вызов метода генерации лабиринта
    {
        //очистка объекта хранени€ лабиринта
        foreach (Transform child in mazeHandler.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Generator generator = new Generator();                 //создание генератора
        Maze maze = generator.GenerateMaze(Width, Height);     //ѕ≈–¬џ… јЋ√ќ–»“ћ

        //создание и размещение визуального представлени€ €чеек лабиринта
        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int z = 0; z < maze.cells.GetLength(1); z++)
            {
                Vector3 pos = new Vector3(x * CellSize.x, 0, z * CellSize.y);

                Cell c = Instantiate(CellPrefab, pos, Quaternion.identity);

                //удаление стен €чейки в соответствии с логической моделью
                if (maze.cells[x, z].Left == false)
                    Destroy(c.Left);
                if (maze.cells[x, z].Right == false)
                    Destroy(c.Right);
                if (maze.cells[x, z].Up == false)
                    Destroy(c.Up);
                if (maze.cells[x, z].Bottom == false)
                    Destroy(c.Bottom);

                //вывод рассто€ни€ до стартовой клетки
                int d = maze.cells[x, z].Distance;
                c.distance.text = d.ToString();

                //добавление €чейки в объект хранени€ лабиринта
                c.transform.parent = mazeHandler.transform;

                //если это стартова€ клетка Ч ставим синий шар
                if (x == maze.startX && z == maze.startY && startPrefab != null)
                {
                    Vector3 ballPos = pos + new Vector3(0, 1f, 0); //чуть приподн€ть шар
                    GameObject ball = Instantiate(startPrefab, ballPos, Quaternion.identity);
                    ball.transform.parent = mazeHandler.transform;
                }
            }
        }

        //установка камеры над лабиринтом
        cam.transform.position = new Vector3(
            (Width * CellSize.x) / 2f,
            Mathf.Max(Width, Height) * 8f,
            (Height * CellSize.y) / 2f
        );
        cam.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
    }

    // 2) ¬“ќ–ќ… —ѕќ—ќЅ: алгоритм ќлдоса-Ѕродера
    public void GenerateMazeAldous() //вызов генерации по второму алгоритму
    {
        //очистка объекта хранени€ лабиринта
        foreach (Transform child in mazeHandler.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Generator generator = new Generator();                     //создание генератора
        Maze maze = generator.GenerateMazeAldous(Width, Height);   //¬“ќ–ќ… јЋ√ќ–»“ћ

        //создание и размещение визуального представлени€ €чеек лабиринта
        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int z = 0; z < maze.cells.GetLength(1); z++)
            {
                Vector3 pos = new Vector3(x * CellSize.x, 0, z * CellSize.y);

                Cell c = Instantiate(CellPrefab, pos, Quaternion.identity);

                //удаление стен €чейки в соответствии с логической моделью
                if (maze.cells[x, z].Left == false)
                    Destroy(c.Left);
                if (maze.cells[x, z].Right == false)
                    Destroy(c.Right);
                if (maze.cells[x, z].Up == false)
                    Destroy(c.Up);
                if (maze.cells[x, z].Bottom == false)
                    Destroy(c.Bottom);

                //вывод рассто€ни€ до стартовой клетки
                int d = maze.cells[x, z].Distance;
                c.distance.text = d.ToString();

                //добавление €чейки в объект хранени€ лабиринта
                c.transform.parent = mazeHandler.transform;

                //если это стартова€ клетка Ч ставим синий шар
                if (x == maze.startX && z == maze.startY && startPrefab != null)
                {
                    Vector3 ballPos = pos + new Vector3(0, 1f, 0);
                    GameObject ball = Instantiate(startPrefab, ballPos, Quaternion.identity);
                    ball.transform.parent = mazeHandler.transform;
                }
            }
        }

        //установка камеры над лабиринтом
        cam.transform.position = new Vector3(
            (Width * CellSize.x) / 2f,
            Mathf.Max(Width, Height) * 8f,
            (Height * CellSize.y) / 2f
        );
        cam.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
    }
}
