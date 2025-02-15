namespace Tx.Core.RabbitMqProvider.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;

    using RabbitMQ.Client;

    using RabbitMqProvider;

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
        public static async Task<IServiceCollection> AddRabbitMqProviderAsync(this IServiceCollection services, RabbitMqSettings rabbitMqSettings)
        {
            services.AddSingleton(rabbitMqSettings);
            services.AddSingleton<IConnectionFactory>(_ => CreateConnectionFactory(rabbitMqSettings));

            var connectionFactory = CreateConnectionFactory(rabbitMqSettings);
            var connection = await connectionFactory.CreateConnectionAsync();
            services.AddSingleton(connection);

            services.AddSingleton<IRabbitMqClientProvider, RabbitMqClientProvider>();

            return services;
        }

        private static ConnectionFactory CreateConnectionFactory(RabbitMqSettings settings)
        {
            return new ConnectionFactory
            {
                UserName = settings.RabbitMqUserName,
                Password = settings.RabbitMqPassword,
                HostName = settings.RabbitMqHostName,
                Port = settings.RabbitMqPort,
                AutomaticRecoveryEnabled = true,
                ConsumerDispatchConcurrency = settings.RabbitMqConcurrency
            };
        }
    }
}