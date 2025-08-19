using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Protocol.Plugins;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
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
        //public async Task PublishMessageAsync(string message)
        //{
        //    var factory = new ConnectionFactory() { HostName = _hostName };
        //    using var connection = await factory.CreateConnectionAsync();
        //    using var channel = await connection.CreateChannelAsync();
        //    await channel.QueueDeclareAsync(queue: _queueName,
        //                         durable: false,
        //                         exclusive: false,
        //                         autoDelete: false,
        //                         arguments: null);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var body = Encoding.UTF8.GetBytes(message);
        //        await channel.BasicPublishAsync(exchange: string.Empty,
        //                             routingKey: _queueName,
        //                             mandatory: true,
        //                             basicProperties: new BasicProperties { Persistent = true},
        //                             body: body);
        //        Console.WriteLine($" [x] Sent {message}");
        //        await Task.Delay(2000);
        //    }
        //}
        // gen code consumer Consumer()
        public async Task ConsumeAsync()
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
                //await ((AsyncEventingBasicConsumer)model).Channel.BasicAckAsync(ea.DeliveryTag, false);
            };
            await channel.BasicConsumeAsync(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);
            Console.ReadLine(); // Keep the application running to listen for messages
        }

    }
}
