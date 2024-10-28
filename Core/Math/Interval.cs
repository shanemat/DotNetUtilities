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

	#endregion
}
