namespace Shanemat.DotNetUtils.Wpf.Extensions;

/// <summary>
/// Contains extension methods for <see cref="Action"/> class
/// </summary>
public static class ActionExtensions
{
	#region Methods

	/// <summary>
	/// Invokes the given action on the UI thread
	/// </summary>
	/// <param name="action">The action to invoke</param>
	/// <exception cref="InvalidOperationException">Thrown when no WPF applications are running</exception>
	public static void InvokeOnUiThread( this Action? action )
	{
		if( System.Windows.Application.Current?.Dispatcher is not { } dispatcher )
			throw new InvalidOperationException( "No running WPF applications found!" );

		if( action is null )
			return;

		if( dispatcher.CheckAccess() )
		{
			action.Invoke();
		}
		else
		{
			dispatcher.Invoke( action );
		}
	}

	#endregion
}
