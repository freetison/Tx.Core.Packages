namespace Tx.Core.RabbitMqProvider.Exceptions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="UnreachableRabbitMqServerException" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnreachableRabbitMqServerException : CustomException
    {
        /// <summary>
        /// Defines the ERRORCODE.
        /// </summary>
        private const int ERRORCODE = (int)HttpStatusCode.InternalServerError;

        /// <summary>
        /// Defines the MESSAGE.
        /// </summary>
        private const string MESSAGE = "Failed to connect to RabbitMQ server";

        /// <summary>
        /// Gets or sets the ErrorCode.
        /// </summary>
        public override int ErrorCode { get; set; } = ERRORCODE;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnreachableRabbitMqServerException"/> class.
        /// </summary>
        public UnreachableRabbitMqServerException()
        : base(ERRORCODE, MESSAGE) => HResult = ERRORCODE;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnreachableRabbitMqServerException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public UnreachableRabbitMqServerException(string message)
        : base(ERRORCODE, message) => HResult = ERRORCODE;
    }
}