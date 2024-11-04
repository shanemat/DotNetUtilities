using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsPositive(double,double)"/> method
/// </summary>
internal sealed class IsPositiveTests
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

		Assert.Throws<ArgumentException>( () => value.IsPositive( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsPositive( -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( PositiveValues ) )]
	public void ShouldReturnTrueWhenValueIsPositive( double value )
	{
		Assert.That( value.IsPositive(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( NegativeValues ) )]
	public void ShouldReturnFalseWhenValueIsNegative( double value )
	{
		Assert.That( value.IsPositive(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsPositiveOutsideTolerance( double tolerance )
	{
		var value = tolerance * 1.5;

		Assert.That( value.IsPositive( tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnFalseWhenValueIsPositiveWithinTolerance( double tolerance )
	{
		var values = tolerance * 0.5;

		Assert.That( values.IsPositive( tolerance ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenValueIsZero()
	{
		Assert.That( 0.0.IsPositive(), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		Assert.Multiple( () =>
		{
			Assert.That( double.NegativeInfinity.IsPositive(), Is.False );
			Assert.That( double.PositiveInfinity.IsPositive(), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		Assert.That( double.NaN.IsPositive(), Is.False );
	}

	#endregion
}
