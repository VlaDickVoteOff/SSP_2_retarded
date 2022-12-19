using System;

namespace SSP_1.Exception
{
    public class DatabaseException : InvalidOperationException
    {
        public DatabaseException(string message) : base(message)
        {

        }
    }
}
