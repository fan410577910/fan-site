namespace FAN.RabbitMQ.Sprache
{
    sealed class Success<T> : ISuccess<T>
    {
        private readonly Input _remainder;
        private readonly T _result;

        public Success(T result, Input remainder)
        {
            this._result = result;
            this._remainder = remainder;
        }

        public T Result { get { return this._result; } }

        public Input Remainder { get { return this._remainder; } }

        public override string ToString()
        {
            return string.Format("Successful parsing of {0}.", this._result);
        }
    }
}