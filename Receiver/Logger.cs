using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ_Tutorial;

public class Logger
{
    private static string LogsExchangeName => "Logging";
    
    public static void Log(string loggerType)
    {
        Console.WriteLine($"Listening to {loggerType} message type");
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(LogsExchangeName, ExchangeType.Direct, durable: false, autoDelete: true);

        var queue = channel.QueueDeclare();
        Console.WriteLine($"Queue Created: {queue.QueueName}");
        channel.QueueBind(queue: queue.QueueName,  exchange: LogsExchangeName, routingKey: loggerType);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            byte[] body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message Logged: {message}");
        };
        channel.BasicConsume(queue: queue.QueueName, autoAck: true, consumer: consumer);
    }
}