using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;


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
            const string MQ_QUEUE_NAME = "RQ_Smev";


            var factory = new ConnectionFactory { HostName = MQ_HOST, UserName = MQ_USER_NAME, Password = MQ_USER_PASSWORD };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (s, e) =>
                {
                    // Получение и обработка сообщения.
                    var message = e.GetMessage();
                    Console.WriteLine($"{message} | Headers: {string.Join("; ", e.BasicProperties.Headers.Select(h => $"{h.Key} = {Encoding.UTF8.GetString((byte[])h.Value)}"))}");
                    channel.BasicAck(e.DeliveryTag, false);
                    
                    //Thread.Sleep(5000);

                    // Отправка ответа.
                    if (!string.IsNullOrWhiteSpace(e.BasicProperties.ReplyTo))
                    {
                        BasicProperties properties = null;
                        if (!string.IsNullOrWhiteSpace(e.BasicProperties.CorrelationId))
                            properties = new BasicProperties { CorrelationId = e.BasicProperties.CorrelationId };
                        channel.BasicPublish(
                            exchange: MQ_EXCHANGE_NAME,
                            routingKey: e.BasicProperties.ReplyTo,
                            body: Encoding.UTF8.GetBytes($"{message} processed"),
                            basicProperties: properties);
                    }
                };
                channel.BasicConsume(MQ_QUEUE_NAME, false, consumer);


                Console.WriteLine($"Прослушивание очереди {MQ_QUEUE_NAME} запущено");
                Console.WriteLine("Нажмите любую клавишу для выхода");
                Console.WriteLine();
                Console.ReadKey();
            }
        }
    }
}