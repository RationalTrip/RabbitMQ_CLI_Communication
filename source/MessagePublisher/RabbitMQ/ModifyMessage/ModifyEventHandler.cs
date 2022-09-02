using AutoMapper;
using MessagePublisher.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMqShared;
using RabbitMqShared.Dtos;
using System.Text;
using System.Text.Json;

namespace MessagePublisher.RabbitMQ.ModifyMessage
{
    internal class ModifyEventHandler : IModifyEventHandler
    {
        private readonly string _exchangeFanout;
        private readonly IMapper _mapper;
        private readonly IConnectionHandler _connectionHandler;

        public ModifyEventHandler(IConnectionHandler connectionHandler, IConfiguration configuration, IMapper mapper)
        {
            _exchangeFanout = configuration.GetSection("RabbitMQ:messaging")["exchange"];
            _mapper = mapper;
            _connectionHandler = connectionHandler;
        }

        public void ProcessEvent(string message)
        {
            var messageTypeModel = JsonSerializer.Deserialize<EventTypeDto>(message);

            switch (messageTypeModel?.EventType)
            {
                case EventTypes.ModificateMessage:
                    ProcessModifyEvent(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"message with message type \"{messageTypeModel?.EventType}\" is not supported");
            }
        }

        private void ProcessModifyEvent(string message)
        {
            var messageModifyModel = JsonSerializer.Deserialize<MessageModifyDto>(message);

            var destinationQueue = messageModifyModel?.QueueOutput;
            var destinationExchange = string.IsNullOrWhiteSpace(destinationQueue) ? _exchangeFanout : "";

            var messageUpload = _mapper.Map<MessageUploadDto>(messageModifyModel);
            messageUpload.EventType = EventTypes.UploadMessage;

            var messageUploadJson = JsonSerializer.Serialize(messageUpload);
            var messageUploadBody = Encoding.UTF8.GetBytes(messageUploadJson);

            using var channel = _connectionHandler.GetChannel();

            channel.ExchangeDeclare(_exchangeFanout, ExchangeType.Fanout);

            channel.BasicPublish(destinationExchange, destinationQueue, null, messageUploadBody);
        }
    }
}
