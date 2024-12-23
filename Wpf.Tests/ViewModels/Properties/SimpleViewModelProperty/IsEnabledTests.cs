using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.SimpleViewModelProperty;

/// <summary>
/// Contains tests for <see cref="SimpleViewModelProperty{T}.IsEnabled"/> property
/// </summary>
internal sealed class IsEnabledTests
{
	#region Tests

	[Test]
	public void ShouldReturnFalseWhenPropertyIsNotApplicable()
	{
		var property = new SimpleViewModelProperty<int>
		{
			IsApplicableGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenPropertyIsReadOnly()
	{
		var property = new SimpleViewModelProperty<int>
		{
			IsReadOnlyGetter = () => true,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenPropertyCanBeEdited()
	{
		var property = new SimpleViewModelProperty<int>
		{
			IsApplicableGetter = () => true,
			IsReadOnlyGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.True );
	}

	#endregion
}
