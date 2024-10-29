namespace Shanemat.DotNetUtils.Core.Math;

/// <summary>
/// Represents a set of intervals
/// </summary>
public readonly struct IntervalSet : IIntervalSet
{
	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="IntervalSet"/> struct
	/// </summary>
	public IntervalSet()
		: this( [] )
	{
		// Intentionally left blank
	}

	/// <summary>
	/// Creates a new instance of <see cref="IntervalSet"/> struct
	/// </summary>
	/// <param name="intervals">The sorted non-overlapping intervals that make up the set</param>
	private IntervalSet( IEnumerable<IInterval> intervals )
	{
		Intervals = intervals.ToList();
	}

	#endregion

	#region Overrides

	public override string ToString() => Intervals.Any()
		? string.Join( " \u222a ", Intervals )
		: "<Empty>";

	#endregion

	#region IIntervalSet

	public IReadOnlyList<IInterval> Intervals { get; } = new List<IInterval>();

	#endregion
}
