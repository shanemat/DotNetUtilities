using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.ViewModelBase;

/// <summary>
/// Contains tests for <see cref="ViewModelBase.SetProperty{T}"/> method
/// </summary>
internal sealed class SetPropertyTests
{
	#region Nested Types

	/// <summary>
	/// Provides access to the protected <see cref="ViewModelBase.SetProperty{T}"/> method
	/// </summary>
	private class ViewModel : Wpf.ViewModels.ViewModelBase
	{
		#region Properties

		/// <summary>
		/// Gets or sets the value
		/// </summary>
		public int? Value { get; set; } = int.MinValue;

		#endregion

		#region Methods

		public bool SetProperty( string propertyName, int? value ) => base.SetProperty( propertyName, () => Value, v => Value = v, value );

		#endregion
	}

	#endregion

	#region Sources

	private static IEnumerable<int?> Values => Sources.Values;

	private static IEnumerable<string> PropertyNames => Sources.PropertyNames;

	#endregion

	#region Tests

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnTrueInCaseTheValueChanges( int? value )
	{
		var viewModel = new ViewModel();

		var result = viewModel.SetProperty( string.Empty, value );

		Assert.That( result, Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseInCaseTheValueDoesNotChange( int? value )
	{
		var viewModel = new ViewModel { Value = value };

		var result = viewModel.SetProperty( string.Empty, value );

		Assert.That( result, Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldSetTheValue( int? value )
	{
		var viewModel = new ViewModel();

		viewModel.SetProperty( string.Empty, value );

		Assert.That( viewModel.Value, Is.EqualTo( value ) );
	}

	[Test]
	[TestCaseSource( nameof( PropertyNames ) )]
	public void ShouldRaiseThePropertyChangedEventWithTheCorrectPropertyName( string propertyName )
	{
		var viewModel = new ViewModel();

		string? eventPropertyName = null;

		viewModel.PropertyChanged += ( _, args ) => eventPropertyName = args.PropertyName;
		viewModel.SetProperty( propertyName, 0 );

		Assert.That( eventPropertyName, Is.EqualTo( propertyName ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldRaiseThePropertyChangedEventInCaseTheValueChanges( int? value )
	{
		var viewModel = new ViewModel();

		var flag = false;

		viewModel.PropertyChanged += ( _, _ ) => flag = true;
		viewModel.SetProperty( string.Empty, value );

		Assert.That( flag, Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldNotRaiseThePropertyChangedEventInCaseTheValueDoesNotChange( int? value )
	{
		var viewModel = new ViewModel { Value = value };

		var flag = false;

		viewModel.PropertyChanged += ( _, _ ) => flag = true;
		viewModel.SetProperty( string.Empty, value );

		Assert.That( flag, Is.False );
	}

	#endregion
}
