
namespace mRemoteNG.Tools
{
	public static class Extensions
	{
		public static Maybe<T> Maybe<T>(this T value)
		{
			return new Maybe<T>(value);
		}
	}
}
