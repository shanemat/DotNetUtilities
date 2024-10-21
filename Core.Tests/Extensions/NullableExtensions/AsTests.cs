using NUnit.Framework;
using Shanemat.DotNetUtils.Core.Extensions;

namespace Shanemat.DotNetUtils.Core.Tests.Extensions.NullableExtensions;

/// <summary>
/// Contains tests for <see cref="NullableExtensions.As{T}"/> method
/// </summary>
internal sealed class AsTests
{
	#region Sources

	private static IReadOnlyCollection<object> Values { get; } = [5, System.Math.PI, "foo", (true, 'c')];

	private static IReadOnlyCollection<int?> FallbackValues { get; } = [null, -5, 0, 18];

	#endregion

	#region Tests

	[Test]
	public void ShouldHandleNullValues()
	{
		object? nullValue = null;

		Assert.That( nullValue.As<object?>(), Is.Null );
	}

	[Test]
	public void ShouldReturnCorrectValueForValidCasts()
	{
		object value = 1;

		Assert.That( value.As<int?>(), Is.EqualTo( value ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldHandleImplicitCasts( object value )
	{
		Assert.That( value.As<object>(), Is.EqualTo( value ) );
	}

	[Test]
	public void ShouldNotHandleExplicitCasts()
	{
		object value = 3.0;

		Assert.That( value.As<int?>(), Is.Null );
	}

	[Test]
	[TestCaseSource( nameof( FallbackValues ) )]
	public void ShouldReturnFallbackValueIfCastFails( int? fallbackValue )
	{
		object value = "Test";

		Assert.That( value.As( fallbackValue: fallbackValue ), Is.EqualTo( fallbackValue ) );
	}

	#endregion
}
