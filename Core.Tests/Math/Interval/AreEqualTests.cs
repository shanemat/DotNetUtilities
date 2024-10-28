using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.AreEqual"/> method
/// </summary>
internal sealed class AreEqualTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.AreEqual( interval, interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.AreEqual( interval, interval, -1.0 ) );
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
			Assert.That( Core.Math.Interval.AreEqual( openInterval, openInterval ), Is.True );
			Assert.That( Core.Math.Interval.AreEqual( openClosedInterval, openClosedInterval ), Is.True );
			Assert.That( Core.Math.Interval.AreEqual( closedOpenInterval, closedOpenInterval ), Is.True );
			Assert.That( Core.Math.Interval.AreEqual( closedInterval, closedInterval ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnTrueWhenBothIntervalsAreNull()
	{
		Assert.That( Core.Math.Interval.AreEqual( null, null ), Is.True );
	}

	[Test]
	public void ShouldReturnFalseWhenOneOfTheIntervalsIsNull()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.AreEqual( interval, null ), Is.False );
			Assert.That( Core.Math.Interval.AreEqual( null, interval ), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMinimumInclusivity()
	{
		var openInterval = Core.Math.Interval.Open( -1, 1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( -1, 1 );

		Assert.That( Core.Math.Interval.AreEqual( openInterval, closedOpenInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMaximumInclusivity()
	{
		var openInterval = Core.Math.Interval.Open( -1, 1 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -1, 1 );

		Assert.That( Core.Math.Interval.AreEqual( openInterval, openClosedInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMinimum()
	{
		var narrowerInterval = Core.Math.Interval.Open( -1, 1 );
		var widerInterval = Core.Math.Interval.Open( -2, 1 );

		Assert.That( Core.Math.Interval.AreEqual( narrowerInterval, widerInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMaximum()
	{
		var narrowerInterval = Core.Math.Interval.Open( -1, 1 );
		var widerInterval = Core.Math.Interval.Open( -1, 2 );

		Assert.That( Core.Math.Interval.AreEqual( narrowerInterval, widerInterval ), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfiniteIntervals()
	{
		var negativeInterval = Core.Math.Interval.OpenClosed( double.NegativeInfinity, 1 );
		var positiveInterval = Core.Math.Interval.ClosedOpen( -1, double.PositiveInfinity );
		var wholeInterval = Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.AreEqual( negativeInterval, negativeInterval ), Is.True );
			Assert.That( Core.Math.Interval.AreEqual( positiveInterval, positiveInterval ), Is.True );
			Assert.That( Core.Math.Interval.AreEqual( wholeInterval, wholeInterval ), Is.True );
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
			Assert.That( Core.Math.Interval.AreEqual( narrowerInterval, widerInterval, tolerance ), Is.True );
			Assert.That( Core.Math.Interval.AreEqual( narrowerInterval, tooWideInterval, tolerance ), Is.False );
		} );
	}

	#endregion
}
