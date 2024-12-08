using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.ParametricAsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand{T}.IsBeingExecuted"/> property
/// </summary>
internal sealed class IsBeingExecutedTests
{
	#region Tests

	[Test]
	public void ShouldReturnFalseBeforeTheCommandIsExecuted()
	{
		var command = new AsyncCommand<int?>( _ => { } );

		Assert.That( command.IsBeingExecuted, Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhileTheCommandIsBeingExecuted()
	{
		var taskCompletionSource = new TaskCompletionSource();

		var command = new AsyncCommand<int?>( Wait );

		command.Execute( null );

		Assert.That( command.IsBeingExecuted, Is.True );

		taskCompletionSource.SetResult();

		async Task Wait( int? value ) => await taskCompletionSource.Task;
	}

	[Test]
	public async Task ShouldReturnFalseAfterTheCommandExecutionFinished()
	{
		var command = new AsyncCommand<int?>( _ => { } );

		await command.ExecuteAsync( null );

		Assert.That( command.IsBeingExecuted, Is.False );
	}

	#endregion
}
