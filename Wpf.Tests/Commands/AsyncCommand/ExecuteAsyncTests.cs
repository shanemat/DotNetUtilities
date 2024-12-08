using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.AsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand.ExecuteAsync"/> method
/// </summary>
internal sealed class ExecuteAsyncTests
{
	#region Tests

	[Test]
	public async Task ShouldExecuteTheCommand()
	{
		var flag = false;
		var command = new Wpf.Commands.AsyncCommand( () => flag = true );

		await command.ExecuteAsync();

		Assert.That( flag, Is.True );
	}

	[Test]
	public void ShouldThrowIfTheCommandCannotBeExecuted()
	{
		var command = new Wpf.Commands.AsyncCommand( () => { }, () => false );

		Assert.ThrowsAsync<InvalidOperationException>( async () => await command.ExecuteAsync() );
	}

	#endregion
}
