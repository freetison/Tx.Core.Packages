namespace TvMaze.RabbitMqProvider
{
    /// <summary>
    /// Defines the <see cref="IMessageSender" />.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// The PublishMessage.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="exchangeName">The exchangeName<see cref="string"/>.</param>
        /// <param name="routingKey">The routingKey<see cref="string"/>.</param>
        void PublishMessage<T>(T message, string exchangeName, string routingKey);
    }
}
