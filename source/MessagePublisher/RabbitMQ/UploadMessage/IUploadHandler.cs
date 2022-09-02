using MessagePublisher.Models;

namespace MessagePublisher.RabbitMQ.UploadMessage
{
    internal interface IUploadHandler
    {
        void PublishMessage(Message message);
    }
}
