namespace RabbitMqShared.Dtos
{
    public class MessageModifyDto
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string QueueOutput { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
    }
}
