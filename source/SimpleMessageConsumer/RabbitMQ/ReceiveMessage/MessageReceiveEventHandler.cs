using RabbitMqShared;
using RabbitMqShared.Dtos;
using System.Text.Json;

namespace SimpleMessageConsumer.RabbitMQ.ReceiveMessage
{
    internal class MessageReceiveEventHandler : IMessageReceiveEventHandler
    {
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

            Console.WriteLine($"Received message with id = {messageUploadModel?.Id} " +
                $"with message : \"{messageUploadModel?.Value}\"");
        }
    }
}
