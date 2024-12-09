using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.ViewModelBase;

/// <summary>
/// Contains tests for <see cref="ViewModelBase.SetField{T}"/> method
/// </summary>
internal sealed class SetFieldTests
{
	#region Nested Types

	/// <summary>
	/// Provides access to the protected <see cref="ViewModelBase.SetField{T}"/> method
	/// </summary>
	private class ViewModel : Wpf.ViewModels.ViewModelBase
	{
		#region Methods

		public new bool SetField<T>( string propertyName, ref T field, T value )
			=> base.SetField( propertyName, ref field, value );

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
		int? field = int.MinValue;
		var viewModel = new ViewModel();

		var result = viewModel.SetField( string.Empty, ref field, value );

		Assert.That( result, Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnFalseInCaseTheValueDoesNotChange( int? value )
	{
		var field = value;
		var viewModel = new ViewModel();

		var result = viewModel.SetField( string.Empty, ref field, value );

		Assert.That( result, Is.False );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldSetTheValue( int? value )
	{
		int? field = int.MinValue;
		var viewModel = new ViewModel();

		viewModel.SetField( string.Empty, ref field, value );

		Assert.That( field, Is.EqualTo( value ) );
	}

	[Test]
	[TestCaseSource( nameof( PropertyNames ) )]
	public void ShouldRaiseThePropertyChangedEventWithTheCorrectPropertyName( string propertyName )
	{
		int? field = int.MinValue;
		var viewModel = new ViewModel();

		string? eventPropertyName = null;

		viewModel.PropertyChanged += ( _, args ) => eventPropertyName = args.PropertyName;
		viewModel.SetField( propertyName, ref field, 0 );

		Assert.That( eventPropertyName, Is.EqualTo( propertyName ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldRaiseThePropertyChangedEventInCaseTheValueChanges( int? value )
	{
		int? field = int.MinValue;
		var viewModel = new ViewModel();

		var flag = false;

		viewModel.PropertyChanged += ( _, _ ) => flag = true;
		viewModel.SetField( string.Empty, ref field, value );

		Assert.That( flag, Is.True );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldNotRaiseThePropertyChangedEventInCaseTheValueDoesNotChange( int? value )
	{
		var field = value;
		var viewModel = new ViewModel();

		var flag = false;

		viewModel.PropertyChanged += ( _, _ ) => flag = true;
		viewModel.SetField( string.Empty, ref field, value );

		Assert.That( flag, Is.False );
	}

	#endregion
}
