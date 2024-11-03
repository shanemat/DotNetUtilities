using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.GetShortened"/> method
/// </summary>
internal sealed class GetShortenedTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.GetShortened( set, set, tolerance: double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var set = Core.Math.IntervalSet.Create();

		Assert.Throws<ArgumentException>( () => Core.Math.IntervalSet.GetShortened( set, set, tolerance: -1.0 ) );
	}

	[Test]
	public void ShouldReturnEmptySetWhenTheOriginalOneIsNull()
	{
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -1, 1 )] );

		Assert.That( Core.Math.IntervalSet.GetShortened( null, set ).Intervals, Has.Count.Zero );
	}

	[Test]
	public void ShouldReturnEmptySetWhenTheOriginalOneIsEmpty()
	{
		var emptySet = Core.Math.IntervalSet.Create();
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -1, 1 )] );

		Assert.That( Core.Math.IntervalSet.GetShortened( emptySet, set ).Intervals, Has.Count.Zero );
	}

	[Test]
	public void ShouldReturnTheOriginalSetWhenTheOtherOneIsNull()
	{
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -1, 1 )] );

		Assert.That( Core.Math.IntervalSet.GetShortened( set, null ), Is.EqualTo( set ) );
	}

	[Test]
	public void ShouldReturnTheOriginalSetWhenTheOtherOneIsEmpty()
	{
		var emptySet = Core.Math.IntervalSet.Create();
		var set = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( -1, 1 )] );

		Assert.That( Core.Math.IntervalSet.GetShortened( set, emptySet ), Is.EqualTo( set ) );
	}

	[Test]
	public void ShouldReturnShortenedSet()
	{
		var originalSet = Core.Math.IntervalSet.Create( [
			Core.Math.Interval.Open( double.NegativeInfinity, -6 ),
			Core.Math.Interval.Closed( -4, 0 ),
			Core.Math.Interval.OpenClosed( 6, 8 ),
			Core.Math.Interval.ClosedOpen( 10, double.PositiveInfinity ),
		] );

		var shortenBySet = Core.Math.IntervalSet.Create( [
			Core.Math.Interval.OpenClosed( double.NegativeInfinity, -10 ),
			Core.Math.Interval.Closed( -7, -2 ),
			Core.Math.Interval.ClosedOpen( 1, 3 ),
			Core.Math.Interval.Open( 7, 12 ),
		] );

		var expectedResult = Core.Math.IntervalSet.Create( [
			Core.Math.Interval.Open( -10, -7 ),
			Core.Math.Interval.OpenClosed( -2, 0 ),
			Core.Math.Interval.OpenClosed( 6, 7 ),
			Core.Math.Interval.ClosedOpen( 12, double.PositiveInfinity ),
		] );

		Assert.That( Core.Math.IntervalSet.GetShortened( originalSet, shortenBySet ), Is.EqualTo( expectedResult ) );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldUseProvidedTolerance( double tolerance )
	{
		const double minimum = -5.0;
		const double middle = 2.0;
		const double maximum = 7.0;

		var lowerWiderSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Closed( minimum, middle - tolerance * 0.25 )] );
		var lowerNarrowerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Closed( minimum, middle - tolerance * 0.75 )] );
		var upperWiderSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Closed( middle + tolerance * 0.25, maximum )] );
		var upperNarrowerSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Closed( middle + tolerance * 0.75, maximum )] );

		var expectedWiderSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.ClosedOpen( minimum, middle - tolerance * 0.25 )] );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.IntervalSet.GetShortened( lowerWiderSet, upperWiderSet, tolerance ), Is.EqualTo( expectedWiderSet ) );
			Assert.That( Core.Math.IntervalSet.GetShortened( lowerNarrowerSet, upperNarrowerSet, tolerance ), Is.EqualTo( lowerNarrowerSet ) );
		} );
	}

	#endregion
}
