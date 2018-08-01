using System;
using System.Threading;
using System.Threading.Tasks;

namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            //new TaskAsync().Run();
            //new AwaitAsync().Run();
            new AwaitNoResultAsync().Run();

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }



        public class TaskAsync
        {
            public void Run()
            {
                // Метод Run синхронный по отношению к потоку Main, т.е. метод Main продолжит
                // выполняться только после завершения метода Run.

                Console.WriteLine("Main process 1");

                // На этой строке поток выполнения не остановится и пойдет дальше.
                // Код метода Wait будет выполняться параллельно коду основного потока.
                var task = Wait();

                // Немного тормознем основной поток, чтобы успел стартануть поток метода Wait и 
                // показать что он работает параллельно с основным потоком.
                Thread.Sleep(500);
                Console.WriteLine("Main process 2");

                // В этой строке мы пытаемся получить результат выполнения метода Wait.
                // Поэтому основной поток остановиться, не будет реагировать на пользовательский 
                // ввод и будет ждать окончания выполнения метода и возвращения результата.
                Console.WriteLine(task.Result);

                Console.WriteLine("Main process 3");
            }


            private Task<string> Wait()
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("Thread process 1");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 2");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 3");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 4");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 5");
                    return "OK";
                });
            }
        }


        public class AwaitAsync
        {
            public async void Run()
            {
                // Метод Run частично синхронный по отношению к потоку Main. Метод Run возвращает 
                // управление в метод Main как только доходит до строки с ключевым словом await.

                Console.WriteLine("Main process 1");

                // На этой строке поток выполнения не остановится и пойдет дальше.
                // Код метода Wait будет выполняться параллельно коду основного потока.
                var task = Wait();

                // Немного тормознем основной поток, чтобы успел стартануть поток метода Wait и 
                // показать что он работает параллельно с основным потоком.
                Thread.Sleep(500);
                Console.WriteLine("Main process 2");

                Console.WriteLine(await task);

                Console.WriteLine("Main process 3");
            }

            private Task<string> Wait()
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("Thread process 1");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 2");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 3");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 4");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 5");
                    return "OK";
                });
            }
        }


        public class AwaitNoResultAsync
        {
            public async void Run()
            {
                Console.WriteLine("Main process 1");

                var task = Wait();

                Thread.Sleep(500);
                Console.WriteLine("Main process 2");

                task.Wait();
                Console.WriteLine("OK");

                Console.WriteLine("Main process 3");
            }

            private async Task Wait()
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Thread process 1");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 2");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 3");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 4");
                    Thread.Sleep(1000);
                    Console.WriteLine("Thread process 5");
                    return "OK";
                });
            }
        }
    }
}