using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using RabbitMQ.Producer;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        var age = -3;
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<TestBackgrounService>();
            })
            .Build();
        await host.RunAsync();
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            if (age < 0 || age > 120)
                throw new ProductException("như cc", age.ToString());
        }
        catch (ProductException ex)
        {
            Console.WriteLine($"Error: {ex.Message} - {ex.ProductId}");
        }

    }

    public class ProductException : Exception
    {
        public string ProductId { get; set; }
        public ProductException(string message, string productId) : base(message)
        {
            ProductId = productId;
        }
    }

    public class TestBackgrounService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Background service is running...");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public class TaskQuartz : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}