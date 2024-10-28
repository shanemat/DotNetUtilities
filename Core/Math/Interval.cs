using System.Globalization;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Math;

/// <summary>
/// Represents an interval
/// </summary>
public readonly struct Interval : IInterval
{
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

	#endregion
}
