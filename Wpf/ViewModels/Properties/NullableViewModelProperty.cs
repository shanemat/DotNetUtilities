namespace Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

/// <summary>
/// Represents a nullable view model property
/// </summary>
/// <typeparam name="T">The type of the value of the property</typeparam>
public class NullableViewModelProperty<T> : ViewModelPropertyBase
{
	#region Fields

	/// <summary>
	/// The cached value
	/// </summary>
	private T? _cachedValue;

	/// <summary>
	/// A value indicating whether the cached value is actual
	/// </summary>
	private bool _isCachedValueActual;

	/// <summary>
	/// The function for obtaining the value of the property
	/// </summary>
	private readonly Func<T?> _valueGetter = () => default;

	#endregion

	#region Properties

	/// <summary>
	/// Gets or initializes the function for obtaining the value of the property
	/// </summary>
	public required Func<T?> ValueGetter
	{
		private get => _valueGetter;
		init
		{
			_valueGetter = value;

			_cachedValue = _valueGetter.Invoke();
			_isCachedValueActual = true;
		}
	}

	/// <summary>
	/// Gets or initializes the function for setting the value of the property
	/// </summary>
	public Action<T?>? ValueSetter { private get; init; }

	/// <summary>
	/// Gets or sets the current value of the property
	/// </summary>
	/// <remarks>If not set, the property will be read-only</remarks>
	public T? Value
	{
		get
		{
			if( _isCachedValueActual )
				return _cachedValue;

			_cachedValue = ValueGetter.Invoke();
			_isCachedValueActual = true;

			return _cachedValue;
		}
		set
		{
			_isCachedValueActual = true;

			if( !SetField( nameof( Value ), ref _cachedValue, value ) )
				return;

			ValueSetter?.Invoke( _cachedValue );
		}
	}

	#endregion

	#region Overrides

	public override bool IsReadOnly => ValueSetter is null || base.IsReadOnly;

	public override void OnChanged( ChangeType changeType = ChangeType.All )
	{
		base.OnChanged( changeType );

		if( changeType.HasFlag( ChangeType.Value ) )
		{
			_isCachedValueActual = false;

			OnPropertyChanged( nameof( Value ) );
		}
	}

	#endregion
}
