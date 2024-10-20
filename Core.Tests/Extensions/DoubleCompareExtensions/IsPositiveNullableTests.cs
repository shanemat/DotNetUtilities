using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsPositive(double?,double)"/> method
/// </summary>
internal sealed class IsPositiveNullableTests
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
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsPositive( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsPositive( -1.0 ) );
	}

	[Test]
	public void ShouldHandleNullValue()
	{
		double? nullValue = null;

		Assert.That( nullValue.IsPositive(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( PositiveValues ) )]
	public void ShouldReturnTrueWhenValueIsPositive( double? value )
	{
		Assert.That( value.IsPositive(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( NegativeValues ) )]
	public void ShouldReturnFalseWhenValueIsNegative( double? value )
	{
		Assert.That( value.IsPositive(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsPositiveOutsideTolerance( double tolerance )
	{
		double? value = tolerance * 1.5;

		Assert.That( value.IsPositive( tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnFalseWhenValueIsPositiveWithinTolerance( double tolerance )
	{
		double? values = tolerance * 0.5;

		Assert.That( values.IsPositive( tolerance ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenValueIsZero()
	{
		double? value = 0.0;

		Assert.That( value.IsPositive(), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		double? negativeInfinity = double.NegativeInfinity;
		double? positiveInfinity = double.PositiveInfinity;

		Assert.Multiple( () =>
		{
			Assert.That( negativeInfinity.IsPositive(), Is.False );
			Assert.That( positiveInfinity.IsPositive(), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		double? invalidNumber = double.NaN;

		Assert.That( invalidNumber.IsPositive(), Is.False );
	}

	#endregion
}
