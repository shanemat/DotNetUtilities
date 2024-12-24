using System.ComponentModel;
using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.SimpleNullableViewModelProperty;

/// <summary>
/// Contains tests for <see cref="SimpleNullableViewModelProperty{T}.OnChanged"/> method
/// </summary>
internal sealed class OnChangedTests
{
	#region Tests

	[Test]
	public void ShouldRequestUpdateOfApplicableStatus()
	{
		var callCount = 0;

		var property = new SimpleNullableViewModelProperty<int?>
		{
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

		var property = new SimpleNullableViewModelProperty<int?>();

		property.PropertyChanged += OnPropertyChanged;

		property.OnChanged( ChangeType.ApplicableStatus );

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( SimpleNullableViewModelProperty<int?>.IsApplicable ) )
				return;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldRequestUpdateOfReadOnlyStatus()
	{
		var callCount = 0;

		var property = new SimpleNullableViewModelProperty<int?>
		{
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

		var property = new SimpleNullableViewModelProperty<int?>();

		property.PropertyChanged += OnPropertyChanged;

		property.OnChanged( ChangeType.ReadOnlyStatus );

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( SimpleNullableViewModelProperty<int?>.IsReadOnly ) )
				return;

			hasBeenRaised = true;
		}
	}

	[Test]
	public void ShouldRequestUpdateOfDisplayName()
	{
		var callCount = 0;

		var property = new SimpleNullableViewModelProperty<int?>
		{
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

		var property = new SimpleNullableViewModelProperty<int?>();

		property.PropertyChanged += OnPropertyChanged;

		property.OnChanged( ChangeType.DisplayName );

		Assert.That( hasBeenRaised, Is.True );

		void OnPropertyChanged( object? sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != nameof( SimpleNullableViewModelProperty<int?>.DisplayName ) )
				return;

			hasBeenRaised = true;
		}
	}

	#endregion
}
