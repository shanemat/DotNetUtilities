using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.ParametricAsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand{T}.ExecuteAsync"/> method
/// </summary>
internal sealed class ExecuteAsyncTests
{
	#region Sources

	private static IEnumerable<int?> Values => Sources.Values;

	#endregion

	#region Tests

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public async Task ShouldExecuteTheCommand( int? parameter )
	{
		int? result = int.MinValue;

		var command = new AsyncCommand<int?>( v => result = v );

		await command.ExecuteAsync( parameter );

		Assert.That( result, Is.EqualTo( parameter ) );
	}

	[Test]
	public void ShouldThrowIfTheCommandCannotBeExecuted()
	{
		var command = new AsyncCommand<int?>( _ => { }, _ => false );

		Assert.ThrowsAsync<InvalidOperationException>( async () => await command.ExecuteAsync( null ) );
	}

	#endregion
}
