using System;
using System.Text;
using EventDrivenDemo.Receiver.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventDrivenDemo.Receiver
{
    public class Receiver : IReceiver
    {
        private readonly IConfigurationRoot _configuration;
        private IConnection rabbitConnection;
        private IModel rabbitChannel;
        public Receiver(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }
        public void Receive(string queue)
        {
            var rabbitHostName = _configuration["RabbitMqHostName"];
            var fileName= string.Format("{0}\\{1}",  _configuration["OutputFolder"], _configuration["OutputFileName"]);
            var factory = new ConnectionFactory() { HostName = rabbitHostName };
            rabbitConnection = factory.CreateConnection();
            rabbitChannel = rabbitConnection.CreateModel();

            var consumer = new EventingBasicConsumer(rabbitChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(fileName, true))
                {
                    file.WriteLine(message);
                }
                Console.WriteLine($"Received: {message}");
            };

            rabbitChannel.BasicConsume(queue: queue,
                autoAck: true,
                consumer: consumer);


        }

        public void StopReceiving()
        {
            rabbitChannel.Close();
           rabbitConnection.Close();
        }
    }
}
