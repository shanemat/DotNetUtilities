using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsZero(double,double)"/> method
/// </summary>
internal sealed class IsZeroTests
{
	#region Sources

	private static IReadOnlyCollection<double> Values => Sources.Values;

	private static IReadOnlyCollection<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsZero( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsZero( -1.0 ) );
	}

	[Test]
	public void ShouldReturnTrueWhenValueIsZero()
	{
		Assert.That( 0.0.IsZero(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseWhenValueIsNotZero( double value )
	{
		Assert.That( value.IsZero(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsWithinTolerance( double tolerance )
	{
		var negativeValue = -tolerance * 0.5;
		var positiveValue = tolerance * 0.5;

		Assert.Multiple( () =>
		{
			Assert.That( negativeValue.IsZero( tolerance ), Is.True );
			Assert.That( positiveValue.IsZero( tolerance ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsOutsideTolerance( double tolerance )
	{
		var negativeValue = -tolerance * 1.5;
		var positiveValue = tolerance * 1.5;

		Assert.Multiple( () =>
		{
			Assert.That( negativeValue.IsZero( tolerance ), Is.False );
			Assert.That( positiveValue.IsZero( tolerance ), Is.False );
		} );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		Assert.Multiple( () =>
		{
			Assert.That( double.NegativeInfinity.IsZero(), Is.False );
			Assert.That( double.PositiveInfinity.IsZero(), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		Assert.That( double.NaN.IsZero(), Is.False );
	}

	#endregion
}
