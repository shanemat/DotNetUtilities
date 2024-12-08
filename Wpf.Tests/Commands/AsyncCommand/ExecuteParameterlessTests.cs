using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.AsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand.Execute()"/> method
/// </summary>
internal sealed class ExecuteParameterlessTests
{
	#region Tests

	[Test]
	public async Task ShouldExecuteTheCommand()
	{
		var flag = false;
		var taskCompletionSource = new TaskCompletionSource();

		var command = new Wpf.Commands.AsyncCommand( SetFlag );

		// ReSharper disable once MethodHasAsyncOverload
		command.Execute();

		await taskCompletionSource.Task;

		Assert.That( flag, Is.True );

		void SetFlag()
		{
			flag = true;

			taskCompletionSource.SetResult();
		}
	}

	[Test]
	public void ShouldThrowIfTheCommandCannotBeExecuted()
	{
		var command = new Wpf.Commands.AsyncCommand( () => { }, () => false );

		Assert.Throws<InvalidOperationException>( () => command.Execute() );
	}

	[Test]
	public async Task ShouldUseProvidedExceptionHandler()
	{
		var flag = false;
		var taskCompletionSource = new TaskCompletionSource();

		var command = new Wpf.Commands.AsyncCommand( () => throw new Exception(), handleException: SetFlag );

		// ReSharper disable once MethodHasAsyncOverload
		command.Execute();

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
