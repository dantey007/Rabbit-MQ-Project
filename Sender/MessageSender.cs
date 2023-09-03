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
        private static readonly string LogsExchangeName = "Logging";
            
        public static void Send(string message)
        {
            // we can pass the routing key as arguments for the channel message.
            var messageArgs = message.Split("--");
            
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
            // just create an exchange and publish the message.
            channel.ExchangeDeclare(LogsExchangeName, ExchangeType.Direct, durable: false, autoDelete: true);

            Console.WriteLine($"This message will be sent to queue with routing key as {messageArgs.Last()}");
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(LogsExchangeName, 
                messageArgs.Length > 1 ? messageArgs.Last() : string.Empty, 
                basicProperties: null, 
                body: body);
            
            Console.WriteLine($"Message sent");
        }
    }
}
