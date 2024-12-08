using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.ParametricAsyncCommand;

/// <summary>
/// Contains tests for <see cref="AsyncCommand{T}.CanExecute(object?)"/> method
/// </summary>
internal sealed class CanExecuteGenericTests
{
	#region Sources

	private static IEnumerable<int?> Values => Sources.Values;

	#endregion

	#region Tests

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldUseTheGivenDelegate( object? parameter )
	{
		var command = new AsyncCommand<int?>( _ => { }, CanExecute );

		Assert.That( command.CanExecute( parameter ), Is.EqualTo( CanExecute( parameter as int? ) ) );

		static bool CanExecute( int? value ) => value is not null;
	}

	#endregion
}
