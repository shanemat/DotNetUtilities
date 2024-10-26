using System.Diagnostics.CodeAnalysis;

namespace Shanemat.DotNetUtils.Core.Hierarchy;

/// <summary>
/// Contains extension methods for <see cref="IOwnedObject"/> interface
/// </summary>
public static class OwnedObjectExtensions
{
	#region Methods

	#region GetOwner

	/// <summary>
	/// Returns the first object of <typeparamref name="T"/> type in the ownership hierarchy
	/// </summary>
	/// <param name="ownedObject">The object to get owner of</param>
	/// <typeparam name="T">The type of the owner to return</typeparam>
	/// <returns> Returns the first object of <typeparamref name="T"/> type in the ownership hierarchy</returns>
	/// <exception cref="ArgumentNullException">Thrown in case the object is <see langword="null"/></exception>
	/// <exception cref="InvalidOperationException">Thrown in case there is no owner of <typeparamref name="T"/> type</exception>
	public static T GetOwner<T>( this IOwnedObject? ownedObject )
	{
		if( ownedObject is null )
			throw new ArgumentNullException( nameof( ownedObject ), "The method GetOwner{T} has been called on a null instance!" );

		if( TryGetOwner( ownedObject, out T? owner ) )
			return owner;

		var currentObject = ownedObject;
		var typeNameStack = new Stack<string>();

		do
		{
			typeNameStack.Push( currentObject.GetType().Name );

			currentObject = currentObject.Owner as IOwnedObject;
		} while( currentObject is not null );

		throw new InvalidOperationException( $"The instance of {ownedObject.GetType().Name} does not have an owner of type {typeof( T ).Name}!\n" +
			$"The actual hierarchy is: {string.Join( "/", typeNameStack )}" );
	}

	#endregion

	#region Helpers

	/// <summary>
	/// Attempts to obtain the first object of <typeparamref name="T"/> type in the ownership hierarchy
	/// </summary>
	/// <param name="ownedObject">The object to get owner of</param>
	/// <param name="owner">The first object of <typeparamref name="T"/> type in the ownership hierarchy or default value if no such exists</param>
	/// <typeparam name="T">The type of the owner to look for</typeparam>
	/// <returns>True in case an owner has been found; false otherwise</returns>
	private static bool TryGetOwner<T>( this IOwnedObject? ownedObject, [NotNullWhen( true )] out T? owner )
	{
		var currentObject = ownedObject;

		do
		{
			currentObject = currentObject?.Owner as IOwnedObject;

			if( currentObject is not T targetObject )
				continue;

			owner = targetObject;

			return true;
		} while( currentObject is not null );

		owner = default;

		return false;
	}

	#endregion

	#endregion
}
