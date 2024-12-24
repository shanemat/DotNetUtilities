using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.SimpleNullableViewModelProperty;

/// <summary>
/// Contains tests for <see cref="SimpleNullableViewModelProperty{T}.IsEnabled"/> property
/// </summary>
internal sealed class IsEnabledTests
{
	#region Tests

	[Test]
	public void ShouldReturnFalseWhenPropertyIsNotApplicable()
	{
		var property = new SimpleNullableViewModelProperty<int?>
		{
			IsApplicableGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenPropertyIsReadOnly()
	{
		var property = new SimpleNullableViewModelProperty<int?>
		{
			IsReadOnlyGetter = () => true,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenPropertyCanBeEdited()
	{
		var property = new SimpleNullableViewModelProperty<int?>
		{
			IsApplicableGetter = () => true,
			IsReadOnlyGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.True );
	}

	#endregion
}
