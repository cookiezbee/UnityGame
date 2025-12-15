using System.Collections.Generic;

public class Maze         
{
    public MazeCell[,] cells;  

    public int startX;       
    public int startY;
    
    // список циклов: каждый цикл - это список клеток по маршруту (A..B),
    // замыкание происходит автоматически (последняя -> первая)
    public List<List<MazeCell>> cycles;
    
    // NEW: выход наружу
    public int exitX;
    public int exitY;

    // 0 = Left, 1 = Right, 2 = Bottom, 3 = Up
    public int exitSide;
}