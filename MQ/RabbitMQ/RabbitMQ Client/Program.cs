using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using RabbitMQ.Client.MessagePatterns;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            const string MQ_HOST = "crmdevapp01";
            const string MQ_USER_NAME = "user";
            const string MQ_USER_PASSWORD = "user";
            const string MQ_EXCHANGE_NAME = "API";
            const string MQ_QUEUE_NAME = "RS_Test";

            var factory = new ConnectionFactory { HostName = MQ_HOST, UserName = MQ_USER_NAME, Password = MQ_USER_PASSWORD, Port = AmqpTcpEndpoint.UseDefaultPort };
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                do
                {
                    string response = null;

                    var correlationId = Guid.NewGuid().ToString();
                    var message = Console.ReadLine();
                    channel.BasicPublish(
                        exchange: MQ_EXCHANGE_NAME,
                        routingKey: "Smev",
                        body: Encoding.UTF8.GetBytes(message),
                        basicProperties: new BasicProperties
                        {
                            ReplyTo = MQ_QUEUE_NAME,
                            DeliveryMode = 2,
                            CorrelationId = correlationId,
                            Headers = new Dictionary<string, object> { { "Method", "Test" } }
                        }
                    );
                    
                    var sub = new Subscription(channel, MQ_QUEUE_NAME, false);
                    foreach (BasicDeliverEventArgs e in sub)
                    {
                        if (e.BasicProperties.CorrelationId == correlationId)
                        {
                            response = e.GetMessage();
                            sub.Ack(e);
                            break;
                        }
                    }

                    Console.WriteLine(response);
                } while (true);
            }
        }
    }
}