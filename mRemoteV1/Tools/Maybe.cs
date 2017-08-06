using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.Tools
{
	public class Maybe<T> : IEnumerable<T>
	{
		private readonly T[] _maybe;

		public Maybe()
		{
			_maybe = new T[0];
		}

		public Maybe(T value)
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

	    public static Maybe<TOut> FromNullable<TOut>(TOut? value) where TOut : struct
	    {
	        return value.HasValue
                ? new Maybe<TOut>(value.Value)
                : new Maybe<TOut>();
	    }
	}
}
