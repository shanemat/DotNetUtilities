using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.ViewModelBase;

/// <summary>
/// Contains tests for <see cref="ViewModelBase.OnPropertyChanged"/> method
/// </summary>
internal sealed class OnPropertyChangedTests
{
	#region Nested Types

	/// <summary>
	/// Provides access to the protected <see cref="ViewModelBase.OnPropertyChanged"/> method
	/// </summary>
	private class ViewModel : Wpf.ViewModels.ViewModelBase
	{
		#region Methods

		public new void OnPropertyChanged( string propertyName ) => base.OnPropertyChanged( propertyName );

		#endregion
	}

	#endregion

	#region Sources

	private static IEnumerable<string> PropertyNames => Sources.PropertyNames;

	#endregion

	#region Tests

	[Test]
	[TestCaseSource( nameof( PropertyNames ) )]
	public void ShouldRaiseThePropertyChangedEventWithTheCorrectPropertyName( string propertyName )
	{
		var viewModel = new ViewModel();
		string? eventPropertyName = null;

		viewModel.PropertyChanged += ( _, args ) => eventPropertyName = args.PropertyName;
		viewModel.OnPropertyChanged( propertyName );

		Assert.That( eventPropertyName, Is.EqualTo( propertyName ) );
	}

	#endregion
}
