namespace TvMaze.RabbitMqProvider.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using RabbitMQ.Client;
    using TvMaze.RabbitMqProvider;
    using TvMaze.ShareCommon.Models.Settings;

    /// <summary>
    /// Defines the <see cref="ConfigureRabbitMqProvider" />.
    /// </summary>
    public static class ConfigureRabbitMqProvider
    {
        /// <summary>
        /// The AddRabbitMqProvider.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        /// <param name="rabbitMqSettings">The rabbitMqSettings<see cref="RabbitMqSettings"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddRabbitMqProvider(this IServiceCollection services, RabbitMqSettings rabbitMqSettings)
        {
            services.AddSingleton(rabbitMqSettings);
            var connectionFactory = new ConnectionFactory
            {
                UserName = rabbitMqSettings.RabbitMqUserName,
                Password = rabbitMqSettings.RabbitMqPassword,
                HostName = rabbitMqSettings.RabbitMqHostName ?? "localhost",
                Port = rabbitMqSettings?.RabbitMqPort ?? 5672,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                ConsumerDispatchConcurrency = rabbitMqSettings?.RabbitMqConcurrency ?? 50,
            };
            services.AddSingleton<IConnectionFactory>(_ => connectionFactory);
            services.AddSingleton<ModelFactory>();
            services.AddSingleton(sp => sp.GetRequiredService<ModelFactory>().CreateChannel());
            services.AddSingleton<IRabbitMqClientProvider, RabbitMqClientProvider>();

            return services;
        }
    }
}