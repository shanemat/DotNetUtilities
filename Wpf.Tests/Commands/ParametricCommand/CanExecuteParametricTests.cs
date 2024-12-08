using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.ParametricCommand;

/// <summary>
/// Contains tests for <see cref="Command{T}.CanExecute(T?)"/> method
/// </summary>
internal sealed class CanExecuteParametricTests
{
	#region Sources

	private static IEnumerable<int?> Values => Sources.Values;

	#endregion

	#region Tests

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldUseTheGivenDelegate( int? parameter )
	{
		var command = new Command<int?>( _ => { }, CanExecute );

		Assert.That( command.CanExecute( parameter ), Is.EqualTo( CanExecute( parameter ) ) );

		static bool CanExecute( int? value ) => value is not null;
	}

	#endregion
}
