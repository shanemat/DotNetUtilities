using System.Windows;

namespace Wpf.Tests.App;

/// <summary>
/// Represents a view for the main window
/// </summary>
public partial class MainWindow
{
	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="MainWindow"/> class
	/// </summary>
	public MainWindow()
	{
		InitializeComponent();
	}

	#endregion

	#region Event Handlers

	private void OnUiInvocationButtonClick( object sender, RoutedEventArgs e ) => new Views.UiInvocation().ShowDialog();

	#endregion
}
