using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsGreaterThanOrEqualTo(double,double,double)"/> method
/// </summary>
internal sealed class IsGreaterThanOrEqualToTests
{
	#region Sources

	private static IReadOnlyCollection<double> Values => Sources.Values;

	private static IEnumerable<double[]> ValuesAndTolerances => Values.CombineWith( Sources.Tolerances );

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsGreaterThanOrEqualTo( value, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsGreaterThanOrEqualTo( value, -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldBeReflexive( double value )
	{
		Assert.That( value.IsGreaterThanOrEqualTo( value ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnTrueWhenFirstValueIsLessThanSecondWithinTolerance( double value, double tolerance )
	{
		Assert.That( value.IsGreaterThanOrEqualTo( value + tolerance * 0.5, tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnFalseWhenFirstValueIsLessThanSecondOutsideTolerance( double value, double tolerance )
	{
		Assert.That( value.IsGreaterThanOrEqualTo( value + tolerance * 1.5, tolerance ), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldWorkWithInfinities( double value )
	{
		Assert.Multiple( () =>
		{
			Assert.That( value.IsGreaterThanOrEqualTo( double.NegativeInfinity ), Is.True );
			Assert.That( value.IsGreaterThanOrEqualTo( double.PositiveInfinity ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseForInvalidNumber( double value )
	{
		Assert.That( value.IsGreaterThanOrEqualTo( double.NaN ), Is.False );
	}

	#endregion
}
