namespace Shanemat.DotNetUtils.Core.Math;

/// <summary>
/// Represents a set of intervals
/// </summary>
public interface IIntervalSet
{
	#region Properties

	/// <summary>
	/// Gets the list of intervals
	/// </summary>
	/// <remarks>The intervals are not overlapping and sorted from the lowest to the highest</remarks>
	IReadOnlyList<IInterval> Intervals { get; }

	#endregion
}
