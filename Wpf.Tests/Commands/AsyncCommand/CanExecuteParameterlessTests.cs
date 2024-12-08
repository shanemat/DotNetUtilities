using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.AsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand.Execute(object?)"/> method
/// </summary>
internal sealed class CanExecuteParameterlessTests
{
	#region Tests

	[Test]
	[TestCase( true )]
	[TestCase( false )]
	public void ShouldUseTheGivenDelegate( bool canExecute )
	{
		var command = new Wpf.Commands.AsyncCommand( () => { }, () => canExecute );

		Assert.That( command.CanExecute(), Is.EqualTo( canExecute ) );
	}

	#endregion
}
