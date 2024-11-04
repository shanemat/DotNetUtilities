using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.EqualityComparer"/> class
/// </summary>
internal sealed class EqualityComparerTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.EqualityComparer.Create( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.EqualityComparer.Create( -1.0 ) );
	}

	[Test]
	public void ShouldBeReflexive()
	{
		var comparer = Core.Math.IntervalSet.EqualityComparer.Create();

		var openInterval = Core.Math.Interval.Open( -4, -3 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -2, -1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( 1, 2 );
		var closedInterval = Core.Math.Interval.Closed( 3, 4 );

		var set = Core.Math.IntervalSet.Create( [openInterval, openClosedInterval, closedOpenInterval, closedInterval] );

		Assert.That( comparer.Equals( set, set ), Is.True );
	}

	[Test]
	public void ShouldReturnTrueWhenBothIntervalSetsAreNull()
	{
		var comparer = Core.Math.IntervalSet.EqualityComparer.Create();

		Assert.That( comparer.Equals( null, null ), Is.True );
	}

	[Test]
	public void ShouldReturnFalseWhenOneOfTheIntervalSetsIsNull()
	{
		var comparer = Core.Math.IntervalSet.EqualityComparer.Create();

		var set = Core.Math.IntervalSet.Create();

		Assert.Multiple( () =>
		{
			Assert.That( comparer.Equals( set, null ), Is.False );
			Assert.That( comparer.Equals( null, set ), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalSetsDiffer()
	{
		var comparer = Core.Math.IntervalSet.EqualityComparer.Create();

		var oneSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.OpenClosed( -3, 1 )] );
		var otherSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.ClosedOpen( -1, 3 )] );

		Assert.That( comparer.Equals( oneSet, otherSet ), Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenTheIntervalsAreEqual()
	{
		var comparer = Core.Math.IntervalSet.EqualityComparer.Create();

		var firstInterval = Core.Math.Interval.OpenClosed( -3, -1 );
		var secondInterval = Core.Math.Interval.Closed( -1, 1 );
		var thirdInterval = Core.Math.Interval.ClosedOpen( 1, 3 );

		var splitSet = Core.Math.IntervalSet.Create( [firstInterval, secondInterval, thirdInterval] );
		var joinedSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -3, 3 )] );

		Assert.That( comparer.Equals( splitSet, joinedSet ), Is.True );
	}

	[Test]
	public void ShouldWorkWithInfiniteIntervals()
	{
		var comparer = Core.Math.IntervalSet.EqualityComparer.Create();

		var negativeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.OpenClosed( double.NegativeInfinity, 1 )] );
		var positiveSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.ClosedOpen( -1, double.PositiveInfinity )] );
		var wholeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.Multiple( () =>
		{
			Assert.That( comparer.Equals( negativeSet, negativeSet ), Is.True );
			Assert.That( comparer.Equals( positiveSet, positiveSet ), Is.True );
			Assert.That( comparer.Equals( wholeSet, wholeSet ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double maximum = 10.0;

		var comparer = Core.Math.IntervalSet.EqualityComparer.Create( tolerance );

		var narrowerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( minimum, maximum )] );
		var widerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( minimum - tolerance * 0.5, maximum + tolerance * 0.5 )] );
		var tooWideSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( minimum - tolerance * 1.5, maximum + tolerance * 1.5 )] );

		Assert.Multiple( () =>
		{
			Assert.That( comparer.Equals( narrowerSet, widerSet ), Is.True );
			Assert.That( comparer.Equals( narrowerSet, tooWideSet ), Is.False );
		} );
	}

	#endregion
}
