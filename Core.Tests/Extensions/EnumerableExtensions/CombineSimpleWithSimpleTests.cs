using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.EnumerableExtensions;

/// <summary>
/// Contains tests for <see cref="EnumerableExtensions.CombineWith{T}(System.Collections.Generic.IEnumerable{T}?,System.Collections.Generic.IReadOnlyCollection{T}?)"/> method
/// </summary>
internal sealed class CombineSimpleWithSimpleTests
{
	#region Tests

	[Test]
	public void ShouldReturnNullWhenBothCollectionsAreNull()
	{
		int[]? nullCollection = null;

		Assert.That( nullCollection.CombineWith( nullCollection ), Is.Null );
	}

	[Test]
	public void ShouldReturnTheContentOfOtherCollectionWhenOneIsNull()
	{
		int[]? nullCollection = null;
		int[] values = [-75, 0, 128];

		int[][] expectedResult = [[-75], [0], [128]];

		Assert.Multiple( () =>
		{
			Assert.That( nullCollection.CombineWith( values ), Is.EquivalentTo( expectedResult ) );
			Assert.That( values.CombineWith( nullCollection ), Is.EquivalentTo( expectedResult ) );
		} );
	}

	[Test]
	public void ShouldReturnCorrectResultWhenBothCollectionsAreNotNull()
	{
		int[] values = [-75, 0, 128];
		int[] otherValues = [51, -468];

		int[][] expectedResult =
		[
			[-75, 51], [-75, -468],
			[0, 51], [0, -468],
			[128, 51], [128, -468],
		];

		Assert.That( values.CombineWith( otherValues ), Is.EquivalentTo( expectedResult ) );
	}

	#endregion
}
