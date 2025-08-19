using RabbitMQ.Consumer;

class Program
{
    static void Main(string[] args)
    {
        var rabbitMQService = new RabbitMQService("localhost", "trunghien");
        rabbitMQService.ConsumeAsync().GetAwaiter().GetResult();
    }
}