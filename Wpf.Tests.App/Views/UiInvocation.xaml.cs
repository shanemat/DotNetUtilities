using System.Windows;
using Shanemat.DotNetUtils.Wpf.Extensions;

namespace Wpf.Tests.App.Views;

/// <summary>
/// Represents a view for testing the invocation on the UI thread
/// </summary>
public partial class UiInvocation
{
	#region Constants

	/// <summary>
	/// The value of the parametric event parameter
	/// </summary>
	private const string ParameterValue = "UiInvocationParameter";

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="UiInvocation"/> class
	/// </summary>
	public UiInvocation()
	{
		InitializeComponent();
	}

	#endregion

	#region Methods

	/// <summary>
	/// Checks the status of the current thread
	/// </summary>
	/// <param name="shouldRunOnUiThread">A value indicating whether the operation should run on the UI thread</param>
	/// <param name="parameter">The value of the parameter to check (if <see langword="null"/> no check will be performed)</param>
	private void CheckThreadStatus( bool shouldRunOnUiThread, string? parameter = null )
	{
		var runsOnUiThread = CheckAccess();
		var threadStatus = runsOnUiThread ? "UI" : "background";

		var (operationStatus, image) = runsOnUiThread == shouldRunOnUiThread
			? ("CORRECT", MessageBoxImage.Information)
			: ("INCORRECT", MessageBoxImage.Error);

		MessageBox.Show( $"The operation runs on {operationStatus} ({threadStatus}) thread!", "Thread Check", MessageBoxButton.OK, image );

		if( parameter is not null && parameter != ParameterValue )
		{
			MessageBox.Show( $"The passed parameter ('{parameter}') does not match the expected value ('{ParameterValue}')!", "Parameter Check", MessageBoxButton.OK, MessageBoxImage.Error );
		}
	}

	#endregion

	#region Event Handlers

	private void OnUiActionButtonClick( object sender, RoutedEventArgs e )
	{
		Task.Run( () => ((Action) (() => CheckThreadStatus( shouldRunOnUiThread: true ))).InvokeOnUiThread() );
	}

	private void OnBackgroundActionButtonClick( object sender, RoutedEventArgs e )
	{
		Task.Run( () => CheckThreadStatus( shouldRunOnUiThread: false ) );
	}

	#endregion
}
