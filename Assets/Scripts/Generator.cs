using System.Collections.Generic;   // List, Stack, Queue

public class Generator        // генератор лабиринта
{
    int Width = 10;           // размеры лабиринта
    int Height = 10;

    int startX;               // координаты старта
    int startY;

    // 1) ПЕРВЫЙ МЕТОД – recursive backtracker
    public Maze GenerateMaze(int Width, int Height)   //метод генерации
    {
        this.Width = Width;
        this.Height = Height;

        MazeCell[,] cells = new MazeCell[Width, Height];   //создание массива ячеек

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeCell { X = x, Y = y };  //инициализация ячеек лабиринта
            }
        }

        //удаляем стены по алгоритму recursive backtracker
        removeWalls(cells);

        //добавляем цикличные пути
        AddCycles(cells);

        //выбираем старт и считаем расстояния
        SetStartAndDistances(cells);

        //добавляем выход из лабиринта
        AddExit(cells);

        Maze maze = new Maze();   //создание лабиринта
        maze.cells = cells;
        maze.startX = startX;
        maze.startY = startY;

        return maze;
    }

    // 2) ВТОРОЙ МЕТОД – алгоритм Олдоса-Бродера
    public Maze GenerateMazeAldous(int Width, int Height)   //метод генерации
    {
        this.Width = Width;
        this.Height = Height;

        MazeCell[,] cells = new MazeCell[Width, Height];   //создание массива ячеек

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeCell { X = x, Y = y };  //инициализация ячеек лабиринта
            }
        }

        //удаляем стены по алгоритму Олдоса-Бродера
        removeWallsAldous(cells);

        //добавляем цикличные пути
        AddCycles(cells);

        //выбираем старт и считаем расстояния
        SetStartAndDistances(cells);

        //добавляем выход из лабиринта
        AddExit(cells);

        Maze maze = new Maze();   //создание лабиринта
        maze.cells = cells;
        maze.startX = startX;
        maze.startY = startY;

        return maze;
    }

    //recursive backtracker
    private void removeWalls(MazeCell[,] maze)    //удаление стен
    {
        MazeCell current = maze[0, 0];           //стартовая ячейка
        current.Visited = true;

        Stack<MazeCell> stack = new Stack<MazeCell>();   //стек посещённых ячеек
        stack.Push(current);

        do
        {
            List<MazeCell> unvisitedNeighbours = new List<MazeCell>(); //список не посещённых соседей

            int x = current.X;
            int y = current.Y;

            //добавление непосещённых соседей в список
            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < Width - 1 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < Height - 1 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)    //если есть не посещённые соседи
            {
                MazeCell chosen = unvisitedNeighbours[
                    UnityEngine.Random.Range(0, unvisitedNeighbours.Count)
                ];                                //выбор случайного соседа

                RemoveWall(current, chosen);      //удаление стен между текущей и выбранной ячейками

                chosen.Visited = true;           //отметка о посещении
                stack.Push(chosen);              //добавление выбранной ячейки в стек

                current = chosen;                //переход к выбранной ячейке
            }
            else
            {
                current = stack.Pop();           //возврат к предыдущей ячейке, если нет непосещённых соседей
            }

        } while (stack.Count > 0);               //до тех пор, пока стек не опустеет
    }

    //Олдос-Бродер
    private void removeWallsAldous(MazeCell[,] maze)   //удаление стен
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        //на всякий случай сбрасываем флаг посещения
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y].Visited = false;
            }
        }

        //случайная стартовая ячейка
        int cx = UnityEngine.Random.Range(0, width);
        int cy = UnityEngine.Random.Range(0, height);

        MazeCell current = maze[cx, cy];
        current.Visited = true;

        int visitedCount = 1;           //сколько клеток уже в дереве
        int total = width * height;     //всего клеток

        //пока не посетили все клетки
        while (visitedCount < total)
        {
            int nx = cx;
            int ny = cy;

            //случайно выбираем допустимого соседа (рандомное блуждание)
            bool found = false;
            while (!found)
            {
                int dir = UnityEngine.Random.Range(0, 4); //0 - влево, 1 - вправо, 2 - вниз, 3 - вверх

                if (dir == 0 && cx > 0)
                {
                    nx = cx - 1;
                    ny = cy;
                    found = true;
                }
                else if (dir == 1 && cx < width - 1)
                {
                    nx = cx + 1;
                    ny = cy;
                    found = true;
                }
                else if (dir == 2 && cy > 0)
                {
                    nx = cx;
                    ny = cy - 1;
                    found = true;
                }
                else if (dir == 3 && cy < height - 1)
                {
                    nx = cx;
                    ny = cy + 1;
                    found = true;
                }
                //если направление ведёт за пределы поля – просто выбираем следующее
            }

            MazeCell next = maze[nx, ny];

            //если сосед ещё не в дереве – соединяем его с текущим
            if (!next.Visited)
            {
                RemoveWall(current, next);
                next.Visited = true;
                visitedCount++;
            }

            //переходим в следующую ячейку (даже если она уже посещена)
            current = next;
            cx = nx;
            cy = ny;
        }
    }

    //общие методы
    private void RemoveWall(MazeCell a, MazeCell b)   //удаление стен между смежными ячейками
    {
        //если ячейки в одном столбце (вертикальные соседи)
        if (a.X == b.X)
        {
            if (a.Y > b.Y)      //b ниже a
            {
                a.Bottom = false;
                b.Up = false;
            }
            else                //b выше a
            {
                a.Up = false;
                b.Bottom = false;
            }
        }
        //если ячейки в одной строке (горизонтальные соседи)
        else if (a.Y == b.Y)
        {
            if (a.X > b.X)      //b левее a
            {
                a.Left = false;
                b.Right = false;
            }
            else                //b правее a
            {
                a.Right = false;
                b.Left = false;
            }
        }
    }

    private void AddCycles(MazeCell[,] maze)   //дополнительные циклы
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        // сколько дополнительных проходов сделать
        int cyclesCount = (width * height) / 5;
        if (cyclesCount < 1) cyclesCount = 1;

        for (int i = 0; i < cyclesCount; i++)
        {
            //случайная ячейка внутри лабиринта
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);

            //случайное направление: 0 - влево, 1 - вправо, 2 - вниз, 3 - вверх
            int dir = UnityEngine.Random.Range(0, 4);

            int nx = x;
            int ny = y;

            if (dir == 0) nx = x - 1;        //сосед слева
            if (dir == 1) nx = x + 1;        //сосед справа
            if (dir == 2) ny = y - 1;        //сосед снизу
            if (dir == 3) ny = y + 1;        //сосед сверху

            //проверяем, что сосед в пределах массива
            if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                continue;

            MazeCell a = maze[x, y];
            MazeCell b = maze[nx, ny];

            //удаляем стену только если она ещё существует – чтобы реально появлялась новая петля
            if (nx == x - 1)        //сосед слева
            {
                if (a.Left && b.Right)
                {
                    a.Left = false;
                    b.Right = false;
                }
            }
            else if (nx == x + 1)   //сосед справа
            {
                if (a.Right && b.Left)
                {
                    a.Right = false;
                    b.Left = false;
                }
            }
            else if (ny == y - 1)   //сосед снизу
            {
                if (a.Bottom && b.Up)
                {
                    a.Bottom = false;
                    b.Up = false;
                }
            }
            else if (ny == y + 1)   //сосед сверху
            {
                if (a.Up && b.Bottom)
                {
                    a.Up = false;
                    b.Bottom = false;
                }
            }
        }
    }

    private void SetStartAndDistances(MazeCell[,] maze)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        int side = UnityEngine.Random.Range(0, 4); //0 - левый край, 1 - правый край, 2 - нижний край, 3 - верхний край

        if (side == 0)               
        {
            startX = 0;
            startY = UnityEngine.Random.Range(0, height);
        }
        else if (side == 1)            
        {
            startX = width - 1;
            startY = UnityEngine.Random.Range(0, height);
        }
        else if (side == 2)         
        {
            startX = UnityEngine.Random.Range(0, width);
            startY = 0;
        }
        else                          
        {
            startX = UnityEngine.Random.Range(0, width);
            startY = height - 1;
        }

        MazeCell start = maze[startX, startY];

        if (startX == 0)
        {
            start.Left = false;          
        }
        else if (startX == width - 1)
        {
            start.Right = false;        
        }
        else if (startY == 0)
        {
            start.Bottom = false;  
        }
        else if (startY == height - 1)
        {
            start.Up = false;        
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y].Distance = -1;
            }
        }

        start.Distance = 0;

        //обход в ширину (BFS)
        Queue<MazeCell> queue = new Queue<MazeCell>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            MazeCell current = queue.Dequeue();
            int x = current.X;
            int y = current.Y;
            int dist = current.Distance;

            //сосед слева
            if (!current.Left && x > 0)
            {
                MazeCell n = maze[x - 1, y];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }

            //сосед справа
            if (!current.Right && x < width - 1)
            {
                MazeCell n = maze[x + 1, y];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }

            //сосед снизу
            if (!current.Bottom && y > 0)
            {
                MazeCell n = maze[x, y - 1];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }

            //сосед сверху
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

        if (ex == 0)
        {
            exitCell.Left = false;
        }
        else if (ex == width - 1)
        {
            exitCell.Right = false;
        }
        else if (ey == 0)
        {
            exitCell.Bottom = false;
        }
        else if (ey == height - 1)
        {
            exitCell.Up = false;
        }
    }
}