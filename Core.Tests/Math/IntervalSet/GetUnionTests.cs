using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.GetUnion"/> method
/// </summary>
internal sealed class GetUnionTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.GetUnion( set, set, tolerance: double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.GetUnion( set, set, tolerance: -1.0 ) );
	}

	[Test]
	public void ShouldReturnEmptySetWhenBothIntervalSetsAreNull()
	{
		Assert.That( Core.Math.IntervalSet.GetUnion( null, null ).Intervals, Has.Count.Zero );
	}

	[Test]
	public void ShouldReturnOneSetWhenTheOtherIsNull()
	{
		var oneSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -3, 1 )] );
		var otherSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Closed( -1, 3 )] );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.IntervalSet.GetUnion( oneSet, null ), Is.EqualTo( oneSet ) );
			Assert.That( Core.Math.IntervalSet.GetUnion( null, otherSet ), Is.EqualTo( otherSet ) );
		} );
	}

	[Test]
	public void ShouldReturnTheUnionOfSets()
	{
		var firstInterval = Core.Math.Interval.OpenClosed( double.NegativeInfinity, -3 );
		var secondInterval = Core.Math.Interval.ClosedOpen( -3, 1 );
		var thirdInterval = Core.Math.Interval.OpenClosed( -1, 3 );
		var fourthInterval = Core.Math.Interval.ClosedOpen( 3, double.PositiveInfinity );

		var oneSet = Core.Math.IntervalSet.Create( [firstInterval, thirdInterval] );
		var otherSet = Core.Math.IntervalSet.Create( [secondInterval, fourthInterval] );

		Assert.That( Core.Math.IntervalSet.GetUnion( oneSet, otherSet ).Intervals, Is.EqualTo( new[] { Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity ) } ) );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -10.0;
		const double middle = 0.0;
		const double maximum = 10.0;

		var narrowerLowerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.OpenClosed( minimum, middle - tolerance * 0.75 )] );
		var narrowerUpperSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.ClosedOpen( middle + tolerance * 0.75, maximum )] );
		var narrowerSet = Core.Math.IntervalSet.GetUnion( narrowerLowerSet, narrowerUpperSet, tolerance );

		var widerLowerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.OpenClosed( minimum, middle - tolerance * 0.25 )] );
		var widerUpperSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.ClosedOpen( middle + tolerance * 0.25, maximum )] );
		var widerSet = Core.Math.IntervalSet.GetUnion( widerLowerSet, widerUpperSet, tolerance );

		Assert.Multiple( () =>
		{
			Assert.That( narrowerSet.Intervals, Is.EqualTo( narrowerLowerSet.Intervals.Concat( narrowerUpperSet.Intervals ) ) );
			Assert.That( widerSet.Intervals, Is.EqualTo( new[] { Core.Math.Interval.Open( minimum, maximum ) } ) );
		} );
	}

	#endregion
}
