using RabbitMQ.Client;

namespace RabbitMqShared
{
    public interface IConnectionHandler : IDisposable
    {
        IConnection GetConnection();
        IModel GetChannel();
    }
}
