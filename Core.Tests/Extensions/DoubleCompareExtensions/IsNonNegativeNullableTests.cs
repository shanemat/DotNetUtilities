using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsNonNegative(double?,double)"/> method
/// </summary>
internal sealed class IsNonNegativeNullableTests
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

		Assert.Throws<ArgumentException>( () => value.IsNonNegative( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsNonNegative( -1.0 ) );
	}

	[Test]
	public void ShouldHandleNullValue()
	{
		double? nullValue = null;

		Assert.That( nullValue.IsNonNegative(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( PositiveValues ) )]
	public void ShouldReturnTrueWhenValueIsPositive( double? value )
	{
		Assert.That( value.IsNonNegative(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( NegativeValues ) )]
	public void ShouldReturnFalseWhenValueIsNegative( double? value )
	{
		Assert.That( value.IsNonNegative(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsNegativeWithinTolerance( double tolerance )
	{
		double? value = -tolerance * 0.5;

		Assert.That( value.IsNonNegative( tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnFalseWhenValueIsNegativeOutsideTolerance( double tolerance )
	{
		double? value = -tolerance * 1.5;

		Assert.That( value.IsNonNegative( tolerance ), Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenValueIsZero()
	{
		double? value = 0.0;

		Assert.That( value.IsNonNegative(), Is.True );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		double? negativeInfinity = double.NegativeInfinity;
		double? positiveInfinity = double.PositiveInfinity;

		Assert.Multiple( () =>
		{
			Assert.That( negativeInfinity.IsNonNegative(), Is.False );
			Assert.That( positiveInfinity.IsNonNegative(), Is.True );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		double? invalidNumber = double.NaN;

		Assert.That( invalidNumber.IsNonNegative(), Is.False );
	}

	#endregion
}
