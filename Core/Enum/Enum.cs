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

	#endregion
}
