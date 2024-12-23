using System.Windows;

namespace Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

/// <summary>
/// Represents a base class for view model properties
/// </summary>
public abstract class ViewModelPropertyBase : ViewModelBase
{
	#region Fields

	/// <summary>
	/// The cached value indicating whether the property is applicable
	/// </summary>
	private bool? _cachedIsApplicable;

	/// <summary>
	/// The cached value indicating whether the property is read-only
	/// </summary>
	private bool? _cachedIsReadOnly;

	/// <summary>
	/// The cached display name of the property
	/// </summary>
	private string? _cachedDisplayName;

	#endregion

	#region Properties

	/// <summary>
	/// Gets or initializes the function for obtaining a value indicating whether the property is applicable
	/// </summary>
	/// <remarks>If not set, will default to always returning <see langword="true"/></remarks>
	public Func<bool>? IsApplicableGetter { private get; init; }

	/// <summary>
	/// Gets or initializes the visibility of the property when <see cref="IsApplicableGetter"/> returns <see langword="false"/>
	/// </summary>
	public Visibility NonApplicableVisibility { private get; init; } = Visibility.Collapsed;

	/// <summary>
	/// Gets or initializes the function for obtaining a value indicating whether the property is read-only
	/// </summary>
	/// <remarks>If not set, will default to always returning <see langword="false"/></remarks>
	public Func<bool>? IsReadOnlyGetter { private get; init; }

	/// <summary>
	/// Gets or initializes the function for obtaining the display name of the property
	/// </summary>
	public Func<string>? DisplayNameGetter { private get; init; }

	/// <summary>
	/// Gets a value indicating whether the property is applicable
	/// </summary>
	public bool IsApplicable => _cachedIsApplicable ??= IsApplicableGetter?.Invoke() ?? true;

	/// <summary>
	/// Gets a value indicating whether the property is read-only
	/// </summary>
	public virtual bool IsReadOnly => _cachedIsReadOnly ??= IsReadOnlyGetter?.Invoke() ?? false;

	/// <summary>
	/// Gets the display name of the property
	/// </summary>
	public string DisplayName => _cachedDisplayName ?? DisplayNameGetter?.Invoke() ?? string.Empty;

	/// <summary>
	/// Gets a value indicating whether the property is enabled
	/// </summary>
	public bool IsEnabled => IsApplicable && !IsReadOnly;

	/// <summary>
	/// Gets the current visibility of the property
	/// </summary>
	public Visibility Visibility => IsApplicable ? Visibility.Visible : NonApplicableVisibility;

	#endregion

	#region Methods

	/// <summary>
	/// Handles the change of the property
	/// </summary>
	/// <param name="changeType">The type of the change</param>
	public virtual void OnChanged( ChangeType changeType = ChangeType.All )
	{
		if( changeType.HasFlag( ChangeType.ApplicableStatus ) )
		{
			_cachedIsApplicable = null;

			OnPropertyChanged( nameof( IsApplicable ) );
		}

		if( changeType.HasFlag( ChangeType.ReadOnlyStatus ) )
		{
			_cachedIsReadOnly = null;

			OnPropertyChanged( nameof( IsReadOnly ) );
		}

		if( changeType.HasFlag( ChangeType.DisplayName ) )
		{
			_cachedDisplayName = null;

			OnPropertyChanged( nameof( DisplayName ) );
		}
	}

	#endregion
}
