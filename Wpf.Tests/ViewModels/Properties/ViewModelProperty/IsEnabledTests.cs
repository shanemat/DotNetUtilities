using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.ViewModelProperty;

/// <summary>
/// Contains tests for <see cref="ViewModelProperty{T}.IsEnabled"/> property
/// </summary>
internal sealed class IsEnabledTests
{
	#region Tests

	[Test]
	public void ShouldReturnFalseWhenPropertyIsNotApplicable()
	{
		var property = new ViewModelProperty<int>
		{
			ValueGetter = () => 0,
			ValueSetter = _ => { },
			IsApplicableGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnFalseWhenPropertyIsReadOnly()
	{
		var property = new ViewModelProperty<int>
		{
			ValueGetter = () => 0,
			IsReadOnlyGetter = () => true,
		};

		Assert.That( property.IsEnabled, Is.False );
	}

	[Test]
	public void ShouldReturnTrueWhenPropertyCanBeEdited()
	{
		var property = new ViewModelProperty<int>
		{
			ValueGetter = () => 0,
			ValueSetter = _ => { },
			IsApplicableGetter = () => true,
			IsReadOnlyGetter = () => false,
		};

		Assert.That( property.IsEnabled, Is.True );
	}

	#endregion
}
