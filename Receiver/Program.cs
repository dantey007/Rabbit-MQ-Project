using RabbitMQ_Tutorial;

while (true)
{
    Console.WriteLine((Environment.GetEnvironmentVariables()));
    MessageReceiver.Receive();
}