using System;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var pad = 20;

            Console.WriteLine(($"=== {typeof(int).Name} ===").PadRight(pad, '='));
            Console.WriteLine($"Min: {int.MinValue:0,0}");
            Console.WriteLine($"Max: {int.MaxValue:0,0}");
            Console.WriteLine();

            Console.WriteLine(($"=== {typeof(decimal).Name} ===").PadRight(pad, '='));
            Console.WriteLine($"Min: {decimal.MinValue:#,#.#}");
            Console.WriteLine($"Max: {decimal.MaxValue:#,#.#}");
            for (var i = 1; i <= 10; i++)
            {
                Console.WriteLine($"Кол-во десятичных знаков - {i}");
                Console.WriteLine($"   Min: {decimal.MinValue / (decimal)Math.Pow(10, i):#,#.##########}");
                Console.WriteLine($"   Max: {decimal.MaxValue / (decimal)Math.Pow(10, i):#,#.##########}");
            }
            Console.WriteLine($"Optimal Min: {-7000000000000000000.0000000000m:#,#.0000000000}");
            Console.WriteLine($"Optimal Max: {7000000000000000000.0000000000m:#,#.0000000000}");
            Console.WriteLine();

            Console.WriteLine(($"=== {typeof(double).Name} ===").PadRight(pad, '='));
            Console.WriteLine($"Min: {double.MinValue:#,#.#}");
            Console.WriteLine($"Max: {double.MaxValue:#,#.#}");
            Console.WriteLine($"Optimal Min: {-10000000000.00000d:#,#.00000}");
            Console.WriteLine($"Optimal Max: { 10000000000.00000d:#,#.00000}");
            Console.WriteLine();


            Console.ReadKey();
        }
    }
}