using System;
using FigureArea;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Проверка с указанием типа фигуры
            Console.WriteLine(Figure.GetArea("7,3,8", "RightTriangle"));
            Console.WriteLine(Figure.GetArea("8", "Circle"));

            //Проверка без указания типа фигуры
            Console.WriteLine(Figure.GetArea("8"));
            Console.WriteLine(Figure.GetArea("7,3,7"));
            Console.Read();
        }
    }
}
