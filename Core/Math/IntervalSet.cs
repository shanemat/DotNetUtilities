﻿using System.Diagnostics.CodeAnalysis;

namespace Shanemat.DotNetUtils.Core.Math;

/// <summary>
/// Represents a set of intervals
/// </summary>
public readonly struct IntervalSet : IIntervalSet
{
	#region Nested Types

	/// <summary>
	/// Compares equality of interval sets within the specified tolerance
	/// </summary>
	public sealed class EqualityComparer : IEqualityComparer<IIntervalSet>
	{
		#region Fields

		/// <summary>
		/// The tolerance to use
		/// </summary>
		private readonly double _tolerance;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of <see cref="EqualityComparer"/> class
		/// </summary>
		/// <param name="tolerance">The tolerance to use</param>
		private EqualityComparer( double tolerance )
		{
			_tolerance = tolerance;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new instance of <see cref="EqualityComparer"/> class
		/// </summary>
		/// <param name="tolerance">The tolerance to use</param>
		/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
		public static IEqualityComparer<IIntervalSet> Create( double tolerance = Tolerance.Standard )
		{
			Tolerance.Validate( tolerance );

			return new EqualityComparer( tolerance );
		}

		#endregion

		#region IEqualityComparer<IIntervalSet>

		public bool Equals( IIntervalSet? x, IIntervalSet? y ) => AreEqual( x, y, _tolerance );

		public int GetHashCode( IIntervalSet obj ) => obj.GetHashCode();

		#endregion
	}

	#endregion

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

	public override int GetHashCode() => HashCode.Combine( Intervals.Count );

	public override bool Equals( [NotNullWhen( true )] object? obj ) => Equals( obj as IIntervalSet );

	#endregion

	#region Operators

	public static bool operator ==( IntervalSet left, IntervalSet right ) => left.Equals( right );

	public static bool operator !=( IntervalSet left, IntervalSet right ) => !(left == right);

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

	/// <summary>
	/// Returns a value indicating whether the given two interval sets are equal (within the specified tolerance)
	/// </summary>
	/// <param name="one">One of the interval sets to check</param>
	/// <param name="other">The other interval set to check</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>A value indicating whether the given two interval sets are equal (within the specified tolerance)</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static bool AreEqual( IIntervalSet? one, IIntervalSet? other, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		if( ReferenceEquals( one, other ) )
			return true;

		if( one is null || other is null )
			return false;

		return one.Intervals.SequenceEqual( other.Intervals, Interval.EqualityComparer.Create( tolerance ) );
	}

	/// <summary>
	/// Returns the union of the given interval sets
	/// </summary>
	/// <param name="one">One of the interval sets to create union of</param>
	/// <param name="other">The other interval set to create union of</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>Union of the given interval sets</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static IIntervalSet GetUnion( IIntervalSet? one, IIntervalSet? other, double tolerance = Tolerance.Standard )
		=> Create( (one?.Intervals ?? []).Concat( other?.Intervals ?? [] ), tolerance );

	/// <summary>
	/// Returns the intersection of the given interval sets
	/// </summary>
	/// <param name="one">One of the interval sets to create intersection of</param>
	/// <param name="other">The other interval set to create intersection of</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>Intersection of the given interval sets</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static IIntervalSet GetIntersection( IIntervalSet? one, IIntervalSet? other, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		return new IntervalSet( GetIntersections() );

		IEnumerable<IInterval> GetIntersections()
		{
			if( one is null || other is null )
				yield break;

			if( !one.Intervals.Any() || !other.Intervals.Any() )
				yield break;

			var orderedIntervalPairs = one.Intervals.Select( i => (IsFromFirst: true, Interval: i) )
				.Concat( other.Intervals.Select( i => (IsFromFirst: false, Interval: i) ) )
				.OrderBy( pair => pair.Interval.Minimum )
				.ToArray();

			var currentPair = orderedIntervalPairs.First();

			foreach( var nextPair in orderedIntervalPairs.Skip( 1 ) )
			{
				if( nextPair.IsFromFirst == currentPair.IsFromFirst )
				{
					currentPair = nextPair;
				}
				else
				{
					if( currentPair.Interval.GetIntersectionWith( nextPair.Interval, tolerance ) is { } intersection )
						yield return intersection;

					currentPair = currentPair.Interval.Maximum > nextPair.Interval.Maximum ? currentPair : nextPair;
				}
			}
		}
	}

	/// <summary>
	/// Returns the first interval set shortened by the second one
	/// </summary>
	/// <param name="toShorten">The interval set to shorten</param>
	/// <param name="shortenBy">The interval set to shorten the other one by</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>Returns the first interval set shortened by the second one</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static IIntervalSet GetShortened( IIntervalSet? toShorten, IIntervalSet? shortenBy, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		if( toShorten?.Intervals.Any() is not true )
			return new IntervalSet( [] );

		if( shortenBy?.Intervals.Any() is not true )
			return toShorten;

		return toShorten.GetIntersectionWith( shortenBy.GetComplement(), tolerance );
	}

	/// <summary>
	/// Returns a complement to the given interval set
	/// </summary>
	/// <param name="set">The set to create a complement to</param>
	/// <returns>A complement to the given interval set</returns>
	public static IIntervalSet GetComplementTo( IIntervalSet? set )
	{
		return new IntervalSet( GetComplementIntervals() );

		IEnumerable<IInterval> GetComplementIntervals()
		{
			if( set is null )
			{
				yield return Interval.Open( double.NegativeInfinity, double.PositiveInfinity );

				yield break;
			}

			if( !set.Intervals.Any() )
			{
				yield return Interval.Open( double.NegativeInfinity, double.PositiveInfinity );

				yield break;
			}

			var firstInterval = set.Intervals[0];

			if( !double.IsNegativeInfinity( firstInterval.Minimum ) )
			{
				yield return firstInterval.IsMinimumIncluded
					? Interval.Open( double.NegativeInfinity, firstInterval.Minimum )
					: Interval.OpenClosed( double.NegativeInfinity, firstInterval.Minimum );
			}

			var previousMaximum = firstInterval.Maximum;
			var wasPreviousMaximumIncluded = firstInterval.IsMaximumIncluded;

			foreach( var interval in set.Intervals.Skip( 1 ) )
			{
				yield return (wasPreviousMaximumIncluded, interval.IsMinimumIncluded) switch
				{
					(true, true) => Interval.Open( previousMaximum, interval.Minimum ),
					(true, false) => Interval.OpenClosed( previousMaximum, interval.Minimum ),
					(false, true) => Interval.ClosedOpen( previousMaximum, interval.Minimum ),
					(false, false) => Interval.Closed( previousMaximum, interval.Minimum ),
				};

				previousMaximum = interval.Maximum;
				wasPreviousMaximumIncluded = interval.IsMaximumIncluded;
			}

			if( !double.IsPositiveInfinity( set.Intervals[^1].Maximum ) )
			{
				yield return wasPreviousMaximumIncluded
					? Interval.Open( previousMaximum, double.PositiveInfinity )
					: Interval.ClosedOpen( previousMaximum, double.PositiveInfinity );
			}
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

	public bool IsEqualTo( [NotNullWhen( true )] IIntervalSet? other, double tolerance = Tolerance.Standard ) => AreEqual( this, other, tolerance );

	public bool Equals( [NotNullWhen( true )] IIntervalSet? other ) => IsEqualTo( other );

	public IIntervalSet GetUnionWith( IIntervalSet? other, double tolerance = Tolerance.Standard ) => GetUnion( this, other, tolerance );

	public IIntervalSet GetIntersectionWith( IIntervalSet? other, double tolerance = Tolerance.Standard ) => GetIntersection( this, other, tolerance );

	public IIntervalSet GetShortenedBy( IIntervalSet? shortenBy, double tolerance = Tolerance.Standard ) => GetShortened( this, shortenBy, tolerance );

	public IIntervalSet GetComplement() => GetComplementTo( this );

	#endregion
}
