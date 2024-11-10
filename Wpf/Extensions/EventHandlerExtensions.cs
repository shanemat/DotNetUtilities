namespace Shanemat.DotNetUtils.Wpf.Extensions;

/// <summary>
/// Contains extension methods for <see cref="EventHandler"/> class
/// </summary>
public static class EventHandlerExtensions
{
	#region Methods

	/// <summary>
	/// Calls the given event handler on the UI thread
	/// </summary>
	/// <param name="eventHandler">The event handler to call</param>
	/// <param name="sender">The sender of the event</param>
	public static void InvokeOnUiThread( this EventHandler eventHandler, object sender )
	{
		((Action) Invoke).InvokeOnUiThread();

		void Invoke() => eventHandler.Invoke( sender, EventArgs.Empty );
	}

	/// <summary>
	/// Calls the given event handler on the UI thread
	/// </summary>
	/// <param name="eventHandler">The event handler to call</param>
	/// <param name="sender">The sender of the event</param>
	/// <param name="arguments">The event arguments to pass to the handler</param>
	public static void InvokeOnUiThread<T>( this EventHandler<T> eventHandler, object sender, T arguments )
	{
		((Action) Invoke).InvokeOnUiThread();

		void Invoke() => eventHandler.Invoke( sender, arguments );
	}

	#endregion
}
