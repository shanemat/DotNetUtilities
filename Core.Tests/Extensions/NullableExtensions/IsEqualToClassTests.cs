using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.NullableExtensions;

/// <summary>
/// Contains tests for <see cref="NullableExtensions.IsEqualTo{T}(T,T,System.Func{T,T,bool})"/> method
/// </summary>
internal sealed class IsEqualToClassTests
{
	#region Nested Types

	internal record Record( int Value );

	#endregion

	#region Sources

	private static IReadOnlyCollection<int> Values { get; } = [-75, 0, 128];

	private static IReadOnlyCollection<Func<Record, Record, bool>> Comparers { get; } =
	[
		AreEqual,
		AreNotEqual,
		HasEitherZeroValue,
	];

	private static IEnumerable<object[]> ValuesAndComparers => Values.Cast<object>().CombineWith( Comparers );

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnTrueWhenBothValuesAreNull()
	{
		Record? nullRecord = null;

		Assert.That( nullRecord.IsEqualTo( nullRecord, AreEqual ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseWhenOnlyOneValueIsNull( int value )
	{
		var record = new Record( value );
		Record? nullRecord = null;

		Assert.Multiple( () =>
		{
			Assert.That( record.IsEqualTo( nullRecord, AreEqual ), Is.False );
			Assert.That( nullRecord.IsEqualTo( record, AreEqual ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnCorrectValueWhenBothValuesAreNotNull( int value )
	{
		var record = new Record( value );

		Assert.That( record.IsEqualTo( record, AreEqual ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndComparers ) )]
	public void ShouldUseTheProvidedComparer( int value, Func<Record, Record, bool> comparer )
	{
		var record = new Record( value );

		Assert.That( record.IsEqualTo( record, comparer ), Is.EqualTo( comparer.Invoke( record, record ) ) );
	}

	#endregion

	#region Methods

	private static bool AreEqual( Record one, Record other ) => one.Value == other.Value;

	private static bool AreNotEqual( Record one, Record other ) => one.Value != other.Value;

	private static bool HasEitherZeroValue( Record one, Record other ) => one.Value == 0 || other.Value == 0;

	#endregion
}
