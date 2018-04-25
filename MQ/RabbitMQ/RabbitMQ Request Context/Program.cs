using System;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace PZone.Samples
{
    /// <summary>
    /// Пример создания контекста обработки запроса, доступного через статическое свойство из любой точки кода.
    /// </summary>
    class Program
    {
        static void Main()
        {
            const string MQ_HOST = "crmdevapp01";
            const string MQ_USER_NAME = "user";
            const string MQ_USER_PASSWORD = "user";
            const string MQ_QUEUE_NAME = "TEST";

            var factory = new ConnectionFactory { HostName = MQ_HOST, UserName = MQ_USER_NAME, Password = MQ_USER_PASSWORD };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (s, e) =>
                {
                    // ReSharper disable once AccessToDisposedClosure
                    ThreadPool.QueueUserWorkItem(OnGetDataMessage, new ProcessingContext { Channel = channel, Message = e });
                };
                channel.BasicConsume(MQ_QUEUE_NAME, false, consumer);

                Console.WriteLine($"Прослушивание очереди {MQ_QUEUE_NAME} запущено");
                Console.WriteLine("Нажмите любую клавишу для выхода");
                Console.WriteLine();
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Контекст запуска обработки запроса в новом потоке.
        /// </summary>
        public class ProcessingContext
        {
            public IModel Channel { get; set; }
            public BasicDeliverEventArgs Message { get; set; }
        }


        private static void OnGetDataMessage(object processingContext)
        {
            var context = (ProcessingContext)processingContext;

            RequestContextFactory.Create(context.Message);

            var messageString = Encoding.UTF8.GetString(context.Message.Body);
            Thread.Sleep(500);
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Message: {messageString}");
            new Other().Foo();
            Console.WriteLine();
            context.Channel.BasicAck(context.Message.DeliveryTag, false);
        }


        /// <summary>
        /// Фабрика для создания новых контекстов обработки запроса.
        /// </summary>
        public class RequestContextFactory
        {
            /// <summary>
            /// Идентификатор контекста обработки запроса в хранилище потока.
            /// </summary>
            internal const string ContextStorageKey = "RmqRequestContext";


            /// <summary>
            /// Создание нового контекста обработки запроса в текущем потоке.
            /// </summary>
            /// <param name="message">Содержимое запроса RMQ.</param>
            public static void Create(BasicDeliverEventArgs message)
            {
                var context = new RequestContext(message);
                CallContext.SetData(ContextStorageKey, context);
            }
        }


        /// <summary>
        /// Контекста обработки запроса.
        /// </summary>
        public class RequestContext
        {
            /// <summary>
            /// Контекст обработки запроса в текущем потоке.
            /// </summary>
            /// <remarks>
            /// Значение свойства может быть нулевым. Это означает, что в текущем потоке нет запроса, находящегося в обработке.
            /// </remarks>
            public static RequestContext Current => CallContext.GetData(RequestContextFactory.ContextStorageKey) as RequestContext;

            /// <summary>
            /// Содержимое запроса RMQ.
            /// </summary>
            public BasicDeliverEventArgs Message { get; }


            /// <summary>
            /// Конструтор класса.
            /// </summary>
            /// <param name="message">Содержимое запроса RMQ.</param>
            public RequestContext(BasicDeliverEventArgs message)
            {
                Message = message;
            }
        }


        public class Other
        {
            public void Foo()
            {
                var message = RequestContext.Current.Message;
                var messageString = Encoding.UTF8.GetString(message.Body);
                Console.WriteLine($"Other Thread: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Other Message: {messageString}");
            }
        }
    }
}