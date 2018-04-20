using System;


namespace PZone.Samples
{
    /// <summary>
    /// Пример получения имени типа разной степени подробности.
    /// </summary>
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