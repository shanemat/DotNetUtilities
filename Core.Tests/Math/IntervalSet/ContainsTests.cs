using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.Contains"/> method
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
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => set.Contains( 0, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => set.Contains( 0, -1.0 ) );
	}

	[Test]
	public void ShouldReturnFalseForInvalidValue()
	{
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.That( set.Contains( double.NaN ), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfiniteValues()
	{
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.Multiple( () =>
		{
			Assert.That( set.Contains( double.NegativeInfinity ), Is.True );
			Assert.That( set.Contains( double.PositiveInfinity ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnTrueWhenValueLiesWithinTheIntervalSet()
	{
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -100, -50 ), Core.Math.Interval.Closed( 0, 50 )] );

		Assert.Multiple( () =>
		{
			Assert.That( set.Contains( 0 ), Is.True );
			Assert.That( set.Contains( -93 ), Is.True );
			Assert.That( set.Contains( 27 ), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnFalseWhenValueLiesOutsideTheIntervalSet()
	{
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -100, -50 ), Core.Math.Interval.Closed( 50, 100 )] );

		Assert.Multiple( () =>
		{
			Assert.That( set.Contains( -273 ), Is.False );
			Assert.That( set.Contains( 968 ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double maximum = 10.0;

		var halfTolerance = tolerance * 0.5;

		var openSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( minimum, maximum )] );
		var closedSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Closed( minimum, maximum )] );

		Assert.Multiple( () =>
		{
			Assert.That( openSet.Contains( minimum - halfTolerance, tolerance ), Is.False );
			Assert.That( closedSet.Contains( minimum - halfTolerance, tolerance ), Is.True );
			Assert.That( openSet.Contains( maximum + halfTolerance, tolerance ), Is.False );
			Assert.That( closedSet.Contains( maximum + halfTolerance, tolerance ), Is.True );
		} );
	}

	#endregion
}
