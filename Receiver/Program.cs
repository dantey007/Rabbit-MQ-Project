using RabbitMQ_Tutorial;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            throw new ApplicationException("The app needs Logger Type args to start");
        }
        
        Console.WriteLine(" [*] Waiting for logs.");
        Logger.Log(args[0]);
        Console.Read();
    }
}