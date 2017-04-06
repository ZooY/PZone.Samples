using System;
using System.IO;


namespace PZone.Samples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: File2Base64.exe <file-path>");
            }
            else
            {
                var filePath = args[0];

                var fileBytes = File.ReadAllBytes(filePath);
                var fileContent = Convert.ToBase64String(fileBytes);
                Console.WriteLine(fileContent);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
