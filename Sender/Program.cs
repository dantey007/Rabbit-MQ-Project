using RabbitMQ_Tutorial;

while (true)
{
    var message = Convert.ToString((Console.ReadLine()))!;
    MessageSender.Send(message);
}