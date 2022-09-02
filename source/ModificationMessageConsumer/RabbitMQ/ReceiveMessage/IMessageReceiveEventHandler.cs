namespace ModificationMessageConsumer.RabbitMQ.ReceiveMessage
{
    internal interface IMessageReceiveEventHandler
    {
        void ProcessEvent(string message);
    }
}
