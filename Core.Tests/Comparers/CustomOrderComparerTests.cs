using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Comparers;

namespace Shanemat.DotNetUtils.Core.Tests.Comparers;

/// <summary>
/// Contains tests for <see cref="CustomOrderComparer{T}"/> class
/// </summary>
internal sealed class CustomOrderComparerTests
{
	#region Methods

	[Test]
	public void ShouldThrowWhenDuplicateValuesAreSpecified()
	{
		Assert.Throws<ArgumentException>( () => _ = CustomOrderComparer<int>.Create( [1, 2, 1] ) );
	}

	[Test]
	public void ShouldNotAffectOrderWhenNoValuesAreSpecified()
	{
		var comparer = CustomOrderComparer<int>.Create();
		int[] array = [9, 5, 8, 1, 3, 2, 6, 4, 0, 5, 1, 8, 3, 9];

		Assert.That( array.Order( comparer ), Is.EqualTo( array ) );
	}

	[Test]
	public void ShouldNotAffectOrderWhenOtherValuesAreSpecified()
	{
		var comparer = CustomOrderComparer<int>.Create( [10, 12, 15, 17, 20] );
		int[] array = [9, 5, 8, 1, 3, 2, 6, 4, 0, 5, 1, 8, 3, 9];

		Assert.That( array.Order( comparer ), Is.EqualTo( array ) );
	}

	[Test]
	public void ShouldPutOrderedSpecifiedValuesFirst()
	{
		var comparer = CustomOrderComparer<int>.Create( [5, 6, 8, 9] );
		int[] array = [9, 5, 8, 1, 3, 2, 6, 4, 0, 5, 1, 8, 3, 9];

		Assert.That( array.Order( comparer ).Take( 7 ), Is.EqualTo( (int[]) [5, 5, 6, 8, 8, 9, 9] ) );
	}

	[Test]
	public void ShouldPutUnSpecifiedValuesSecond()
	{
		var comparer = CustomOrderComparer<int>.Create( [5, 6, 8, 9] );
		int[] array = [9, 5, 8, 1, 3, 2, 6, 4, 0, 5, 1, 8, 3, 9];

		Assert.That( array.Order( comparer ).Skip( 7 ), Is.EqualTo( (int[]) [1, 3, 2, 4, 0, 1, 3] ) );
	}

	#endregion
}
