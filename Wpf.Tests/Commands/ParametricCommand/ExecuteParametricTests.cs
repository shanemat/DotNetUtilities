using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.ParametricCommand;

/// <summary>
/// Contains tests for <see cref="Command{T}.Execute(T?)"/> method
/// </summary>
internal class ExecuteParametricTests
{
	#region Sources

	private static IEnumerable<int?> Values => Sources.Values;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowIfTheCommandCannotBeExecuted()
	{
		var command = new Command<int?>( _ => { }, _ => false );

		Assert.Throws<InvalidOperationException>( () => command.Execute( null ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldExecuteTheCommand( int? parameter )
	{
		int? result = int.MinValue;

		var command = new Command<int?>( v => result = v );

		command.Execute( parameter );

		Assert.That( result, Is.EqualTo( parameter ) );
	}

	#endregion
}
