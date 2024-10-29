using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.Create"/> method
/// </summary>
internal sealed class CreateTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.Create( tolerance: double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.Create( tolerance: -1.0 ) );
	}

	[Test]
	public void ShouldReturnEmptySetForNoIntervals()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.That( set.Intervals.Any(), Is.False );
	}

	[Test]
	public void ShouldReturnSetWithTheSameIntervalsWhenTheyDoNotIntersect()
	{
		var firstInterval = Core.Math.Interval.Open( -5, -3 );
		var secondInterval = Core.Math.Interval.OpenClosed( -1, 1 );
		var thirdInterval = Core.Math.Interval.Closed( 3, 5 );

		var set = Core.Math.IntervalSet.Create( [firstInterval, secondInterval, thirdInterval] );

		Assert.That( set.Intervals, Is.EqualTo( new[] { firstInterval, secondInterval, thirdInterval } ) );
	}

	[Test]
	public void ShouldReturnSortedSet()
	{
		var firstInterval = Core.Math.Interval.Open( -7, -5 );
		var secondInterval = Core.Math.Interval.OpenClosed( -3, -1 );
		var thirdInterval = Core.Math.Interval.Closed( 1, 3 );
		var fourthInterval = Core.Math.Interval.ClosedOpen( 5, 7 );

		var set = Core.Math.IntervalSet.Create( [thirdInterval, firstInterval, fourthInterval, secondInterval] );

		Assert.That( set.Intervals, Is.EqualTo( new[] { firstInterval, secondInterval, thirdInterval, fourthInterval } ) );
	}

	[Test]
	public void ShouldReturnReducedSet()
	{
		var firstInterval = Core.Math.Interval.OpenClosed( -5, -3 );
		var secondInterval = Core.Math.Interval.ClosedOpen( -3, -1 );
		var thirdInterval = Core.Math.Interval.Open( 1, 4 );
		var fourthInterval = Core.Math.Interval.Open( 2, 5 );

		var set = Core.Math.IntervalSet.Create( [thirdInterval, firstInterval, fourthInterval, secondInterval] );

		Assert.That( set.Intervals, Is.EqualTo( new[] { Core.Math.Interval.Open( -5, -1 ), Core.Math.Interval.Open( 1, 5 ) } ) );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double middle = 0.0;
		const double maximum = 10.0;

		var narrowerLowerInterval = Core.Math.Interval.OpenClosed( minimum, middle - tolerance * 0.75 );
		var narrowerUpperInterval = Core.Math.Interval.ClosedOpen( middle + tolerance * 0.75, maximum );
		var narrowerSet = Core.Math.IntervalSet.Create( [narrowerLowerInterval, narrowerUpperInterval], tolerance );

		var widerLowerInterval = Core.Math.Interval.OpenClosed( minimum, middle - tolerance * 0.25 );
		var widerUpperInterval = Core.Math.Interval.ClosedOpen( middle + tolerance * 0.25, maximum );
		var widerSet = Core.Math.IntervalSet.Create( [widerLowerInterval, widerUpperInterval], tolerance );

		Assert.Multiple( () =>
		{
			Assert.That( narrowerSet.Intervals, Is.EqualTo( new[] { narrowerLowerInterval, narrowerUpperInterval } ) );
			Assert.That( widerSet.Intervals, Is.EqualTo( new[] { Core.Math.Interval.Open( minimum, maximum ) } ) );
		} );
	}

	#endregion
}
