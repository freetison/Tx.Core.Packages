namespace Tx.Core.Abstractions.Exceptions.Exceptions
{
    using System.Collections.Generic;

    public class ApiException : CustomException
    {
        public List<ValidationError> Errors { get; set; }

        public ApiException(int errorCode) : base(errorCode, "Api Exception") => HResult = errorCode;

        public ApiException(int errorCode, string message) : base(errorCode, message) => HResult = errorCode;

        public ApiException(int errorCode,string message, List<ValidationError> errors = null) : base(errorCode, message)
        {
            Errors = errors;
            HResult = errorCode;
        }
    }

}
