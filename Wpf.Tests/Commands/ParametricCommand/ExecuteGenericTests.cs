using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.ParametricCommand;

/// <summary>
/// Contains tests for <see cref="Command{T}.Execute(object?)"/> method
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
		var command = new Command<int?>( _ => { }, _ => false );

		Assert.Throws<InvalidOperationException>( () => command.Execute( parameter ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldExecuteTheCommand( object? parameter )
	{
		int? result = int.MinValue;

		var command = new Command<int?>( v => result = v );

		command.Execute( parameter );

		Assert.That( result, Is.EqualTo( parameter ) );
	}

	[Test]
	public void ShouldExecuteTheCommandWithDefaultParameterWhenItsTypeDoesNotMatch()
	{
		int? result = int.MinValue;

		var command = new Command<int?>( v => result = v );

		command.Execute( "Foo" );

		Assert.That( result, Is.Null );
	}

	#endregion
}
