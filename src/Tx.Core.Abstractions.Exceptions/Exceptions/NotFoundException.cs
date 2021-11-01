namespace Tx.Core.Abstractions.Exceptions.Exceptions
{
    public class NotFoundException: CustomException
    {
        private const int ERROR_CODE = -400;

        public NotFoundException() : base(ERROR_CODE, "Not Fount") => HResult = ERROR_CODE;

        public NotFoundException(string message) : base(ERROR_CODE, message) => HResult = ERROR_CODE;

    }

}
