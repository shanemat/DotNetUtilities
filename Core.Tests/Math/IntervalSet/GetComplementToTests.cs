using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.GetComplementTo"/> method
/// </summary>
internal sealed class GetComplementToTests
{
	#region Tests

	[Test]
	public void ShouldReturnWholeSetWhenTheGivenSetIsNull()
	{
		var wholeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.That( Core.Math.IntervalSet.GetComplementTo( null ), Is.EqualTo( wholeSet ) );
	}

	[Test]
	public void ShouldReturnWholeSetWhenTheGivenSetIsEmpty()
	{
		var emptySet = Core.Math.IntervalSet.Create();
		var wholeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.That( Core.Math.IntervalSet.GetComplementTo( emptySet ), Is.EqualTo( wholeSet ) );
	}

	[Test]
	public void ShouldReturnEmptySetWhenTheGivenSetIsWhole()
	{
		var emptySet = Core.Math.IntervalSet.Create();
		var wholeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.That( Core.Math.IntervalSet.GetComplementTo( wholeSet ), Is.EqualTo( emptySet ) );
	}

	[Test]
	public void ShouldReturnCorrectComplement()
	{
		var set = Core.Math.IntervalSet.Create( [
			Core.Math.Interval.Open( -6, -4 ),
			Core.Math.Interval.OpenClosed( -4, -2 ),
			Core.Math.Interval.ClosedOpen( 0, 2 ),
			Core.Math.Interval.Closed( 4, 6 ),
		] );

		var expectedComplement = Core.Math.IntervalSet.Create( [
			Core.Math.Interval.OpenClosed( double.NegativeInfinity, -6 ),
			Core.Math.Interval.Closed( -4, -4 ),
			Core.Math.Interval.Open( -2, 0 ),
			Core.Math.Interval.ClosedOpen( 2, 4 ),
			Core.Math.Interval.Open( 6, double.PositiveInfinity ),
		] );

		Assert.That( Core.Math.IntervalSet.GetComplementTo( set ), Is.EqualTo( expectedComplement ) );
	}

	#endregion
}
