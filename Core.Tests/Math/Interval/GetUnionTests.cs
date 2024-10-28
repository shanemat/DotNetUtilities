using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains test for <see cref="Interval.GetUnion"/> method
/// </summary>
internal sealed class GetUnionTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.GetUnion( interval, interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.GetUnion( interval, interval, -1.0 ) );
	}

	[Test]
	public void ShouldReturnNullWhenEitherIntervalIsNull()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetUnion( null, null ), Is.Null );
			Assert.That( Core.Math.Interval.GetUnion( oneInterval, null ), Is.Null );
			Assert.That( Core.Math.Interval.GetUnion( null, oneInterval ), Is.Null );
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
			Assert.That( Core.Math.Interval.GetUnion( openInterval, openInterval ), Is.EqualTo( openInterval ) );
			Assert.That( Core.Math.Interval.GetUnion( openClosedInterval, openClosedInterval ), Is.EqualTo( openClosedInterval ) );
			Assert.That( Core.Math.Interval.GetUnion( closedOpenInterval, closedOpenInterval ), Is.EqualTo( closedOpenInterval ) );
			Assert.That( Core.Math.Interval.GetUnion( closedInterval, closedInterval ), Is.EqualTo( closedInterval ) );
		} );
	}

	[Test]
	public void ShouldReturnNullWhenIntervalsDoNotIntersect()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );
		var otherInterval = Core.Math.Interval.Closed( 1, 3 );

		Assert.That( Core.Math.Interval.GetUnion( oneInterval, otherInterval ), Is.Null );
	}

	[Test]
	public void ShouldReturnCorrectUnionWhenIntervalsIntersect()
	{
		const double lowerOuterValue = -3;
		const double lowerInnerValue = -1;
		const double upperInnerValue = 1;
		const double upperOuterValue = 3;

		var oneInterval = Core.Math.Interval.Closed( lowerOuterValue, upperInnerValue );
		var otherInterval = Core.Math.Interval.Closed( lowerInnerValue, upperOuterValue );

		var expectedUnion = Core.Math.Interval.Closed( lowerOuterValue, upperOuterValue );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetUnion( oneInterval, otherInterval ), Is.EqualTo( expectedUnion ) );
			Assert.That( Core.Math.Interval.GetUnion( otherInterval, oneInterval ), Is.EqualTo( expectedUnion ) );
		} );
	}

	[Test]
	public void ShouldReturnUnionOfCorrectTypeWhenIntervalsShareMinimum()
	{
		var openInterval = Core.Math.Interval.Open( -1, 5 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( -1, 2 );

		var expectedUnion = Core.Math.Interval.ClosedOpen( -1, 5 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetUnion( openInterval, closedOpenInterval ), Is.EqualTo( expectedUnion ) );
			Assert.That( Core.Math.Interval.GetUnion( closedOpenInterval, openInterval ), Is.EqualTo( expectedUnion ) );
		} );
	}

	[Test]
	public void ShouldReturnUnionOfCorrectTypeWhenIntervalsShareMaximum()
	{
		var openInterval = Core.Math.Interval.Open( -5, 1 );
		var closedOpenInterval = Core.Math.Interval.OpenClosed( -2, 1 );

		var expectedUnion = Core.Math.Interval.OpenClosed( -5, 1 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetUnion( openInterval, closedOpenInterval ), Is.EqualTo( expectedUnion ) );
			Assert.That( Core.Math.Interval.GetUnion( closedOpenInterval, openInterval ), Is.EqualTo( expectedUnion ) );
		} );
	}

	[Test]
	public void ShouldReturnUnionWhenIntervalsTouch()
	{
		const double minimum = -5.0;
		const double middle = 2.0;
		const double maximum = 7.0;

		var lowerOpenInterval = Core.Math.Interval.Open( minimum, middle );
		var lowerClosedInterval = Core.Math.Interval.OpenClosed( minimum, middle );
		var upperOpenInterval = Core.Math.Interval.Open( middle, maximum );
		var upperClosedInterval = Core.Math.Interval.ClosedOpen( middle, maximum );

		var expectedUnion = Core.Math.Interval.Open( minimum, maximum );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetUnion( lowerOpenInterval, upperClosedInterval ), Is.EqualTo( expectedUnion ) );
			Assert.That( Core.Math.Interval.GetUnion( upperClosedInterval, lowerOpenInterval ), Is.EqualTo( expectedUnion ) );
			Assert.That( Core.Math.Interval.GetUnion( lowerClosedInterval, upperOpenInterval ), Is.EqualTo( expectedUnion ) );
			Assert.That( Core.Math.Interval.GetUnion( upperOpenInterval, lowerClosedInterval ), Is.EqualTo( expectedUnion ) );
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

		var expectedUnion = Core.Math.Interval.Closed( minimum, maximum );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetUnion( lowerWiderInterval, upperWiderInterval, tolerance ), Is.EqualTo( expectedUnion ) );
			Assert.That( Core.Math.Interval.GetUnion( lowerNarrowerInterval, upperNarrowerInterval, tolerance ), Is.Null );
		} );
	}

	#endregion
}
