using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqShared;
using System.Text;

namespace SimpleMessageConsumer.RabbitMQ.ReceiveMessage
{
    internal class MessageReceiveService : IDisposable
    {
        private readonly IMessageReceiveEventHandler _eventHandler;
        private readonly IConnectionHandler _connectionHandler;
        private IModel _channel = null!;
        private string _receiveQueueName = null!;
        private string _exchangeName = null!;

        public MessageReceiveService(IMessageReceiveEventHandler eventHandler,
            IConnectionHandler connectionHandler,
            IConfiguration configuration)
        {
            _eventHandler = eventHandler;
            _connectionHandler = connectionHandler;

            InitializeRabbitMQ(configuration);
        }

        void InitializeRabbitMQ(IConfiguration configuration)
        {
            _channel = _connectionHandler.GetChannel();

            var configMessageSection = configuration.GetSection("RabbitMQ:messaging");

            _receiveQueueName = configMessageSection["receiveQueue"];
            _exchangeName = configMessageSection["exchange"];

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);
            _channel.QueueDeclare(_receiveQueueName);

            _channel.QueueBind(_receiveQueueName, _exchangeName, "");
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

            _channel.BasicConsume(_receiveQueueName, true, consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
