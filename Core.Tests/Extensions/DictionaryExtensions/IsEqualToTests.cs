using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.DictionaryExtensions;

/// <summary>
/// Contains tests for <see cref="DictionaryExtensions.IsEqualTo{TKey,TValue}"/> method
/// </summary>
internal sealed class IsEqualToTests
{
	#region Methods

	[Test]
	public void ShouldReturnTrueWhenBothDictionariesAreNull()
	{
		Dictionary<int, string>? dictionary = null;

		Assert.That( dictionary.IsEqualTo( dictionary ), Is.True );
	}

	[Test]
	public void ShouldReturnFalseWhenOneOfTheDictionariesIsNull()
	{
		Dictionary<int, string> dictionary = [];
		Dictionary<int, string>? nullDictionary = null;

		Assert.Multiple( () =>
		{
			Assert.That( dictionary.IsEqualTo( nullDictionary ), Is.False );
			Assert.That( nullDictionary.IsEqualTo( dictionary ), Is.False );
		} );
	}

	[Test]
	public void ShouldReturnTrueWhenBothDictionariesAreEmpty()
	{
		Dictionary<int, string> oneDictionary = [];
		Dictionary<int, string> otherDictionary = [];

		Assert.That( oneDictionary.IsEqualTo( otherDictionary ), Is.True );
	}

	[Test]
	public void ShouldBeReflexive()
	{
		Dictionary<int, string> oneDictionary = new()
		{
			[0] = "foo",
			[1] = "bar",
		};

		Assert.That( oneDictionary.IsEqualTo( oneDictionary ), Is.True );
	}

	[Test]
	public void ShouldReturnFalseWhenDictionariesHaveDifferentNumberOfElements()
	{
		Dictionary<int, string> oneDictionary = new()
		{
			[0] = "foo",
			[1] = "bar",
		};

		Dictionary<int, string> otherDictionary = new()
		{
			[0] = "foo",
		};

		Assert.That( oneDictionary.IsEqualTo( otherDictionary ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenDictionariesHaveEqualElementsButIndexedDifferently()
	{
		Dictionary<int, string> oneDictionary = new()
		{
			[0] = "foo",
			[1] = "bar",
		};

		Dictionary<int, string> otherDictionary = new()
		{
			[0] = "bar",
			[1] = "foo",
		};

		Assert.That( oneDictionary.IsEqualTo( otherDictionary ), Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenDictionariesHaveEqualIndexesButDifferentElements()
	{
		Dictionary<int, string> oneDictionary = new()
		{
			[0] = "foo",
			[1] = "bar",
		};

		Dictionary<int, string> otherDictionary = new()
		{
			[0] = "not foo",
			[1] = "not bar",
		};

		Assert.That( oneDictionary.IsEqualTo( otherDictionary ), Is.False );
	}

	[Test]
	public void ShouldUseTheSuppliedEqualityComparer()
	{
		Dictionary<int, string> oneDictionary = new()
		{
			[0] = "foo",
			[1] = "bar",
		};

		Dictionary<int, string> otherDictionary = new()
		{
			[0] = "FOO",
			[1] = "BAR",
		};

		Assert.Multiple( () =>
		{
			Assert.That( oneDictionary.IsEqualTo( otherDictionary, StringComparer.Ordinal ), Is.False );
			Assert.That( oneDictionary.IsEqualTo( otherDictionary, StringComparer.OrdinalIgnoreCase ), Is.True );
		} );
	}

	#endregion
}
