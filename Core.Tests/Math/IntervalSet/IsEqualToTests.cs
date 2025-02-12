﻿using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.IsEqualTo"/> method
/// </summary>
internal sealed class IsEqualToTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Interval.Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => set.IsEqualTo( set, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => set.IsEqualTo( set, -1.0 ) );
	}

	[Test]
	public void ShouldBeReflexive()
	{
		var openInterval = Core.Math.Interval.Open( -4, -3 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -2, -1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( 1, 2 );
		var closedInterval = Core.Math.Interval.Closed( 3, 4 );

		var set = Core.Math.IntervalSet.Create( [openInterval, openClosedInterval, closedOpenInterval, closedInterval] );

		Assert.That( set.IsEqualTo( set ), Is.True );
	}

	[Test]
	public void ShouldReturnFalseWhenTheOtherIntervalSetIsNull()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.That( set.IsEqualTo( null ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalSetsDiffer()
	{
		var oneSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.OpenClosed( -3, 1 )] );
		var otherSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.ClosedOpen( -1, 3 )] );

		Assert.That( oneSet.IsEqualTo( otherSet ), Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenTheIntervalsAreEqual()
	{
		var firstInterval = Core.Math.Interval.OpenClosed( -3, -1 );
		var secondInterval = Core.Math.Interval.Closed( -1, 1 );
		var thirdInterval = Core.Math.Interval.ClosedOpen( 1, 3 );

		var splitSet = Core.Math.IntervalSet.Create( [firstInterval, secondInterval, thirdInterval] );
		var joinedSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -3, 3 )] );

		Assert.That( splitSet.IsEqualTo( joinedSet ), Is.True );
	}

	[Test]
	public void ShouldWorkWithInfiniteIntervals()
	{
		var negativeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.OpenClosed( double.NegativeInfinity, 1 )] );
		var positiveSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.ClosedOpen( -1, double.PositiveInfinity )] );
		var wholeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.Multiple( () =>
		{
			Assert.That( negativeSet.IsEqualTo( negativeSet ), Is.True );
			Assert.That( positiveSet.IsEqualTo( positiveSet ), Is.True );
			Assert.That( wholeSet.IsEqualTo( wholeSet ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double maximum = 10.0;

		var narrowerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( minimum, maximum )] );
		var widerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( minimum - tolerance * 0.5, maximum + tolerance * 0.5 )] );
		var tooWideSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( minimum - tolerance * 1.5, maximum + tolerance * 1.5 )] );

		Assert.Multiple( () =>
		{
			Assert.That( narrowerSet.IsEqualTo( widerSet, tolerance ), Is.True );
			Assert.That( narrowerSet.IsEqualTo( tooWideSet, tolerance ), Is.False );
		} );
	}

	#endregion
}
