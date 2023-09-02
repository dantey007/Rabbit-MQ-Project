using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Tutorial
{
    public class MessageSender
    { 
        private static readonly string LogsExchangeName = "logs";
            
        public static void Send(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
            // just create an exchange and publish the message.
            channel.ExchangeDeclare(LogsExchangeName, ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(LogsExchangeName, string.Empty, basicProperties: null, body: body);
            Console.WriteLine($"Message sent");
        }
    }
}
