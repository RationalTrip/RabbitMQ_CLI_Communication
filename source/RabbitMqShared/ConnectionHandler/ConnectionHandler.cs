using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RabbitMqShared
{
    public class ConnectionHandler : IConnectionHandler
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection? connection;
        private bool disposedValue = false;

        public ConnectionHandler(IConfiguration configuration)
        {
            var connectionConfig = configuration.GetSection("RabbitMQ:connection");

            var host = connectionConfig["host"];
            var port = int.Parse(connectionConfig["port"]);
            var virtualHost = connectionConfig["virtualHost"];

            var login = connectionConfig["login"];
            var password = connectionConfig["password"];

            _connectionFactory = new ConnectionFactory()
            {
                HostName = host,
                Port = port,
                VirtualHost = virtualHost,

                UserName = login,
                Password = password
            };
        }

        public IModel GetChannel()
        {
            if (disposedValue)
                throw new ObjectDisposedException(nameof(ConnectionHandler));

            var connection = GetConnection();

            return connection.CreateModel();
        }

        public IConnection GetConnection()
        {
            if (disposedValue)
                throw new ObjectDisposedException(nameof(ConnectionHandler));

            if (connection == null || !connection.IsOpen)
            {
                connection?.Dispose();
                connection = _connectionFactory.CreateConnection();
            }

            return connection;
        }

        public void Dispose()
        {
            if (!disposedValue)
            {
                connection?.Dispose();

                disposedValue = true;
            }
        }
    }
}
