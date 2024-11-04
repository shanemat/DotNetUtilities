using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.IntersectsWith"/> method
/// </summary>
internal sealed class IntersectsWithTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.IntersectsWith( interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.IntersectsWith( interval, -1.0 ) );
	}

	[Test]
	public void ShouldReturnFalseWhenTheOtherIntervalIsNull()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.That( interval.IntersectsWith( null ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenIntervalsDoNotIntersect()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );
		var otherInterval = Core.Math.Interval.Closed( 1, 3 );

		Assert.That( oneInterval.IntersectsWith( otherInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenIntervalsIntersect()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, 1 );
		var otherInterval = Core.Math.Interval.Closed( -1, 3 );

		Assert.That( oneInterval.IntersectsWith( otherInterval ), Is.True );
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
			Assert.That( openInterval.IntersectsWith( openInterval ), Is.True );
			Assert.That( openClosedInterval.IntersectsWith( openClosedInterval ), Is.True );
			Assert.That( closedOpenInterval.IntersectsWith( closedOpenInterval ), Is.True );
			Assert.That( closedInterval.IntersectsWith( closedInterval ), Is.True );
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
			Assert.That( openInterval.IntersectsWith( openInterval ), Is.True );
			Assert.That( openClosedInterval.IntersectsWith( openClosedInterval ), Is.True );
			Assert.That( closedOpenInterval.IntersectsWith( closedOpenInterval ), Is.True );
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
			Assert.That( lowerInterval.IntersectsWith( upperInterval ), Is.True );
			Assert.That( upperInterval.IntersectsWith( lowerInterval ), Is.True );
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
			Assert.That( lowerInterval.IntersectsWith( upperInterval ), Is.False );
			Assert.That( upperInterval.IntersectsWith( lowerInterval ), Is.False );
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
			Assert.That( lowerWiderInterval.IntersectsWith( upperWiderInterval, tolerance ), Is.True );
			Assert.That( lowerNarrowerInterval.IntersectsWith( upperNarrowerInterval, tolerance ), Is.False );
		} );
	}

	#endregion
}
