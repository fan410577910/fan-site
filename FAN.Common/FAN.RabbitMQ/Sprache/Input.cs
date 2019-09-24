using System;
using System.Collections.Generic;

namespace FAN.RabbitMQ.Sprache
{
    public class Input
    {
        public string Source { get; set; }
        readonly string _source;
        readonly int _position;
        private readonly int _line;
        private readonly int _column;

        internal IDictionary<object, object> Memos = new Dictionary<object, object>();

        public Input(string source)
            : this(source, 0)
        {
        }

        internal Input(string source, int position, int line = 1, int column = 1)
        {
            this.Source = source;

            this._source = source;
            this._position = position;
            this._line = line;
            this._column = column;
        }

        public Input Advance()
        {
            if (this.AtEnd)
                throw new InvalidOperationException("The input is already at the end of the source.");

            return new Input(this._source, this._position + 1, this.Current == '\n' ? this._line + 1 : this._line, Current == '\n' ? 1 : this._column + 1);
        }

        public char Current { get { return this._source[this._position]; } }

        public bool AtEnd { get { return this._position == this._source.Length; } }

        public int Position { get { return this._position; } }

        public int Line { get { return this._line; } }

        public int Column { get { return this._column; } }

        public override string ToString()
        {
            return string.Format("Line {0}, Column {1}", this._line, this._column);
        }

        public override bool Equals(object obj)
        {
            var i = obj as Input;
            return i != null && i._source == this._source && i._position == this._position;
        }

        public override int GetHashCode()
        {
            return this._source.GetHashCode() ^ this._position.GetHashCode();
        }
    }
}
