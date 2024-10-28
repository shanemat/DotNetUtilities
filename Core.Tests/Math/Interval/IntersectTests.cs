using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.Intersect"/> method
/// </summary>
internal sealed class IntersectTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.Intersect( interval, interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.Intersect( interval, interval, -1.0 ) );
	}

	[Test]
	public void ShouldReturnFalseWhenEitherIntervalIsNull()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.Intersect( null, null ), Is.False );
			Assert.That( Core.Math.Interval.Intersect( interval, null ), Is.False );
			Assert.That( Core.Math.Interval.Intersect( null, interval ), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseWhenIntervalsDoNotIntersect()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );
		var otherInterval = Core.Math.Interval.Closed( 1, 3 );

		Assert.That( Core.Math.Interval.Intersect( oneInterval, otherInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenIntervalsIntersect()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, 1 );
		var otherInterval = Core.Math.Interval.Closed( -1, 3 );

		Assert.That( Core.Math.Interval.Intersect( oneInterval, otherInterval ), Is.True );
	}

	[Test]
	public void ShouldBeReflexive()
	{
		var openInterval = Core.Math.Interval.Open( -4, -3 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -2, -1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( 1, 2 );
		var closedInterval = Core.Math.Interval.Closed( 3, 4 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.Intersect( openInterval, openInterval ), Is.True );
			Assert.That( Core.Math.Interval.Intersect( openClosedInterval, openClosedInterval ), Is.True );
			Assert.That( Core.Math.Interval.Intersect( closedOpenInterval, closedOpenInterval ), Is.True );
			Assert.That( Core.Math.Interval.Intersect( closedInterval, closedInterval ), Is.True );
		} );
	}

	[Test]
	public void ShouldWorkWithInfiniteIntervals()
	{
		var openInterval = Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity );
		var openClosedInterval = Core.Math.Interval.OpenClosed( double.NegativeInfinity, -1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( 1, double.PositiveInfinity );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.Intersect( openInterval, openInterval ), Is.True );
			Assert.That( Core.Math.Interval.Intersect( openClosedInterval, openClosedInterval ), Is.True );
			Assert.That( Core.Math.Interval.Intersect( closedOpenInterval, closedOpenInterval ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnTrueForClosedIntervalsOverlappingOnlyOnBounds()
	{
		const double minimum = -5.0;
		const double middle = 2.0;
		const double maximum = 7.0;

		var lowerInterval = Core.Math.Interval.Closed( minimum, middle );
		var upperInterval = Core.Math.Interval.Closed( middle, maximum );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.Intersect( lowerInterval, upperInterval ), Is.True );
			Assert.That( Core.Math.Interval.Intersect( upperInterval, lowerInterval ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnFalseForOpenIntervalsOverlappingOnlyOnBounds()
	{
		const double minimum = -5.0;
		const double middle = 2.0;
		const double maximum = 7.0;

		var lowerInterval = Core.Math.Interval.Open( minimum, middle );
		var upperInterval = Core.Math.Interval.Open( middle, maximum );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.Intersect( lowerInterval, upperInterval ), Is.False );
			Assert.That( Core.Math.Interval.Intersect( upperInterval, lowerInterval ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -5.0;
		const double middle = 2.0;
		const double maximum = 7.0;

		var lowerWiderInterval = Core.Math.Interval.Closed( minimum, middle - tolerance * 0.25 );
		var lowerNarrowerInterval = Core.Math.Interval.Closed( minimum, middle - tolerance * 0.75 );
		var upperWiderInterval = Core.Math.Interval.Closed( middle + tolerance * 0.25, maximum );
		var upperNarrowerInterval = Core.Math.Interval.Closed( middle + tolerance * 0.75, maximum );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.Intersect( lowerWiderInterval, upperWiderInterval, tolerance ), Is.True );
			Assert.That( Core.Math.Interval.Intersect( lowerNarrowerInterval, upperNarrowerInterval, tolerance ), Is.False );
		} );
	}

	#endregion
}
