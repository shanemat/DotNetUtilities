using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Enum;

namespace Shanemat.DotNetUtils.Core.Tests.Enum.Enum;

/// <summary>
/// Contains tests for <see cref="Enum{T}.GetValuesExcept"/> class
/// </summary>
internal sealed class GetValuesExceptTests
{
	#region Sources

	private static IEnumerable<TestEnum> AllValues => [TestEnum.Unknown, TestEnum.A, TestEnum.B, TestEnum.C];

	private static IEnumerable<TestEnum[]> Exceptions => [[TestEnum.Unknown], [TestEnum.C, TestEnum.A], [TestEnum.Unknown, TestEnum.B], AllValues.ToArray()];

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnAllValuesForNoExceptions()
	{
		Assert.That( Enum<TestEnum>.GetValuesExcept(), Is.EquivalentTo( AllValues ) );
	}

	[Test]
	[TestCaseSource( nameof( Exceptions ) )]
	public void ShouldReturnValuesWithoutProvidedExceptions( TestEnum[] exceptions )
	{
		Assert.That( Enum<TestEnum>.GetValuesExcept( exceptions ), Is.EquivalentTo( AllValues.Where( v => !exceptions.Contains( v ) ) ) );
	}

	#endregion
}
