using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.ParametricAsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand{T}.Execute(object?)"/> method
/// </summary>
internal class ExecuteGenericTests
{
	#region Sources

	private static IEnumerable<int?> Values => Sources.Values;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowIfTheCommandCannotBeExecuted()
	{
		object? parameter = null;
		var command = new AsyncCommand<int?>( _ => { }, _ => false );

		Assert.Throws<InvalidOperationException>( () => command.Execute( parameter ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public async Task ShouldExecuteTheCommand( object? parameter )
	{
		int? result = int.MinValue;
		var taskCompletionSource = new TaskCompletionSource();

		var command = new AsyncCommand<int?>( SetResult );

		command.Execute( parameter );

		await taskCompletionSource.Task;

		Assert.That( result, Is.EqualTo( parameter ) );

		void SetResult( int? value )
		{
			result = value;

			taskCompletionSource.SetResult();
		}
	}

	[Test]
	public async Task ShouldExecuteTheCommandWithDefaultParameterWhenItsTypeDoesNotMatch()
	{
		int? result = int.MinValue;
		var taskCompletionSource = new TaskCompletionSource();

		var command = new AsyncCommand<int?>( SetResult );

		command.Execute( "Foo" );

		await taskCompletionSource.Task;

		Assert.That( result, Is.Null );

		void SetResult( int? value )
		{
			result = value;

			taskCompletionSource.SetResult();
		}
	}

	[Test]
	public async Task ShouldUseProvidedExceptionHandler()
	{
		var flag = false;
		var taskCompletionSource = new TaskCompletionSource();

		var command = new AsyncCommand<int?>( _ => throw new Exception(), handleException: SetFlag );

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
