using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Math;

/// <summary>
/// Represents an interval
/// </summary>
public readonly struct Interval : IInterval
{
	#region Nested Types

	/// <summary>
	/// Compares equality of intervals within the specified tolerance
	/// </summary>
	public sealed class EqualityComparer : IEqualityComparer<IInterval>
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
		public static IEqualityComparer<IInterval> Create( double tolerance = Tolerance.Standard )
		{
			Tolerance.Validate( tolerance );

			return new EqualityComparer( tolerance );
		}

		#endregion

		#region IEqualityComparer<IInterval>

		public bool Equals( IInterval? x, IInterval? y ) => AreEqual( x, y, _tolerance );

		public int GetHashCode( IInterval obj ) => obj.GetHashCode();

		#endregion
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="Interval"/> struct
	/// </summary>
	/// <param name="minimum">The minimum value</param>
	/// <param name="isMinimumIncluded">A value indicating whether the minimum value is included</param>
	/// <param name="maximum">The maximum value</param>
	/// <param name="isMaximumIncluded">A value indicating whether the maximum value is included</param>
	private Interval( double minimum, bool isMinimumIncluded, double maximum, bool isMaximumIncluded )
	{
		Minimum = minimum;
		IsMinimumIncluded = isMinimumIncluded;
		Maximum = maximum;
		IsMaximumIncluded = isMaximumIncluded;
	}

	#endregion

	#region Overrides

	public override string ToString() => $"{(IsMinimumIncluded ? '[' : '(')}{Minimum}{CultureInfo.CurrentCulture.TextInfo.ListSeparator} {Maximum}{(IsMaximumIncluded ? ']' : ')')}";

	public override int GetHashCode() => HashCode.Combine( IsMinimumIncluded, IsMaximumIncluded );

	public override bool Equals( [NotNullWhen( true )] object? obj ) => Equals( obj as IInterval );

	#endregion

	#region Operators

	public static bool operator ==( Interval left, Interval right ) => left.Equals( right );

	public static bool operator !=( Interval left, Interval right ) => !(left == right);

	#endregion

	#region Methods

	/// <summary>
	/// Creates a new open interval
	/// </summary>
	/// <param name="minimum">The minimum value</param>
	/// <param name="maximum">The maximum value</param>
	/// <returns>The created interval</returns>
	public static IInterval Open( double minimum, double maximum ) => CreateInterval( minimum, isMinimumIncluded: false, maximum, isMaximumIncluded: false );

	/// <summary>
	/// Creates a new open-closed interval
	/// </summary>
	/// <param name="minimum">The minimum value</param>
	/// <param name="maximum">The maximum value</param>
	/// <returns>The created interval</returns>
	public static IInterval OpenClosed( double minimum, double maximum ) => CreateInterval( minimum, isMinimumIncluded: false, maximum, isMaximumIncluded: true );

	/// <summary>
	/// Creates a new closed-open interval
	/// </summary>
	/// <param name="minimum">The minimum value</param>
	/// <param name="maximum">The maximum value</param>
	/// <returns>The created interval</returns>
	public static IInterval ClosedOpen( double minimum, double maximum ) => CreateInterval( minimum, isMinimumIncluded: true, maximum, isMaximumIncluded: false );

	/// <summary>
	/// Creates a new closed interval
	/// </summary>
	/// <param name="minimum">The minimum value</param>
	/// <param name="maximum">The maximum value</param>
	/// <returns>The created interval</returns>
	public static IInterval Closed( double minimum, double maximum ) => CreateInterval( minimum, isMinimumIncluded: true, maximum, isMaximumIncluded: true );

	/// <summary>
	/// Creates a new instance of <see cref="Interval"/> struct
	/// </summary>
	/// <param name="minimum">The minimum value</param>
	/// <param name="isMinimumIncluded">A value indicating whether the minimum value is included</param>
	/// <param name="maximum">The maximum value</param>
	/// <param name="isMaximumIncluded">A value indicating whether the maximum value is included</param>
	/// <exception cref="ArgumentException">Thrown when the bounds cannot be used to form an interval</exception>
	private static IInterval CreateInterval( double minimum, bool isMinimumIncluded, double maximum, bool isMaximumIncluded )
	{
		if( double.IsNaN( minimum ) )
			throw new ArgumentException( "The minimum value must be a valid number!", nameof( minimum ) );

		if( double.IsNaN( maximum ) )
			throw new ArgumentException( "The maximum value must be a valid number!", nameof( maximum ) );

		if( minimum > maximum )
			throw new ArgumentException( "The minimum value cannot be greater than the maximum value!", nameof( minimum ) );

		if( isMinimumIncluded && double.IsInfinity( minimum ) )
			throw new ArgumentException( "The minimum value cannot be infinite!", nameof( minimum ) );

		if( isMaximumIncluded && double.IsInfinity( maximum ) )
			throw new ArgumentException( "The maximum value cannot be infinite!", nameof( maximum ) );

		return new Interval( minimum, isMinimumIncluded, maximum, isMaximumIncluded );
	}

	/// <summary>
	/// Returns a value indicating whether the given two intervals are equal (within the specified tolerance)
	/// </summary>
	/// <param name="one">One of the intervals to check</param>
	/// <param name="other">The other interval to check</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>A value indicating whether the given two intervals are equal (within the specified tolerance)</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static bool AreEqual( IInterval? one, IInterval? other, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		if( ReferenceEquals( one, other ) )
			return true;

		if( one is null || other is null )
			return false;

		if( one.IsMinimumIncluded != other.IsMinimumIncluded )
			return false;

		if( one.IsMaximumIncluded != other.IsMaximumIncluded )
			return false;

		if( !one.Minimum.IsEqualTo( other.Minimum, tolerance ) && (!double.IsNegativeInfinity( one.Minimum ) || !double.IsNegativeInfinity( other.Minimum )) )
			return false;

		return one.Maximum.IsEqualTo( other.Maximum, tolerance ) || (double.IsPositiveInfinity( one.Maximum ) && double.IsPositiveInfinity( other.Maximum ));
	}

	/// <summary>
	/// Returns a value indicating whether the two given intervals intersect (within the specified tolerance)
	/// </summary>
	/// <param name="one">One of the intervals to check</param>
	/// <param name="other">The other interval to check</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>A value indicating whether the two given intervals intersect (within the specified tolerance)</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static bool Intersect( [NotNullWhen( true )] IInterval? one, [NotNullWhen( true )] IInterval? other, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		if( one is null || other is null )
			return false;

		if( IsWithin( one.Minimum, other ) || IsWithin( one.Maximum, other ) )
			return true;

		if( IsWithin( other.Minimum, one ) || IsWithin( other.Maximum, one ) )
			return true;

		var areLowerBoundsEqual = one.Minimum.IsEqualTo( other.Minimum, tolerance ) || (double.IsNegativeInfinity( one.Minimum ) && double.IsNegativeInfinity( other.Minimum ));
		var areUpperBoundsEqual = one.Maximum.IsEqualTo( other.Maximum, tolerance ) || (double.IsPositiveInfinity( one.Maximum ) && double.IsPositiveInfinity( other.Maximum ));

		if( areLowerBoundsEqual && areUpperBoundsEqual )
			return true;

		if( one.IsMaximumIncluded && other.IsMinimumIncluded && one.Maximum.IsEqualTo( other.Minimum, tolerance ) )
			return true;

		if( other.IsMaximumIncluded && one.IsMinimumIncluded && other.Maximum.IsEqualTo( one.Minimum, tolerance ) )
			return true;

		return false;

		bool IsWithin( double value, IInterval interval )
			=> value.IsGreaterThan( interval.Minimum, tolerance ) && value.IsLessThan( interval.Maximum, tolerance );
	}

	/// <summary>
	/// Returns the intersection of the given two intervals
	/// </summary>
	/// <param name="one">One of the intervals to create intersection of</param>
	/// <param name="other">The other interval to create intersection of</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>Intersection of the given two intervals (or <see langword="null"/> if the intervals do not intersect)</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static IInterval? GetIntersection( IInterval? one, IInterval? other, double tolerance = Tolerance.Standard )
	{
		if( !Intersect( one, other, tolerance ) )
			return null;

		var (minimum, isMinimumIncluded) = (one.Minimum.IsEqualTo( other.Minimum, tolerance ), one.Minimum > other.Minimum) switch
		{
			(false, true) => (one.Minimum, one.IsMinimumIncluded),
			(false, false) => (other.Minimum, other.IsMinimumIncluded),
			(true, true) => (one.Minimum, one.IsMinimumIncluded && other.IsMinimumIncluded),
			(true, false) => (other.Minimum, one.IsMinimumIncluded && other.IsMinimumIncluded)
		};

		var (maximum, isMaximumIncluded) = (one.Maximum.IsEqualTo( other.Maximum, tolerance ), one.Maximum < other.Maximum) switch
		{
			(false, true) => (one.Maximum, one.IsMaximumIncluded),
			(false, false) => (other.Maximum, other.IsMaximumIncluded),
			(true, true) => (one.Maximum, one.IsMaximumIncluded && other.IsMaximumIncluded),
			(true, false) => (other.Maximum, one.IsMaximumIncluded && other.IsMaximumIncluded)
		};

		return new Interval( System.Math.Min( minimum, maximum ), isMinimumIncluded, System.Math.Max( minimum, maximum ), isMaximumIncluded );
	}

	/// <summary>
	/// Returns the union of the given two intervals
	/// </summary>
	/// <param name="one">One of the intervals to create union of</param>
	/// <param name="other">The other interval to create union of</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>Union of the given two intervals (or <see langword="null"/> if the union would not be a single interval)</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static IInterval? GetUnion( IInterval? one, IInterval? other, double tolerance = Tolerance.Standard )
	{
		if( !CanFormUnion( one, other, tolerance ) )
			return null;

		var (minimum, isMinimumIncluded) = (one.Minimum.IsEqualTo( other.Minimum, tolerance ), one.Minimum < other.Minimum) switch
		{
			(false, true) => (one.Minimum, one.IsMinimumIncluded),
			(false, false) => (other.Minimum, other.IsMinimumIncluded),
			(true, true) => (one.Minimum, one.IsMinimumIncluded || other.IsMinimumIncluded),
			(true, false) => (other.Minimum, one.IsMinimumIncluded || other.IsMinimumIncluded)
		};

		var (maximum, isMaximumIncluded) = (one.Maximum.IsEqualTo( other.Maximum, tolerance ), one.Maximum > other.Maximum) switch
		{
			(false, true) => (one.Maximum, one.IsMaximumIncluded),
			(false, false) => (other.Maximum, other.IsMaximumIncluded),
			(true, true) => (one.Maximum, one.IsMaximumIncluded || other.IsMaximumIncluded),
			(true, false) => (other.Maximum, one.IsMaximumIncluded || other.IsMaximumIncluded)
		};

		return new Interval( System.Math.Min( minimum, maximum ), isMinimumIncluded, System.Math.Max( minimum, maximum ), isMaximumIncluded );

		static bool CanFormUnion( [NotNullWhen( true )] IInterval? one, [NotNullWhen( true )] IInterval? other, double tolerance )
		{
			if( Intersect( one, other, tolerance ) )
				return true;

			if( one is null || other is null )
				return false;

			if( (one.IsMaximumIncluded || other.IsMinimumIncluded) && one.Maximum.IsEqualTo( other.Minimum, tolerance ) )
				return true;

			return (other.IsMaximumIncluded || one.IsMinimumIncluded) && other.Maximum.IsEqualTo( one.Minimum, tolerance );
		}
	}

	/// <summary>
	/// Returns the result of shortening the first interval by the second one
	/// </summary>
	/// <param name="toShorten">The interval to shorten</param>
	/// <param name="shortenBy">The interval to shorten the other one by</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>Returns the result of shortening the first interval by the second one (or <see langword="null"/> if the resulting interval would be empty or the result would be an interval set)</returns>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static IInterval? GetShortened( IInterval? toShorten, IInterval? shortenBy, double tolerance = Tolerance.Standard )
	{
		if( !Intersect( toShorten, shortenBy, tolerance ) )
			return toShorten;

		var containsMinimum = toShorten.Contains( shortenBy.Minimum, tolerance ) && !double.IsNegativeInfinity( shortenBy.Minimum );
		var containsMaximum = toShorten.Contains( shortenBy.Maximum, tolerance ) && !double.IsPositiveInfinity( shortenBy.Maximum );

		return (containsMinimum, containsMaximum) switch
		{
			(true, false) => new Interval( toShorten.Minimum, toShorten.IsMinimumIncluded, System.Math.Min( toShorten.Maximum, shortenBy.Minimum ), !shortenBy.IsMinimumIncluded ),
			(false, true) => new Interval( System.Math.Max( toShorten.Minimum, shortenBy.Maximum ), !shortenBy.IsMaximumIncluded, toShorten.Maximum, toShorten.IsMaximumIncluded ),
			(true, true)
				or (false, false) => null
		};
	}

	#endregion

	#region Interval

	public double Minimum { get; } = double.NegativeInfinity;

	public bool IsMinimumIncluded { get; }

	public double Maximum { get; } = double.PositiveInfinity;

	public bool IsMaximumIncluded { get; }

	public double Length
	{
		get
		{
			if( double.IsInfinity( Minimum ) || double.IsInfinity( Maximum ) )
				return double.PositiveInfinity;

			return Maximum - Minimum;
		}
	}

	public bool Contains( double value, double tolerance = Tolerance.Standard )
	{
		Tolerance.Validate( tolerance );

		if( double.IsNegativeInfinity( value ) && double.IsNegativeInfinity( Minimum ) )
			return true;

		if( double.IsPositiveInfinity( value ) && double.IsPositiveInfinity( Maximum ) )
			return true;

		return (IsMinimumIncluded, IsMaximumIncluded) switch
		{
			(true, true) => value.IsGreaterThanOrEqualTo( Minimum, tolerance ) && value.IsLessThanOrEqualTo( Maximum, tolerance ),
			(true, false) => value.IsGreaterThanOrEqualTo( Minimum, tolerance ) && value.IsLessThan( Maximum, tolerance ),
			(false, true) => value.IsGreaterThan( Minimum, tolerance ) && value.IsLessThanOrEqualTo( Maximum, tolerance ),
			(false, false) => value.IsGreaterThan( Minimum, tolerance ) && value.IsLessThan( Maximum, tolerance ),
		};
	}

	public bool IsEqualTo( [NotNullWhen( true )] IInterval? other, double tolerance = Tolerance.Standard ) => AreEqual( this, other, tolerance );

	public bool Equals( [NotNullWhen( true )] IInterval? other ) => IsEqualTo( other );

	public bool IntersectsWith( [NotNullWhen( true )] IInterval? other, double tolerance = Tolerance.Standard ) => Intersect( this, other, tolerance );

	public IInterval? GetIntersectionWith( IInterval? other, double tolerance = Tolerance.Standard ) => GetIntersection( this, other, tolerance );

	public IInterval? GetUnionWith( IInterval? other, double tolerance = Tolerance.Standard ) => GetUnion( this, other, tolerance );

	#endregion
}
