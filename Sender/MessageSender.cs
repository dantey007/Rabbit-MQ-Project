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
        public static void Send(string message)
        {
            // creating a connection factory.
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            // since we have not declared the exchnage here, the default exchange is being used. i.e Direct exchange is being used.
            channel.QueueDeclare(
                queue: "test_queue_durable",
                durable: true, // durable means that data will persist on server restart.
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            // const string message = "Hello! Manu this side.";
            var body = Encoding.UTF8.GetBytes( message );
            var props = channel.CreateBasicProperties();
            props.Persistent = true;
            
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "test_queue",
                basicProperties: props,
                body: body
                );
            Console.WriteLine($"Sent Message");
        }
    }
}
