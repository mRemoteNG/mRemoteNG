
using System;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.Tools
{
    public static class Extensions
	{
		public static Optional<T> Maybe<T>(this T value)
		{
			return new Optional<T>(value);
		}

	    public static Optional<U> MaybeParse<T, U>(this T value, Func<T, U> parseFunc)
	    {
	        try
	        {
	            return new Optional<U>(parseFunc(value));
	        }
	        catch
	        {
	            return new Optional<U>();
	        }
	    }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given value is
        /// null. Otherwise, return the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="argName">
        /// The name of the argument
        /// </param>
	    public static T ThrowIfNull<T>(this T value, string argName)
	    {
            if (value == null)
                throw new ArgumentNullException(argName);
	        return value;
	    }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the value
        /// is null or an empty string. Otherwise, returns the value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argName"></param>
	    public static string ThrowIfNullOrEmpty(this string value, string argName)
	    {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty", argName);
	        return value;
	    }

        /// <summary>
        /// Perform an action for each item in the given collection. The item
        /// is the pass along the processing chain.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
	    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection = collection.ToList();

            foreach (var item in collection)
	            action(item);

	        return collection;
	    }
	}
}
