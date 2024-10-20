using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsZero(double?,double)"/> method
/// </summary>
internal sealed class IsZeroNullableTests
{
	#region Sources

	private static IReadOnlyCollection<double> Values => Sources.Values;

	private static IReadOnlyCollection<double> Tolerances => Sources.Tolerances;

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsZero( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsZero( -1.0 ) );
	}

	[Test]
	public void ShouldHandleNullValue()
	{
		double? nullValue = null;

		Assert.That( nullValue.IsZero(), Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenValueIsZero()
	{
		double? value = 0.0;

		Assert.That( value.IsZero(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseWhenValueIsNotZero( double? value )
	{
		Assert.That( value.IsZero(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsWithinTolerance( double tolerance )
	{
		double? negativeValue = -tolerance * 0.5;
		double? positiveValue = tolerance * 0.5;

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
		double? negativeValue = -tolerance * 1.5;
		double? positiveValue = tolerance * 1.5;

		Assert.Multiple( () =>
		{
			Assert.That( negativeValue.IsZero( tolerance ), Is.False );
			Assert.That( positiveValue.IsZero( tolerance ), Is.False );
		} );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		double? negativeInfinity = double.NegativeInfinity;
		double? positiveInfinity = double.PositiveInfinity;

		Assert.Multiple( () =>
		{
			Assert.That( negativeInfinity.IsZero(), Is.False );
			Assert.That( positiveInfinity.IsZero(), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		double? invalidNumber = double.NaN;

		Assert.That( invalidNumber.IsZero(), Is.False );
	}

	#endregion
}
