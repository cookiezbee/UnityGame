using System.Collections.Generic;

public class Maze         
{
    public MazeCell[,] cells;  

    public int startX;       
    public int startY;
    
    // Список циклов: каждый цикл - это список клеток по маршруту (A..B),
    // Замыкание происходит автоматически (последняя -> первая)
    public List<List<MazeCell>> cycles;
}