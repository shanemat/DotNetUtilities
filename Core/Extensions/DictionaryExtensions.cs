namespace Shanemat.DotNetUtils.Core.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IDictionary{TKey,TValue}"/>
/// </summary>
public static class DictionaryExtensions
{
	#region Methods

	#region IsEqualTo<TKey, TValue>

	/// <summary>
	/// Returns a value indicating whether the given two dictionaries are equal
	/// </summary>
	/// <param name="one">One of the dictionaries to compare</param>
	/// <param name="other">The other dictionary to compare</param>
	/// <param name="valueComparer">The comparer to use for value comparison (if <see langword="null"/> that <see cref="EqualityComparer{T}.Default"/> will be used)</param>
	/// <typeparam name="TKey">The type of keys of the dictionaries</typeparam>
	/// <typeparam name="TValue">The type of values of the dictionaries</typeparam>
	/// <returns>A value indicating whether the given two dictionaries are equal</returns>
	public static bool IsEqualTo<TKey, TValue>( this IDictionary<TKey, TValue>? one, IDictionary<TKey, TValue>? other, IEqualityComparer<TValue>? valueComparer = null )
	{
		if( ReferenceEquals( one, other ) )
			return true;

		if( one is null || other is null )
			return false;

		if( one.Count != other.Count )
			return false;

		valueComparer ??= EqualityComparer<TValue>.Default;

		foreach( var pair in one )
		{
			if( !other.TryGetValue( pair.Key, out var secondValue ) )
				return false;

			if( !valueComparer.Equals( pair.Value, secondValue ) )
				return false;
		}

		return true;
	}

	#endregion

	#endregion
}
