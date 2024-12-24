using System.Windows;
using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.NullableViewModelProperty;

/// <summary>
/// Contains tests for <see cref="NullableViewModelProperty{T}.Visibility"/> property
/// </summary>
internal sealed class VisibilityTests
{
	#region Sources

	private static IEnumerable<Visibility> Visibilities => Sources.Visibilities;

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnVisibleWhenPropertyIsApplicable()
	{
		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			IsApplicableGetter = () => true,
		};

		Assert.That( property.Visibility, Is.EqualTo( Visibility.Visible ) );
	}

	[Test]
	[TestCaseSource( nameof( Visibilities ) )]
	public void ShouldReturnNonApplicableVisibilityWhenPropertyIsNotApplicable( Visibility visibility )
	{
		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			IsApplicableGetter = () => false,
			NonApplicableVisibility = visibility,
		};

		Assert.That( property.Visibility, Is.EqualTo( visibility ) );
	}

	#endregion
}
