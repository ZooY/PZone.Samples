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
                int workerThreads;
                int completionPortThreads;
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                Console.WriteLine($"Processors: {Environment.ProcessorCount}");
                Console.WriteLine($"Threads: worker = {workerThreads}, completion port = {completionPortThreads}");
                var isSuccess = ThreadPool.SetMaxThreads(8, 8);
                Console.WriteLine($"Thread set: {isSuccess}");
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                Console.WriteLine($"Threads: worker = {workerThreads}, completion port = {completionPortThreads}");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, e) => { ThreadPool.QueueUserWorkItem(OnGetDataMessage, e); };
                channel.BasicConsume(MQ_QUEUE_NAME, true, consumer);
                
                Console.WriteLine($"Main thread ID = {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine();
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }


        private static void OnGetDataMessage(object arg)
        {
            BasicDeliverEventArgs e = (BasicDeliverEventArgs)arg;
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Thread.Sleep(2000);
            Console.WriteLine($" [{Thread.CurrentThread.ManagedThreadId}] {message}");
        }
    }
}
