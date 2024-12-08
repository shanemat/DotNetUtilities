using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.Command;

/// <summary>
/// Contains tests for <see cref="Command.CanExecute(object?)"/> method
/// </summary>
internal sealed class CanExecuteParametricTests
{
	#region Tests

	[Test]
	[TestCase( true )]
	[TestCase( false )]
	public void ShouldUseTheGivenDelegate( bool canExecute )
	{
		var command = new Wpf.Commands.Command( () => { }, () => canExecute );

		Assert.That( command.CanExecute( new object() ), Is.EqualTo( canExecute ) );
	}

	#endregion
}
