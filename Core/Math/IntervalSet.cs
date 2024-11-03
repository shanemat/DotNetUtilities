using Shanemat.DotNetUtils.Core.Validation;

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

	#region Methods

	/// <summary>
	/// Creates a new interval set from the given collection of intervals
	/// </summary>
	/// <param name="intervals">The intervals to create a set of</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>The created interval set</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static IIntervalSet Create( IEnumerable<IInterval?>? intervals = null, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		return new IntervalSet( GetSortedReducedIntervals() );

		IEnumerable<IInterval> GetSortedReducedIntervals()
		{
			if( intervals is null )
				yield break;

			var sortedValidIntervals = intervals
				.OfType<IInterval>()
				.OrderBy( i => i.Minimum )
				.ToArray();

			var intervalsCount = sortedValidIntervals.Length;

			if( intervalsCount == 0 )
				yield break;

			var currentInterval = sortedValidIntervals[0];

			foreach( var interval in sortedValidIntervals.Skip( 1 ) )
			{
				if( currentInterval.GetUnionWith( interval, tolerance ) is { } union )
				{
					currentInterval = union;
				}
				else
				{
					yield return currentInterval;

					currentInterval = interval;
				}
			}

			yield return currentInterval;
		}
	}

	#endregion

	#region IIntervalSet

	public IReadOnlyList<IInterval> Intervals { get; } = new List<IInterval>();

	public IIntervalSet GetExtendedBy( IInterval? interval, double tolerance = Tolerance.Standard ) => interval is not null
		? Create( Intervals.Append( interval ), tolerance )
		: this;

	public IIntervalSet GetExtendedBy( IEnumerable<IInterval?>? intervals, double tolerance = Tolerance.Standard ) => intervals is not null
		? Create( Intervals.Concat( intervals ), tolerance )
		: this;

	public bool Contains( double value, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		return Intervals.Any( i => i.Contains( value, tolerance ) );
	}

	#endregion
}
