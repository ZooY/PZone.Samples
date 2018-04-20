using System;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            var type = typeof(Program);
            Console.WriteLine($"Name = {type.Name}");
            Console.WriteLine($"FullName = {type.FullName}");
            Console.WriteLine($"AssemblyQualifiedName = {type.AssemblyQualifiedName}");

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}