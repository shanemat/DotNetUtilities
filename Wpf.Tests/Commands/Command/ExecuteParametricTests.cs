using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.Command;

/// <summary>
/// Contains tests for <see cref="Command.Execute(object?)"/> method
/// </summary>
internal sealed class ExecuteParametricTests
{
	#region Tests

	[Test]
	public void ShouldExecuteTheCommandRegardlessOfParameter()
	{
		var flag = false;
		var command = new Wpf.Commands.Command( () => flag = true );

		command.Execute( new object() );

		Assert.That( flag, Is.True );
	}

	[Test]
	public void ShouldThrowIfTheCommandCannotBeExecuted()
	{
		var command = new Wpf.Commands.Command( () => { }, () => false );

		Assert.Throws<InvalidOperationException>( () => command.Execute( new object() ) );
	}

	#endregion
}
