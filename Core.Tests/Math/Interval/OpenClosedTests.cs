using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains tests for <see cref="Interval.OpenClosed"/> method
/// </summary>
internal sealed class OpenClosedTests
{
	#region Methods

	[Test]
	public void ShouldThrowWhenMinimumIsNotANumber()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.OpenClosed( double.NaN, -2 ) );
	}

	[Test]
	public void ShouldThrowWhenMaximumIsNotANumber()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.OpenClosed( -17, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenMinimumExceedsMaximum()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.OpenClosed( 1, 0 ) );
	}

	[Test]
	public void ShouldThrowWhenMaximumIsNotFinite()
	{
		Assert.Throws<ArgumentException>( () => Core.Math.Interval.OpenClosed( -17, double.PositiveInfinity ) );
	}

	[Test]
	public void ShouldCreateIntervalWhichDoesNotHaveMinimumIncluded()
	{
		var interval = Core.Math.Interval.OpenClosed( -17, -2 );

		Assert.That( interval.IsMinimumIncluded, Is.False );
	}

	[Test]
	public void ShouldCreateIntervalWhichHasMaximumIncluded()
	{
		var interval = Core.Math.Interval.OpenClosed( -17, -2 );

		Assert.That( interval.IsMaximumIncluded, Is.True );
	}

	[Test]
	public void ShouldCreateIntervalWithCorrectMinimum()
	{
		var interval = Core.Math.Interval.OpenClosed( -17, -2 );

		Assert.That( interval.Minimum, Is.EqualTo( -17 ) );
	}

	[Test]
	public void ShouldCreateIntervalWithCorrectMaximum()
	{
		var interval = Core.Math.Interval.OpenClosed( -17, -2 );

		Assert.That( interval.Maximum, Is.EqualTo( -2 ) );
	}

	#endregion
}
