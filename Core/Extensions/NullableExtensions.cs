namespace Shanemat.DotNetUtils.Core.Extensions;

/// <summary>
/// Contains extension methods for nullable objects
/// </summary>
public static class NullableExtensions
{
	#region Methods

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

	#endregion

	#endregion
}
