using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Hierarchy;

namespace Shanemat.DotNetUtils.Core.Tests.Hierarchy.OwnedObjectExtensions;

/// <summary>
/// Contains tests for <see cref="OwnedObjectExtensions.GetOwnerOrDefault{T}"/> method
/// </summary>
internal sealed class GetOwnerOrNullTests
{
	#region Methods

	[Test]
	public void ShouldReturnNullWhenCalledOnNullObject()
	{
		IOwnedObject? value = null;

		Assert.That( value.GetOwnerOrDefault<IOwnedObject>(), Is.Null );
	}

	[Test]
	public void ShouldReturnDefaultValueWhenTheObjectHasNoOwnerForStructs()
	{
		var @struct = new StructA();

		Assert.That( @struct.GetOwnerOrDefault<StructA>(), Is.EqualTo( default( StructA ) ) );
	}

	[Test]
	public void ShouldReturnDefaultValueWhenTheObjectHasNoOwnerForClasses()
	{
		var @class = new ClassA();

		Assert.That( @class.GetOwnerOrDefault<ClassA>(), Is.Null );
	}

	[Test]
	public void ShouldReturnDefaultValueWhenOnlyTheObjectIsOfTheTargetTypeForStructs()
	{
		var structA = new StructA();
		var structB = new StructB( structA );
		var structC = new StructC( structB );

		Assert.That( structC.GetOwnerOrDefault<StructC>(), Is.EqualTo( default( StructC ) ) );
	}

	[Test]
	public void ShouldReturnDefaultValueWhenOnlyTheObjectIsOfTheTargetTypeForClasses()
	{
		var classA = new ClassA();
		var classB = new ClassB { Owner = classA };
		var classC = new ClassC { Owner = classB };

		Assert.That( classC.GetOwnerOrDefault<ClassC>(), Is.Null );
	}

	[Test]
	public void ShouldReturnTheOwnerWhenItExistsForStructs()
	{
		var structA = new StructA();
		var structB = new StructB( structA );
		var structC = new StructC( structB );

		Assert.Multiple( () =>
		{
			Assert.That( structC.GetOwnerOrDefault<StructB>(), Is.EqualTo( structB ) );
			Assert.That( structC.GetOwnerOrDefault<StructA>(), Is.EqualTo( structA ) );
			Assert.That( structB.GetOwnerOrDefault<StructA>(), Is.EqualTo( structA ) );
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
			Assert.That( classC.GetOwnerOrDefault<ClassB>(), Is.SameAs( classB ) );
			Assert.That( classC.GetOwnerOrDefault<ClassA>(), Is.SameAs( classA ) );
			Assert.That( classB.GetOwnerOrDefault<ClassA>(), Is.SameAs( classA ) );
		} );
	}

	[Test]
	public void ShouldUseSuppliedFallbackValueForStructs()
	{
		var @struct = new StructA();
		var fallbackValue = new StructB( @struct );

		Assert.That( @struct.GetOwnerOrDefault( fallbackValue ), Is.EqualTo( fallbackValue ) );
	}

	[Test]
	public void ShouldUseSuppliedFallbackValueForClasses()
	{
		var @class = new ClassA();
		var fallbackValue = new ClassB { Owner = @class };

		Assert.That( @class.GetOwnerOrDefault( fallbackValue ), Is.SameAs( fallbackValue ) );
	}

	[Test]
	public void ShouldReturnTheClosestOwnerOfTheGivenTypeForStructs()
	{
		var structA = new StructA();
		var structB = new StructB( structA );
		var structA2 = new StructA( structB );
		var structC = new StructC( structA2 );

		Assert.That( structC.GetOwnerOrDefault<StructA>(), Is.EqualTo( structA2 ) );
	}

	[Test]
	public void ShouldReturnTheClosestOwnerOfTheGivenTypeForClasses()
	{
		var classA = new ClassA();
		var classB = new ClassB { Owner = classA };
		var classA2 = new ClassA { Owner = classB };
		var classC = new ClassC { Owner = classA2 };

		Assert.That( classC.GetOwnerOrDefault<ClassA>(), Is.SameAs( classA2 ) );
	}

	#endregion
}
