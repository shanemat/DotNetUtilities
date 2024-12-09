using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Enum;

namespace Shanemat.DotNetUtils.Core.Tests.Enum.Enum;

/// <summary>
/// Contains tests for <see cref="Enum{T}.GetValues(System.Func{T, bool})"/> class
/// </summary>
internal sealed class GetValuesByFilterTests
{
	#region Sources

	private static IEnumerable<TestEnum> AllValues => [TestEnum.Unknown, TestEnum.A, TestEnum.B, TestEnum.C];

	private static IEnumerable<Func<TestEnum, bool>> Filters => [_ => true, e => e is not TestEnum.B, _ => false];

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnAllValuesForNullFilter()
	{
		Assert.That( Enum<TestEnum>.GetValues( null ), Is.EquivalentTo( AllValues ) );
	}

	[Test]
	[TestCaseSource( nameof( Filters ) )]
	public void ShouldReturnValuesMatchingTheFilter( Func<TestEnum, bool> filter )
	{
		Assert.That( Enum<TestEnum>.GetValues( filter ), Is.EquivalentTo( AllValues.Where( filter ) ) );
	}

	#endregion
}
