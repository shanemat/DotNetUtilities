using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains test for <see cref="Interval.GetIntersection"/> method
/// </summary>
internal sealed class GetIntersectionTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.GetIntersection( interval, interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.GetIntersection( interval, interval, -1.0 ) );
	}

	[Test]
	public void ShouldReturnNullWhenEitherIntervalIsNull()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetIntersection( null, null ), Is.Null );
			Assert.That( Core.Math.Interval.GetIntersection( oneInterval, null ), Is.Null );
			Assert.That( Core.Math.Interval.GetIntersection( null, oneInterval ), Is.Null );
		} );
	}

	[Test]
	public void ShouldReturnTheIntervalWhenItIsCalledWithTheSameInterval()
	{
		var openInterval = Core.Math.Interval.Open( -4, -3 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -2, -1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( 1, 2 );
		var closedInterval = Core.Math.Interval.Closed( 3, 4 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetIntersection( openInterval, openInterval ), Is.EqualTo( openInterval ) );
			Assert.That( Core.Math.Interval.GetIntersection( openClosedInterval, openClosedInterval ), Is.EqualTo( openClosedInterval ) );
			Assert.That( Core.Math.Interval.GetIntersection( closedOpenInterval, closedOpenInterval ), Is.EqualTo( closedOpenInterval ) );
			Assert.That( Core.Math.Interval.GetIntersection( closedInterval, closedInterval ), Is.EqualTo( closedInterval ) );
		} );
	}

	[Test]
	public void ShouldReturnNullWhenIntervalsDoNotIntersect()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );
		var otherInterval = Core.Math.Interval.Closed( 1, 3 );

		Assert.That( Core.Math.Interval.GetIntersection( oneInterval, otherInterval ), Is.Null );
	}

	[Test]
	public void ShouldReturnCorrectIntersectionWhenIntervalsIntersect()
	{
		const double lowerOuterValue = -3;
		const double lowerInnerValue = -1;
		const double upperInnerValue = 1;
		const double upperOuterValue = 3;

		var oneInterval = Core.Math.Interval.Closed( lowerOuterValue, upperInnerValue );
		var otherInterval = Core.Math.Interval.Closed( lowerInnerValue, upperOuterValue );

		var expectedIntersection = Core.Math.Interval.Closed( lowerInnerValue, upperInnerValue );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetIntersection( oneInterval, otherInterval ), Is.EqualTo( expectedIntersection ) );
			Assert.That( Core.Math.Interval.GetIntersection( otherInterval, oneInterval ), Is.EqualTo( expectedIntersection ) );
		} );
	}

	[Test]
	public void ShouldReturnIntersectionOfCorrectTypeWhenIntervalsShareMinimum()
	{
		var openInterval = Core.Math.Interval.Open( -1, 5 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( -1, 2 );

		var expectedIntersection = Core.Math.Interval.Open( -1, 2 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetIntersection( openInterval, closedOpenInterval ), Is.EqualTo( expectedIntersection ) );
			Assert.That( Core.Math.Interval.GetIntersection( closedOpenInterval, openInterval ), Is.EqualTo( expectedIntersection ) );
		} );
	}

	[Test]
	public void ShouldReturnIntersectionOfCorrectTypeWhenIntervalsShareMaximum()
	{
		var openInterval = Core.Math.Interval.Open( -5, 1 );
		var closedOpenInterval = Core.Math.Interval.OpenClosed( -2, 1 );

		var expectedIntersection = Core.Math.Interval.Open( -2, 1 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetIntersection( openInterval, closedOpenInterval ), Is.EqualTo( expectedIntersection ) );
			Assert.That( Core.Math.Interval.GetIntersection( closedOpenInterval, openInterval ), Is.EqualTo( expectedIntersection ) );
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

		var expectedIntersection = Core.Math.Interval.Closed( middle - tolerance * 0.25, middle + tolerance * 0.25 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetIntersection( lowerWiderInterval, upperWiderInterval, tolerance ), Is.EqualTo( expectedIntersection ) );
			Assert.That( Core.Math.Interval.GetIntersection( lowerNarrowerInterval, upperNarrowerInterval, tolerance ), Is.Null );
		} );
	}

	#endregion
}
