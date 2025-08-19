using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Producer
{
    public class RabbitMQService
    {
        private readonly string _hostName;
        private readonly string _queueName;
        public RabbitMQService(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }
        public async Task PublishMessageAsync()
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            for (int i = 0; i < 10; i++)
            {
                string message = $"{DateTime.UtcNow} - {Guid.NewGuid()}";
                var body = Encoding.UTF8.GetBytes(message);
                await channel.BasicPublishAsync(exchange: string.Empty,
                                     routingKey: _queueName,
                                     mandatory: true,
                                     basicProperties: new BasicProperties { Persistent = true},
                                     body: body);
                Console.WriteLine($" [x] Sent {message}");
                await Task.Delay(2000);
            }
        }
    }
}
