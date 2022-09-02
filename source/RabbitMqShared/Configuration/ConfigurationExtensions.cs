using Microsoft.Extensions.Configuration;

namespace RabbitMqShared.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddRabbitMQSharedConfiguration(this IConfigurationBuilder builder) 
            => builder.AddJsonFile("rabbitmqSharedConfig.json");
    }
}
