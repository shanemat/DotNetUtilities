using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.TaskExtensions;

/// <summary>
/// Contains tests for <see cref="Core.Extensions.TaskExtensions.FireAndForget"/> method
/// </summary>
internal sealed class FireAndForgetTests
{
	#region Tests

	[Test]
	public void ShouldHandleNullTask()
	{
		Task? task = null;

		Assert.DoesNotThrow( () => task.FireAndForget() );
	}

	[Test]
	public void ShouldIgnoreInnerExceptions()
	{
		Assert.DoesNotThrow( () => Task.Run( () => throw new Exception() ).FireAndForget() );
	}

	[Test]
	public async Task ShouldUseExceptionHandler()
	{
		var flag = false;
		var taskCompletionSource = new TaskCompletionSource();

		Assert.DoesNotThrow( () => Task.Run( () => throw new Exception() ).FireAndForget( onExceptionCaught: SetFlag ) );

		await taskCompletionSource.Task;

		Assert.That( flag, Is.True );

		void SetFlag( Exception e )
		{
			flag = true;

			taskCompletionSource.SetResult();
		}
	}

	#endregion
}
