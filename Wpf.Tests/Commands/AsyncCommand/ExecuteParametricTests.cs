using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.AsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand.Execute(object?)"/> method
/// </summary>
internal sealed class ExecuteParametricTests
{
	#region Tests

	[Test]
	public async Task ShouldExecuteTheCommandRegardlessOfParameter()
	{
		var flag = false;
		var taskCompletionSource = new TaskCompletionSource();

		var command = new Wpf.Commands.AsyncCommand( SetFlag );

		command.Execute( new object() );

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

		Assert.Throws<InvalidOperationException>( () => command.Execute( new object() ) );
	}

	[Test]
	public async Task ShouldUseProvidedExceptionHandler()
	{
		var flag = false;
		var taskCompletionSource = new TaskCompletionSource();

		var command = new Wpf.Commands.AsyncCommand( () => throw new Exception(), handleException: SetFlag );

		command.Execute( new object() );

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
