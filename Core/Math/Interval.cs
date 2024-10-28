using System.Globalization;

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

	#region Interval

	public double Minimum { get; } = double.NegativeInfinity;

	public bool IsMinimumIncluded { get; }

	public double Maximum { get; } = double.PositiveInfinity;

	public bool IsMaximumIncluded { get; }

	#endregion
}
