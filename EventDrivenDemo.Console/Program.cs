using System;
using System.IO;
using EventDrivenDemo.Publisher.Interfaces;
using EventDrivenDemo.Receiver.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventDrivenDemo.ConsoleApp
{
    public class Program
    {

        static void Main(string[] args)
        {
            //configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            var services = new ServiceCollection()
                .AddSingleton(configuration)
                .AddTransient<IPublisher, Publisher.Publisher>()
                .AddTransient<IReceiver, Receiver.Receiver>();
            var serviceProvider = services.BuildServiceProvider();
            var publisher = serviceProvider.GetService<IPublisher>();
            var receiver = serviceProvider.GetService<IReceiver>();
            var queue = "testje";
            publisher.CreateQueue(queue);
            receiver.Receive(queue);
            var userMessage = string.Empty;
            do
            {
                Console.WriteLine("Give input");
                userMessage = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userMessage))
                    break;
               
                publisher.PublishMessage(userMessage, queue);
            } while (!string.IsNullOrWhiteSpace(userMessage));
            receiver.StopReceiving();
            Console.ReadLine();
        }

    }
}
