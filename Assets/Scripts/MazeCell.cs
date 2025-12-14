public class MazeCell        //логическое описание одной ячейки
{
    public int X;
    public int Y;

    public bool Left = true;
    public bool Bottom = true;
    public bool Up = true;
    public bool Right = true;

    public bool Visited = false;

    public int Distance = -1;   //кратчайшее расстояние от клетки "Старт"
}