using System.Text;
using RabbitMQ.Client.Events;


namespace PZone.Samples
{
    public static class BasicDeliverEventArgsExtensions
    {
        public static string GetMessage(this BasicDeliverEventArgs e)
        {
            return Encoding.UTF8.GetString(e.Body);
        }
    }
}