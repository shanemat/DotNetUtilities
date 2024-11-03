namespace Shanemat.DotNetUtils.Core.Math;

/// <summary>
/// Represents an interval
/// </summary>
public interface IInterval : IEquatable<IInterval>
{
	#region Properties

	/// <summary>
	/// Gets the minimum value
	/// </summary>
	double Minimum { get; }

	/// <summary>
	/// Gets a value indicating whether the minimum value is included
	/// </summary>
	bool IsMinimumIncluded { get; }

	/// <summary>
	/// Gets the maximum value
	/// </summary>
	double Maximum { get; }

	/// <summary>
	/// Gets a value indicating whether the maximum value is included
	/// </summary>
	bool IsMaximumIncluded { get; }

	/// <summary>
	/// Gets the length
	/// </summary>
	double Length { get; }

	#endregion

	#region Methods

	/// <summary>
	/// Returns a value indicating whether the interval contains the given value (within the specified tolerance)
	/// </summary>
	/// <param name="value">The value to check</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>A value indicating whether the interval contains the given value (within the specified tolerance)</returns>
	bool Contains( double value, double tolerance = Tolerance.Standard );

	/// <summary>
	/// Returns a value indicating whether this and the given interval are equal (within the specified tolerance)
	/// </summary>
	/// <param name="other">The other interval to check</param>
	/// <param name="tolerance">The tolerance to use</param>
	/// <returns>A value indicating whether this and the given interval are equal (within the specified tolerance)</returns>
	bool IsEqualTo( IInterval? other, double tolerance = Tolerance.Standard );

	#endregion
}
