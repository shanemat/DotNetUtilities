namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DoubleCompareExtensions;

/// <summary>
/// Contains the source values to use in double comparison tests
/// </summary>
internal static class Sources
{
	#region Properties

	internal static IReadOnlyCollection<double> PositiveValues { get; } = [2.54785956, 5986.126478, System.Math.E];

	internal static IReadOnlyCollection<double> NegativeValues { get; } = [-250.349745, -1.515687312, -System.Math.PI];

	internal static IReadOnlyCollection<double> SpecialValues { get; } = [double.NaN, double.PositiveInfinity, double.NegativeInfinity];

	internal static IReadOnlyCollection<double> Tolerances { get; } = [1e-3, 1e-12];

	internal static IReadOnlyCollection<double> Values { get; } = NegativeValues.Concat( PositiveValues ).ToArray();

	#endregion
}
