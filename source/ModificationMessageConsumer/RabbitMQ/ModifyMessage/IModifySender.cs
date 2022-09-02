using ModificationMessageConsumer.Models;

namespace ModificationMessageConsumer.RabbitMQ.ModifyMessage
{
    internal interface IModifySender
    {
        public void SendMessage(Message message, bool modifyForEveryone);
    }
}
