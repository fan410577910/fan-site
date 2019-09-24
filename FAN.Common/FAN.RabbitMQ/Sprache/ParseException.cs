using System;

namespace FAN.RabbitMQ.Sprache
{
    public class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }
    }
}
