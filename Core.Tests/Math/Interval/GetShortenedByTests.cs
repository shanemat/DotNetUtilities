using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.GetShortenedBy"/> method
/// </summary>
internal sealed class GetShortenedByTests
{
	#region Sources

	private static IEnumerable<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Methods

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.GetShortenedBy( interval, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		var interval = Core.Math.Interval.Open( -1, 1 );

		Assert.Throws<ArgumentException>( () => interval.GetShortenedBy( interval, -1.0 ) );
	}

	[Test]
	public void ShouldReturnTheOriginalIntervalWhenTheOtherIntervalIsNull()
	{
		var interval = Core.Math.Interval.Closed( -3, -1 );

		Assert.That( interval.GetShortenedBy( null ), Is.EqualTo( interval ) );
	}

	[Test]
	public void ShouldReturnTheOriginalIntervalWhenTheIntervalsDoNotIntersect()
	{
		var lowerInterval = Core.Math.Interval.Closed( -3, -1 );
		var upperInterval = Core.Math.Interval.Closed( 1, 3 );

		Assert.Multiple( () =>
		{
			Assert.That( lowerInterval.GetShortenedBy( upperInterval ), Is.EqualTo( lowerInterval ) );
			Assert.That( upperInterval.GetShortenedBy( lowerInterval ), Is.EqualTo( upperInterval ) );
		} );
	}

	[Test]
	public void ShouldReturnNullWhenTheResultWouldBeAnEmptyInterval()
	{
		var innerInterval = Core.Math.Interval.Closed( -1, 1 );
		var outerInterval = Core.Math.Interval.Closed( -3, 3 );

		Assert.That( innerInterval.GetShortenedBy( outerInterval ), Is.Null );
	}

	[Test]
	public void ShouldReturnNullWhenTheResultWouldBeAnIntervalSet()
	{
		var innerInterval = Core.Math.Interval.Closed( -1, 1 );
		var outerInterval = Core.Math.Interval.Closed( -3, 3 );

		Assert.That( outerInterval.GetShortenedBy( innerInterval ), Is.Null );
	}

	[Test]
	public void ShouldWorkWithIntervalsWithMinimumOfNegativeInfinity()
	{
		var narrowerInterval = Core.Math.Interval.Open( double.NegativeInfinity, 1 );
		var widerInterval = Core.Math.Interval.Open( double.NegativeInfinity, 3 );

		var expectedResult = Core.Math.Interval.ClosedOpen( 1, 3 );

		Assert.That( widerInterval.GetShortenedBy( narrowerInterval ), Is.EqualTo( expectedResult ) );
	}

	[Test]
	public void ShouldWorkWithIntervalsWithMaximumOfPositiveInfinity()
	{
		var narrowerInterval = Core.Math.Interval.Open( -1, double.PositiveInfinity );
		var widerInterval = Core.Math.Interval.Open( -3, double.PositiveInfinity );

		var expectedResult = Core.Math.Interval.OpenClosed( -3, -1 );

		Assert.That( widerInterval.GetShortenedBy( narrowerInterval ), Is.EqualTo( expectedResult ) );
	}

	[Test]
	public void ShouldExcludeTheBoundaryPointsOfTheOtherIntervalWhenItIsClosed()
	{
		var lowerInterval = Core.Math.Interval.Open( double.NegativeInfinity, 0 );
		var middleInterval = Core.Math.Interval.Closed( -1, 1 );
		var upperInterval = Core.Math.Interval.Open( 0, double.PositiveInfinity );

		Assert.Multiple( () =>
		{
			Assert.That( lowerInterval.GetShortenedBy( middleInterval )?.IsMaximumIncluded, Is.False );
			Assert.That( middleInterval.GetShortenedBy( lowerInterval )?.IsMinimumIncluded, Is.True );
			Assert.That( upperInterval.GetShortenedBy( middleInterval )?.IsMinimumIncluded, Is.False );
			Assert.That( middleInterval.GetShortenedBy( upperInterval )?.IsMaximumIncluded, Is.True );
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
			Assert.That( lowerWiderInterval.GetShortenedBy( upperWiderInterval, tolerance ), Is.EqualTo( expectedWiderInterval ) );
			Assert.That( lowerNarrowerInterval.GetShortenedBy( upperNarrowerInterval, tolerance ), Is.EqualTo( lowerNarrowerInterval ) );
		} );
	}

	#endregion
}
