using System.ComponentModel;
using Shanemat.DotNetUtils.Core.Hierarchy;

namespace Shanemat.DotNetUtils.Wpf.ViewModels;

/// <summary>
/// Represents a base class for view models
/// </summary>
public abstract class ViewModelBase : IOwnedObject
	, INotifyPropertyChanged
{
	#region Methods

	/// <summary>
	/// Sets the value of the given field to the specified value
	/// </summary>
	/// <param name="propertyName">The name of the property, that is being changed</param>
	/// <param name="field">The field to set the value of</param>
	/// <param name="value">The value to set</param>
	/// <typeparam name="T">The type of field and value to set</typeparam>
	/// <returns>True in case the value was changed; false otherwise</returns>
	/// <remarks>This method will raise <see cref="PropertyChanged"/> event in case the old and new values are not equal</remarks>
	protected bool SetField<T>( string propertyName, ref T field, T value )
	{
		if( Equals( field, value ) )
			return false;

		field = value;

		OnPropertyChanged( propertyName );

		return true;
	}

	/// <summary>
	/// Sets a property to the specified value
	/// </summary>
	/// <param name="propertyName">The name of the property, that is being changed</param>
	/// <param name="getter">The getter of the property to set value of</param>
	/// <param name="setter">The setter of the property to set value of</param>
	/// <param name="value">The value to set</param>
	/// <typeparam name="T">The type of property and value to set</typeparam>
	/// <returns>True in case the value was changed; false otherwise</returns>
	/// <remarks>This method will raise <see cref="PropertyChanged"/> event in case the old and new values are not equal</remarks>
	protected bool SetProperty<T>( string propertyName, Func<T> getter, Action<T> setter, T value )
	{
		var oldValue = getter();

		if( Equals( oldValue, value ) )
			return false;

		setter.Invoke( value );

		if( Equals( getter(), oldValue ) )
			return false;

		OnPropertyChanged( propertyName );

		return true;
	}

	#endregion

	#region IOwnedObject

	public object? Owner { get; set; }

	#endregion

	#region INotifyPropertyChanged

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged( string? propertyName = null )
	{
		PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
	}

	#endregion
}
