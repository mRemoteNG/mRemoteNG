using System;

namespace mRemoteNG.Tools
{
	/// <summary>
	/// Represents an action that will be executed when the <see cref="Dispose"/>
	/// method is called. Useful for creating Using blocks around logical start/end
	/// actions.
	/// </summary>
	public class DisposableAction : IDisposable
	{
		private bool _isDisposed;
		private readonly Action _disposeAction;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="initializeAction">
		///	An <see cref="Action"/> that should be performed immediately
		/// when this object is initialized. It should return quickly.
		/// </param>
		/// <param name="disposeAction">
		/// An <see cref="Action"/> to be executed when this object is disposed.
		/// </param>
		public DisposableAction(Action initializeAction, Action disposeAction)
		{
			initializeAction();
			_disposeAction = disposeAction;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing || _isDisposed)
				return;

			_isDisposed = true;
			_disposeAction();
		}
	}
}
