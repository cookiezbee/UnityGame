using System.Collections.Generic;
using UnityEngine;

// List, Stack, Queue

public class Generator //
{
    int Width = 10;           // ������� ���������
    int Height = 10;

    int startX; // ���������� ������
    int startY;

    // 1) ������ ����� � recursive backtracker
    public Maze GenerateMaze(int Width, int Height) //����� ���������
    {
        this.Width = Width;
        this.Height = Height;

        MazeCell[,] cells = new MazeCell[Width, Height]; //�������� ������� �����

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeCell { X = x, Y = y }; //������������� ����� ���������
            }
        }

        //������� ����� �� ��������� recursive backtracker
        removeWalls(cells);

        //��������� ��������� ����
        AddCycles(cells);

        //�������� ����� � ������� ����������
        SetStartAndDistances(cells);

        //��������� ����� �� ���������
        AddExit(cells);

        Maze maze = new Maze(); //�������� ���������
        maze.cells = cells;
        maze.startX = startX;
        maze.startY = startY;

        return maze;
    }

    // 2) ������ ����� � �������� ������-�������
    public Maze GenerateMazeAldous(int Width, int Height) //����� ���������
    {
        this.Width = Width;
        this.Height = Height;

        MazeCell[,] cells = new MazeCell[Width, Height]; //�������� ������� �����

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeCell { X = x, Y = y }; //������������� ����� ���������
            }
        }

        //������� ����� �� ��������� ������-�������
        removeWallsAldous(cells);

        //��������� ��������� ����
        AddCycles(cells);

        //�������� ����� � ������� ����������
        SetStartAndDistances(cells);

        //��������� ����� �� ���������
        AddExit(cells);

        Maze maze = new Maze(); //�������� ���������
        maze.cells = cells;
        maze.startX = startX;
        maze.startY = startY;

        return maze;
    }

    //recursive backtracker
    private void removeWalls(MazeCell[,] maze) //�������� ����
    {
        MazeCell current = maze[0, 0]; //��������� ������
        current.Visited = true;

        Stack<MazeCell> stack = new Stack<MazeCell>(); //���� ���������� �����
        stack.Push(current);

        do
        {
            List<MazeCell> unvisitedNeighbours = new List<MazeCell>(); //������ �� ���������� �������

            int x = current.X;
            int y = current.Y;

            //���������� ������������ ������� � ������
            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < Width - 1 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < Height - 1 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0) //���� ���� �� ���������� ������
            {
                MazeCell chosen = unvisitedNeighbours[
                    Random.Range(0, unvisitedNeighbours.Count)
                ]; //����� ���������� ������

                RemoveWall(current, chosen); //�������� ���� ����� ������� � ��������� ��������

                chosen.Visited = true; //������� � ���������
                stack.Push(chosen); //���������� ��������� ������ � ����

                current = chosen; //������� � ��������� ������
            }
            else
            {
                current = stack.Pop(); //������� � ���������� ������, ���� ��� ������������ �������
            }
        } while (stack.Count > 0); //�� ��� ���, ���� ���� �� ��������
    }

    //�����-������
    private void removeWallsAldous(MazeCell[,] maze) //�������� ����
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        //�� ������ ������ ���������� ���� ���������
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y].Visited = false;
            }
        }

        //��������� ��������� ������
        int cx = Random.Range(0, width);
        int cy = Random.Range(0, height);

        MazeCell current = maze[cx, cy];
        current.Visited = true;

        int visitedCount = 1; //������� ������ ��� � ������
        int total = width * height; //����� ������

        //���� �� �������� ��� ������
        while (visitedCount < total)
        {
            int nx = cx;
            int ny = cy;

            //�������� �������� ����������� ������ (��������� ���������)
            bool found = false;
            while (!found)
            {
                int dir = Random.Range(0, 4); //0 - �����, 1 - ������, 2 - ����, 3 - �����

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
                //���� ����������� ���� �� ������� ���� � ������ �������� ���������
            }

            MazeCell next = maze[nx, ny];

            //���� ����� ��� �� � ������ � ��������� ��� � �������
            if (!next.Visited)
            {
                RemoveWall(current, next);
                next.Visited = true;
                visitedCount++;
            }

            //��������� � ��������� ������ (���� ���� ��� ��� ��������)
            current = next;
            cx = nx;
            cy = ny;
        }
    }

    //����� ������
    private void RemoveWall(MazeCell a, MazeCell b) //�������� ���� ����� �������� ��������
    {
        //���� ������ � ����� ������� (������������ ������)
        if (a.X == b.X)
        {
            if (a.Y > b.Y) //b ���� a
            {
                a.Bottom = false;
                b.Up = false;
            }
            else //b ���� a
            {
                a.Up = false;
                b.Bottom = false;
            }
        }
        //���� ������ � ����� ������ (�������������� ������)
        else if (a.Y == b.Y)
        {
            if (a.X > b.X) //b ����� a
            {
                a.Left = false;
                b.Right = false;
            }
            else //b ������ a
            {
                a.Right = false;
                b.Left = false;
            }
        }
    }

    private void AddCycles(MazeCell[,] maze) //�������������� �����
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        // ������� �������������� �������� �������
        int cyclesCount = (width * height) / 5;
        if (cyclesCount < 1) cyclesCount = 1;

        for (int i = 0; i < cyclesCount; i++)
        {
            //��������� ������ ������ ���������
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            //��������� �����������: 0 - �����, 1 - ������, 2 - ����, 3 - �����
            int dir = Random.Range(0, 4);

            int nx = x;
            int ny = y;

            if (dir == 0) nx = x - 1; //����� �����
            if (dir == 1) nx = x + 1; //����� ������
            if (dir == 2) ny = y - 1; //����� �����
            if (dir == 3) ny = y + 1; //����� ������

            //���������, ��� ����� � �������� �������
            if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                continue;

            MazeCell a = maze[x, y];
            MazeCell b = maze[nx, ny];

            //������� ����� ������ ���� ��� ��� ���������� � ����� ������� ���������� ����� �����
            if (nx == x - 1) //����� �����
            {
                if (a.Left && b.Right)
                {
                    a.Left = false;
                    b.Right = false;
                }
            }
            else if (nx == x + 1) //����� ������
            {
                if (a.Right && b.Left)
                {
                    a.Right = false;
                    b.Left = false;
                }
            }
            else if (ny == y - 1) //����� �����
            {
                if (a.Bottom && b.Up)
                {
                    a.Bottom = false;
                    b.Up = false;
                }
            }
            else if (ny == y + 1) //����� ������
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

        int side = Random.Range(0, 4); //0 - ����� ����, 1 - ������ ����, 2 - ������ ����, 3 - ������� ����

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

        //����� � ������ (BFS)
        Queue<MazeCell> queue = new Queue<MazeCell>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            MazeCell current = queue.Dequeue();
            int x = current.X;
            int y = current.Y;
            int dist = current.Distance;

            //����� �����
            if (!current.Left && x > 0)
            {
                MazeCell n = maze[x - 1, y];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }

            //����� ������
            if (!current.Right && x < width - 1)
            {
                MazeCell n = maze[x + 1, y];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }

            //����� �����
            if (!current.Bottom && y > 0)
            {
                MazeCell n = maze[x, y - 1];
                if (n.Distance == -1)
                {
                    n.Distance = dist + 1;
                    queue.Enqueue(n);
                }
            }

            //����� ������
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