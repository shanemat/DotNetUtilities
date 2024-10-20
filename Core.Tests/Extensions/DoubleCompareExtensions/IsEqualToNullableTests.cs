using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains tests for <see cref="DoubleCompareExtensions.IsEqualTo(double?,double?,double)"/> method
/// </summary>
internal sealed class IsEqualToTestsNullable
{
	#region Sources

	private static IReadOnlyCollection<double> Values => Sources.Values;

	private static IReadOnlyCollection<double> SpecialValues => Sources.SpecialValues;

	private static IEnumerable<double[]> NegativeAndPositiveValues => Sources.NegativeValues.CombineWith( Sources.PositiveValues );

	private static IEnumerable<double[]> ValuesAndTolerances => Values.CombineWith( Sources.Tolerances );

	#endregion

	#region Tests

	[Test]
	public void ShouldThrowWhenToleranceIsNotANumber()
	{
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsEqualTo( value, double.NaN ) );
	}

	[Test]
	public void ShouldThrowWhenToleranceIsNegative()
	{
		double? value = 0.0;

		Assert.Throws<ArgumentException>( () => value.IsEqualTo( value, -1.0 ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldBeReflexive( double? value )
	{
		Assert.That( value.IsEqualTo( value ), Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldHandleNullValues( double? value )
	{
		double? nullValue = null;

		Assert.Multiple( () =>
		{
			Assert.That( nullValue.IsEqualTo( nullValue ), Is.True );
			Assert.That( nullValue.IsEqualTo( value ), Is.False );
			Assert.That( value.IsEqualTo( nullValue ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnTrueWhenComparedValuesAreEqualWithinTolerance( double? value, double tolerance )
	{
		Assert.Multiple( () =>
		{
			Assert.That( value.IsEqualTo( value + tolerance * 0.5, tolerance ), Is.True );
			Assert.That( value.IsEqualTo( value - tolerance * 0.5, tolerance ), Is.True );
		} );
	}

	[Test]
	[TestCaseSource( nameof( ValuesAndTolerances ) )]
	public void ShouldReturnFalseWhenComparedValuesAreJustOutsideTolerance( double? value, double tolerance )
	{
		Assert.Multiple( () =>
		{
			Assert.That( value.IsEqualTo( value + tolerance * 1.5, tolerance ), Is.False );
			Assert.That( value.IsEqualTo( value - tolerance * 1.5, tolerance ), Is.False );
		} );
	}

	[Test]
	[TestCaseSource( nameof( NegativeAndPositiveValues ) )]
	public void ShouldReturnFalseWhenComparedValuesAreNotEqual( double? one, double? other )
	{
		Assert.That( one.IsEqualTo( other ), Is.False );
	}

	[Test]
	[TestCaseSource( nameof( SpecialValues ) )]
	public void ShouldReturnFalseForSpecialValues( double? specialValue )
	{
		Assert.That( specialValue.IsEqualTo( specialValue ), Is.False );
	}

	#endregion
}
