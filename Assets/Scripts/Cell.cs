using UnityEngine;
// using UnityEngine.UI;  
using TMPro;                   

public class Cell : MonoBehaviour //класс, описывающий ячейку лабиринта
{
    public GameObject Left;    //наличие левой стены
    public GameObject Bottom;  //наличие нижней стены
    public GameObject Up;      //наличие верхней стены
    public GameObject Right;   //наличие правой стены

    public TMP_Text distance;  //ссылка на текст, находящийся в ячейке
}