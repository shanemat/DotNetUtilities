using Shanemat.DotNetUtils.Core.Extensions;
using Shanemat.DotNetUtils.Wpf.Extensions;

namespace Shanemat.DotNetUtils.Wpf.Commands;

/// <summary>
/// Represents the parameterless synchronous command
/// </summary>
public sealed class Command : ICommand
{
	#region Fields

	/// <summary>
	/// The action to execute
	/// </summary>
	private readonly Action _execute;

	/// <summary>
	/// The function to figure out whether the command can be executed
	/// </summary>
	private readonly Func<bool> _canExecute;

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="Command"/> class
	/// </summary>
	/// <param name="execute">The action to execute</param>
	/// <param name="canExecute">The function to figure out whether the command can be executed</param>
	/// <remarks>If <paramref name="canExecute"/> is <see langword="null"/>, the <see cref="CanExecute()"/> will always return <see langword="true"/></remarks>
	public Command( Action execute, Func<bool>? canExecute = null )
	{
		_execute = execute;
		_canExecute = canExecute ?? (() => true);
	}

	#endregion

	#region Properties

	/// <summary>
	/// Gets the "Hidden" command
	/// </summary>
	/// <remarks>This command does nothing and is disabled</remarks>
	public static Command Hidden { get; } = new( () => { }, () => false );

	#endregion

	#region ICommand

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute() => _canExecute.Invoke();

	public bool CanExecute( object? parameter ) => CanExecute();

	public void Execute()
	{
		if( !CanExecute() )
			throw new InvalidOperationException( "The command cannot be executed at the current state!" );

		_execute.Invoke();
	}

	public void Execute( object? parameter ) => Execute();

	public void OnCanExecuteChanged() => CanExecuteChanged?.InvokeOnUiThread( this );

	#endregion
}

/// <summary>
/// Represents the parametric synchronous command
/// </summary>
/// <typeparam name="T">The type of the command parameter</typeparam>
public sealed class Command<T> : ICommand<T>
{
	#region Fields

	/// <summary>
	/// The action to execute
	/// </summary>
	private readonly Action<T?> _execute;

	/// <summary>
	/// The function to figure out whether the command can be executed
	/// </summary>
	private readonly Func<T?, bool> _canExecute;

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="Command{T}"/> class
	/// </summary>
	/// <param name="execute">The action to execute</param>
	/// <param name="canExecute">The function to figure out whether the command can be executed</param>
	/// <remarks>If <paramref name="canExecute"/> is <see langword="null"/>, the <see cref="CanExecute(T)"/> will always return <see langword="true"/></remarks>
	public Command( Action<T?> execute, Func<T?, bool>? canExecute = null )
	{
		_execute = execute;
		_canExecute = canExecute ?? (_ => true);
	}

	#endregion

	#region ICommand<T>

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute( T? parameter ) => _canExecute.Invoke( parameter );

	public bool CanExecute( object? parameter ) => CanExecute( parameter.As<T>() );

	public void Execute( T? parameter )
	{
		if( !CanExecute( parameter ) )
			throw new InvalidOperationException( "The command cannot be executed at the current state!" );

		_execute.Invoke( parameter );
	}

	public void Execute( object? parameter ) => Execute( parameter.As<T>() );

	public void OnCanExecuteChanged() => CanExecuteChanged?.InvokeOnUiThread( this );

	#endregion
}
