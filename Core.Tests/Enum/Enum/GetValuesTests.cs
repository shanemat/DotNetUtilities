using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Enum;

namespace Shanemat.DotNetUtils.Core.Tests.Enum.Enum;

/// <summary>
/// Contains tests for <see cref="Enum{T}.GetValues()"/> class
/// </summary>
internal sealed class GetValuesTests
{
	#region Tests

	[Test]
	public void ShouldReturnAllValues()
	{
		Assert.That( Enum<TestEnum>.GetValues(), Is.EquivalentTo( new[] { TestEnum.Unknown, TestEnum.A, TestEnum.B, TestEnum.C } ) );
	}

	#endregion
}
