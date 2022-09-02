namespace RabbitMqShared.Dtos
{
    public class MessageUploadDto
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
    }
}
