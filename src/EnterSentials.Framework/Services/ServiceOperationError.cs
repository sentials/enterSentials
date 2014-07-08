using System;

namespace EnterSentials.Framework
{
    public class ServiceOperationError
    {
        public int? Code { get; set; }
        public string Message { get; set; }

        public ServiceOperationError()
        { }

        public ServiceOperationError(Exception exception)
        {
            Guard.AgainstNull(exception, "exception");

            if (exception != null)
            {
                Message = exception.Message;
            }
        }
    }
}