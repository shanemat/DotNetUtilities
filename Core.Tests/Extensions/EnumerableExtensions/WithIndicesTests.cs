using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.EnumerableExtensions;

/// <summary>
/// Contains tests for <see cref="EnumerableExtensions.WithIndices{T}"/> method
/// </summary>
internal sealed class WithIndicesTests
{
	#region Tests

	[Test]
	public void ShouldHandleNullCollection()
	{
		int[]? nullCollection = null;

		Assert.That( nullCollection.WithIndices(), Is.Null );
	}

	[Test]
	public void ShouldReturnCorrectNumberOfElements()
	{
		int[] values = [1, 5, 3, 4, 9, 10, 15, 21];

		Assert.That( values, Has.Length.EqualTo( values.WithIndices().Count() ) );
	}

	[Test]
	public void ShouldProvideCorrectIndicesForStructs()
	{
		List<int> values = [1, 5, 3, 4, 9, 10, 15, 21];

		Assert.Multiple( () =>
		{
			foreach( var (index, element) in values.WithIndices() )
			{
				Assert.That( index, Is.EqualTo( values.IndexOf( element ) ) );
			}
		} );
	}

	[Test]
	public void ShouldProvideCorrectIndicesForClasses()
	{
		List<string> values = ["test", "FOO", string.Empty, "$#@!"];

		Assert.Multiple( () =>
		{
			foreach( var (index, element) in values.WithIndices() )
			{
				Assert.That( index, Is.EqualTo( values.IndexOf( element ) ) );
			}
		} );
	}

	#endregion
}
