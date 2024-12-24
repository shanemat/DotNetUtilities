using System.ComponentModel;
using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.NullableViewModelProperty;

/// <summary>
/// Contains tests for <see cref="NullableViewModelProperty{T}.OnChanged"/> method
/// </summary>
internal sealed class OnChangedTests
{
	#region Tests

	[Test]
	public void ShouldRequestUpdateOfApplicableStatus()
	{
		var callCount = 0;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			IsApplicableGetter = IsApplicable,
		};

		Assert.Multiple( () =>
		{
			Assert.That( property.IsApplicable, Is.True );
			Assert.That( callCount, Is.EqualTo( 1 ) );
		} );

		property.OnChanged( ChangeType.ApplicableStatus );

		Assert.Multiple( () =>
		{
			Assert.That( property.IsApplicable, Is.False );
			Assert.That( callCount, Is.EqualTo( 2 ) );
		} );

		bool IsApplicable()
		{
			callCount++;

			return callCount < 2;
		}
	}

	[Test]
	public void ShouldRaisePropertyChangedEventForIsApplicableProperty()
	{
		var hasBeenRaised = false;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
		};

		property.PropertyChanged += OnPropertyChanged;

		property.OnChanged( ChangeType.ApplicableStatus );

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( NullableViewModelProperty<int?>.IsApplicable ) )
				return;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldRequestUpdateOfReadOnlyStatus()
	{
		var callCount = 0;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			ValueSetter = _ => { },
			IsReadOnlyGetter = IsReadOnly,
		};

		Assert.Multiple( () =>
		{
			Assert.That( property.IsReadOnly, Is.True );
			Assert.That( callCount, Is.EqualTo( 1 ) );
		} );

		property.OnChanged( ChangeType.ReadOnlyStatus );

		Assert.Multiple( () =>
		{
			Assert.That( property.IsReadOnly, Is.False );
			Assert.That( callCount, Is.EqualTo( 2 ) );
		} );

		bool IsReadOnly()
		{
			callCount++;

			return callCount < 2;
		}
	}

	[Test]
	public void ShouldRaisePropertyChangedEventForIsReadOnlyProperty()
	{
		var hasBeenRaised = false;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
		};

		property.PropertyChanged += OnPropertyChanged;

		property.OnChanged( ChangeType.ReadOnlyStatus );

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( NullableViewModelProperty<int?>.IsReadOnly ) )
				return;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldRequestUpdateOfDisplayName()
	{
		var callCount = 0;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
			DisplayNameGetter = GetDisplayName,
		};

		Assert.Multiple( () =>
		{
			Assert.That( property.DisplayName, Is.EqualTo( 1.ToString() ) );
			Assert.That( callCount, Is.EqualTo( 1 ) );
		} );

		property.OnChanged( ChangeType.DisplayName );

		Assert.Multiple( () =>
		{
			Assert.That( property.DisplayName, Is.EqualTo( 2.ToString() ) );
			Assert.That( callCount, Is.EqualTo( 2 ) );
		} );

		string GetDisplayName()
		{
			callCount++;

			return callCount.ToString();
		}
	}

	[Test]
	public void ShouldRaisePropertyChangedEventForDisplayNameProperty()
	{
		var hasBeenRaised = false;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
		};

		property.PropertyChanged += OnPropertyChanged;

		property.OnChanged( ChangeType.DisplayName );

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( NullableViewModelProperty<int?>.DisplayName ) )
				return;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldRequestUpdateOfValue()
	{
		var callCount = 0;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = GetValue,
		};

		Assert.Multiple( () =>
		{
			Assert.That( property.Value, Is.EqualTo( 1 ) );
			Assert.That( callCount, Is.EqualTo( 1 ) );
		} );

		property.OnChanged( ChangeType.Value );

		Assert.Multiple( () =>
		{
			Assert.That( property.Value, Is.EqualTo( 2 ) );
			Assert.That( callCount, Is.EqualTo( 2 ) );
		} );

		int? GetValue()
		{
			callCount++;

			return callCount;
		}
	}

	[Test]
	public void ShouldRaisePropertyChangedEventForValueProperty()
	{
		var hasBeenRaised = false;

		var property = new NullableViewModelProperty<int?>
		{
			ValueGetter = () => null,
		};

		property.PropertyChanged += OnPropertyChanged;

		property.OnChanged( ChangeType.Value );

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( NullableViewModelProperty<int?>.Value ) )
				return;

			hasBeenRaised = true;
		}
	}

	#endregion
}
