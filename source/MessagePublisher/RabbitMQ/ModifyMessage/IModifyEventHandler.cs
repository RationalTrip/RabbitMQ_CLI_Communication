namespace MessagePublisher.RabbitMQ.ModifyMessage
{
    internal interface IModifyEventHandler
    {
        void ProcessEvent(string message);
    }
}
