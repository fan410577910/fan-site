using System.Collections.Generic;

namespace FAN.RabbitMQ.Sprache
{
    public interface IFailure<out T> : IResult<T>
    {
        string Message { get; }
        IEnumerable<string> Expectations { get; }
        Input FailedInput { get; }
    }
}