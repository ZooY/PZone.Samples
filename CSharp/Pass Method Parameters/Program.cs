using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            int intVal = 0;
            SetInt(intVal);
            Console.WriteLine(intVal); // output: 0

            string stringVal = "Old";
            SetString(stringVal);
            Console.WriteLine(stringVal); // output: Old

            List<int> listVal = new List<int> { 1, 2 };
            AddList(listVal);
            Console.WriteLine(string.Join(", ", listVal)); // output: 1, 2, 3

            SetList(listVal);
            Console.WriteLine(string.Join(", ", listVal)); // output: 1, 2, 3

            SetList(ref listVal);
            Console.WriteLine(string.Join(", ", listVal)); // output: 0

            Console.ReadKey();
        }


        private static void SetInt(int intVal)
        {
            intVal = 1;
        }


        private static void SetString(string stringVal)
        {
            stringVal = "New";
        }


        private static void AddList(List<int> listVal)
        {
            listVal.Add(3);
        }


        private static void SetList(List<int> listVal)
        {
            listVal = new List<int>();
            listVal.Clear();
            listVal.Add(0);
        }


        private static void SetList(ref List<int> listVal)
        {
            listVal = new List<int>();
            listVal.Clear();
            listVal.Add(0);
        }
    }
}