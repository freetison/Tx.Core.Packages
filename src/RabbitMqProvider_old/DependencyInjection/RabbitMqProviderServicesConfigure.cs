using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMqProvider.Connection;
using RabbitMqProvider.Models;

namespace RabbitMqProvider.DependencyInjection;

public static class RabbitMqProviderServicesConfigure {
    public static void AddRabbitMqProvider(this IServiceCollection services,RabbitMqConfigurationSettings settings)
    {
        services.AddSingleton<RabbitMqConfigurationSettings>(settings);
            
        services.AddSingleton<IRabbitMqClientProvider>(provider =>
        {
            var factory = new ConnectionFactory
            {
                UserName = settings.RabbitMqUsername,
                Password = settings.RabbitMqPassword,
                HostName = settings.RabbitMqHostname,
                Port = settings.RabbitMqPort.GetValueOrDefault(),
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                ConsumerDispatchConcurrency = settings.RabbitMqConsumerConcurrency.GetValueOrDefault(),
            };

            return new RabbitMqClientProvider(factory);
        });
            
    }
        
}