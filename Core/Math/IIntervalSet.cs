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

	#region Methods

	/// <summary>
	/// Returns a new interval set extended by the given interval
	/// </summary>
	/// <param name="interval">The interval to add to this set</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>A new interval set extended by the given interval</returns>
	IIntervalSet GetExtendedBy( IInterval? interval, double tolerance = Tolerance.Standard );

	#endregion
}
