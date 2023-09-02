using RabbitMQ_Tutorial;

Console.WriteLine((Environment.GetEnvironmentVariables()));
MessageReceiver.Receive();
Console.Read();