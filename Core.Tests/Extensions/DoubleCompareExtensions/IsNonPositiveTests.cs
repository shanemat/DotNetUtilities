using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsNonPositive(double,double)"/> method
/// </summary>
internal sealed class IsNonPositiveTests
{
	#region Sources

	private static IReadOnlyCollection<double> PositiveValues => Sources.PositiveValues;

	private static IReadOnlyCollection<double> NegativeValues => Sources.NegativeValues;

	private static IReadOnlyCollection<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsNonPositive( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsNonPositive( -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( NegativeValues ) )]
	public void ShouldReturnTrueWhenValueIsNegative( double value )
	{
		Assert.That( value.IsNonPositive(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( PositiveValues ) )]
	public void ShouldReturnFalseWhenValueIsPositive( double value )
	{
		Assert.That( value.IsNonPositive(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsPositiveWithinTolerance( double tolerance )
	{
		var value = tolerance * 0.5;

		Assert.That( value.IsNonPositive( tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnFalseWhenValueIsPositiveOutsideTolerance( double tolerance )
	{
		var value = tolerance * 1.5;

		Assert.That( value.IsNonPositive( tolerance ), Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenValueIsZero()
	{
		Assert.That( 0.0.IsNonPositive(), Is.True );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		Assert.Multiple( () =>
		{
			Assert.That( double.NegativeInfinity.IsNonPositive(), Is.True );
			Assert.That( double.PositiveInfinity.IsNonPositive(), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		Assert.That( double.NaN.IsNonPositive(), Is.False );
	}

	#endregion
}
