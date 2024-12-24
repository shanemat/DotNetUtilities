using System.ComponentModel;
using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.ViewModelProperty;

/// <summary>
/// Contains tests for <see cref="ViewModelProperty{T}.Value"/> property
/// </summary>
internal sealed class ValueTests
{
	#region Sources

	private static IEnumerable<int> Values => Sources.Values;

	#endregion

	#region Tests

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldUseTheGivenGetter( int value )
	{
		var property = new ViewModelProperty<int>
		{
			ValueGetter = () => value,
		};

		Assert.That( property.Value, Is.EqualTo( value ) );
	}

	[Test]
	[TestCaseSource( nameof( Values ) )]
	public void ShouldUseTheGivenSetter( int value )
	{
		var hasBeenCalled = false;
		var storedValue = int.MinValue;

		var property = new ViewModelProperty<int>
		{
			ValueGetter = () => storedValue,
			ValueSetter = SetValue,
			Value = value,
		};

		Assert.Multiple( () =>
		{
			Assert.That( property.Value, Is.EqualTo( value ) );
			Assert.That( hasBeenCalled, Is.True );
		} );

		void SetValue( int valueToSet )
		{
			storedValue = valueToSet;

			hasBeenCalled = true;
		}
	}

	[Test]
	public void ShouldRaisePropertyChangedEventWhenValueChanges()
	{
		var hasBeenRaised = false;

		var property = new ViewModelProperty<int>
		{
			ValueGetter = () => 0,
		};

		property.PropertyChanged += OnPropertyChanged;

		property.Value = 5;

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( ViewModelProperty<int>.Value ) )
				return;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldNotCallSetterWhenValueDoesNotChange()
	{
		var hasBeenRaised = false;
		var value = 5;

		_ = new ViewModelProperty<int>
		{
			ValueGetter = () => value,
			ValueSetter = SetValue,
			Value = 5,
		};

		Assert.That( hasBeenRaised, Is.False );

		void SetValue( int valueToSet )
		{
			value = valueToSet;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldNotRaisePropertyChangedEventWhenValueDoesNotChange()
	{
		var hasBeenRaised = false;
		var value = 5;

		var property = new ViewModelProperty<int>
		{
			ValueGetter = () => value,
			ValueSetter = v => value = v,
		};

		property.PropertyChanged += OnPropertyChanged;

		property.Value = 5;

		Assert.That( hasBeenRaised, Is.False );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( ViewModelProperty<int>.Value ) )
				return;

			hasBeenRaised = true;
		}
	}

	#endregion
}
