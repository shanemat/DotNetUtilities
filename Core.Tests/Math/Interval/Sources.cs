namespace Shanemat.DotNetUtils.Core.Tests.Math.Interval;

/// <summary>
/// Contains the source values to use in interval tests
/// </summary>
internal static class Sources
{
	#region Properties

	internal static IReadOnlyCollection<double> Tolerances { get; } = [1e-3, 1e-12];

	#endregion
}
