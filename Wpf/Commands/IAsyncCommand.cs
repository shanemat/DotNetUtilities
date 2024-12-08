namespace Shanemat.DotNetUtils.Wpf.Commands;

/// <summary>
/// Extends the <see cref="ICommand"/> interface by ability to execute the command asynchronously
/// </summary>
public interface IAsyncCommand : ICommand
{
	#region Properties

	/// <summary>
	/// Gets a value indicating whether the command is being executed at the moment
	/// </summary>
	bool IsBeingExecuted { get; }

	#endregion

	#region Methods

	/// <summary>
	/// Executes the command asynchronously
	/// </summary>
	Task ExecuteAsync();

	#endregion
}

/// <summary>
/// Extends the <see cref="ICommand{T}"/> interface by ability to execute the command asynchronously
/// </summary>
public interface IAsyncCommand<in T> : ICommand<T>
{
	#region Properties

	/// <summary>
	/// Gets a value indicating whether the command is being executed at the moment
	/// </summary>
	bool IsBeingExecuted { get; }

	#endregion

	#region Methods

	/// <summary>
	/// Executes the command asynchronously
	/// </summary>
	/// <param name="parameter">The parameter of the command</param>
	Task ExecuteAsync( T? parameter );

	#endregion
}
