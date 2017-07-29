
using System;

namespace mRemoteNG.Tools
{
	public static class Extensions
	{
		public static Maybe<T> Maybe<T>(this T value)
		{
			return new Maybe<T>(value);
		}

	    public static Maybe<U> MaybeParse<T, U>(this T value, Func<T, U> parseFunc)
	    {
	        try
	        {
	            return new Maybe<U>(parseFunc(value));
	        }
	        catch
	        {
	            return new Maybe<U>();
	        }
	    }
	}
}
