using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="IInterval.Contains"/> method
/// </summary>
internal sealed class ContainsTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.Contains( 0, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.Contains( 0, -1.0 ) );
	}

	[Test]
	public void ShouldReturnFalseForInvalidValue()
	{
		var interval = Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity );

		Assert.That( interval.Contains( double.NaN ), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfiniteValues()
	{
		var interval = Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity );

		Assert.Multiple( () =>
		{
			Assert.That( interval.Contains( double.NegativeInfinity ), Is.True );
			Assert.That( interval.Contains( double.PositiveInfinity ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnTrueWhenValueLiesWithinTheInterval()
	{
		var interval = Core.Math.Interval.Open( -100, 100 );

		Assert.Multiple( () =>
		{
			Assert.That( interval.Contains( 0 ), Is.True );
			Assert.That( interval.Contains( -93 ), Is.True );
			Assert.That( interval.Contains( 27 ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnFalseWhenValueLiesOutsideTheInterval()
	{
		var interval = Core.Math.Interval.Open( -100, 100 );

		Assert.Multiple( () =>
		{
			Assert.That( interval.Contains( -273 ), Is.False );
			Assert.That( interval.Contains( 968 ), Is.False );
		} );
	}

	[Test]
	public void ShouldConsiderTypeOfInterval()
	{
		const double minimum = -10.0;
		const double maximum = 10.0;
		const double halfTolerance = Tolerance.Standard * 0.5;

		var openInterval = Core.Math.Interval.Open( minimum, maximum );
		var closedInterval = Core.Math.Interval.Closed( minimum, maximum );

		Assert.Multiple( () =>
		{
			Assert.That( openInterval.Contains( minimum - halfTolerance ), Is.False );
			Assert.That( closedInterval.Contains( minimum - halfTolerance ), Is.True );
			Assert.That( openInterval.Contains( maximum + halfTolerance ), Is.False );
			Assert.That( closedInterval.Contains( maximum + halfTolerance ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double maximum = 10.0;

		var halfTolerance = tolerance * 0.5;

		var openInterval = Core.Math.Interval.Open( minimum, maximum );
		var closedInterval = Core.Math.Interval.Closed( minimum, maximum );

		Assert.Multiple( () =>
		{
			Assert.That( openInterval.Contains( minimum - halfTolerance, tolerance ), Is.False );
			Assert.That( closedInterval.Contains( minimum - halfTolerance, tolerance ), Is.True );
			Assert.That( openInterval.Contains( maximum + halfTolerance, tolerance ), Is.False );
			Assert.That( closedInterval.Contains( maximum + halfTolerance, tolerance ), Is.True );
		} );
	}

	#endregion
}
