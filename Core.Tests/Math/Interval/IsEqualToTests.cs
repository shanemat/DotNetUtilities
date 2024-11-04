using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.IsEqualTo"/> method
/// </summary>
internal sealed class IsEqualToTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.IsEqualTo( interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.IsEqualTo( interval, -1.0 ) );
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
			Assert.That( openInterval.IsEqualTo( openInterval ), Is.True );
			Assert.That( openClosedInterval.IsEqualTo( openClosedInterval ), Is.True );
			Assert.That( closedOpenInterval.IsEqualTo( closedOpenInterval ), Is.True );
			Assert.That( closedInterval.IsEqualTo( closedInterval ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnFalseWhenTheOtherIntervalsIsNull()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.That( interval.IsEqualTo( null ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMinimumInclusivity()
	{
		var openInterval = Core.Math.Interval.Open( -1, 1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( -1, 1 );

		Assert.That( openInterval.IsEqualTo( closedOpenInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMaximumInclusivity()
	{
		var openInterval = Core.Math.Interval.Open( -1, 1 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -1, 1 );

		Assert.That( openInterval.IsEqualTo( openClosedInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMinimum()
	{
		var narrowerInterval = Core.Math.Interval.Open( -1, 1 );
		var widerInterval = Core.Math.Interval.Open( -2, 1 );

		Assert.That( narrowerInterval.IsEqualTo( widerInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMaximum()
	{
		var narrowerInterval = Core.Math.Interval.Open( -1, 1 );
		var widerInterval = Core.Math.Interval.Open( -1, 2 );

		Assert.That( narrowerInterval.IsEqualTo( widerInterval ), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfiniteIntervals()
	{
		var negativeInterval = Core.Math.Interval.OpenClosed( double.NegativeInfinity, 1 );
		var positiveInterval = Core.Math.Interval.ClosedOpen( -1, double.PositiveInfinity );
		var wholeInterval = Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity );

		Assert.Multiple( () =>
		{
			Assert.That( negativeInterval.IsEqualTo( negativeInterval ), Is.True );
			Assert.That( positiveInterval.IsEqualTo( positiveInterval ), Is.True );
			Assert.That( wholeInterval.IsEqualTo( wholeInterval ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double maximum = 10.0;

		var narrowerInterval = Core.Math.Interval.Open( minimum, maximum );
		var widerInterval = Core.Math.Interval.Open( minimum - tolerance * 0.5, maximum + tolerance * 0.5 );
		var tooWideInterval = Core.Math.Interval.Open( minimum - tolerance * 1.5, maximum + tolerance * 1.5 );

		Assert.Multiple( () =>
		{
			Assert.That( narrowerInterval.IsEqualTo( widerInterval, tolerance ), Is.True );
			Assert.That( narrowerInterval.IsEqualTo( tooWideInterval, tolerance ), Is.False );
		} );
	}

	#endregion
}
