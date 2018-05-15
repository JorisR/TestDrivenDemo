using System;
using System.Text;
using EventDrivenDemo.Publisher.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace EventDrivenDemo.Publisher
{
    public class Publisher : IPublisher
    {
        private readonly IConfigurationRoot _configuration;

        public Publisher(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }
        public void PublishMessage(string message, string queue)
        {
            var rabbitHostName = _configuration["RabbitMqHostName"];
            var body = Encoding.UTF8.GetBytes(message);
            var factory = new ConnectionFactory() {HostName = rabbitHostName};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var prop = channel.CreateBasicProperties();
                    prop.Persistent = true;
                  channel.BasicPublish(exchange:"", routingKey:queue, basicProperties:prop, body:body);
                }
            }
            Console.WriteLine($"Published message {message}");
        }

        public void CreateQueue(string queue)
        {
            var rabbitHostName = _configuration["RabbitMqHostName"];

            var factory = new ConnectionFactory() { HostName = rabbitHostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false);
                }
            }
            Console.WriteLine($"Create queue:  {queue}");
        }
    }
}
