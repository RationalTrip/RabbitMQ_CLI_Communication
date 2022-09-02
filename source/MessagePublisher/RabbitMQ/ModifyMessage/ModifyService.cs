using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqShared;
using System.Text;

namespace MessagePublisher.RabbitMQ.ModifyMessage
{
    internal class ModifyService : IDisposable
    {
        private readonly IConnectionHandler _connectionHandler;
        private readonly IModifyEventHandler _eventHandler;
        private string _queueModifyName = null!;
        private IModel _channel = null!;

        public ModifyService(IConnectionHandler connectionHandler, IModifyEventHandler eventHandler, IConfiguration configuration)
        {
            _connectionHandler = connectionHandler;
            _eventHandler = eventHandler;

            InitializeRabbitMQ(configuration);
        }
        void InitializeRabbitMQ(IConfiguration configuration)
        {
            _queueModifyName = configuration.GetSection("RabbitMQ:messaging")["modifyRequestQueue"];

            _channel = _connectionHandler.GetChannel();

            _channel.QueueDeclare(_queueModifyName);
        }
        public void StartCapture()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                var body = ea.Body;

                string message = Encoding.UTF8.GetString(body.ToArray());

                _eventHandler.ProcessEvent(message);
            };

            _channel.BasicConsume(_queueModifyName, true, consumer);
        }
        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
