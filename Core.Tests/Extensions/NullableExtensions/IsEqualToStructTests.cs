using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.NullableExtensions;

/// <summary>
/// Contains tests for <see cref="NullableExtensions.IsEqualTo{T}(System.Nullable{T},System.Nullable{T},System.Func{T,T,bool})"/> method
/// </summary>
internal sealed class IsEqualToStructTests
{
	#region Sources

	private static IReadOnlyCollection<int> Values { get; } = [-75, 0, 128];

	private static IReadOnlyCollection<Func<int, int, bool>> Comparers { get; } =
	[
		AreEqual,
		AreNotEqual,
		IsEitherValueZero,
	];

	private static IEnumerable<object[]> ValuesAndComparers => Values.Cast<object>().CombineWith( Comparers );

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnTrueWhenBothValuesAreNull()
	{
		int? nullValue = null;

		Assert.That( nullValue.IsEqualTo( nullValue, AreEqual ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseWhenOnlyOneValueIsNull( int value )
	{
		int? nullValue = null;

		Assert.Multiple( () =>
		{
			Assert.That( value.IsEqualTo( nullValue, AreEqual ), Is.False );
			Assert.That( nullValue.IsEqualTo( value, AreEqual ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnCorrectValueWhenBothValuesAreNotNull( int value )
	{
		Assert.That( value.IsEqualTo( value, AreEqual ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndComparers ) )]
	public void ShouldUseTheProvidedComparer( int value, Func<int, int, bool> comparer )
	{
		Assert.That( value.IsEqualTo( value, comparer ), Is.EqualTo( comparer.Invoke( value, value ) ) );
	}

	#endregion

	#region Methods

	private static bool AreEqual( int one, int other ) => one == other;

	private static bool AreNotEqual( int one, int other ) => one != other;

	private static bool IsEitherValueZero( int one, int other ) => one == 0 || other == 0;

	#endregion
}
