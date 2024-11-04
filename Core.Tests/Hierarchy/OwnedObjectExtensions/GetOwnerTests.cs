using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Hierarchy;

namespace Shanemat.DotNetUtils.Core.Tests.Hierarchy.OwnedObjectExtensions;

/// <summary>
/// Contains tests for <see cref="OwnedObjectExtensions.GetOwner{T}"/> method
/// </summary>
internal sealed class GetOwnerTests
{
	#region Methods

	[Test]
	public void ShouldThrowWhenCalledOnNullObject()
	{
		IOwnedObject? value = null;

		Assert.Throws<ArgumentNullException>( () => value.GetOwner<IOwnedObject>() );
	}

	[Test]
	public void ShouldThrowWhenTheObjectHasNoOwnerForStructs()
	{
		var @struct = new StructA();

		Assert.Throws<InvalidOperationException>( () => @struct.GetOwner<StructA>() );
	}

	[Test]
	public void ShouldThrowWhenTheObjectHasNoOwnerForClasses()
	{
		var @class = new ClassA();

		Assert.Throws<InvalidOperationException>( () => @class.GetOwner<ClassA>() );
	}

	[Test]
	public void ShouldThrowWhenOnlyTheObjectIsOfTheTargetTypeForStructs()
	{
		var structA = new StructA();
		var structB = new StructB( structA );
		var structC = new StructC( structB );

		Assert.Throws<InvalidOperationException>( () => structC.GetOwner<StructC>() );
	}

	[Test]
	public void ShouldThrowWhenOnlyTheObjectIsOfTheTargetTypeForClasses()
	{
		var classA = new ClassA();
		var classB = new ClassB { Owner = classA };
		var classC = new ClassC { Owner = classB };

		Assert.Throws<InvalidOperationException>( () => classC.GetOwner<ClassC>() );
	}

	[Test]
	public void ShouldReturnTheOwnerWhenItExistsForStructs()
	{
		var structA = new StructA();
		var structB = new StructB( structA );
		var structC = new StructC( structB );

		Assert.Multiple( () =>
		{
			Assert.That( structC.GetOwner<StructB>(), Is.EqualTo( structB ) );
			Assert.That( structC.GetOwner<StructA>(), Is.EqualTo( structA ) );
			Assert.That( structB.GetOwner<StructA>(), Is.EqualTo( structA ) );
		} );
	}

	[Test]
	public void ShouldReturnTheOwnerWhenItExistsForClasses()
	{
		var classA = new ClassA();
		var classB = new ClassB { Owner = classA };
		var classC = new ClassC { Owner = classB };

		Assert.Multiple( () =>
		{
			Assert.That( classC.GetOwner<ClassB>(), Is.SameAs( classB ) );
			Assert.That( classC.GetOwner<ClassA>(), Is.SameAs( classA ) );
			Assert.That( classB.GetOwner<ClassA>(), Is.SameAs( classA ) );
		} );
	}

	[Test]
	public void ShouldReturnTheClosestOwnerOfTheGivenTypeForStructs()
	{
		var structA = new StructA();
		var structB = new StructB( structA );
		var structA2 = new StructA( structB );
		var structC = new StructC( structA2 );

		Assert.That( structC.GetOwner<StructA>(), Is.EqualTo( structA2 ) );
	}

	[Test]
	public void ShouldReturnTheClosestOwnerOfTheGivenTypeForClasses()
	{
		var classA = new ClassA();
		var classB = new ClassB { Owner = classA };
		var classA2 = new ClassA { Owner = classB };
		var classC = new ClassC { Owner = classA2 };

		Assert.That( classC.GetOwner<ClassA>(), Is.SameAs( classA2 ) );
	}

	#endregion
}
