using Shanemat.DotNetUtils.Wpf.Commands;

namespace Wpf.Tests.App.ViewModels;

/// <summary>
/// Represents a view model for testing commands
/// </summary>
public sealed class Commands
{
	#region Fields

	/// <summary>
	/// The value controlling enabled status of synchronous commands
	/// </summary>
	private bool _switch;

	/// <summary>
	/// The value controlling enabled status of parametric synchronous commands
	/// </summary>
	private bool _parametricSwitch;

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="Commands"/> class
	/// </summary>
	public Commands()
	{
		SwitchOnCommand = new Command( ToggleSwitch, () => !_switch );
		SwitchOffCommand = new Command( ToggleSwitch, () => _switch );

		ParametricSwitchOnCommand = new Command<bool>( ToggleParametricSwitch, _ => !_parametricSwitch );
		ParametricSwitchOffCommand = new Command<bool>( ToggleParametricSwitch, _ => _parametricSwitch );

		void ToggleSwitch()
		{
			_switch = !_switch;

			SwitchOnCommand?.OnCanExecuteChanged();
			SwitchOffCommand?.OnCanExecuteChanged();
		}

		void ToggleParametricSwitch( bool value )
		{
			_parametricSwitch = value;

			ParametricSwitchOnCommand?.OnCanExecuteChanged();
			ParametricSwitchOffCommand?.OnCanExecuteChanged();
		}
	}

	#endregion

	#region Properties

	/// <summary>
	/// Gets the command for switching the synchronous switch on
	/// </summary>
	public ICommand SwitchOnCommand { get; }

	/// <summary>
	/// Gets the command for switching the synchronous switch off
	/// </summary>
	public ICommand SwitchOffCommand { get; }

	/// <summary>
	/// Gets the command for switching the parametric synchronous switch on
	/// </summary>
	public ICommand<bool> ParametricSwitchOnCommand { get; }

	/// <summary>
	/// Gets the command for switching the parametric synchronous switch off
	/// </summary>
	public ICommand<bool> ParametricSwitchOffCommand { get; }

	#endregion
}
