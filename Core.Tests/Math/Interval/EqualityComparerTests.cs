using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.EqualityComparer"/> class
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
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.EqualityComparer.Create( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.EqualityComparer.Create( -1.0 ) );
	}

	[Test]
	public void ShouldBeReflexive()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();

		var openInterval = Core.Math.Interval.Open( -4, -3 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -2, -1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( 1, 2 );
		var closedInterval = Core.Math.Interval.Closed( 3, 4 );

		Assert.Multiple( () =>
		{
			Assert.That( comparer.Equals( openInterval, openInterval ), Is.True );
			Assert.That( comparer.Equals( openClosedInterval, openClosedInterval ), Is.True );
			Assert.That( comparer.Equals( closedOpenInterval, closedOpenInterval ), Is.True );
			Assert.That( comparer.Equals( closedInterval, closedInterval ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnTrueWhenBothIntervalsAreNull()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();

		Assert.That( comparer.Equals( null, null ), Is.True );
	}

	[Test]
	public void ShouldReturnFalseWhenOneOfTheIntervalsIsNull()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Multiple( () =>
		{
			Assert.That( comparer.Equals( interval, null ), Is.False );
			Assert.That( comparer.Equals( null, interval ), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMinimumInclusivity()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();
		var openInterval = Core.Math.Interval.Open( -1, 1 );
		var closedOpenInterval = Core.Math.Interval.ClosedOpen( -1, 1 );

		Assert.That( comparer.Equals( openInterval, closedOpenInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMaximumInclusivity()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();
		var openInterval = Core.Math.Interval.Open( -1, 1 );
		var openClosedInterval = Core.Math.Interval.OpenClosed( -1, 1 );

		Assert.That( comparer.Equals( openInterval, openClosedInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMinimum()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();
		var narrowerInterval = Core.Math.Interval.Open( -1, 1 );
		var widerInterval = Core.Math.Interval.Open( -2, 1 );

		Assert.That( comparer.Equals( narrowerInterval, widerInterval ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenTheIntervalsDifferInMaximum()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();

		var narrowerInterval = Core.Math.Interval.Open( -1, 1 );
		var widerInterval = Core.Math.Interval.Open( -1, 2 );

		Assert.That( comparer.Equals( narrowerInterval, widerInterval ), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfiniteIntervals()
	{
		var comparer = Core.Math.Interval.EqualityComparer.Create();

		var negativeInterval = Core.Math.Interval.OpenClosed( double.NegativeInfinity, 1 );
		var positiveInterval = Core.Math.Interval.ClosedOpen( -1, double.PositiveInfinity );
		var wholeInterval = Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity );

		Assert.Multiple( () =>
		{
			Assert.That( comparer.Equals( negativeInterval, negativeInterval ), Is.True );
			Assert.That( comparer.Equals( positiveInterval, positiveInterval ), Is.True );
			Assert.That( comparer.Equals( wholeInterval, wholeInterval ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double maximum = 10.0;

		var comparer = Core.Math.Interval.EqualityComparer.Create( tolerance );

		var narrowerInterval = Core.Math.Interval.Open( minimum, maximum );
		var widerInterval = Core.Math.Interval.Open( minimum - tolerance * 0.5, maximum + tolerance * 0.5 );
		var tooWideInterval = Core.Math.Interval.Open( minimum - tolerance * 1.5, maximum + tolerance * 1.5 );

		Assert.Multiple( () =>
		{
			Assert.That( comparer.Equals( narrowerInterval, widerInterval ), Is.True );
			Assert.That( comparer.Equals( narrowerInterval, tooWideInterval ), Is.False );
		} );
	}

	#endregion
}
