using System;
using System.Text;
using RabbitMQ.Client;


namespace PZone.Samples
{
    /// <summary>
    /// Чтение одного сообщения из очереди RabbitMQ.
    /// </summary>
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

                var result = channel.BasicGet(MQ_QUEUE_NAME, true);
                if (result == null)
                {
                    Console.WriteLine("Result is NULL");
                }
                else
                {
                    var message = Encoding.UTF8.GetString(result.Body);
                    Console.WriteLine(message);
                }
            }

            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine("Press any key...");
        }
    }
}
