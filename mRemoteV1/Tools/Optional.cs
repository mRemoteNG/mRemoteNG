using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.Tools
{
	public class Optional<T> : IEnumerable<T>
	{
		private readonly T[] _maybe;

		public Optional()
		{
			_maybe = new T[0];
		}

		public Optional(T value)
		{
			_maybe = value != null 
				? new[] {value} 
				: new T[0];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)_maybe).GetEnumerator();
		}

	    public override string ToString()
	    {
	        return _maybe.Any() ? _maybe.First().ToString() : "";
	    }

	    public static implicit operator Optional<T>(T value)
	    {
	        return new Optional<T>(value);
	    }

        public static Optional<TOut> FromNullable<TOut>(TOut? value) where TOut : struct
	    {
	        return value.HasValue
                ? new Optional<TOut>(value.Value)
                : new Optional<TOut>();
	    }
	}
}
