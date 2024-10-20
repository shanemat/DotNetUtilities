using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.EnumerableExtensions;

/// <summary>
/// Contains tests for <see cref="EnumerableExtensions.CombineWith{T}(System.Collections.Generic.IEnumerable{T}?,System.Collections.Generic.IReadOnlyCollection{T[]}?)"/> method
/// </summary>
internal sealed class CombineSimpleWithComplexTests
{
	#region Tests

	[Test]
	public void ShouldReturnNullWhenBothCollectionsAreNull()
	{
		int[]? simpleNullCollection = null;
		int[][]? complexNullCollection = null;

		Assert.That( simpleNullCollection.CombineWith( complexNullCollection ), Is.Null );
	}

	[Test]
	public void ShouldReturnTheContentOfOtherCollectionWhenOneIsNull()
	{
		int[]? simpleNullCollection = null;
		int[][]? complexNullCollection = null;

		int[] simpleValues = [-75, 0, 128];
		int[][] expectedSimpleResult = [[-75], [0], [128]];

		int[][] complexValues = [[15, -2], [-39, 0]];
		int[][] expectedComplexResult = [[15, -2], [-39, 0]];

		Assert.Multiple( () =>
		{
			Assert.That( simpleNullCollection.CombineWith( complexValues ), Is.EquivalentTo( expectedComplexResult ) );
			Assert.That( simpleValues.CombineWith( complexNullCollection ), Is.EquivalentTo( expectedSimpleResult ) );
		} );
	}

	[Test]
	public void ShouldReturnCorrectResultWhenBothCollectionsAreNotNull()
	{
		int[] simpleValues = [-75, 0, 128];
		int[][] complexValues = [[15, -2], [-39, 0]];

		int[][] expectedResult =
		[
			[-75, 15, -2], [0, 15, -2], [128, 15, -2],
			[-75, -39, 0], [0, -39, 0], [128, -39, 0],
		];

		Assert.That( simpleValues.CombineWith( complexValues ), Is.EquivalentTo( expectedResult ) );
	}

	#endregion
}
