namespace Wpf.Tests.App.Views;

/// <summary>
/// Represents a view for testing commands
/// </summary>
public partial class Commands
{
	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="Commands"/> class
	/// </summary>
	public Commands()
	{
		InitializeComponent();

		DataContext = new ViewModels.Commands();
	}

	#endregion
}
