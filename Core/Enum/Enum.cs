namespace Shanemat.DotNetUtils.Core.Enum;

/// <summary>
/// Provides helper methods for the enumeration types (not the specific values)
/// </summary>
/// <typeparam name="T">The type of the enumeration</typeparam>
public static class Enum<T>
	where T : struct, System.Enum
{
	#region Methods

	/// <summary>
	/// Returns a collection of all the enumeration's specific values
	/// </summary>
	public static IEnumerable<T> GetValues() => System.Enum.GetValues<T>();

	/// <summary>
	/// Returns a collection of the enumeration's values satisfying the filter
	/// </summary>
	/// <param name="filter">The filter for enumeration values</param>
	public static IEnumerable<T> GetValues( Func<T, bool>? filter ) => GetValues().Where( filter ?? (_ => true) );

	/// <summary>
	/// Returns a collection of the enumeration's values except for the specifically excluded ones
	/// </summary>
	/// <param name="excludedValues">The values to be excluded</param>
	public static IEnumerable<T> GetValuesExcept( params T[] excludedValues ) => GetValues( v => !excludedValues.Contains( v ) );

	#endregion
}
