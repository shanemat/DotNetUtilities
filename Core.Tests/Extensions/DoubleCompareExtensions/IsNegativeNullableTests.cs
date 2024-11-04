using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsNegative(double?,double)"/> method
/// </summary>
internal sealed class IsNegativeNullableTests
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

		Assert.Throws<ArgumentException>( () => value.IsNegative( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsNegative( -1.0 ) );
	}

	[Test]
	public void ShouldHandleNullValue()
	{
		double? nullValue = null;

		Assert.That( nullValue.IsNegative(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( NegativeValues ) )]
	public void ShouldReturnTrueWhenValueIsNegative( double? value )
	{
		Assert.That( value.IsNegative(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( PositiveValues ) )]
	public void ShouldReturnFalseWhenValueIsPositive( double? value )
	{
		Assert.That( value.IsNegative(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsNegativeOutsideTolerance( double tolerance )
	{
		double? value = -tolerance * 1.5;

		Assert.That( value.IsNegative( tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnFalseWhenValueIsNegativeWithinTolerance( double tolerance )
	{
		double? value = -tolerance * 0.5;

		Assert.That( value.IsNegative( tolerance ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenValueIsZero()
	{
		double? value = 0.0;

		Assert.That( value.IsNegative(), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		double? negativeInfinity = double.NegativeInfinity;
		double? positiveInfinity = double.PositiveInfinity;

		Assert.Multiple( () =>
		{
			Assert.That( negativeInfinity.IsNegative(), Is.True );
			Assert.That( positiveInfinity.IsNegative(), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		double? invalidNumber = double.NaN;

		Assert.That( invalidNumber.IsNegative(), Is.False );
	}

	#endregion
}
