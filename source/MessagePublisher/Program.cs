using AutoMapper;
using MessagePublisher.Models;
using MessagePublisher.Profiles;
using MessagePublisher.RabbitMQ.ModifyMessage;
using MessagePublisher.RabbitMQ.UploadMessage;
using Microsoft.Extensions.Configuration;
using RabbitMqShared;
using RabbitMqShared.Configuration;



IConfiguration configuration = new ConfigurationBuilder()
    .AddRabbitMQSharedConfiguration()
    .Build();

IMapper mapper = new MapperConfiguration(opt => opt.AddProfile<MessageProfile>())
    .CreateMapper();

using IConnectionHandler connectionHandler = new ConnectionHandler(configuration);

IUploadHandler uploadHandler = new UploadHandler(connectionHandler, configuration, mapper);

IModifyEventHandler modifyEventHandler = new ModifyEventHandler(connectionHandler, configuration, mapper);
using var modifyService = new ModifyService(connectionHandler, modifyEventHandler, configuration);

modifyService.StartCapture();





string exitString = "/exit";

for(int messageId = 0;; messageId++)
{
    Console.Write($"Print message or \"{exitString}\" to end the program: ");

    var input = Console.ReadLine();

    if (input == null || input.Trim() == exitString)
        break;

    var message = new Message { Id = messageId, Value = input };

    uploadHandler.PublishMessage(message);
}