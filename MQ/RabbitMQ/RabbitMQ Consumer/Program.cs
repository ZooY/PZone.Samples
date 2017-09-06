using System;
using System.Text;
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
            const string MQ_QUEUE_NAME = "RQ_API";


            var factory = new ConnectionFactory { HostName = MQ_HOST, UserName = MQ_USER_NAME, Password = MQ_USER_PASSWORD };
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(MQ_QUEUE_NAME, true, consumer);

                Console.WriteLine();
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }
    }
}