namespace Shanemat.DotNetUtils.Wpf.Commands;

/// <summary>
/// Extends the <see cref="System.Windows.Input.ICommand"/> interface by ability to notify that the condition that determines whether the command can be executed changed
/// </summary>
public interface ICommand : System.Windows.Input.ICommand
{
	#region Methods

	/// <summary>
	/// Handles the change of the condition that determines whether this command can be executed
	/// </summary>
	void OnCanExecuteChanged();

	/// <summary>
	/// Checks whether the command can be executed
	/// </summary>
	/// <returns>True in case the command can be executed; false otherwise</returns>
	bool CanExecute();

	/// <summary>
	/// Executes the command
	/// </summary>
	void Execute();

	#endregion
}

/// <summary>
/// Extends the <see cref="System.Windows.Input.ICommand"/> interface by ability to notify that the condition that determines whether the command can be executed changed
/// </summary>
public interface ICommand<in T> : System.Windows.Input.ICommand
{
	#region Methods

	/// <summary>
	/// Handles the change of the condition that determines whether this command can be executed
	/// </summary>
	void OnCanExecuteChanged();

	/// <summary>
	/// Checks whether the command can be executed
	/// </summary>
	/// <param name="parameter">The parameter of the command</param>
	/// <returns>True in case the command can be executed; false otherwise</returns>
	bool CanExecute( T? parameter );

	/// <summary>
	/// Executes the command
	/// </summary>
	/// <param name="parameter">The parameter of the command</param>
	void Execute( T? parameter );

	#endregion
}
