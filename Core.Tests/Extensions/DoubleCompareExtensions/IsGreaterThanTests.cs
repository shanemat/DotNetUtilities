using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsGreaterThan(double,double,double)"/> method
/// </summary>
internal sealed class IsGreaterThanTests
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

		Assert.Throws<ArgumentException>( () => value.IsGreaterThan( value, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsGreaterThan( value, -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldNotBeReflexive( double value )
	{
		Assert.That( value.IsGreaterThan( value ), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnTrueWhenFirstValueIsGreaterThanSecondOutsideTolerance( double value, double tolerance )
	{
		Assert.That( value.IsGreaterThan( value - tolerance * 1.5, tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnFalseWhenFirstValueIsGreaterThanSecondWithinTolerance( double value, double tolerance )
	{
		Assert.That( value.IsGreaterThan( value - tolerance * 0.5, tolerance ), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldWorkWithInfinities( double value )
	{
		Assert.Multiple( () =>
		{
			Assert.That( value.IsGreaterThan( double.NegativeInfinity ), Is.True );
			Assert.That( value.IsGreaterThan( double.PositiveInfinity ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseForInvalidNumber( double value )
	{
		Assert.That( value.IsGreaterThan( double.NaN ), Is.False );
	}

	#endregion
}
