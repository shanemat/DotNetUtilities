using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="IInterval.Length"/> method
/// </summary>
internal sealed class LengthTests
{
	#region Methods

	[Test]
	public void ShouldReturnInfinityWhenTheIntervalMinimumIsNegativeInfinity()
	{
		var interval = Core.Math.Interval.Open( double.NegativeInfinity, 0 );

		Assert.That( interval.Length, Is.EqualTo( double.PositiveInfinity ) );
	}

	[Test]
	public void ShouldReturnInfinityWhenTheIntervalMaximumIsPositiveInfinity()
	{
		var interval = Core.Math.Interval.Open( 0, double.PositiveInfinity );

		Assert.That( interval.Length, Is.EqualTo( double.PositiveInfinity ) );
	}

	[Test]
	public void ShouldReturnSameLengthRegardlessOfIntervalType()
	{
		const double minimum = -3569;
		const double maximum = 8426;
		const double length = maximum - minimum;

		Assert.Multiple( () =>
		{
			Assert.That( Core.Math.Interval.Open( minimum, maximum ).Length, Is.EqualTo( length ) );
			Assert.That( Core.Math.Interval.OpenClosed( minimum, maximum ).Length, Is.EqualTo( length ) );
			Assert.That( Core.Math.Interval.ClosedOpen( minimum, maximum ).Length, Is.EqualTo( length ) );
			Assert.That( Core.Math.Interval.Closed( minimum, maximum ).Length, Is.EqualTo( length ) );
		} );
	}

	#endregion
}
