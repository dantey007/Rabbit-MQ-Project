using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Tutorial
{
    public class MessageReceiver
    {
        public static void Receive()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); // prefetch count tells that at max 1 request can only be processed at a time.
            
            channel.QueueDeclare(
                queue: "test_queue_durable",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            var handler = new EventHandler<BasicDeliverEventArgs>((sender, args) =>
            {
                // body is a byte stream.
                var body = args.Body.ToArray();
                // decode the body from stream.
                var message = Encoding.UTF8.GetString(body);
                Thread.Sleep(message.Split('.').Length * 1000);
                Console.WriteLine($"Received message:-  {message}");
                channel.BasicAck(args.DeliveryTag, false);
            });
            // add the event listener to the consumer.
            consumer.Received += handler;

            channel.BasicConsume(
                queue: "test_queue",
                autoAck: false,
                consumer: consumer
                );

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
