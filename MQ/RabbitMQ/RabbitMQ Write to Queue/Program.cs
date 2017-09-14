using System;
using System.Text;
using RabbitMQ.Client;


namespace PZone.Samples
{
    /// <summary>
    /// Отправка одного сообщения в точку обмена.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            const string MQ_HOST = "crmdevapp01";
            const string MQ_USER_NAME = "user";
            const string MQ_USER_PASSWORD = "user";
            const string MQ_EXCHANGE_NAME = "API";


            var factory = new ConnectionFactory { HostName = MQ_HOST, UserName = MQ_USER_NAME, Password = MQ_USER_PASSWORD };
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                var message = $"Hello! - {DateTime.Now}";
                channel.BasicPublish(exchange: MQ_EXCHANGE_NAME, routingKey: "", body: Encoding.UTF8.GetBytes(message));
            }
        }
    }
}