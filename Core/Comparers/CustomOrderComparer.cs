using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Comparers;

/// <summary>
/// Represents a value comparer which uses a predefined sort order
/// </summary>
/// <typeparam name="T">The type of values which are to be sorted</typeparam>
/// <remarks>Any values not included will be kept at the end of the collection in the same order as they were</remarks>
public sealed class CustomOrderComparer<T> : IComparer<T>
	where T : notnull
{
	#region Fields

	/// <summary>
	/// The indices of individual values
	/// </summary>
	private readonly IReadOnlyDictionary<T, int> _valueIndices;

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="CustomOrderComparer{T}"/> class
	/// </summary>
	/// <param name="valueIndices">The indices of individual values</param>
	private CustomOrderComparer( IReadOnlyDictionary<T, int> valueIndices )
	{
		_valueIndices = valueIndices;
	}

	#endregion

	#region Methods

	/// <summary>
	/// Creates a new instance of <see cref="CustomOrderComparer{T}"/> class
	/// </summary>
	/// <param name="values">A collection of values in the desired sort order</param>
	/// <param name="equalityComparer">The comparer to use for value comparison (if <see langword="null"/> that <see cref="EqualityComparer{T}.Default"/> will be used)</param>
	public static IComparer<T> Create( IEnumerable<T>? values = null, IEqualityComparer<T>? equalityComparer = null )
	{
		var valueArray = values?.ToArray() ?? [];

		if( valueArray.Length != valueArray.Distinct().Count() )
			throw new ArgumentException( "Duplicate values are not allowed!", nameof( values ) );

		equalityComparer ??= EqualityComparer<T>.Default;

		var valueIndices = valueArray
			.WithIndices()
			.ToDictionary( i => i.Element, i => i.Index, equalityComparer );

		return new CustomOrderComparer<T>( valueIndices );
	}

	#endregion

	#region IComparer<T>

	int IComparer<T>.Compare( T? x, T? y )
	{
		var xIndex = GetSortIndex( x );
		var yIndex = GetSortIndex( y );

		return xIndex.CompareTo( yIndex );

		int GetSortIndex( T? element ) => element is not null
			? _valueIndices.GetValueOrDefault( element, int.MaxValue )
			: int.MaxValue;
	}

	#endregion
}
