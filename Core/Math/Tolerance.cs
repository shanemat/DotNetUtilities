namespace Shanemat.DotNetUtils.Core.Math;

/// <summary>
/// Handles tolerance used to compare <see cref="double"/> values
/// </summary>
public static class Tolerance
{
	#region Constants

	/// <summary>
	/// The standard precision used for comparison
	/// </summary>
	public const double Standard = 1e-09;

	#endregion

	#region Methods

	/// <summary>
	/// Checks whether the given value can be used as a tolerance
	/// </summary>
	/// <param name="tolerance">The tolerance to check</param>
	/// <exception cref="ArgumentException">Thrown in case the supplied tolerance is not valid</exception>
	public static void Validate( double tolerance )
	{
		if( double.IsNaN( tolerance ) )
			throw new ArgumentException( "Tolerance must be a valid number!", nameof( tolerance ) );

		if( double.IsNegative( tolerance ) )
			throw new ArgumentException( "Tolerance must be non-negative!", nameof( tolerance ) );
	}

	#endregion
}
