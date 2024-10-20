using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsLessThanOrEqualTo(double,double,double)"/> method
/// </summary>
internal sealed class IsLessThanOrEqualToTests
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

		Assert.Throws<ArgumentException>( () => value.IsLessThanOrEqualTo( value, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsLessThanOrEqualTo( value, -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldBeReflexive( double value )
	{
		Assert.That( value.IsLessThanOrEqualTo( value ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnTrueWhenFirstValueIsGreaterThanSecondWithinTolerance( double value, double tolerance )
	{
		Assert.That( value.IsLessThanOrEqualTo( value - tolerance * 0.5, tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnFalseWhenFirstValueIsGreaterThanSecondOutsideTolerance( double value, double tolerance )
	{
		Assert.That( value.IsLessThanOrEqualTo( value - tolerance * 1.5, tolerance ), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldWorkWithInfinities( double value )
	{
		Assert.Multiple( () =>
		{
			Assert.That( value.IsLessThanOrEqualTo( double.NegativeInfinity ), Is.False );
			Assert.That( value.IsLessThanOrEqualTo( double.PositiveInfinity ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseForInvalidNumber( double value )
	{
		Assert.That( value.IsLessThanOrEqualTo( double.NaN ), Is.False );
	}

	#endregion
}
