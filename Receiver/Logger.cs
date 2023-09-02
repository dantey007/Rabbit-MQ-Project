using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ_Tutorial;

public class Logger
{
    private static string LogsExchangeName => "logs";
    
    public static void Log()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(LogsExchangeName, ExchangeType.Fanout);

        var queue = channel.QueueDeclare();
        Console.WriteLine($"Queue Created: {queue.QueueName}");
        channel.QueueBind(queue: queue.QueueName,  exchange: LogsExchangeName, routingKey: string.Empty);

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