using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.Closed"/> method
/// </summary>
internal sealed class ClosedTests
{
	#region Methods

	[Test]
	public void ShouldThrowWhenMinimumIsNotANumber()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.Closed( double.NaN, 589 ) );
	}

	[Test]
	public void ShouldThrowWhenMaximumIsNotANumber()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.Closed( -219, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenMinimumExceedsMaximum()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.Closed( 1, 0 ) );
	}

	[Test]
	public void ShouldThrowWhenMinimumIsNotFinite()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.Closed( double.NegativeInfinity, 589 ) );
	}

	[Test]
	public void ShouldThrowWhenMaximumIsNotFinite()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.Closed( -219, double.PositiveInfinity ) );
	}

	[Test]
	public void ShouldCreateIntervalWhichHasMinimumIncluded()
	{
		var interval = Core.Math.Interval.Closed( -219, 589 );

		Assert.That( interval.IsMinimumIncluded, Is.True );
	}

	[Test]
	public void ShouldCreateIntervalWhichHasMaximumIncluded()
	{
		var interval = Core.Math.Interval.Closed( -219, 589 );

		Assert.That( interval.IsMaximumIncluded, Is.True );
	}

	[Test]
	public void ShouldCreateIntervalWithCorrectMinimum()
	{
		var interval = Core.Math.Interval.Closed( -219, 589 );

		Assert.That( interval.Minimum, Is.EqualTo( -219 ) );
	}

	[Test]
	public void ShouldCreateIntervalWithCorrectMaximum()
	{
		var interval = Core.Math.Interval.Closed( -219, 589 );

		Assert.That( interval.Maximum, Is.EqualTo( 589 ) );
	}

	#endregion
}
