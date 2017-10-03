using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            const string MQ_HOST = "crmdevapp01";
            const string MQ_USER_NAME = "user";
            const string MQ_USER_PASSWORD = "user";
            const string MQ_QUEUE_NAME = "TEST";


            var factory = new ConnectionFactory { HostName = MQ_HOST, UserName = MQ_USER_NAME, Password = MQ_USER_PASSWORD };
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    // Попутно с текстом сообщения, выводим номер потока, чтобы показать, что все 
                    // сообщения обрабатываются в одном потоке последовательно. Т.е. после 
                    // вычитывания из очереди сообщения складываются в очередь в памяти.
                    // Обработка сообщений происходит в потоке, отдельном от основного потока.
                    Console.WriteLine($" [{Thread.CurrentThread.ManagedThreadId}] {message}");
                    Wait(2000);
                };
                channel.BasicConsume(MQ_QUEUE_NAME, true, consumer);

                Console.WriteLine($"Main thread ID = {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine();
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }

        private static void Wait(int interval)
        {
            var s = new[] { "|", "/", "-", @"\" };
            var i = 0;
            var repeats = interval / 100;
            var autoEvent = new AutoResetEvent(false);
            var timer = new Timer(state =>
            {
                Console.CursorLeft = 1;
                if (repeats == 0)
                    ((AutoResetEvent)state).Set();
                repeats--;
                Console.Write(s[i++]);
                if (i > 3)
                    i = 0;
                //Console.SetCursorPosition(1, Console.CursorTop);
            }, autoEvent, 0, 100);
            autoEvent.WaitOne();
            timer.Dispose();
            //Console.SetCursorPosition(1, Console.CursorTop);
            Console.CursorLeft = 0;
            Console.Write(" >");
            Console.CursorLeft = 0;
        }
    }
}
