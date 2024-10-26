using Shanemat.DotNetUtils.Core.Hierarchy;

namespace Shanemat.DotNetUtils.Core.Tests.Hierarchy.OwnedObjectExtensions;

internal abstract class BaseClass : IOwnedObject
{
	#region IOwnedObject

	public object? Owner { get; set; }

	#endregion
}

internal sealed class ClassA : BaseClass;

internal sealed class ClassB : BaseClass;

internal sealed class ClassC : BaseClass;

internal record struct StructA( object? Owner ) : IOwnedObject;

internal record struct StructB( object? Owner ) : IOwnedObject;

internal record struct StructC( object? Owner ) : IOwnedObject;
