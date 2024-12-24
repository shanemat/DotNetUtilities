using System.ComponentModel;
using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.SimpleNullableViewModelProperty;

/// <summary>
/// Contains tests for <see cref="SimpleNullableViewModelProperty{T}.Value"/> property
/// </summary>
internal sealed class ValueTests
{
	#region Sources

	private static IEnumerable<int> Values => Sources.Values;

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnDefaultValueByDefault()
	{
		var property = new SimpleNullableViewModelProperty<int?>();

		Assert.That( property.Value, Is.Null );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldReturnSetValue( int value )
	{
		var property = new SimpleNullableViewModelProperty<int?>
		{
			Value = value,
		};

		Assert.That( property.Value, Is.EqualTo( value ) );
	}

	[Test]
	public void ShouldRaisePropertyChangedEventWhenValueChanges()
	{
		var hasBeenRaised = false;

		var property = new SimpleNullableViewModelProperty<int?>();

		property.PropertyChanged += OnPropertyChanged;

		property.Value = 5;

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( SimpleNullableViewModelProperty<int?>.Value ) )
				return;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldNotRaisePropertyChangedEventWhenValueDoesNotChange()
	{
		var hasBeenRaised = false;

		var property = new SimpleNullableViewModelProperty<int?>( 5 );

		property.PropertyChanged += OnPropertyChanged;

		property.Value = 5;

		Assert.That( hasBeenRaised, Is.False );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( SimpleNullableViewModelProperty<int?>.Value ) )
				return;

			hasBeenRaised = true;
		}
	}

	#endregion
}
