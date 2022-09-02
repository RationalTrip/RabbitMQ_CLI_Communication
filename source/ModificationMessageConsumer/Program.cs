using AutoMapper;
using Microsoft.Extensions.Configuration;
using ModificationMessageConsumer.Profiles;
using ModificationMessageConsumer.RabbitMQ.ModifyMessage;
using ModificationMessageConsumer.RabbitMQ.ReceiveMessage;
using RabbitMqShared;
using RabbitMqShared.Configuration;



IConfiguration configuration = new ConfigurationBuilder()
    .AddRabbitMQSharedConfiguration()
    .AddJsonFile("appconfig.json")
    .Build();

IMapper mapper = new MapperConfiguration(opt => opt.AddProfile<MessageProfile>())
    .CreateMapper();

using IConnectionHandler connectionHandler = new ConnectionHandler(configuration);

IModifySender modifyHandler = new ModifySender(connectionHandler, configuration, mapper);
IMessageReceiveEventHandler eventHandler = new MessageReceiveEventHandler(modifyHandler, mapper);

using var messageReceiveService = new MessageReceiveService(eventHandler, connectionHandler, configuration);
messageReceiveService.StartCapture();




Console.WriteLine("Enter any key to close the program");
Console.ReadKey();