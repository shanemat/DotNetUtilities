using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.NullableViewModelProperty;

/// <summary>
/// Contains tests for <see cref="NullableViewModelProperty{T}.IsEnabled"/> property
/// </summary>
internal sealed class IsEnabledTests
{
	#region Tests

	[Test]
	public void ShouldReturnFalseWhenPropertyIsNotApplicable()
	{
		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			ValueSetter = _ => { },
			IsApplicableGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenPropertyIsReadOnly()
	{
		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			IsReadOnlyGetter = () => true,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenPropertyCanBeEdited()
	{
		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			ValueSetter = _ => { },
			IsApplicableGetter = () => true,
			IsReadOnlyGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.True );
	}

	#endregion
}
