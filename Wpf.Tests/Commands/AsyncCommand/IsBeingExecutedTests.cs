using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.AsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand.IsBeingExecuted"/> property
/// </summary>
internal sealed class IsBeingExecutedTests
{
	#region Tests

	[Test]
	public void ShouldReturnFalseBeforeTheCommandIsExecuted()
	{
		var command = new Wpf.Commands.AsyncCommand( () => { } );

		Assert.That( command.IsBeingExecuted, Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhileTheCommandIsBeingExecuted()
	{
		var taskCompletionSource = new TaskCompletionSource();

		var command = new Wpf.Commands.AsyncCommand( Wait );

		command.Execute();

		Assert.That( command.IsBeingExecuted, Is.True );

		taskCompletionSource.SetResult();

		async Task Wait() => await taskCompletionSource.Task;
	}

	[Test]
	public async Task ShouldReturnFalseAfterTheCommandExecutionFinished()
	{
		var command = new Wpf.Commands.AsyncCommand( () => { } );

		await command.ExecuteAsync();

		Assert.That( command.IsBeingExecuted, Is.False );
	}

	#endregion
}
