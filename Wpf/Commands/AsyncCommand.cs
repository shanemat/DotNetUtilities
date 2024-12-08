using Shanemat.DotNetUtils.Core.Extensions;
using Shanemat.DotNetUtils.Wpf.Extensions;

namespace Shanemat.DotNetUtils.Wpf.Commands;

/// <summary>
/// Represents the parameterless asynchronous command
/// </summary>
public sealed class AsyncCommand : IAsyncCommand
{
	#region Fields

	/// <summary>
	/// The action to execute
	/// </summary>
	private readonly Func<Task> _execute;

	/// <summary>
	/// The function to figure out whether the command can be executed
	/// </summary>
	private readonly Func<bool> _canExecute;

	/// <summary>
	/// The action used to handle exceptions thrown during the asynchronous execution of the command
	/// </summary>
	private readonly Action<Exception>? _handleException;

	/// <summary>
	/// The value indicating whether the command is being executed at the moment
	/// </summary>
	private bool _isBeingExecuted;

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="AsyncCommand"/> class
	/// </summary>
	/// <param name="execute">The action to execute</param>
	/// <param name="canExecute">The function to figure out whether the command can be executed</param>
	/// <param name="handleException">The action used to handle exceptions thrown during the asynchronous execution of the command</param>
	/// <remarks>If <paramref name="canExecute"/> is <see langword="null"/>, the <see cref="CanExecute()"/> will always return <see langword="true"/></remarks>
	public AsyncCommand( Func<Task> execute, Func<bool>? canExecute = null, Action<Exception>? handleException = null )
	{
		_execute = execute;
		_canExecute = canExecute ?? (() => true);
		_handleException = handleException;
	}

	/// <summary>
	/// Creates a new instance of <see cref="AsyncCommand"/> class
	/// </summary>
	/// <param name="execute">The action to execute (asynchronously on background thread)</param>
	/// <param name="canExecute">The function to figure out whether the command can be executed</param>
	/// <param name="handleException">The action used to handle exceptions thrown during the asynchronous execution of the command</param>
	/// <remarks>If <paramref name="canExecute"/> is <see langword="null"/>, the <see cref="CanExecute()"/> will always return <see langword="true"/></remarks>
	public AsyncCommand( Action execute, Func<bool>? canExecute = null, Action<Exception>? handleException = null )
		: this( async () => await Task.Run( execute ), canExecute, handleException )
	{
		// intentionally left blank
	}

	#endregion

	#region Methods

	/// <summary>
	/// Executes the command asynchronously
	/// </summary>
	private async Task ExecuteAsyncInternal()
	{
		try
		{
			IsBeingExecuted = true;

			await _execute.Invoke();
		}
		finally
		{
			IsBeingExecuted = false;
		}
	}

	#endregion

	#region IAsyncCommand

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute() => !IsBeingExecuted && _canExecute.Invoke();

	public bool CanExecute( object? parameter ) => CanExecute();

	public void Execute()
	{
		if( !CanExecute() )
			throw new InvalidOperationException( "The command cannot be executed at the current state!" );

		ExecuteAsyncInternal().FireAndForget( onExceptionCaught: _handleException );
	}

	public void Execute( object? parameter ) => Execute();

	public bool IsBeingExecuted
	{
		get => _isBeingExecuted;
		private set
		{
			if( _isBeingExecuted == value )
				return;

			_isBeingExecuted = value;

			OnCanExecuteChanged();
		}
	}

	public Task ExecuteAsync()
	{
		if( !CanExecute() )
			throw new InvalidOperationException( "The command cannot be executed at the current state!" );

		return ExecuteAsyncInternal();
	}

	public void OnCanExecuteChanged() => CanExecuteChanged?.InvokeOnUiThread( this );

	#endregion
}

/// <summary>
/// Represents the parametric asynchronous command
/// </summary>
/// <typeparam name="T">The type of the command parameter</typeparam>
public sealed class AsyncCommand<T> : IAsyncCommand<T>
{
	#region Fields

	/// <summary>
	/// The action to execute
	/// </summary>
	private readonly Func<T?, Task> _execute;

	/// <summary>
	/// The function to figure out whether the command can be executed
	/// </summary>
	private readonly Func<T?, bool> _canExecute;

	/// <summary>
	/// The action used to handle exceptions thrown during the asynchronous execution of the command
	/// </summary>
	private readonly Action<Exception>? _handleException;

	/// <summary>
	/// The value indicating whether the command is being executed at the moment
	/// </summary>
	private bool _isBeingExecuted;

	#endregion

	#region Constructors

	/// <summary>
	/// Creates a new instance of <see cref="AsyncCommand{T}"/> class
	/// </summary>
	/// <param name="execute">The action to execute</param>
	/// <param name="canExecute">The function to figure out whether the command can be executed</param>
	/// <param name="handleException">The action used to handle exceptions thrown during the asynchronous execution of the command</param>
	/// <remarks>If <paramref name="canExecute"/> is <see langword="null"/>, the <see cref="CanExecute(T)"/> will always return <see langword="true"/></remarks>
	public AsyncCommand( Func<T?, Task> execute, Func<T?, bool>? canExecute = null, Action<Exception>? handleException = null )
	{
		_execute = execute;
		_canExecute = canExecute ?? (_ => true);
		_handleException = handleException;
	}

	/// <summary>
	/// Creates a new instance of <see cref="AsyncCommand{T}"/> class
	/// </summary>
	/// <param name="execute">The action to execute (asynchronously on background thread)</param>
	/// <param name="canExecute">The function to figure out whether the command can be executed</param>
	/// <param name="handleException">The action used to handle exceptions thrown during the asynchronous execution of the command</param>
	/// <remarks>If <paramref name="canExecute"/> is <see langword="null"/>, the <see cref="CanExecute(T)"/> will always return <see langword="true"/></remarks>
	public AsyncCommand( Action<T?> execute, Func<T?, bool>? canExecute = null, Action<Exception>? handleException = null )
		: this( async value => await Task.Run( () => execute( value ) ), canExecute, handleException )
	{
		// intentionally left blank
	}

	#endregion

	#region Methods

	/// <summary>
	/// Executes the command asynchronously
	/// </summary>
	/// <param name="parameter">The parameter of the command</param>
	private async Task ExecuteAsyncInternal( T? parameter )
	{
		try
		{
			IsBeingExecuted = true;

			await _execute.Invoke( parameter );
		}
		finally
		{
			IsBeingExecuted = false;
		}
	}

	#endregion

	#region IAsyncCommand<T>

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute( T? parameter ) => !IsBeingExecuted && _canExecute.Invoke( parameter );

	public bool CanExecute( object? parameter ) => CanExecute( parameter.As<T>() );

	public void Execute( T? parameter )
	{
		if( !CanExecute( parameter ) )
			throw new InvalidOperationException( "The command cannot be executed at the current state!" );

		ExecuteAsyncInternal( parameter ).FireAndForget( onExceptionCaught: _handleException );
	}

	public void Execute( object? parameter ) => Execute( parameter.As<T>() );

	public bool IsBeingExecuted
	{
		get => _isBeingExecuted;
		private set
		{
			if( _isBeingExecuted == value )
				return;

			_isBeingExecuted = value;

			OnCanExecuteChanged();
		}
	}

	public Task ExecuteAsync( T? parameter )
	{
		if( !CanExecute( parameter ) )
			throw new InvalidOperationException( "The command cannot be executed at the current state!" );

		return ExecuteAsyncInternal( parameter );
	}

	public void OnCanExecuteChanged() => CanExecuteChanged?.InvokeOnUiThread( this );

	#endregion
}
