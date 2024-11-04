namespace Shanemat.DotNetUtils.Core.Extensions;

/// <summary>
/// Contains extension methods for nullable objects
/// </summary>
public static class NullableExtensions
{
	#region Methods

	#region As<T>

	/// <summary>
	/// Attempts to cast the given object to the specified type
	/// </summary>
	/// <param name="object">The object to cast</param>
	/// <param name="fallbackValue">The value to return if the cast fails</param>
	/// <typeparam name="T">The type to cast object to</typeparam>
	/// <returns>The cast object or a fallback value in case the object cannot be cast</returns>
	public static T? As<T>( this object? @object, T? fallbackValue = default )
		=> @object is T castObject ? castObject : fallbackValue;

	#endregion

	#region IsEqualTo<T>

	/// <summary>
	/// Returns a value indicating whether the given two values are equal
	/// </summary>
	/// <param name="one">One of the values to compare</param>
	/// <param name="other">The other value to compare</param>
	/// <param name="equalityComparer">The function to use for comparing two non-null values</param>
	/// <typeparam name="T">The type of the values to compare</typeparam>
	/// <returns>A value indicating whether the given two values are equal</returns>
	public static bool IsEqualTo<T>( this T? one, T? other, Func<T, T, bool> equalityComparer )
		where T : struct
		=> (one, other) switch
		{
			(null, null) => true,
			(null, _)
				or (_, null) => false,
			_ => equalityComparer.Invoke( one.Value, other.Value )
		};

	/// <summary>
	/// Returns a value indicating whether the given two values are equal
	/// </summary>
	/// <param name="one">One of the values to compare</param>
	/// <param name="other">The other value to compare</param>
	/// <param name="equalityComparer">The function to use for comparing two non-null values</param>
	/// <typeparam name="T">The type of the values to compare</typeparam>
	/// <returns>A value indicating whether the given two values are equal</returns>
	public static bool IsEqualTo<T>( this T one, T? other, Func<T, T, bool> equalityComparer )
		where T : struct
		=> IsEqualTo( (T?) one, other, equalityComparer );

	/// <summary>
	/// Returns a value indicating whether the given two values are equal
	/// </summary>
	/// <param name="one">One of the values to compare</param>
	/// <param name="other">The other value to compare</param>
	/// <param name="equalityComparer">The function to use for comparing two non-null values</param>
	/// <typeparam name="T">The type of the values to compare</typeparam>
	/// <returns>A value indicating whether the given two values are equal</returns>
	public static bool IsEqualTo<T>( this T? one, T other, Func<T, T, bool> equalityComparer )
		where T : struct
		=> IsEqualTo( one, (T?) other, equalityComparer );

	/// <summary>
	/// Returns a value indicating whether the given two values are equal
	/// </summary>
	/// <param name="one">One of the values to compare</param>
	/// <param name="other">The other value to compare</param>
	/// <param name="equalityComparer">The function to use for comparing two non-null values</param>
	/// <typeparam name="T">The type of the values to compare</typeparam>
	/// <returns>A value indicating whether the given two values are equal</returns>
	public static bool IsEqualTo<T>( this T? one, T? other, Func<T, T, bool> equalityComparer )
		where T : class
		=> (one, other) switch
		{
			(null, null) => true,
			(null, _)
				or (_, null) => false,
			_ => equalityComparer.Invoke( one, other )
		};

	#endregion

	#endregion
}
