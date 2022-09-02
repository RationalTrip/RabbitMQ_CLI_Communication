using AutoMapper;
using ModificationMessageConsumer.Models;
using ModificationMessageConsumer.RabbitMQ.ModifyMessage;
using RabbitMqShared;
using RabbitMqShared.Dtos;
using System.Text.Json;

namespace ModificationMessageConsumer.RabbitMQ.ReceiveMessage
{
    internal class MessageReceiveEventHandler : IMessageReceiveEventHandler
    {
        private readonly IModifySender _modifyHandler;
        private readonly IMapper _mapper;

        public MessageReceiveEventHandler(IModifySender modifyHandler, IMapper mapper)
        {
            _modifyHandler = modifyHandler;
            _mapper = mapper;
        }

        public void ProcessEvent(string messageModel)
        {
            var messageTypeModel = JsonSerializer.Deserialize<EventTypeDto>(messageModel);

            switch (messageTypeModel?.EventType)
            {
                case EventTypes.UploadMessage:
                    UploadMessageHandle(messageModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"message with message type \"{messageTypeModel?.EventType}\" is not supported");
            }
        }

        void UploadMessageHandle(string messageModel)
        {
            var messageUploadModel = JsonSerializer.Deserialize<MessageUploadDto>(messageModel);

            if (messageUploadModel == null)
                throw new ArgumentException("Message model can not be deserialized to MessageUploadDto type. " +
                    $"{nameof(messageModel)} \"{messageModel}\"");

            Console.WriteLine($"Received message with id = {messageUploadModel.Id} " +
                $"with message : \"{messageUploadModel.Value}\"");

            ModifyMessageIfRequired(_mapper.Map<Message>(messageUploadModel));
        }

        void ModifyMessageIfRequired(Message message)
        {
            var messageValue = message.Value;

            if (messageValue.StartsWith(" ") || messageValue.EndsWith(" "))
            {
                var isMessageModifyForEveryone = messageValue.StartsWith(" ");

                message.Value = messageValue.Trim();

                var modifyType = isMessageModifyForEveryone ? "for everyone" : "only for current service";

                Console.WriteLine($"\tMessage with {message.Id} should be modified {modifyType}: " +
                    $"\"{message.Value}\"");

                _modifyHandler.SendMessage(message, isMessageModifyForEveryone);
            }
        }
    }
}
