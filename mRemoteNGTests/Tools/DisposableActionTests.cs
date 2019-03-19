using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
	public class DisposableActionTests
	{
		[Test]
		public void InitializerActionRunsWhenObjectIsCreated()
		{
			var initializerRan = false;
			new DisposableAction(() => initializerRan = true, () => { });

			Assert.That(initializerRan);
		}

		[Test]
		public void DisposalActionRunsWhenDisposeIsCalled()
		{
			var disposeActionRan = false;
			var action = new DisposableAction(() => {}, () => disposeActionRan = true);

			Assert.That(disposeActionRan, Is.False);
			action.Dispose();
			Assert.That(disposeActionRan, Is.True);
		}

		[Test]
		public void DisposeActionOnlyExecutedOnceWhenCallingDisposeMultipleTimes()
		{
			var invokeCount = 0;
			var action = new DisposableAction(() => { }, () => invokeCount++);

			action.Dispose();
			action.Dispose();
			action.Dispose();
			action.Dispose();
			action.Dispose();
			Assert.That(invokeCount, Is.EqualTo(1));
		}
	}
}
