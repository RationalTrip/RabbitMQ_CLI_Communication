using AutoMapper;
using ModificationMessageConsumer.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMqShared;
using System.Text;
using System.Text.Json;
using RabbitMqShared.Dtos;

namespace ModificationMessageConsumer.RabbitMQ.ModifyMessage
{
    internal class ModifySender : IModifySender
    {
        private readonly IConnectionHandler _connectionHandler;
        private readonly IMapper _mapper;
        private readonly string _requestQueue;
        private readonly string _receiveQueue;

        public ModifySender(IConnectionHandler connectionHandler, IConfiguration configuration, IMapper mapper)
        {
            _connectionHandler = connectionHandler;
            _mapper = mapper;
            _requestQueue = configuration.GetSection("RabbitMQ:messaging")["modifyRequestQueue"];
            _receiveQueue = configuration.GetSection("RabbitMQ:messaging")["receiveQueue"];
        }

        public void SendMessage(Message message, bool modifyForEveryone)
        {
            if(message is null)
                throw new ArgumentNullException(nameof(message));

            var messageModify = _mapper.Map<MessageModifyDto>(message);

            messageModify.EventType = EventTypes.ModificateMessage;
            messageModify.QueueOutput = modifyForEveryone ? "" : _receiveQueue;

            var messageModifyJson = JsonSerializer.Serialize(messageModify);
            var messageModifyBody = Encoding.UTF8.GetBytes(messageModifyJson);

            using var channel = _connectionHandler.GetChannel();

            channel.BasicPublish("", _requestQueue, null, messageModifyBody);
        }
    }
}
