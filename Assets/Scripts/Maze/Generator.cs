using System.Collections.Generic;
using UnityEngine;

public class Generator
{
    int Width = 10; 
    int Height = 10;

    int startX;
    int startY;
    
    // 0 = Left, 1 = Right, 2 = Bottom, 3 = Up
    int exitX;
    int exitY;
    int exitSide;

    private List<List<MazeCell>> cycles = new List<List<MazeCell>>();
    
    public Maze GenerateMaze(int Width, int Height) 
    {
        this.Width = Width;
        this.Height = Height;
        
        cycles.Clear();

        MazeCell[,] cells = new MazeCell[Width, Height];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeCell { X = x, Y = y };
            }
        }

        // recursive backtracker
        removeWalls(cells);
        
        // добавляем циклы и запоминаем маршруты циклов
        AddCycles(cells);
        
        SetStartAndDistances(cells);
        
        AddExit(cells);

        Maze maze = new Maze();
        maze.cells = cells;
        maze.startX = startX;
        maze.startY = startY;
        maze.cycles = cycles;

        // NEW: отдаём наружу информацию о выходе
        maze.exitX = exitX;
        maze.exitY = exitY;
        maze.exitSide = exitSide;

        return maze;
    }

    // recursive backtracker
    private void removeWalls(MazeCell[,] maze) 
    {
        MazeCell current = maze[0, 0]; 
        current.Visited = true;

        Stack<MazeCell> stack = new Stack<MazeCell>(); 
        stack.Push(current);

        do
        {
            List<MazeCell> unvisitedNeighbours = new List<MazeCell>(); 

            int x = current.X;
            int y = current.Y;
            
            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < Width - 1 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < Height - 1 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0) 
            {
                MazeCell chosen = unvisitedNeighbours[
                    Random.Range(0, unvisitedNeighbours.Count)
                ]; 

                RemoveWall(current, chosen);

                chosen.Visited = true; 
                stack.Push(chosen);

                current = chosen; 
            }
            else
            {
                current = stack.Pop(); 
            }
        } while (stack.Count > 0); 
    }
    
    private void RemoveWall(MazeCell a, MazeCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y) 
            {
                a.Bottom = false;
                b.Up = false;
            }
            else 
            {
                a.Up = false;
                b.Bottom = false;
            }
        }
        else if (a.Y == b.Y)
        {
            if (a.X > b.X)
            {
                a.Left = false;
                b.Right = false;
            }
            else
            {
                a.Right = false;
                b.Left = false;
            }
        }
    }

    // создаем дополнительные проходы, но теперь каждый проход превращаем в маршрут цикла
    private void AddCycles(MazeCell[,] maze)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);
        
        int cyclesCount = (width * height) / 5;
        if (cyclesCount < 1) cyclesCount = 1;

        for (int i = 0; i < cyclesCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            
            int dir = Random.Range(0, 4);

            int nx = x;
            int ny = y;

            if (dir == 0) nx = x - 1;
            if (dir == 1) nx = x + 1; 
            if (dir == 2) ny = y - 1; 
            if (dir == 3) ny = y + 1;
            
            if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                continue;

            MazeCell a = maze[x, y];
            MazeCell b = maze[nx, ny];
 
            // если между ними уже есть проход - это не даст нового цикла, пропускаем
            if (HasOpenPassage(a, b))
                continue;

            // в идеальном лабиринте путь между a и b всегда существует (через другие клетки)
            // находим путь в текущем лабиринте (до открытия новой стенки)
            List<MazeCell> path = FindPath(maze, a, b);

            if (path == null || path.Count < 2)
                continue;

            // открываем стену между a и b (создаем цикл)
            OpenWallBetween(a, b);

            // запоминаем маршрут цикла: a..b, а замыкание будет через новый проход b->a
            cycles.Add(path);
        }
    }

    private bool HasOpenPassage(MazeCell a, MazeCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y == b.Y + 1) return !a.Bottom && !b.Up;
            if (a.Y + 1 == b.Y) return !a.Up && !b.Bottom;
        }
        else if (a.Y == b.Y)
        {
            if (a.X == b.X + 1) return !a.Left && !b.Right;
            if (a.X + 1 == b.X) return !a.Right && !b.Left;
        }

        return false;
    }

    private void OpenWallBetween(MazeCell a, MazeCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y == b.Y + 1)
            {
                a.Bottom = false;
                b.Up = false;
            }
            else if (a.Y + 1 == b.Y)
            {
                a.Up = false;
                b.Bottom = false;
            }
        }
        else if (a.Y == b.Y)
        {
            if (a.X == b.X + 1)
            {
                a.Left = false;
                b.Right = false;
            }
            else if (a.X + 1 == b.X)
            {
                a.Right = false;
                b.Left = false;
            }
        }
    }

    private List<MazeCell> FindPath(MazeCell[,] maze, MazeCell start, MazeCell goal)
    {
        Queue<MazeCell> q = new Queue<MazeCell>();
        Dictionary<MazeCell, MazeCell> prev = new Dictionary<MazeCell, MazeCell>();

        q.Enqueue(start);
        prev[start] = null;

        while (q.Count > 0)
        {
            MazeCell cur = q.Dequeue();
            if (cur == goal)
                break;

            foreach (MazeCell n in GetOpenNeighbors(maze, cur))
            {
                if (prev.ContainsKey(n))
                    continue;

                prev[n] = cur;
                q.Enqueue(n);
            }
        }

        if (!prev.ContainsKey(goal))
            return null;

        List<MazeCell> path = new List<MazeCell>();
        MazeCell p = goal;

        while (p != null)
        {
            path.Add(p);
            p = prev[p];
        }

        path.Reverse();
        return path;
    }

    private IEnumerable<MazeCell> GetOpenNeighbors(MazeCell[,] maze, MazeCell c)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        int x = c.X;
        int y = c.Y;

        if (!c.Left && x > 0) yield return maze[x - 1, y];
        if (!c.Right && x < width - 1) yield return maze[x + 1, y];
        if (!c.Bottom && y > 0) yield return maze[x, y - 1];
        if (!c.Up && y < height - 1) yield return maze[x, y + 1];
    }

    private void SetStartAndDistances(MazeCell[,] maze)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        int side = Random.Range(0, 4);

        if (side == 0)
        {
            startX = 0;
            startY = Random.Range(0, height);
        }
        else if (side == 1)
        {
            startX = width - 1;
            startY = Random.Range(0, height);
        }
        else if (side == 2)
        {
            startX = Random.Range(0, width);
            startY = 0;
        }
        else
        {
            startX = Random.Range(0, width);
            startY = height - 1;
        }

        MazeCell start = maze[startX, startY];

        start.Distance = 0;

        // (BFS)
        Queue<MazeCell> queue = new Queue<MazeCell>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            MazeCell current = queue.Dequeue();
            int x = current.X;
            int y = current.Y;
            int dist = current.Distance;
            
            if (!current.Left && x > 0)
            {
                MazeCell n = maze[x - 1, y];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }
            
            if (!current.Right && x < width - 1)
            {
                MazeCell n = maze[x + 1, y];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }
            
            if (!current.Bottom && y > 0)
            {
                MazeCell n = maze[x, y - 1];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }
            
            if (!current.Up && y < height - 1)
            {
                MazeCell n = maze[x, y + 1];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }
        }
    }
    
    private void AddExit(MazeCell[,] maze)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        MazeCell exitCell = null;
        int maxDist = -1;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isBorder =
                    x == 0 || x == width - 1 ||
                    y == 0 || y == height - 1;

                if (!isBorder)
                    continue;

                int d = maze[x, y].Distance;
                if (d > maxDist)
                {
                    maxDist = d;
                    exitCell = maze[x, y];
                }
            }
        }

        if (exitCell == null)
            return;

        int ex = exitCell.X;
        int ey = exitCell.Y;

        // NEW: запоминаем выход
        exitX = ex;
        exitY = ey;

        if (ex == 0)
        {
            exitCell.Left = false;
            exitSide = 0;
        }
        else if (ex == width - 1)
        {
            exitCell.Right = false;
            exitSide = 1;
        }
        else if (ey == 0)
        {
            exitCell.Bottom = false;
            exitSide = 2;
        }
        else if (ey == height - 1)
        {
            exitCell.Up = false;
            exitSide = 3;
        }
    }
}
