namespace Tx.Core.RabbitMqProvider
{
    public class RabbitMqSettings
    {
        /// <summary>
        /// Gets or sets the RabbitMqHostName.
        /// </summary>
        public string RabbitMqHostName { get; set; } = "localhost";

        /// <summary>
        /// Gets or sets the RabbitMqPort.
        /// </summary>
        public int RabbitMqPort { get; set; } = 5672;

        /// <summary>
        /// Gets or sets the RabbitMqConcurrency.
        /// </summary>
        public ushort RabbitMqConcurrency { get; set; } = 50;

        /// <summary>
        /// Gets or sets the RabbitMqUserName.
        /// </summary>
        public string RabbitMqUserName { get; set; } = "guest";

        /// <summary>
        /// Gets or sets the RabbitMqPassword.
        /// </summary>
        public string RabbitMqPassword { get; set; } = "guest";

        /// <summary>
        /// Gets or sets the Vhost.
        /// </summary>
        public string Vhost { get; set; } = "/";

        /// <summary>
        /// Gets or sets the ExchangeName.
        /// </summary>
        public string ExchangeName { get; set; } = "amq.direct";

        /// <summary>
        /// Gets or sets the RabbitMqQueueName.
        /// </summary>
        public string RabbitMqQueueName { get; set; }
    }
}