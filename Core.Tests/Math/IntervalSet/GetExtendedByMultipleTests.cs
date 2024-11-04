using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.GetExtendedBy(System.Collections.Generic.IEnumerable{IInterval?}?,double)"/> method
/// </summary>
internal sealed class GetExtendedByMultipleTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => set.GetExtendedBy( [], tolerance: double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => set.GetExtendedBy( [], tolerance: -1.0 ) );
	}

	[Test]
	public void ShouldReturnCurrentSetWhenAddingNullCollection()
	{
		var firstInterval = Core.Math.Interval.Open( -5, -3 );
		var secondInterval = Core.Math.Interval.OpenClosed( -1, 1 );

		var set = Core.Math.IntervalSet.Create( [firstInterval, secondInterval] );

		Assert.That( set.GetExtendedBy( (IInterval[]?) null ).Intervals, Is.EqualTo( new[] { firstInterval, secondInterval } ) );
	}

	[Test]
	public void ShouldReturnSetWithTheSameIntervalsWhenTheyDoNotIntersect()
	{
		var firstInterval = Core.Math.Interval.Open( -7, -5 );
		var secondInterval = Core.Math.Interval.OpenClosed( -3, -1 );
		var thirdInterval = Core.Math.Interval.Closed( 1, 3 );
		var fourthInterval = Core.Math.Interval.ClosedOpen( 5, 7 );

		var set = Core.Math.IntervalSet.Create( [firstInterval, thirdInterval] );

		Assert.That( set.GetExtendedBy( [secondInterval, fourthInterval] ).Intervals, Is.EqualTo( new[] { firstInterval, secondInterval, thirdInterval, fourthInterval } ) );
	}

	[Test]
	public void ShouldReturnSortedSet()
	{
		var firstInterval = Core.Math.Interval.Open( -7, -5 );
		var secondInterval = Core.Math.Interval.OpenClosed( -3, -1 );
		var thirdInterval = Core.Math.Interval.Closed( 1, 3 );
		var fourthInterval = Core.Math.Interval.ClosedOpen( 5, 7 );

		var set = Core.Math.IntervalSet.Create( [firstInterval, thirdInterval] );

		Assert.That( set.GetExtendedBy( [fourthInterval, secondInterval] ).Intervals, Is.EqualTo( new[] { firstInterval, secondInterval, thirdInterval, fourthInterval } ) );
	}

	[Test]
	public void ShouldReturnReducedSet()
	{
		var firstInterval = Core.Math.Interval.OpenClosed( -4, -2 );
		var secondInterval = Core.Math.Interval.Closed( -2, 0 );
		var thirdInterval = Core.Math.Interval.Closed( 0, 2 );
		var fourthInterval = Core.Math.Interval.ClosedOpen( 2, 4 );

		var set = Core.Math.IntervalSet.Create( [firstInterval, thirdInterval] );

		Assert.That( set.GetExtendedBy( [fourthInterval, secondInterval] ).Intervals, Is.EqualTo( new[] { Core.Math.Interval.Open( -4, 4 ) } ) );
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
		var narrowerSet = Core.Math.IntervalSet.Create( [narrowerLowerInterval] );

		var widerLowerInterval = Core.Math.Interval.OpenClosed( minimum, middle - tolerance * 0.25 );
		var widerUpperInterval = Core.Math.Interval.ClosedOpen( middle + tolerance * 0.25, maximum );
		var widerSet = Core.Math.IntervalSet.Create( [widerLowerInterval] );

		Assert.Multiple( () =>
		{
			Assert.That( narrowerSet.GetExtendedBy( [narrowerUpperInterval], tolerance ).Intervals, Is.EqualTo( new[] { narrowerLowerInterval, narrowerUpperInterval } ) );
			Assert.That( widerSet.GetExtendedBy( [widerUpperInterval], tolerance ).Intervals, Is.EqualTo( new[] { Core.Math.Interval.Open( minimum, maximum ) } ) );
		} );
	}

	#endregion
}
