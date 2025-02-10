namespace TvMaze.RabbitMqProvider
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.Exceptions;
    using TvMaze.ShareCommon.Exceptions;
    using TvMaze.ShareCommon.Models.Settings;

    /// <summary>
    /// Defines the <see cref="ModelFactory" />.
    /// </summary>
    public class ModelFactory : IDisposable
    {
        /// <summary>
        /// Defines the _connection.
        /// </summary>
        private readonly IConnection _connection;

        /// <summary>
        /// Defines the _rabbitMqSettings.
        /// </summary>
        private readonly RabbitMqSettings _rabbitMqSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFactory"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connectionFactory<see cref="IConnectionFactory"/>.</param>
        /// <param name="rabbitMqSettings">The rabbitMqSettings<see cref="RabbitMqSettings"/>.</param>
        public ModelFactory(IConnectionFactory connectionFactory, RabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
            _connection = StartConnection(connectionFactory);
        }

        /// <summary>
        /// The CreateChannel.
        /// </summary>
        /// <returns>The <see cref="IModel"/>.</returns>
        public IModel CreateChannel()
        {
            var channel = _connection.CreateModel();
            channel.ExchangeDeclare(_rabbitMqSettings.ExchangeName, ExchangeType.Direct, durable: true);
            return channel;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose() => _connection?.Dispose();

        /// <summary>
        /// The StartConnection.
        /// </summary>
        /// <param name="connectionFactory">The connectionFactory<see cref="IConnectionFactory"/>.</param>
        /// <returns>The <see cref="IConnection"/>.</returns>
        private IConnection StartConnection(IConnectionFactory connectionFactory)
        {
            try
            {
                return connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                throw new UnreachableRabbitMqServerException();
            }
        }
    }
}