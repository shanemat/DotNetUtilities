using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsNegative(double,double)"/> method
/// </summary>
internal sealed class IsNegativeTests
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

		Assert.Throws<ArgumentException>( () => value.IsNegative( double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsNegative( -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( NegativeValues ) )]
	public void ShouldReturnTrueWhenValueIsNegative( double value )
	{
		Assert.That( value.IsNegative(), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( PositiveValues ) )]
	public void ShouldReturnFalseWhenValueIsPositive( double value )
	{
		Assert.That( value.IsNegative(), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnTrueWhenValueIsNegativeOutsideTolerance( double tolerance )
	{
		var value = -tolerance * 1.5;

		Assert.That( value.IsNegative( tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Tolerances ) )]
	public void ShouldReturnFalseWhenValueIsNegativeWithinTolerance( double tolerance )
	{
		var values = -tolerance * 0.5;

		Assert.That( values.IsNegative( tolerance ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenValueIsZero()
	{
		Assert.That( 0.0.IsNegative(), Is.False );
	}

	[Test]
	public void ShouldWorkWithInfinities()
	{
		Assert.Multiple( () =>
		{
			Assert.That( double.NegativeInfinity.IsNegative(), Is.True );
			Assert.That( double.PositiveInfinity.IsNegative(), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnFalseForInvalidNumber()
	{
		Assert.That( double.NaN.IsNegative(), Is.False );
	}

	#endregion
}
