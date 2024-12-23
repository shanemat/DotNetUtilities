using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.SimpleViewModelProperty;

/// <summary>
/// Contains tests for <see cref="SimpleViewModelProperty{T}.IsReadOnly"/> property
/// </summary>
internal sealed class IsReadOnlyTests
{
	#region Sources

	private static IEnumerable<Func<bool>> IsReadOnlyGetters => Sources.BooleanGetters;

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnFalseByDefault()
	{
		var property = new SimpleViewModelProperty<int>();

		Assert.That( property.IsReadOnly, Is.False );
	}

	[Test]
	[TestCaseSource( nameof( IsReadOnlyGetters ) )]
	public void ShouldUseTheGivenGetter( Func<bool> isReadOnlyGetter )
	{
		var property = new SimpleViewModelProperty<int>
		{
			IsReadOnlyGetter = isReadOnlyGetter,
		};

		Assert.That( property.IsReadOnly, Is.EqualTo( isReadOnlyGetter.Invoke() ) );
	}

	[Test]
	public void ShouldNotNeedlesslyQueryTheValue()
	{
		var hasBeenCalled = false;

		var property = new SimpleViewModelProperty<int>
		{
			IsReadOnlyGetter = IsReadOnly,
		};

		Assert.That( property.IsReadOnly, Is.True );
		Assert.That( property.IsReadOnly, Is.True );

		bool IsReadOnly()
		{
			if( !hasBeenCalled )
				return !hasBeenCalled;

			hasBeenCalled = true;

			throw new InvalidOperationException();
		}
	}

	#endregion
}
