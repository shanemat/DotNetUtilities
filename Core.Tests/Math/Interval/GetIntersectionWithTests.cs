using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains test for <see cref="Interval.GetIntersectionWith"/> method
/// </summary>
internal sealed class GetIntersectionWithTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.GetIntersectionWith( interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.GetIntersectionWith( interval, -1.0 ) );
	}

	[Test]
	public void ShouldReturnNullWhenTheOtherIntervalIsNull()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );

		Assert.That( oneInterval.GetIntersectionWith( null ), Is.Null );
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
			Assert.That( openInterval.GetIntersectionWith( openInterval ), Is.EqualTo( openInterval ) );
			Assert.That( openClosedInterval.GetIntersectionWith( openClosedInterval ), Is.EqualTo( openClosedInterval ) );
			Assert.That( closedOpenInterval.GetIntersectionWith( closedOpenInterval ), Is.EqualTo( closedOpenInterval ) );
			Assert.That( closedInterval.GetIntersectionWith( closedInterval ), Is.EqualTo( closedInterval ) );
		} );
	}

	[Test]
	public void ShouldReturnNullWhenIntervalsDoNotIntersect()
	{
		var oneInterval = Core.Math.Interval.Closed( -3, -1 );
		var otherInterval = Core.Math.Interval.Closed( 1, 3 );

		Assert.That( oneInterval.GetIntersectionWith( otherInterval ), Is.Null );
	}

	[Test]
	public void ShouldReturnCorrectIntersectionWithWhenIntervalsIntersect()
	{
		const double lowerOuterValue = -3;
		const double lowerInnerValue = -1;
		const double upperInnerValue = 1;
		const double upperOuterValue = 3;

		var oneInterval = Core.Math.Interval.Closed( lowerOuterValue, upperInnerValue );
		var otherInterval = Core.Math.Interval.Closed( lowerInnerValue, upperOuterValue );

		var expectedIntersectionWith = Core.Math.Interval.Closed( lowerInnerValue, upperInnerValue );

		Assert.Multiple( () =>
		{
			Assert.That( oneInterval.GetIntersectionWith( otherInterval ), Is.EqualTo( expectedIntersectionWith ) );
			Assert.That( otherInterval.GetIntersectionWith( oneInterval ), Is.EqualTo( expectedIntersectionWith ) );
		} );
	}

	[Test]
	public void ShouldReturnIntersectionWithOfCorrectTypeWhenIntervalsShareMinimum()
	{
		var openInterval = Core.Math.Interval.Open( -1, 5 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( -1, 2 );

		var expectedIntersectionWith = Core.Math.Interval.Open( -1, 2 );

		Assert.Multiple( () =>
		{
			Assert.That( openInterval.GetIntersectionWith( closedOpenInterval ), Is.EqualTo( expectedIntersectionWith ) );
			Assert.That( closedOpenInterval.GetIntersectionWith( openInterval ), Is.EqualTo( expectedIntersectionWith ) );
		} );
	}

	[Test]
	public void ShouldReturnIntersectionWithOfCorrectTypeWhenIntervalsShareMaximum()
	{
		var openInterval = Core.Math.Interval.Open( -5, 1 );
		var closedOpenInterval = Core.Math.Interval.OpenClosed( -2, 1 );

		var expectedIntersectionWith = Core.Math.Interval.Open( -2, 1 );

		Assert.Multiple( () =>
		{
			Assert.That( openInterval.GetIntersectionWith( closedOpenInterval ), Is.EqualTo( expectedIntersectionWith ) );
			Assert.That( closedOpenInterval.GetIntersectionWith( openInterval ), Is.EqualTo( expectedIntersectionWith ) );
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

		var expectedIntersectionWith = Core.Math.Interval.Closed( middle - tolerance * 0.25, middle + tolerance * 0.25 );

		Assert.Multiple( () =>
		{
			Assert.That( lowerWiderInterval.GetIntersectionWith( upperWiderInterval, tolerance ), Is.EqualTo( expectedIntersectionWith ) );
			Assert.That( lowerNarrowerInterval.GetIntersectionWith( upperNarrowerInterval, tolerance ), Is.Null );
		} );
	}

	#endregion
}
