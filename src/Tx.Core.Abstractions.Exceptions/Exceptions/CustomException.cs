namespace Tx.Core.Abstractions.Exceptions.Exceptions
{
    using System;

    public abstract class CustomException : Exception
    {
        public virtual string ExceptionType { get; } = "GeneralException";

        public virtual bool isPublic { get; } = false;

        protected CustomException() : base() { }

        protected CustomException(int code, string message) : base(message) => base.HResult = code;

        protected CustomException(int code, string message, Exception inner) : base(message, inner) => base.HResult = code;
    }

}
