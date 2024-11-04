using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Math;

namespace Shanemat.DotNetUtils.Core.Tests.Math.IntervalSet;

/// <summary>
/// Contains tests for <see cref="IntervalSet.GetComplement"/> method
/// </summary>
internal sealed class GetComplementTests
{
	#region Tests

	[Test]
	public void ShouldReturnWholeSetWhenTheSetIsEmpty()
	{
		var emptySet = Core.Math.IntervalSet.Create();
		var wholeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.That( emptySet.GetComplement(), Is.EqualTo( wholeSet ) );
	}

	[Test]
	public void ShouldReturnEmptySetWhenTheSetIsWhole()
	{
		var emptySet = Core.Math.IntervalSet.Create();
		var wholeSet = Core.Math.IntervalSet.Create( [Core.Math.Interval.Open( double.NegativeInfinity, double.PositiveInfinity )] );

		Assert.That( wholeSet.GetComplement(), Is.EqualTo( emptySet ) );
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

		Assert.That( set.GetComplement(), Is.EqualTo( expectedComplement ) );
	}

	#endregion
}
