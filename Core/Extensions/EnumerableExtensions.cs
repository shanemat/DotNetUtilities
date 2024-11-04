using System.Diagnostics.CodeAnalysis;

namespace Shanemat.DotNetUtils.Core.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumerableExtensions
{
	#region Methods

	#region CombineWith<T>

	/// <summary>
	/// Combines the values of the given two collections
	/// </summary>
	/// <param name="source">The source collection to combine</param>
	/// <param name="collection">The other collection to combine source collection with</param>
	/// <typeparam name="T">The type of values stored in collections to combine</typeparam>
	/// <returns>Returns a collection of combination products</returns>
	/// <remarks>The result will be a collection of pairs where the first value will be from the <paramref name="source"/> and the second will be from <paramref name="collection"/></remarks>
	[return: NotNullIfNotNull( nameof( source ) )]
	[return: NotNullIfNotNull( nameof( collection ) )]
	public static IEnumerable<T[]>? CombineWith<T>( this IEnumerable<T>? source, IReadOnlyCollection<T>? collection ) => (source, collection) switch
	{
		(null, null) => null,
		(null, _) => collection.Select( v => new[] { v } ),
		(_, null) => source.Select( v => new[] { v } ),
		_ => source.SelectMany( x => collection.Select( y => new[] { x, y } ) )
	};

	/// <summary>
	/// Combines the values of the given two collections
	/// </summary>
	/// <param name="source">The source collection to combine</param>
	/// <param name="collection">The other collection to combine source collection with</param>
	/// <typeparam name="T">The type of values stored in collections to combine</typeparam>
	/// <returns>Returns a collection of combination products</returns>
	/// <remarks>The result will be a collection of arrays where each item is created by appending a value from <paramref name="collection"/> to an array from <paramref name="source"/></remarks>
	[return: NotNullIfNotNull( nameof( source ) )]
	[return: NotNullIfNotNull( nameof( collection ) )]
	public static IEnumerable<T[]>? CombineWith<T>( this IEnumerable<T[]>? source, IReadOnlyCollection<T>? collection ) => (source, collection) switch
	{
		(null, null) => null,
		(null, _) => collection.Select( v => new[] { v } ),
		(_, null) => source,
		_ => source.SelectMany( x => collection.Select( y => x.Append( y ).ToArray() ) )
	};

	/// <summary>
	/// Combines the values of the given two collections
	/// </summary>
	/// <param name="source">The source collection to combine</param>
	/// <param name="collection">The other collection to combine source collection with</param>
	/// <typeparam name="T">The type of values stored in collections to combine</typeparam>
	/// <returns>Returns a collection of combination products</returns>
	/// <remarks>The result will be a collection of arrays where each item is created by prepending a value from <paramref name="source"/> to an array from <paramref name="collection"/></remarks>
	[return: NotNullIfNotNull( nameof( source ) )]
	[return: NotNullIfNotNull( nameof( collection ) )]
	public static IEnumerable<T[]>? CombineWith<T>( this IEnumerable<T>? source, IReadOnlyCollection<T[]>? collection ) => (source, collection) switch
	{
		(null, null) => null,
		(null, _) => collection,
		(_, null) => source.Select( v => new[] { v } ),
		_ => source.SelectMany( x => collection.Select( y => y.Prepend( x ).ToArray() ) )
	};

	/// <summary>
	/// Combines the values of the given two collections
	/// </summary>
	/// <param name="source">The source collection to combine</param>
	/// <param name="collection">The other collection to combine source collection with</param>
	/// <typeparam name="T">The type of values stored in collections to combine</typeparam>
	/// <returns>Returns a collection of combination products</returns>
	/// <remarks>The result will be a collection of arrays where each item is created by concatenating an array from <paramref name="collection"/> to an array from <paramref name="source"/></remarks>
	[return: NotNullIfNotNull( nameof( source ) )]
	[return: NotNullIfNotNull( nameof( collection ) )]
	public static IEnumerable<T[]>? CombineWith<T>( this IEnumerable<T[]>? source, IReadOnlyCollection<T[]>? collection ) => (source, collection) switch
	{
		(null, null) => null,
		(null, _) => collection,
		(_, null) => source,
		_ => source.SelectMany( x => collection.Select( y => x.Concat( y ).ToArray() ) )
	};

	#endregion

	#region WithIndices<T>

	/// <summary>
	/// Adds indices to the elements of the given collection
	/// </summary>
	/// <param name="source">The source collection to add indices to</param>
	/// <typeparam name="T">The type of elements stored in the collection</typeparam>
	/// <returns>A collection of tuples with the original elements and their indices</returns>
	[return: NotNullIfNotNull( nameof( source ) )]
	public static IEnumerable<(int Index, T Element)>? WithIndices<T>( this IEnumerable<T>? source )
		=> source?.Select( ( element, index ) => (index, element) );

	#endregion

	#endregion
}
