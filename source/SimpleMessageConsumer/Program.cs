using Microsoft.Extensions.Configuration;
using RabbitMqShared;
using RabbitMqShared.Configuration;
using SimpleMessageConsumer.RabbitMQ.ReceiveMessage;



IConfiguration configuration = new ConfigurationBuilder()
    .AddRabbitMQSharedConfiguration()
    .AddJsonFile("appconfig.json")
    .Build();

using IConnectionHandler connectionHandler = new ConnectionHandler(configuration);

IMessageReceiveEventHandler eventHandler = new MessageReceiveEventHandler();
using var eventService = new MessageReceiveService(eventHandler, connectionHandler, configuration);

eventService.StartCapture();



Console.WriteLine("Enter any key to close the program");
Console.ReadKey();