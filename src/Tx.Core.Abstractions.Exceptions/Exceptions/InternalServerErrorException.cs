namespace Tx.Core.Abstractions.Exceptions.Exceptions
{
    public class InternalServerErrorException : CustomException
    {
        private const int ERROR_CODE = -500;

        public InternalServerErrorException() : base(ERROR_CODE, "Internal Server Error") => HResult = ERROR_CODE;

        public InternalServerErrorException(string message) : base(ERROR_CODE, message) => HResult = ERROR_CODE;

    }

}
