using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.Command;

/// <summary>
/// Contains tests for <see cref="Command.Execute()"/> method
/// </summary>
internal sealed class ExecuteParameterlessTests
{
	#region Tests

	[Test]
	public void ShouldExecuteTheCommand()
	{
		var flag = false;
		var command = new Wpf.Commands.Command( () => flag = true );

		command.Execute();

		Assert.That( flag, Is.True );
	}

	[Test]
	public void ShouldThrowIfTheCommandCannotBeExecuted()
	{
		var command = new Wpf.Commands.Command( () => { }, () => false );

		Assert.Throws<InvalidOperationException>( () => command.Execute() );
	}

	#endregion
}
