using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsLessThan(double,double,double)"/> method
/// </summary>
internal sealed class IsLessThanTests
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

		Assert.Throws<ArgumentException>( () => value.IsLessThan( value, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		const double value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsLessThan( value, -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldNotBeReflexive( double value )
	{
		Assert.That( value.IsLessThan( value ), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnTrueWhenFirstValueIsLessThanSecondOutsideTolerance( double value, double tolerance )
	{
		Assert.That( value.IsLessThan( value + tolerance * 1.5, tolerance ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnFalseWhenFirstValueIsLessThanSecondWithinTolerance( double value, double tolerance )
	{
		Assert.That( value.IsLessThan( value + tolerance * 0.5, tolerance ), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldWorkWithInfinities( double value )
	{
		Assert.Multiple( () =>
		{
			Assert.That( value.IsLessThan( double.NegativeInfinity ), Is.False );
			Assert.That( value.IsLessThan( double.PositiveInfinity ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseForInvalidNumber( double value )
	{
		Assert.That( value.IsLessThan( double.NaN ), Is.False );
	}

	#endregion
}
