using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.EnumerableExtensions;

/// <summary>
/// Contains tests for <see cref="EnumerableExtensions.CombineWith{T}(System.Collections.Generic.IEnumerable{T[]}?,System.Collections.Generic.IReadOnlyCollection{T[]}?)"/> method
/// </summary>
internal sealed class CombineComplexWithComplexTests
{
	#region Tests

	[Test]
	public void ShouldReturnNullWhenBothCollectionsAreNull()
	{
		int[][]? nullCollection = null;

		Assert.That( nullCollection.CombineWith( nullCollection ), Is.Null );
	}

	[Test]
	public void ShouldReturnTheContentOfOtherCollectionWhenOneIsNull()
	{
		int[][]? nullCollection = null;
		int[][] values = [[15, -2], [-39, 0]];

		Assert.Multiple( () =>
		{
			Assert.That( nullCollection.CombineWith( values ), Is.EquivalentTo( values ) );
			Assert.That( values.CombineWith( nullCollection ), Is.EquivalentTo( values ) );
		} );
	}

	[Test]
	public void ShouldReturnCorrectResultWhenBothCollectionsAreNotNull()
	{
		int[][] values = [[15, -2], [-39, 0]];
		int[][] otherValues = [[51, -468], [0, 7]];

		int[][] expectedResult =
		[
			[15, -2, 51, -468], [15, -2, 0, 7],
			[-39, 0, 51, -468], [-39, 0, 0, 7],
		];

		Assert.That( values.CombineWith( otherValues ), Is.EquivalentTo( expectedResult ) );
	}

	#endregion
}
