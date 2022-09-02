using AutoMapper;
using MessagePublisher.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMqShared;
using RabbitMqShared.Dtos;
using System.Text;
using System.Text.Json;

namespace MessagePublisher.RabbitMQ.UploadMessage
{
    internal class UploadHandler : IUploadHandler
    {
        private readonly IConnectionHandler _connectionHandler;
        private readonly IMapper _mapper;
        private readonly string _exchange;

        public UploadHandler(IConnectionHandler connectionHandler, IConfiguration configuration, IMapper mapper)
        {
            _connectionHandler = connectionHandler;
            _mapper = mapper;
            _exchange = configuration.GetSection("RabbitMQ:messaging")["exchange"];
        }

        public void PublishMessage(Message message)
        {
            if(message is null)
                throw new ArgumentNullException(nameof(message));

            var messageUpload = _mapper.Map<MessageUploadDto>(message);
            messageUpload.EventType = EventTypes.UploadMessage;

            var messageUploadJson = JsonSerializer.Serialize(messageUpload);
            var messageUploadBody = Encoding.UTF8.GetBytes(messageUploadJson);

            using var channel = _connectionHandler.GetChannel();

            channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);

            channel.BasicPublish(_exchange, "", null, messageUploadBody);
        }
    }
}
