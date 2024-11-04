using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.GetShortened"/> method
/// </summary>
internal sealed class GetShortenedTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.GetShortened( interval, interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => Core.Math.Interval.GetShortened( interval, interval, -1.0 ) );
	}

	[Test]
	public void ShouldReturnTheOriginalIntervalWhenTheOtherIntervalIsNull()
	{
		var interval = Core.Math.Interval.Closed( -3, -1 );

		Assert.That( Core.Math.Interval.GetShortened( interval, null ), Is.EqualTo( interval ) );
	}

	[Test]
	public void ShouldReturnNullWhenTheOriginalIntervalIsNull()
	{
		var interval = Core.Math.Interval.Closed( -3, -1 );

		Assert.That( Core.Math.Interval.GetShortened( null, interval ), Is.Null );
	}

	[Test]
	public void ShouldReturnTheOriginalIntervalWhenTheIntervalsDoNotIntersect()
	{
		var lowerInterval = Core.Math.Interval.Closed( -3, -1 );
		var upperInterval = Core.Math.Interval.Closed( 1, 3 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetShortened( lowerInterval, upperInterval ), Is.EqualTo( lowerInterval ) );
			Assert.That( Core.Math.Interval.GetShortened( upperInterval, lowerInterval ), Is.EqualTo( upperInterval ) );
		} );
	}

	[Test]
	public void ShouldReturnNullWhenTheResultWouldBeAnEmptyInterval()
	{
		var innerInterval = Core.Math.Interval.Closed( -1, 1 );
		var outerInterval = Core.Math.Interval.Closed( -3, 3 );

		Assert.That( Core.Math.Interval.GetShortened( innerInterval, outerInterval ), Is.Null );
	}

	[Test]
	public void ShouldReturnNullWhenTheResultWouldBeAnIntervalSet()
	{
		var innerInterval = Core.Math.Interval.Closed( -1, 1 );
		var outerInterval = Core.Math.Interval.Closed( -3, 3 );

		Assert.That( Core.Math.Interval.GetShortened( outerInterval, innerInterval ), Is.Null );
	}

	[Test]
	public void ShouldWorkWithIntervalsWithMinimumOfNegativeInfinity()
	{
		var narrowerInterval = Core.Math.Interval.Open( double.NegativeInfinity, 1 );
		var widerInterval = Core.Math.Interval.Open( double.NegativeInfinity, 3 );

		var expectedResult = Core.Math.Interval.ClosedOpen( 1, 3 );

		Assert.That( Core.Math.Interval.GetShortened( widerInterval, narrowerInterval ), Is.EqualTo( expectedResult ) );
	}

	[Test]
	public void ShouldWorkWithIntervalsWithMaximumOfPositiveInfinity()
	{
		var narrowerInterval = Core.Math.Interval.Open( -1, double.PositiveInfinity );
		var widerInterval = Core.Math.Interval.Open( -3, double.PositiveInfinity );

		var expectedResult = Core.Math.Interval.OpenClosed( -3, -1 );

		Assert.That( Core.Math.Interval.GetShortened( widerInterval, narrowerInterval ), Is.EqualTo( expectedResult ) );
	}

	[Test]
	public void ShouldExcludeTheBoundaryPointsOfTheOtherIntervalWhenItIsClosed()
	{
		var lowerInterval = Core.Math.Interval.Open( double.NegativeInfinity, 0 );
		var middleInterval = Core.Math.Interval.Closed( -1, 1 );
		var upperInterval = Core.Math.Interval.Open( 0, double.PositiveInfinity );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetShortened( lowerInterval, middleInterval )?.IsMaximumIncluded, Is.False );
			Assert.That( Core.Math.Interval.GetShortened( middleInterval, lowerInterval )?.IsMinimumIncluded, Is.True );
			Assert.That( Core.Math.Interval.GetShortened( upperInterval, middleInterval )?.IsMinimumIncluded, Is.False );
			Assert.That( Core.Math.Interval.GetShortened( middleInterval, upperInterval )?.IsMaximumIncluded, Is.True );
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

		var expectedWiderInterval = Core.Math.Interval.ClosedOpen( minimum, middle - tolerance * 0.25 );

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.GetShortened( lowerWiderInterval, upperWiderInterval, tolerance ), Is.EqualTo( expectedWiderInterval ) );
			Assert.That( Core.Math.Interval.GetShortened( lowerNarrowerInterval, upperNarrowerInterval, tolerance ), Is.EqualTo( lowerNarrowerInterval ) );
		} );
	}

	#endregion
}
