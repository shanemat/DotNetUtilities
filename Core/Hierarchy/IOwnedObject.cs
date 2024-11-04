namespace Shanemat.DotNetUtils.Core.Hierarchy;

/// <summary>
/// Represents an object with an owner
/// </summary>
public interface IOwnedObject
{
	#region Properties

	/// <summary>
	/// Gets or sets the owner
	/// </summary>
	object? Owner { get; set; }

	#endregion
}
