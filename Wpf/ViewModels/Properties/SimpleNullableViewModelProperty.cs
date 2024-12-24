namespace Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

/// <summary>
/// Represents a simple nullable view model property
/// </summary>
/// <typeparam name="T">The type of the value of the property</typeparam>
public class SimpleNullableViewModelProperty<T>( T? defaultValue = default ) : ViewModelPropertyBase
{
	#region Fields

	/// <summary>
	/// The current value of the property
	/// </summary>
	private T? _value = defaultValue;

	#endregion

	#region Properties

	/// <summary>
	/// Gets or sets the current value of the property
	/// </summary>
	public T? Value
	{
		get => _value;
		set => SetField( nameof( Value ), ref _value, value );
	}

	#endregion
}
